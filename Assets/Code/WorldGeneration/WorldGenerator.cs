using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WorldGenerator : MonoBehaviour
{
  public string Seed;
  private int currentSeed;
  const string Glyphs = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
  const int SeedLength = 10;

  [Header("Grid Size")]
  public int Size = 100;

  private WorldCell[,] worldGrid;

  private int roomsAmount;

  [Header("Rooms")]
  public int MinRooms = 7;
  public int MaxRooms = 12;
  public int RoomWidthBase = 20;
  public int RoomHeightBase = 20;

  [Header("Terrain")]
  public Material TerrainMaterial;
  public Material EdgeMaterial;
  public GameObject[] DecorationPrefabs;

  private GameObject player;
  private GameObject waterPlane;

  private void Start()
  {
    player = GameObject.Find("Player");
    waterPlane = GameObject.Find("Water");

    GenerateSeed();
    GenerateWorld();
    DrawTerrainMesh();
  }

  private void Update()
  {

  }

  private void GenerateWorld()
  {
    GenerateGrid();
    PlacePlayer();
    PlaceWater();
  }

  private void GenerateSeed()
  {
    if (Seed == "")
    {
      for (int i = 0; i < SeedLength; i++)
      {
        Seed += Glyphs[Random.Range(0, Glyphs.Length)];
      }
    }

    currentSeed = Seed.GetHashCode();
    Random.InitState(currentSeed);
  }

  private void GenerateGrid()
  {
    worldGrid = new WorldCell[Size, Size];

    int x = Size / 2;
    int y = Size / 2;

    FillGridWithCell(x, y, WorldCell.CellType.StartRoomFloor);

    roomsAmount = Random.Range(MinRooms, MaxRooms + 1) - 1;

    PopulateGrid(x, y, Random.Range(0, 4));
  }

  private void FillGridWithCell(int x, int y, WorldCell.CellType worldCellType)
  {
    for (int i = 0; i < RoomWidthBase && x + i < Size; i++)
    {
      for (int j = 0; j < RoomHeightBase && y + j < Size; j++)
      {
        worldGrid[x + i, y + j] = new WorldCell(worldCellType);
      }
    }
  }

  private void PopulateGrid(int x, int y, int direction)
  {
    if (roomsAmount < 0)
    {
      return;
    }

    switch (direction)
    {
      case 0:
        // Right
        x += RoomWidthBase;
        if (x >= Size)
        {
          x = Size / 2;
          y = Size / 2;
        }
        break;
      case 1:
        // Left
        x -= RoomWidthBase;
        if (x < 0)
        {
          x = Size / 2;
          y = Size / 2;
        }
        break;
      case 2:
        // Up
        y += RoomHeightBase;
        if (y >= Size)
        {
          y = Size / 2;
          x = Size / 2;
        }
        break;
      case 3:
        // Down
        y -= RoomHeightBase;
        if (y < 0)
        {
          y = Size / 2;
          x = Size / 2;
        }
        break;
    }

    // If the room is already occupied, repeat with that room
    if (worldGrid[x, y] != null)
    {
      PopulateGrid(x, y, Random.Range(0, 4));
      return;
    }

    FillGridWithCell(x, y, WorldCell.CellType.NormalRoomFloor);
    roomsAmount--;

    // If the room is valid, call PopulateGrid() again
    PopulateGrid(x, y, Random.Range(0, 4));
  }

  private void PlacePlayer()
  {
    player.GetComponent<CharacterController>().Move(new Vector3(Size / 2, 0, Size / 2));
  }

  private void PlaceWater()
  {
    waterPlane.transform.position = new Vector3(Size / 2, -.75f, Size / 2);
  }

  void DrawTerrainMesh()
  {
    Mesh mesh = new();

    List<Vector3> vertices = new();
    List<int> triangles = new();
    List<Vector2> uvs = new();

    for (int i = 0; i < Size; i++)
    {
      for (int j = 0; j < Size; j++)
      {
        if (worldGrid[i, j] == null || worldGrid[i, j].Type == WorldCell.CellType.Empty)
        {
          continue;
        }

        Vector3 topLeft = new(i - .5f, 0, j + .5f);
        Vector3 topRight = new(i + .5f, 0, j + .5f);
        Vector3 bottomLeft = new(i - .5f, 0, j - .5f);
        Vector3 bottomRight = new(i + .5f, 0, j - .5f);

        vertices.Add(topLeft);
        vertices.Add(topRight);
        vertices.Add(bottomLeft);
        vertices.Add(bottomRight);

        Vector2 uvTopLeft = new(i / (float)Size, j / (float)Size);
        Vector2 uvTopRight = new((i + 1) / (float)Size, j / (float)Size);
        Vector2 uvBottomLeft = new(i / (float)Size, (j + 1) / (float)Size);
        Vector2 uvBottomRight = new((i + 1) / (float)Size, (j + 1) / (float)Size);

        uvs.Add(uvTopLeft);
        uvs.Add(uvTopRight);
        uvs.Add(uvBottomLeft);
        uvs.Add(uvBottomRight);

        triangles.Add(vertices.Count - 4);
        triangles.Add(vertices.Count - 3);
        triangles.Add(vertices.Count - 2);

        triangles.Add(vertices.Count - 3);
        triangles.Add(vertices.Count - 1);
        triangles.Add(vertices.Count - 2);
      }
    }

    mesh.vertices = vertices.ToArray();
    mesh.triangles = triangles.ToArray();
    mesh.uv = uvs.ToArray();

    mesh.RecalculateNormals();

    MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
    meshFilter.mesh = mesh;

    DrawTerrainTexture();
    DrawEdgesMesh();
    GenerateDecoration();
  }

  void DrawTerrainTexture()
  {
    Texture2D texture = new(Size, Size);
    Color32[] colorMap = new Color32[Size * Size];

    for (int i = 0; i < Size; i++)
    {
      for (int j = 0; j < Size; j++)
      {
        if (worldGrid[i, j] == null)
        {
          continue;
        }

        colorMap[i + j * Size] = new Color(153 / 255f, 191 / 255f, 115 / 255f);
      }
    }

    texture.filterMode = FilterMode.Point;
    texture.SetPixels32(colorMap, 0);
    texture.Apply();

    MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
    meshRenderer.material = TerrainMaterial;
    meshRenderer.material.mainTexture = texture;
  }

  void DrawEdgesMesh()
  {
    Mesh mesh = new();

    List<Vector3> vertices = new();
    List<int> triangles = new();

    for (int i = 0; i < Size; i++)
    {
      for (int j = 0; j < Size; j++)
      {
        if (worldGrid[i, j] == null)
        {
          continue;
        }

        if (i > 0)
        {
          WorldCell leftCell = worldGrid[i - 1, j];
          if (leftCell == null || leftCell.Type == WorldCell.CellType.Empty)
          {
            Vector3 topLeft = new(i - .5f, 0, j + .5f);
            Vector3 topRight = new(i - .5f, 0, j - .5f);
            Vector3 bottomLeft = new(i - .5f, -1, j + .5f);
            Vector3 bottomRight = new(i - .5f, -1, j - .5f);

            vertices.Add(topLeft);
            vertices.Add(topRight);
            vertices.Add(bottomLeft);
            vertices.Add(bottomRight);

            triangles.Add(vertices.Count - 4);
            triangles.Add(vertices.Count - 3);
            triangles.Add(vertices.Count - 2);

            triangles.Add(vertices.Count - 3);
            triangles.Add(vertices.Count - 1);
            triangles.Add(vertices.Count - 2);
          }
        }

        if (i < Size - 1)
        {
          WorldCell rightCell = worldGrid[i + 1, j];
          if (rightCell == null || rightCell.Type == WorldCell.CellType.Empty)
          {
            Vector3 topLeft = new(i + .5f, 0, j - .5f);
            Vector3 topRight = new(i + .5f, 0, j + .5f);
            Vector3 bottomLeft = new(i + .5f, -1, j - .5f);
            Vector3 bottomRight = new(i + .5f, -1, j + .5f);

            vertices.Add(topLeft);
            vertices.Add(topRight);
            vertices.Add(bottomLeft);
            vertices.Add(bottomRight);

            triangles.Add(vertices.Count - 4);
            triangles.Add(vertices.Count - 3);
            triangles.Add(vertices.Count - 2);

            triangles.Add(vertices.Count - 3);
            triangles.Add(vertices.Count - 1);
            triangles.Add(vertices.Count - 2);
          }
        }

        if (j > 0)
        {
          WorldCell bottomCell = worldGrid[i, j - 1];
          if (bottomCell == null || bottomCell.Type == WorldCell.CellType.Empty)
          {
            Vector3 topLeft = new(i - .5f, 0, j - .5f);
            Vector3 topRight = new(i + .5f, 0, j - .5f);
            Vector3 bottomLeft = new(i - .5f, -1, j - .5f);
            Vector3 bottomRight = new(i + .5f, -1, j - .5f);

            vertices.Add(topLeft);
            vertices.Add(topRight);
            vertices.Add(bottomLeft);
            vertices.Add(bottomRight);

            triangles.Add(vertices.Count - 4);
            triangles.Add(vertices.Count - 3);
            triangles.Add(vertices.Count - 2);

            triangles.Add(vertices.Count - 3);
            triangles.Add(vertices.Count - 1);
            triangles.Add(vertices.Count - 2);
          }
        }

        if (j < Size - 1)
        {
          WorldCell topCell = worldGrid[i, j + 1];
          if (topCell == null || topCell.Type == WorldCell.CellType.Empty)
          {
            Vector3 topLeft = new(i + .5f, 0, j + .5f);
            Vector3 topRight = new(i - .5f, 0, j + .5f);
            Vector3 bottomLeft = new(i + .5f, -1, j + .5f);
            Vector3 bottomRight = new(i - .5f, -1, j + .5f);

            vertices.Add(topLeft);
            vertices.Add(topRight);
            vertices.Add(bottomLeft);
            vertices.Add(bottomRight);

            triangles.Add(vertices.Count - 4);
            triangles.Add(vertices.Count - 3);
            triangles.Add(vertices.Count - 2);

            triangles.Add(vertices.Count - 3);
            triangles.Add(vertices.Count - 1);
            triangles.Add(vertices.Count - 2);
          }
        }
      }
    }

    mesh.vertices = vertices.ToArray();
    mesh.triangles = triangles.ToArray();

    mesh.RecalculateNormals();

    GameObject edgeObject = new("Edge");
    edgeObject.transform.SetParent(transform);

    MeshFilter meshFilter = edgeObject.AddComponent<MeshFilter>();
    meshFilter.mesh = mesh;

    MeshRenderer meshRenderer = edgeObject.AddComponent<MeshRenderer>();
    meshRenderer.material = EdgeMaterial;
  }

  void GenerateDecoration()
  {
    for (int i = 0; i < Size; i++)
    {
      for (int j = 0; j < Size; j++)
      {
        if (worldGrid[i, j] == null || worldGrid[i, j].Type == WorldCell.CellType.Empty)
        {
          continue;
        }

        float densityValue = Random.Range(0f, 1f);
        if (densityValue > 0.01f)
        {
          continue;
        }

        GameObject prefab = DecorationPrefabs[Random.Range(0, DecorationPrefabs.Length)];
        GameObject tree = Instantiate(prefab, transform);

        tree.transform.SetPositionAndRotation(new Vector3(i, 0, j), Quaternion.Euler(0, Random.Range(0, 360), 0));
        tree.transform.localScale = Vector3.one * Random.Range(0.8f, 1.2f);
      }
    }
  }
}

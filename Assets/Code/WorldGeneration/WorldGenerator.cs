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

  public int Width = 7;
  public int Height = 7;

  private Room[,] worldGrid;

  private int leftRooms;
  public int MinRooms = 7;
  public int MaxRooms = 12;
  public int RoomSize = 10;

  private GameObject roomPrefab;
  private GameObject doorPrefab;
  private GameObject runtimeFolder;
  private GameObject player;

  private void Start()
  {
    roomPrefab = PrefabUtility.LoadPrefabContents("Assets/Level/Prefabs/WorldGeneration/Room.prefab");
    doorPrefab = PrefabUtility.LoadPrefabContents("Assets/Level/Prefabs/WorldGeneration/Door.prefab");
    runtimeFolder = new GameObject("Runtime");
    player = GameObject.Find("Player");

    GenerateSeed();
    GenerateWorld();
  }

  private void Update()
  {

  }

  private void GenerateWorld()
  {
    GenerateGrid();
    GenerateRooms();
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
    worldGrid = new Room[Width, Height];

    int x = Width / 2;
    int y = Height / 2;

    worldGrid[x, y] = new Room(Room.RoomType.Start, x, y, RoomSize, RoomSize);
    leftRooms = Random.Range(MinRooms, MaxRooms + 1) - 1;

    PopulateGrid(x, y);
  }

  private void PopulateGrid(int x, int y)
  {
    if (leftRooms == 0)
    {
      return;
    }

    // Create a room in a random direction from the current room
    int direction = Random.Range(0, 4);
    switch (direction)
    {
      case 0:
        x++;
        break;
      case 1:
        x--;
        break;
      case 2:
        y++;
        break;
      case 3:
        y--;
        break;
    }

    // If the room is out of bounds, try again
    if (x < 0 || x >= Width || y < 0 || y >= Height)
    {
      PopulateGrid(x, y);
      return;
    }

    // If the room is already occupied, repeat with that room
    if (worldGrid[x, y] != null)
    {
      PopulateGrid(x, y);
      return;
    }

    // If the room is valid, create it and decrement the number of rooms left to create
    worldGrid[x, y] = new Room(Room.RoomType.Normal, x, y, RoomSize, RoomSize);
    leftRooms--;

    // If the room is valid, create a door in the current room and the new room
    switch (direction)
    {
      case 0:
        worldGrid[x, y].SetDoor(1, new Door(Door.DoorType.Normal, x, y));
        break;
      case 1:
        worldGrid[x, y].SetDoor(0, new Door(Door.DoorType.Normal, x, y));
        break;
      case 2:
        worldGrid[x, y].SetDoor(3, new Door(Door.DoorType.Normal, x, y));
        break;
      case 3:
        worldGrid[x, y].SetDoor(2, new Door(Door.DoorType.Normal, x, y));
        break;
    }

    // If the room is valid, call PopulateGrid() again
    PopulateGrid(x, y);
  }

  private void GenerateRooms()
  {
    for (int x = 0; x < Width - 1; x++)
    {
      for (int y = 0; y < Height - 1; y++)
      {
        if (worldGrid[x, y] != null)
        {
          GenerateRoom(worldGrid[x, y]);
        }
      }
    }
  }

  private void GenerateRoom(Room room)
  {
    // Generate the room based on the type
    // Generate the doors
    // If the room is a start room, generate a player
    // If the room is a boss room, generate a boss
    // If the room is a normal room, generate enemies

    // Generate the room based on the type
    switch (room.Type)
    {
      case Room.RoomType.Start:
        // Generate the start room
        GenerateStartRoom(room);
        break;
      case Room.RoomType.Boss:
        // Generate the boss room
        Instantiate(roomPrefab, new Vector3(room.IndexX * room.Width, 0, room.IndexY * room.Height), Quaternion.identity, runtimeFolder.transform);
        break;
      case Room.RoomType.Normal:
        // Generate the normal room
        Instantiate(roomPrefab, new Vector3(room.IndexX * room.Width, 0, room.IndexY * room.Height), Quaternion.identity, runtimeFolder.transform);
        break;
    }

    // // Generate the doors
    // for (int i = 0; i < room.doors.Length; i++)
    // {
    //   if (room.doors[i] != null)
    //   {
    //     room.doors[i].GenerateDoor();
    //   }
    // }
  }

  private void GenerateStartRoom(Room room)
  {
    Vector3 roomPos = new Vector3(room.IndexX * room.Width, 0, room.IndexY * room.Height);

    GameObject startRoom = Instantiate(roomPrefab, roomPos, Quaternion.identity, runtimeFolder.transform);
    room.SetRoomPrefab(startRoom);

    // Spawn the player on the middle of the room terrain
    player.transform.position = new Vector3(roomPos.x + room.Width / 2, 1, roomPos.z + room.Height / 2);
  }
}

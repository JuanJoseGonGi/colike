using UnityEngine;

public class Room
{
  public enum RoomType
  {
    Start,
    Boss,
    Normal
  }

  public RoomType Type;
  public int IndexX;
  public int IndexY;
  public int Width;
  public int Height;

  public Door[] Doors;

  private GameObject roomPrefab;

  public Room(RoomType type, int indexX, int indexY, int width, int height)
  {
    this.Type = type;
    this.IndexX = indexX;
    this.IndexY = indexY;
    this.Width = width;
    this.Height = height;

    Doors = new Door[4];
  }

  public void SetDoor(int index, Door door)
  {
    Doors[index] = door;
  }

  public void SetRoomPrefab(GameObject roomPrefab)
  {
    this.roomPrefab = roomPrefab;
  }
}

using System.Collections;
using System.Collections.Generic;
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
  public int X;
  public int Y;
  public Vector3 Position;
  public Vector3 Scale;

  public Door[] Doors;

  private GameObject roomPrefab;
  private GameObject floor;
  private GameObject walls;

  public Room(RoomType type, int x, int y, Vector3 scale)
  {
    Type = type;
    X = x;
    Y = y;
    Position = Vector3.zero;
    Scale = scale;

    Doors = new Door[4];
  }

  public void SetDoor(int index, Door door)
  {
    Doors[index] = door;
  }

  public void SetRoomPrefab(GameObject roomPrefab)
  {
    this.roomPrefab = roomPrefab;
    this.roomPrefab.name = "Room " + X + ", " + Y;

    this.walls = this.roomPrefab.transform.GetChild(0).gameObject;
    this.walls.GetComponent<RoomWalls>().SetSize((int)Scale.x, (int)Scale.z);

    this.floor = this.roomPrefab.transform.GetChild(1).gameObject;
    this.floor.transform.localScale = new Vector3(Scale.x, 0.1f, Scale.z);
    this.floor.transform.localPosition = new Vector3(Scale.x / 2f, 0, Scale.z / 2f);
  }
}

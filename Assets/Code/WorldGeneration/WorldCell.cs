public class WorldCell
{
  public enum CellType
  {
    Empty,
    StartRoomFloor,
    NormalRoomFloor,
    BossRoomFloor,
    Door,
    Wall
  }

  public CellType Type;

  public WorldCell(CellType type)
  {
    Type = type;
  }
}
public class Door
{
  public enum DoorType
  {
    None,
    Normal,
    Locked,
    Boss
  }

  private DoorType type;

  public Door(DoorType type, int roomIndex, int doorIndex)
  {
    this.type = type;
  }
}
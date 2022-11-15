public class JumpItem : AbstractItem
{
  public float JumpHeight;
  public float JumpPerStack;

  public JumpItem(float jumpHeight, float jumpPerStack)
  {
    JumpHeight = jumpHeight;
    JumpPerStack = jumpPerStack;
  }

  public override string GiveName()
  {
    return "JumpItem";
  }
}
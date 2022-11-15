public class HealingItem : AbstractItem
{
  public int HealAmount;
  public int HealPerStack;

  public HealingItem(int healAmount, int healPerStack)
  {
    HealAmount = healAmount;
    HealPerStack = healPerStack;
  }

  public override void Update(Player player, int stacks)
  {
    player.Heal(HealAmount + HealPerStack * (stacks - 1));
  }

  public override string GiveName()
  {
    return "HealingItem";
  }
}
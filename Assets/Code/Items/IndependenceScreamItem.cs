using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndependenceScreamItem : AbstractItem
{
  public int HealAmount;
  public int HealPerStack;

  public override void ApplyEffect(Player player, int stack)
  {
    player.Heal(HealAmount + HealPerStack * (stack - 1));
  }
}
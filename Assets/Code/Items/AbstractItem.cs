using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class AbstractItem
{
  public abstract string GiveName();

  public virtual void Update(Player player, int stacks)
  {

  }
}
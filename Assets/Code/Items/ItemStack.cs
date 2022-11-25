using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStack
{
  public AbstractItem Item;
  public string Name;
  public int Stack;

  public ItemStack(AbstractItem item, string name, int stack)
  {
    Item = item;
    Name = name;
    Stack = stack;
  }
}
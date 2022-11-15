using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  public int Health = 100;
  public int MaxHealth = 100;

  private readonly List<ItemStack> inventory = new();

  public HealthBar HealthBar;

  void Start()
  {
    HealthBar.UpdateHealthBar(MaxHealth, Health);

    HealingItem healingItem = new(5, 2);
    ItemStack healingItemStack = new(healingItem, healingItem.GiveName(), 1);
    inventory.Add(healingItemStack);

    JumpItem jumpItem = new(8, 2);
    ItemStack jumpItemStack = new(jumpItem, jumpItem.GiveName(), 1);
    inventory.Add(jumpItemStack);

    StartCoroutine(CallItemUpdate());
  }

  IEnumerator CallItemUpdate()
  {
    foreach (ItemStack itemStack in inventory)
    {
      itemStack.Item.Update(this, itemStack.Stack);
    }

    yield return new WaitForSeconds(1f);
    StartCoroutine(CallItemUpdate());
  }

  void Update()
  {
  }

  public void Heal(int amount)
  {
    if (Health + amount > 100)
    {
      Health = 100;
      return;
    }

    Health += amount;

    HealthBar.UpdateHealthBar(MaxHealth, Health);
  }

  public float GetJumpSpeed()
  {
    foreach (ItemStack itemStack in inventory)
    {
      if (itemStack.Name == "JumpItem")
      {
        JumpItem jumpItem = (JumpItem)itemStack.Item;
        return jumpItem.JumpHeight + jumpItem.JumpPerStack * (itemStack.Stack - 1);
      }
    }

    return 0;
  }
}
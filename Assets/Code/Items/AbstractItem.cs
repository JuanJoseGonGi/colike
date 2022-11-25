using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class AbstractItem : MonoBehaviour
{
  public virtual void ApplyEffect(Player player, int stack) { }

  public virtual void RemoveEffect(Player player, int stack) { }
}
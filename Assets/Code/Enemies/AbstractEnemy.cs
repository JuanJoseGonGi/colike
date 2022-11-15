using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Enemy : MonoBehaviour
{
  [Header("Health")]
  public int Health;
  public int MaxHealth;
  public HealthBar HealthBar;

  public virtual void TakeDamage(int damage)
  {
    Health -= damage;
    HealthBar.UpdateHealthBar(MaxHealth, Health);

    AnimateDamage();

    if (Health <= 0)
    {
      Die();
    }
  }

  public virtual void Start()
  {
    Health = MaxHealth;
    HealthBar.UpdateHealthBar(MaxHealth, Health);
  }

  public abstract void Die();

  public abstract void AnimateDamage();

  public abstract void Update();
}
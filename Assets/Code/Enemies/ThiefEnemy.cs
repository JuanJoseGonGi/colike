using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefEnemy : Enemy
{
  public Animator Animator;

  public override void Start()
  {
    MaxHealth = 100;
    Health = MaxHealth;

    base.Start();
  }

  public override void Die()
  {
    Animator.SetBool("IsDead", true);

    this.enabled = false;
    GetComponent<BoxCollider>().enabled = false;
    HealthBar.gameObject.SetActive(false);
  }

  public override void AnimateDamage()
  {
    Animator.SetTrigger("TakeDamage");
  }

  public override void Update()
  { }
}
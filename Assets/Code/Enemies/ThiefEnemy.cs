using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefEnemy : Enemy
{
  public Animator Animator;

  public override void Start()
  {
    base.Start();
  }

  public override void Die()
  {
    Animator.SetBool("IsDead", true);

    this.enabled = false;
    GetComponent<BoxCollider>().enabled = false;
    HealthBar.gameObject.SetActive(false);
    agent.enabled = false;
  }

  public override void Attack()
  {
    if (AttackTimer > 0)
    {
      AttackTimer -= Time.deltaTime;
      return;
    }

    AttackTimer = AttackCooldown;

    Animator.SetTrigger("Attack");
    player.GetComponent<Player>().TakeDamage(AttackDamage);
  }

  public override void AnimateDamage()
  {
    Animator.SetTrigger("TakeDamage");
  }

  public override void AnimateMovement()
  {
    Animator.SetFloat("Speed", agent.velocity.magnitude);
  }

  public override void Update()
  {
    base.Patrol();
  }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericEnemy : Enemy
{
  public Animator Animator;

  public override void Start()
  {
    PatrolCooldown = Random.Range(1f, 10f);
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
    if (attackTimer > 0)
    {
      attackTimer -= Time.deltaTime;
      return;
    }

    attackTimer = AttackCooldown;

    Animator.SetTrigger("Attack");
    player.GetComponent<Player>().TakeDamage(AttackDamage);
  }

  public override void AnimateDamage()
  {
    Animator.SetTrigger("TakeDamage");
  }

  void HandleRotation()
  {
    if (agent.velocity == Vector3.zero)
    {
      return;
    }

    float angle = Mathf.Atan2(agent.velocity.x, agent.velocity.z) * Mathf.Rad2Deg;

    bool isMovingLeft = angle < 0;
    bool isMovingRight = angle > 0;

    if (isMovingLeft)
    {
      Prefab.transform.localScale = new(1, 1, 1);
      return;
    }

    if (isMovingRight)
    {
      Prefab.transform.localScale = new(-1, 1, 1);
    }
  }

  public override void Update()
  {
    base.Patrol();
    Animator.SetFloat("Speed", agent.velocity.magnitude);
    HandleRotation();
  }
}
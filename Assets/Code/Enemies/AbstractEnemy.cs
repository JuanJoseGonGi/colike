using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public abstract class Enemy : MonoBehaviour
{
  [Header("Health")]
  public int Health;
  public int MaxHealth;
  public HealthBar HealthBar;

  [Header("AI")]
  public float AttackRange;
  public float AttackCooldown;
  public int AttackDamage;
  public float AttackTimer;
  public float HuntRange;
  protected GameObject player;
  protected NavMeshAgent agent;

  public virtual void Start()
  {
    Health = MaxHealth;
    HealthBar.UpdateHealthBar(MaxHealth, Health);

    player = GameObject.FindGameObjectWithTag("Player");

    agent = GetComponent<NavMeshAgent>();
    agent.stoppingDistance = AttackRange;
  }

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

  public virtual void Hunt()
  {
    if (Vector3.Distance(transform.position, player.transform.position) <= AttackRange)
    {
      Attack();
      return;
    }

    agent.SetDestination(player.transform.position);

    if (Vector3.Distance(transform.position, player.transform.position) > HuntRange)
    {
      Patrol();
      return;
    }

    AnimateMovement();
  }

  public virtual Vector3 RandomNavmeshLocation(float radius)
  {
    Vector3 randomDirection = Random.insideUnitSphere * radius;
    randomDirection += transform.position;

    if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, 1))
    {
      return hit.position;
    }

    return Vector3.zero;
  }

  public virtual void Patrol()
  {
    if (IsDead())
    {
      return;
    }

    if (Vector3.Distance(transform.position, player.transform.position) <= HuntRange)
    {
      Hunt();
      return;
    }

    if (agent.remainingDistance <= agent.stoppingDistance + 0.1f)
    {
      agent.SetDestination(RandomNavmeshLocation(10));
    }

    AnimateMovement();
  }

  public virtual bool IsDead()
  {
    return Health <= 0;
  }

  public abstract void Die();

  public abstract void Update();

  public abstract void Attack();

  public abstract void AnimateDamage();

  public abstract void AnimateMovement();
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  public float Speed = 5f;
  public float Gravity = 20f;

  [Header("Combat")]
  public Transform attackPoint;
  public float attackRange = 0.5f;
  public float attackRate = 2f;
  public LayerMask enemyLayers;
  private float nextAttackTime = 0f;

  [Header("FX")]
  public AudioSource attackFX;

  private float verticalSpeed = 0f;
  private Vector3 input = Vector3.zero;
  private CharacterController controller;
  private Animator animator;
  private GameObject playerPrefab;
  private GameObject cameraPivot;
  private Player player;

  private void Start()
  {
    controller = GetComponent<CharacterController>();
    animator = GetComponentInChildren<Animator>();
    playerPrefab = transform.GetChild(0).gameObject;
    cameraPivot = GameObject.Find("CameraPivot");
    player = GetComponent<Player>();
  }

  void GatherInput()
  {
    Vector3 userInput = new(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

    Matrix4x4 matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    Vector3 rotatedInput = matrix.MultiplyPoint3x4(userInput);

    input = rotatedInput.normalized;
  }

  private void Attack()
  {
    animator.SetTrigger("Attack");
    attackFX.Play();

    Collider[] hitEnemies = new Collider[10];
    Physics.OverlapSphereNonAlloc(attackPoint.position, attackRange, hitEnemies, enemyLayers);

    foreach (Collider enemy in hitEnemies)
    {
      if (enemy == null)
      {
        continue;
      }

      enemy.GetComponent<Enemy>().TakeDamage(10);
    }
  }

  private void HandleAttack()
  {
    if (Time.time < nextAttackTime)
    {
      return;
    }

    if (Input.GetMouseButtonDown(0))
    {
      Attack();
      nextAttackTime = Time.time + 1f / attackRate;
    }
  }

  void HandleRotation()
  {
    playerPrefab.transform.LookAt(cameraPivot.transform.position, -Vector3.up);

    bool isMovingLeft = Input.GetAxisRaw("Horizontal") < 0;
    bool isMovingRight = Input.GetAxisRaw("Horizontal") > 0;

    if (isMovingLeft)
    {
      attackPoint.transform.localPosition = new Vector3(-0.5f, 0, 0.5f);
      playerPrefab.GetComponent<SpriteRenderer>().flipX = true;
      return;
    }

    if (isMovingRight)
    {
      attackPoint.transform.localPosition = new Vector3(0.5f, 0, -0.5f);
      playerPrefab.GetComponent<SpriteRenderer>().flipX = false;
    }
  }

  private void HandleJump()
  {
    float jumpSpeed = player.GetJumpSpeed();

    if (Input.GetButtonDown("Jump") && controller.isGrounded && jumpSpeed != 0)
    {
      verticalSpeed = jumpSpeed;
    }

    verticalSpeed -= Gravity * Time.deltaTime;
  }

  // Update is called once per frame
  void Update()
  {
    GatherInput();
    HandleAttack();
    HandleJump();
    HandleRotation();
  }

  private void HandleMovement()
  {
    Vector3 movement = input * Speed;
    movement.y = verticalSpeed;

    controller.Move(Time.deltaTime * movement);

    animator.SetFloat("Speed", controller.velocity.magnitude);
  }

  private void FixedUpdate()
  {
    HandleMovement();
  }
}

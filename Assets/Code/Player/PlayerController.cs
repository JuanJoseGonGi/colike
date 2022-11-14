using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  public float Speed = 5f;
  public float JumpSpeed = 8f;
  public float Gravity = 9.8f;
  private float verticalSpeed = 0f;
  private bool isAttacking = false;
  private Vector3 input = Vector3.zero;
  private CharacterController controller;
  private Animator animator;
  private GameObject playerPrefab;
  private GameObject cameraPivot;

  private void Start()
  {
    controller = GetComponent<CharacterController>();
    animator = GetComponentInChildren<Animator>();
    playerPrefab = transform.GetChild(0).gameObject;
    cameraPivot = GameObject.Find("CameraPivot");
  }

  void GatherInput()
  {
    Vector3 userInput = new(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

    Matrix4x4 matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    Vector3 rotatedInput = matrix.MultiplyPoint3x4(userInput);

    input = rotatedInput.normalized;
  }

  private void HandleAttack()
  {
    if (Input.GetMouseButtonDown(0) && !isAttacking)
    {
      isAttacking = true;
      return;
    }

    isAttacking = false;
  }

  void HandleRotation()
  {
    playerPrefab.transform.LookAt(cameraPivot.transform.position, -Vector3.up);

    bool isMovingLeft = Input.GetAxisRaw("Horizontal") < 0;
    bool isMovingRight = Input.GetAxisRaw("Horizontal") > 0;

    if (isMovingLeft)
    {
      playerPrefab.GetComponent<SpriteRenderer>().flipX = true;
      return;
    }

    if (isMovingRight)
    {
      playerPrefab.GetComponent<SpriteRenderer>().flipX = false;
    }
  }

  void HandleAnimation()
  {
    animator.SetFloat("Speed", controller.velocity.magnitude);
    animator.SetBool("isAttacking", isAttacking);
  }

  private void HandleJump()
  {
    if (Input.GetButtonDown("Jump") && controller.isGrounded)
    {
      verticalSpeed = JumpSpeed;
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
    HandleAnimation();
  }

  private void HandleMovement()
  {
    Vector3 movement = input * Speed;
    movement.y = verticalSpeed;

    controller.Move(Time.deltaTime * movement);
  }

  private void FixedUpdate()
  {
    HandleMovement();
  }
}

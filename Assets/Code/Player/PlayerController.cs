using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  public float Speed = 5f;
  public float JumpForce = 5f;
  public float Gravity = 9.81f;
  public float GroundDistance = 0.4f;
  public LayerMask GroundMask;

  private CharacterController controller;
  private Vector3 velocity;
  private bool isGrounded;

  private Animator animator;

  private void Start()
  {
    controller = GetComponent<CharacterController>();
  }

  // Update is called once per frame
  void Update()
  {
    FixedUpdate();
  }

  private void FixedUpdate()
  {
    isGrounded = Physics.CheckSphere(transform.position, GroundDistance, GroundMask);

    if (isGrounded && velocity.y < 0)
    {
      velocity.y = -2f;
    }

    float x = Input.GetAxis("Horizontal");
    float z = Input.GetAxis("Vertical");

    Vector3 move = transform.right * x + transform.forward * z;

    controller.Move(move * Speed * Time.deltaTime);

    if (Input.GetButtonDown("Jump") && isGrounded)
    {
      velocity.y = Mathf.Sqrt(JumpForce * -2f * Gravity);
    }

    velocity.y += Gravity * Time.deltaTime;

    controller.Move(velocity * Time.deltaTime);

    animator.SetFloat("Speed", Mathf.Abs(x) + Mathf.Abs(z));
  }
}

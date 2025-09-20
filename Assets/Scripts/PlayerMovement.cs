using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] CharacterController2D controller;
    [SerializeField] Animator animator;

    [SerializeField] Rigidbody2D RB;

    [SerializeField] float runSpeed = 40f;

    float HorizontalMove = 0f;
    bool jumpTrigger = false;
    bool crouchTrigger = false;
    bool isGoingUp = false;
    bool IsGrounded = true;

    InputAction moveAction;
    InputAction jumpAction;
    InputAction crouchAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        crouchAction = InputSystem.actions.FindAction("Crouch");
    }

    // Update is called once per frame
    void Update()
    {
        HorizontalMove = moveAction.ReadValue<Vector2>().x;
        jumpTrigger = jumpAction.IsPressed();
        crouchTrigger = crouchAction.IsPressed();

        animator.SetFloat("Speed", Math.Abs(HorizontalMove));
        animator.SetBool("IsJumping", false);

        if (isGoingUp)
        {
            JumpToFall();
        }

        if (jumpTrigger)
        {
            animator.SetBool("IsJumping", true);
            IsGrounded = false;
            isGoingUp = true;
        }

        animator.SetBool("IsCrouchBuffering", crouchTrigger);

        if (!IsGrounded)
            {
                crouchTrigger = false;
            }
    }

    void FixedUpdate()
    {
        controller.Move(HorizontalMove * Time.fixedDeltaTime * runSpeed, crouchTrigger, jumpTrigger);
    }

    void JumpToFall()
    {
        if (RB.linearVelocity.y < 0.1f)
        {
            animator.SetBool("IsFalling", true);
            isGoingUp = false;
        }
    }

    public void OnLanding()
    {
        animator.SetBool("IsFalling", false);
        IsGrounded = true;
    }

    public void OnCrouching(bool isCrouching)
    {
        animator.SetBool("IsCrouching", isCrouching);
    }
}

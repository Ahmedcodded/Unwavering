using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;

    public float runSpeed = 40f;

    float HorizontalMove = 0f;
    bool jumpTrigger = false;
    bool crouchTrigger = false;

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
        if (jumpTrigger)
        {
            animator.SetBool("IsJumping", true);
        }
    }

    void FixedUpdate()
    {
        controller.Move(HorizontalMove * Time.fixedDeltaTime * runSpeed, crouchTrigger, jumpTrigger);
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }
    
    public void OnCrouching(bool isCrouching)
    {
        animator.SetBool("IsCrouching", isCrouching);
    }
}

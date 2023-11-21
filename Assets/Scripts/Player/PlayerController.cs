using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header ("Game Objects")]
    public Canvas gameOver;
    public AudioSource music;
    public TouchingDirections touching;
    private CharacterController controller;
    private Rigidbody rb;
    private Animator animator;
    private Damageable damageable;

    [Header ("Audio")]
    public AudioSource audioSource;

    [Header ("Movement")]
    Vector2 moveInput;
    private Vector3 velocity;
    public float walkSpeed = 7.5f;
    public float sprintSpeed = 12f;
    public float airWalkSpeed = 4f;
    public bool isRunning;

    [Header ("Jumping")]
    public float jumpImpulse = 7.5f;
    public float fallGravityScale = 7f;
    private int jumpCount;

    public float CurrentMoveSpeed
    {
        get
        {
            if (CanMove)
            {
                if(IsMoving && !touching.IsOnWall)
                {
                    if (touching.IsGrounded)
                    {
                        if (isRunning)
                        {
                            return sprintSpeed;
                        }
                        else
                        {
                            return walkSpeed;
                        }
                    }
                    else
                    {
                        return airWalkSpeed;
                    }
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
    }


    private bool isMoving;
    public bool IsMoving
    {
        get
        {
            return isMoving;
        }

        private set
        {
            isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }
    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }


    private bool isFacingRight = true;
    public bool IsFacingRight
    {
        get
        {
            return isFacingRight;
        }
        private set
        {
            if (isFacingRight != value)
            {
                Vector3 currentScale = transform.localScale;
                currentScale.x *= -1;  // Flip horizontally
                transform.localScale = currentScale;
            }

            isFacingRight = value;
        }
    }

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        touching = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        //Update Animator params
        animator.SetFloat(AnimationStrings.yVelocity, controller.velocity.y);
        animator.SetBool(AnimationStrings.isGrounded, touching.IsGrounded);

        //Ground checks and gravity application
        if (touching.IsGrounded && velocity.y < 0)
        {
            velocity.y = -4f;
        }
        else if (!touching.IsGrounded)
        {
            velocity.y = Mathf.Lerp(velocity.y, -fallGravityScale, 2f * Time.deltaTime);
        }
        if (touching.IsOnWall && controller.velocity.y < 0)
        {
            velocity.y = -1f;
        }

        //Death check
        if (!damageable.IsAlive)
        {
            Time.timeScale = 0f;
            gameOver.gameObject.SetActive(true);
            music.Stop();
        }

        //Movement
        Vector3 moveDirection = new(moveInput.x, 0f, 0f); ;
        if (CanMove) controller.Move(CurrentMoveSpeed * Time.deltaTime * moveDirection);

        controller.Move(velocity * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        //Hit check
        if (damageable.IsHit)
        {
            animator.SetTrigger(AnimationStrings.isHit);
        }
    }

    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Jump reset
        jumpCount = 0;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        //Input handling; moveIpnput is 1 if pressing right, -1 if pressing left.
        moveInput = context.ReadValue<Vector2>();
        //IsMoving depends on the moveInput not being 0.
        isMoving = moveInput != Vector2.zero;

        SetFacingDirection(moveInput);
    }

    //What the name says
    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //Double jump allowed thanks to jumpCount < 2
        if (context.started && jumpCount < 1)
        {
            velocity.y = Mathf.Sqrt(-2f * jumpImpulse * -9.81f);
            animator.SetTrigger(AnimationStrings.jump);

            Vector2 jumpDir = Vector3.up;
            Vector2 jumpForce = jumpDir * jumpImpulse;

            rb.AddForce(jumpForce, ForceMode.Impulse);
            rb.velocity = new Vector3(controller.velocity.x, velocity.y, 0);

            jumpCount++;
        }
        else if (context.started && jumpCount < 2)
        {
            velocity.y = Mathf.Sqrt(-2f * jumpImpulse * -9.81f);
            animator.SetTrigger(AnimationStrings.jump2);

            Vector2 jumpDir = Vector3.up;
            Vector2 jumpForce = jumpDir * jumpImpulse;

            rb.AddForce(jumpForce, ForceMode.Impulse);
            rb.velocity = new Vector3(controller.velocity.x, velocity.y, 0);

            jumpCount++;

        }
    }

    //Sprint button
    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isRunning = true;
        }
        else if (context.canceled)
        {
            isRunning = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header ("Game Objects")]
    public TouchingDirections touching;
    public Canvas pauseCanvas;
    private CharacterController controller;
    private Rigidbody rb;
    private Animator animator;
    private Damageable damageable;

    [Header ("Audio")]
    public AudioSource audioSource;
    public AudioClip walk1;
    public AudioClip walk2;

    [Header ("Movement")]
    public Vector2 moveInput;
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

    private void Start()
    {
        Time.timeScale = 1f;
        pauseCanvas.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseCanvas.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }

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
        if(controller.collisionFlags == CollisionFlags.Below)
            jumpCount = 0;
        //Jump reset
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (DialogueManager.GetInstance() != null)
        {
            if (DialogueManager.GetInstance().DialogueIsPlaying)
            {
                animator.SetBool(AnimationStrings.canMove, false);
            }
        }        

        if (context.canceled)
        {
            moveInput = Vector2.zero;
            IsMoving = false;
        }

        if (Time.timeScale > 0 && CanMove && context.started) 
        {
            //Input handling; moveInput is 1 if pressing right, -1 if pressing left.
            moveInput = context.ReadValue<Vector2>();
            //IsMoving depends on the moveInput not being 0.
            IsMoving = moveInput.x != 0f;

            SetFacingDirection(moveInput); 
        }
        
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
        if (DialogueManager.GetInstance() != null)
        {
            if (DialogueManager.GetInstance().DialogueIsPlaying)
            {
                return;
            }
        }

        //Double jump allowed thanks to jumpCount < 2
        if (context.started && jumpCount < 1 && CanMove)
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

    public void Trigger(string triggerName)
    {
        if (triggerName.Equals("trigger2"))
        {
            animator.SetTrigger(triggerName);
            GameObject.FindAnyObjectByType<TowerRise>().animator.SetTrigger(triggerName);
        }
        animator.SetTrigger(triggerName);
    }

    public void TimeStop()
    {
        Time.timeScale = Mathf.Lerp(1, 0f, 2);
    }

    public void SwingSound()
    {
        audioSource.clip = GetComponentInChildren<Attack>().attack;
        audioSource.Play();
    }
    public void AltSwingSound()
    {
        audioSource.clip = GetComponentInChildren<Attack>().altAttack;
        audioSource.Play();
    }

    public void SlashSound()
    {
        audioSource.clip = GetComponentInChildren<Attack>().bigSlash;
        audioSource.Play();
    }

    public void Walk()
    {
        audioSource.PlayOneShot(walk1);
    }
}

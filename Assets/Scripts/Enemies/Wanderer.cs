using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Wanderer : MonoBehaviour
{
    public float moveSpeed = 3f;  // Speed at which the enemy moves
    public float detectionRange = 10f;  // Range within which the enemy can detect the player
    public float distanceX;
    public float attackRange = 2f; // Range within which the animation is triggered
    public float attackCD;
    public float attackimer;

    public bool isMoving;
    public bool canAttack = false;
    public bool isAttacking;
    public bool isFollowingPlayer;  // Indicates whether the enemy is following the player

    public enum WalkableDirection { Right, Left }

    private WalkableDirection walkDirection;
    private Vector2 walkDirectionVector;
    public WalkableDirection WalkDirection
    {
        get { return walkDirection; }
        set
        {
            if (walkDirection != value)
            {
                gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z * -1);

                if (value == WalkableDirection.Right)
                {
                    walkDirectionVector = Vector2.right;
                }
                else if (value == WalkableDirection.Left)
                {
                    walkDirectionVector = Vector2.left;
                }
            }
            walkDirection = value;
        }
    }

    private CharacterController enemyController;
    private Animator animator;
    private Damageable damageable;
    private Transform playerTransform;  // Reference to the player's transform
    private bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

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

    private void Start()
    {
        attackimer = attackCD;
    }

    private void Awake()
    {
        enemyController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();

        // Find the player's transform by tag or other means
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        if (damageable.IsAlive)
        {
            if (isAttacking)
            {
                enemyController.Move(Vector3.zero);
            }

            // Get only the X values of positions
            float myX = transform.position.x;
            float playerX = playerTransform.position.x;

            // Calculate the distance only on the X-axis
            distanceX = Mathf.Abs(myX - playerX);

            if (distanceX <= attackRange && !isAttacking && (transform.position.y - playerTransform.position.y < 2))
            {
                isAttacking = true;
                animator.SetTrigger(AnimationStrings.atk);
            }

            // Check if the player is within the detection range
            else if (playerTransform != null && distanceX <= detectionRange && (transform.position.y - playerTransform.position.y < 1.5) && CanMove)
            {
                // Calculate the direction to the player along the X-axis
                float directionX = Mathf.Sign(playerTransform.position.x - transform.position.x);

                // Set the walking direction based on the X-axis direction
                WalkDirection = directionX > 0f ? WalkableDirection.Right : WalkableDirection.Left;

                // Set the enemy's velocity to follow the player along the X-axis
                enemyController.SimpleMove(new Vector3(moveSpeed * directionX, 0, 0));

                isFollowingPlayer = true;
            }

            // If the player is not within range, continue with the regular wandering behavior
            else if (CanMove && distanceX > detectionRange)
            {
                isFollowingPlayer = false;
            }
        }

        if (enemyController.velocity != Vector3.zero || isFollowingPlayer)
        {
            IsMoving = true;
        }
        else
        {
            IsMoving = false;
        }
    }

    private void Update()
    {
        if (attackimer < attackCD) { attackimer += Time.deltaTime; canAttack = false; }

        if (attackimer >= attackCD) canAttack = true;

        if (damageable.isInvincible) ResetAttack();
    }

    private void FlipDirection()
    {
        if (WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        }
        else if (WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        }
        else
        {
            Debug.LogError("Current walkable direction is invalid.");
        }
    }

    public void ResetAttack()
    {
        isAttacking = false;
        attackimer = 0f;
    }
}

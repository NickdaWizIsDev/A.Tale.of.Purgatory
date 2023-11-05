using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dash : MonoBehaviour
{
    Animator animator;
    Rigidbody rb;
    PlayerController controller;
    Damageable damageable;

    [SerializeField]
    private bool isDashing = false;
    public bool IsDashing
    {
        get
        {
            return isDashing;
        }
        private set
        {
            isDashing = value;
            animator.SetBool(AnimationStrings.isDashing, value);
            damageable.isInvincible = value;
        }
    }

    public float dashDistance = 3f;
    public float dashDuration = 0.25f;
    public float dashSpeed = 35f;
    public float dashCooldown = 1.5f;
    private float dashStartTime;
    private float dashTimer;

    public AudioSource audioSource;
    public AudioClip dash;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        controller = GetComponent<PlayerController>();
        damageable = GetComponent<Damageable>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(dashTimer < 3f)
        {
            dashTimer += Time.deltaTime;
        }

        if(dashTimer >= 3f)
        {
            dashTimer = 0f;
        }
    }

    private void Start()
    {
        dashCooldown = 0f;
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started && !IsDashing && Time.time >= dashStartTime + dashCooldown)
        {
            StartCoroutine(UltraDash(transform.localScale.x));
        }
    }

    private IEnumerator UltraDash(float direction)
    {
        IsDashing = true;
        dashStartTime = Time.time;
        audioSource.PlayOneShot(dash, 0.25f);

        float normalSpeed = controller.walkSpeed;
        controller.walkSpeed = dashSpeed;

        // Determine the dash direction based on the player's facing direction
        Vector2 dashDirection = controller.IsFacingRight ? Vector2.right : Vector2.left;
        Vector2 dashForce = dashDirection * (dashDistance / dashDuration);

        rb.AddForce(dashForce, ForceMode.Impulse);
        rb.velocity = new Vector2(rb.velocity.x, 0);

        yield return new WaitForSeconds(dashDuration);

        controller.walkSpeed = normalSpeed;
        IsDashing = false;
    }
}

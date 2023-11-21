using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Attack : MonoBehaviour
{
    Animator animator;
    Rigidbody rb;
    PlayerController controller;
    Damageable damageable;    

    public AudioSource audioSource;
    public AudioClip attack;

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
    }

    private void Start()
    {
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started && controller.touching.IsGrounded)
        {
            animator.SetTrigger(AnimationStrings.atk);
        }
        else if(context.started && !controller.touching.IsGrounded)
        {
            animator.SetTrigger(AnimationStrings.airAtk);
        }
    }    
}

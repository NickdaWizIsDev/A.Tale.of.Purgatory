using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class Attack : MonoBehaviour
{
    Animator animator;
    PlayerController controller;
    Damageable damageable;    

    public AudioSource audioSource;
    public AudioClip attack;
    public GameObject slash;
    public Transform fireP;
    public int speed;

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
        controller = GetComponentInParent<PlayerController>();
        damageable = GetComponentInParent<Damageable>();
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
        if (context.started && controller.touching.IsGrounded && damageable.IsAlive)
        {
            animator.SetTrigger(AnimationStrings.atk);
        }
        else if(context.started && !controller.touching.IsGrounded)
        {
            animator.SetTrigger(AnimationStrings.airAtk);
        }
    }
}

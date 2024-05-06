using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class Attack : MonoBehaviour
{
    Animator animator;
    PlayerController controller;
    Damageable damageable;
    Damageable enemyDamageable;

    public AudioSource audioSource;
    public AudioClip attack;
    public AudioClip altAttack;
    
    public PlayerMana mana;

    public int damage;

    [Header("Power Slash")]
    public AudioClip bigSlash;
    public GameObject slash;
    public Transform fireP;
    public Vector3 speed;
    public float slashTimer;
    public float slashCD;

    [Header("Thunder Spell")]
    public GameObject lightningBolt;

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
        controller = GetComponentInParent<PlayerController>();
        damageable = GetComponentInParent<Damageable>();
        audioSource = GetComponentInParent<AudioSource>();
    }

    private void Update()
    {
        if(slashTimer < slashCD)
            slashTimer += Time.deltaTime;

        float xScale = controller.transform.localScale.x;
        if(xScale < 0)
        {
            speed.x = -Mathf.Abs(speed.x);
        }
        else if(xScale > 0)
        {
            speed.x = Mathf.Abs(speed.x);
        }
    }

    private void Start()
    {
        slashTimer = slashCD;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (DialogueManager.GetInstance() != null)
        {
            if (DialogueManager.GetInstance().DialogueIsPlaying)
            {
                return;
            }
        }            

        if (context.started && controller.touching.IsGrounded && damageable.IsAlive)
        {
            animator.SetTrigger(AnimationStrings.atk);
        }
        else if(context.started && !controller.touching.IsGrounded)
        {
            animator.SetTrigger(AnimationStrings.airAtk);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        enemyDamageable = other.gameObject.GetComponentInParent<Damageable>();
        if (enemyDamageable != null)
        {
            enemyDamageable.Hit(damage);            
        }
    }
    
    public void OnPowerSlash(InputAction.CallbackContext context)
    {
        if (DialogueManager.GetInstance() != null)
        {
            if (DialogueManager.GetInstance().DialogueIsPlaying)
            {
                return;
            }
        }

        if (slashTimer >= slashCD && context.started && mana.Mana > 10)
        {
            Debug.Log("Slash!");
            mana.Mana -= 10;
            slashTimer = 0;
            GameObject powerSlash = Instantiate(slash, fireP);

            audioSource.clip = GetComponentInChildren<Attack>().bigSlash;
            audioSource.Play();

            Rigidbody rb = powerSlash.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.AddForce(speed, ForceMode.Impulse);
            powerSlash.transform.parent = null;

            Destroy(powerSlash, 1.3f);
        }
    }

    public void OnThunderSpell(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GameObject enemy = FindAnyObjectByType<Wanderer>().gameObject;
            Vector3 targetPos = new Vector3(enemy.transform.position.x, (enemy.transform.position.y + 5), 0);
            if(enemy != null)
            {
                GameObject bolt = Instantiate(lightningBolt, targetPos, Quaternion.identity);
                bolt.GetComponent<Rigidbody>().velocity = new Vector3(0, -20, 0);
                Destroy(bolt, 0.7f);
            }
        }
    }

    public void OnFireSpell(InputAction.CallbackContext context)
    {
        animator.SetTrigger(AnimationStrings.e);
    }

    public void OnUltimate(InputAction.CallbackContext context)
    {
        animator.SetTrigger(AnimationStrings.r);
    }
}

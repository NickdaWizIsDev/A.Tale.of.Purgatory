using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    public Image slashUI;

    [Header("Fire Spell")]
    public AudioClip fireSpell;
    public GameObject fireBall;
    public Transform spellCastPos;
    public Vector3 fireBallSpeed;
    public float fireTimer;
    public float fireCD;
    public Image fireUI;

    [Header("Thunder Spell")]
    public AudioClip thunderSpell;
    public GameObject lightningBolt;
    public float thunderTimer;
    public float thunderCD;
    public Image thunderUI;

    [Header("Ultimate")]
    public GameObject ulti;
    public float manaCost;
    public float ultiTimer;
    public float ultiCD;
    public Image ultiUI;

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
        controller = GetComponentInParent<PlayerController>();
        damageable = GetComponentInParent<Damageable>();
        audioSource = GetComponentInParent<AudioSource>();
    }

    private void Update()
    {
        slashUI.fillAmount = (slashTimer / slashCD);
        fireUI.fillAmount = (fireTimer / fireCD);
        thunderUI.fillAmount = (thunderTimer / thunderCD);
        ultiUI.fillAmount =(ultiTimer / ultiCD);

        if (slashTimer > 0)
            slashTimer -= Time.deltaTime;

        if (thunderTimer > 0)
            thunderTimer -= Time.deltaTime;

        if (fireTimer > 0)
            fireTimer -= Time.deltaTime;

        if (ultiTimer > 0)
            ultiTimer -= Time.deltaTime;

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
        slashTimer = 0;
        thunderTimer = 0;
        fireTimer = 0;
        ultiTimer = 0;
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
            mana.Mana += 10;
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

        if (slashTimer <= 0 && context.started && mana.Mana > 10)
        {
            Debug.Log("Slash!");
            mana.Mana -= 10;
            slashTimer = slashCD;
            GameObject powerSlash = Instantiate(slash, fireP);            
            audioSource.PlayOneShot(bigSlash);

            Rigidbody rb = powerSlash.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.AddForce(speed, ForceMode.Impulse);
            powerSlash.transform.parent = null;

            Destroy(powerSlash, 1.3f);
        }
    }

    public void OnThunderSpell(InputAction.CallbackContext context)
    {
        if (context.started && mana.Mana > 20 && thunderTimer <= 0)
        {
            Debug.Log("Thunder!");
            mana.Mana -= 15;
            thunderTimer = thunderCD;

            GameObject enemy = FindAnyObjectByType<Wanderer>().gameObject;
            Vector3 targetPos = new Vector3(enemy.transform.position.x, enemy.transform.position.y, 0);
            if(enemy != null)
            {
                GameObject bolt = Instantiate(lightningBolt, targetPos, Quaternion.identity);
                audioSource.PlayOneShot(thunderSpell, 0.5f);
            }
        }
    }

    public void OnFireSpell(InputAction.CallbackContext context)
    {
        if (context.started && mana.Mana > 10 && fireTimer <= 0)
        {
            Debug.Log("Fireball!");
            audioSource.PlayOneShot(fireSpell, 0.5f);
            mana.Mana -= 10;
            fireTimer = fireCD;

            GameObject fireB = Instantiate(fireBall, spellCastPos);

            Rigidbody fireRB = fireB.GetComponent<Rigidbody>();
            fireRB.velocity = Vector3.zero;
            fireRB.AddForce((fireBallSpeed * controller.gameObject.transform.localScale.x), ForceMode.Impulse);
            fireB.transform.parent = null;
            Destroy(fireB, 2f);
        }
    }

    public void OnUltimate(InputAction.CallbackContext context)
    {
        if(context.started && mana.Mana > manaCost && ultiTimer <= 0)
        {
            Debug.Log("ULTI");
            mana.Mana -= manaCost;
            ultiTimer = ultiCD;
            animator.SetTrigger(AnimationStrings.r);
            SpawnOnActivate(ulti, controller.gameObject.transform, 0.5f);

        }
    }

    public void SpawnOnActivate(GameObject gameObject, Transform transform, float lifetime)
    {
        GameObject gameObj = Instantiate(gameObject, transform.position, Quaternion.identity);
        Destroy(gameObj, lifetime);
    }
}

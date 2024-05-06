using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 100f;
    public float MaxHealth
    {
        get
        {
            return maxHealth;
        }
        set
        {
            maxHealth = value;
        }
    }

    [SerializeField]
    private float health = 100f;
    public float Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;

            if (health <= 0f)
            {
                IsAlive = false;
            }
        }
    }

    [SerializeField]
    private bool isAlive = true;
    public bool IsAlive
    {
        get
        {
            return isAlive;
        }
        set
        {
            isAlive = value;
            animator.SetBool(AnimationStrings.isAlive, value);
            Debug.Log(gameObject.name + "'s IsAlive was set to " + value);

            if (!isAlive && gameObject.CompareTag("Projectile"))
            {
                Destroy(gameObject, 0.1f);
            }

            else if (!isAlive && !gameObject.CompareTag("Player"))
            {
                //Restore mana for the player
                PlayerMana mana = FindAnyObjectByType<PlayerMana>();
                mana.Mana += 15;
                Debug.Log("Restored 15 mana");

                // Play death audio clip
                if (deathAudioSource == null)
                {
                    GameObject audioObject = new("Death Audio");

                    AudioSource deathAudioSource = audioObject.AddComponent<AudioSource>();
                    deathAudioSource.outputAudioMixerGroup = audioSource.outputAudioMixerGroup;

                    deathAudioSource.PlayOneShot(deathClip, 0.3f);
                    Destroy(audioObject, deathClip.length);
                }
                Destroy(gameObject, 1f);
            }
        }
    }

    public AudioSource audioSource;
    public AudioSource deathAudioSource;
    public AudioClip dmgClip;
    public AudioClip deathClip;

    public bool isInvincible;
    public bool IsHit
    {
        get
        {
            return animator.GetBool(AnimationStrings.isHit);
        }
        private set
        {
            animator.SetBool(AnimationStrings.isHit, value);

            // Play hit audio clip
            if (audioSource != null && dmgClip != null && IsHit)
            {
                audioSource.PlayOneShot(dmgClip, 0.5f);
            }
        }
    }
    public float iFrames = 0.5f;
    private float timeSinceHit = 0;
    Animator animator;

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
    }

    private void Start()
    {
        Health = maxHealth;
    }

    private void Update()
    {
        if (isInvincible)
        {
            if (timeSinceHit > iFrames)
            {
                isInvincible = false;
                timeSinceHit = 0;
                IsHit = false;
            }
            timeSinceHit += Time.deltaTime;
        }
    }

    public bool Hit(float damage)
    {
        if (IsAlive && !isInvincible)
        {
            Health -= damage;
            isInvincible = true;

            Debug.Log("Dealt " + damage + " damage to " + gameObject.name);

            IsHit = true;

            return true;
        }
        else if(IsAlive && iFrames == 0)
        {
            Health -= damage;

            Debug.Log("Dealt " + damage + " damage to " + gameObject.name);

            return true;
        }
        else
        {
            return false;
        }
    }
}

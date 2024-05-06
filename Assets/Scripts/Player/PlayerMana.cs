using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMana : MonoBehaviour
{
    [SerializeField]
    private float maxMana = 100f;
    public float MaxMana
    {
        get
        {
            return maxMana;
        }
        set
        {
            maxMana = value;
        }
    }

    [SerializeField]
    private float mana = 100f;
    public float Mana
    {
        get
        {
            return mana;
        }
        set
        {
            mana = value;
        }
    }    

    public AudioSource audioSource;
    public AudioClip dmgClip;

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
        Mana = MaxMana;
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

        mana = Mathf.Clamp(mana, -15f, 100f);
    }

    public bool Hit(float damage)
    {
        if (!isInvincible)
        {
            Mana -= damage;
            isInvincible = true;

            Debug.Log("Taken " + damage + " mana as damage");

            IsHit = true;

            return true;
        }
        else
        {
            return false;
        }
    }
}

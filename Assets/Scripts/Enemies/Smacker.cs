using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smacker : MonoBehaviour
{
    public float timer;
    public float attackCD;

    public bool didRight;

    public AudioSource audioSource;
    public AudioClip roar;
    public AudioClip stomp;

    Damageable damageable;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        damageable = GetComponent<Damageable>();
        animator = GetComponent<Animator>();
    }
    private void Awake()
    {
        audioSource.PlayOneShot(roar);
    }

    // Update is called once per frame
    void Update()
    {
        if(timer < attackCD)
        {
            timer += Time.deltaTime;
        }
        else if(timer >= attackCD)
        {
            if (!didRight)
            {
                animator.SetTrigger(AnimationStrings.atk);
                didRight = true;
                timer = 0f;
            }
            else if (didRight)
            {
                animator.SetTrigger(AnimationStrings.trigger1);
                didRight = false;
                timer = 0f;
            }
        }

        if (damageable.isInvincible)
        {
            timer = 0f;
        }
    }

    public void Stomp()
    {
        audioSource.PlayOneShot(stomp);
    }
}

using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seer : MonoBehaviour
{
    [Header("Attack Attributes")]
    public GameObject projectile;
    public Transform firePoint;
    public bool isReady;
    public float projectileForce;
    public float attackCD;
    public float attackTimer;

    Animator animator;

    private void Start()
    {
        attackTimer = 0f;
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if(!isReady)
        {
            attackTimer += Time.deltaTime;
        }
        else if (isReady)
        {
            animator.SetTrigger(AnimationStrings.atk);
            isReady = false;
            attackTimer = 0;
        }
        
        if (attackTimer >= attackCD)
        {
            isReady = true;
        }

        if (gameObject.GetComponent<Damageable>().isInvincible)
        {
            attackTimer = 0f;
        }
    }

    public void Shoot()
    {
        GameObject shot = Instantiate(projectile, firePoint.position, Quaternion.identity);

        Transform player = FindFirstObjectByType<PlayerController>().gameObject.transform;
        Vector3 direction = (player.position - shot.transform.position).normalized;
        shot.GetComponent<Rigidbody>().velocity = direction * projectileForce;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        shot.transform.rotation = Quaternion.Euler(angle, -90f, 0);
        attackTimer = 0f;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDamage : MonoBehaviour
{
    public int damage;

    Damageable enemyDamageable;

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit!");
        enemyDamageable = other.gameObject.GetComponentInParent<Damageable>();
        if (enemyDamageable != null)
        {
            enemyDamageable.Hit(damage);
        }
    }

    public void OnTriggerStay(Collider other)
    {
        Debug.Log("Hit!");
        enemyDamageable = other.gameObject.GetComponentInParent<Damageable>();
        if (enemyDamageable != null)
        {
            enemyDamageable.Hit(damage);
        }
    }
}

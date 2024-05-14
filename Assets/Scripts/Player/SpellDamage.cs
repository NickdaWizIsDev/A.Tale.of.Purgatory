using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDamage : MonoBehaviour
{
    public int damage;
    public bool doRepeatedDamage;
    public bool doExplode;
    public int damageTicks;

    public GameObject explosion;

    Damageable enemyDamageable;

    public void OnTriggerEnter(Collider other)
    {
        if (doExplode)
        {
            GameObject exp = Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
            Destroy(exp, 2f);
        }
        else
        {
            Debug.Log("Hit!");
            enemyDamageable = other.gameObject.GetComponentInParent<Damageable>();
            if (enemyDamageable != null)
            {
                enemyDamageable.Hit(damage);
                damageTicks--;
            }
        }        
    }

    public void OnTriggerStay(Collider other)
    {
        if (!doRepeatedDamage)
            return;

        if (doRepeatedDamage)
        {
            enemyDamageable = other.gameObject.GetComponentInParent<Damageable>();
            if (enemyDamageable != null && damageTicks > 0 && !enemyDamageable.isInvincible)
            {
                enemyDamageable.Hit(damage);
                Debug.Log("Hit!");
                damageTicks--;
            }
        }        
    }

    public void EndSpell()
    {
        Destroy(gameObject);
    }
}

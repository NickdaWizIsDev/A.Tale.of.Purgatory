using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    Damageable damageable;
    PlayerMana mana;
    public int damage;

    public void OnTriggerEnter(Collider other)
    {
        damageable = other.gameObject.GetComponentInParent<Damageable>();
        mana = other.gameObject.GetComponent<PlayerMana>();

        if (damageable.isActiveAndEnabled)
        {
            damageable.Hit(damage);
        }
        else if (!damageable.isActiveAndEnabled)
        {
            mana.Hit(damage);
        }
    }
}

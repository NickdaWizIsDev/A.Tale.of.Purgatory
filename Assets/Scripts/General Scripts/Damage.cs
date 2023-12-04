using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    Damageable damageable;
    public int damage;

    public void OnTriggerEnter(Collider other)
    {
        damageable = other.gameObject.GetComponent<Damageable>();
        if (damageable != null) { damageable.Hit(damage); }
        else if(damageable == null)
        {
            damageable = other.gameObject.GetComponentInParent<Damageable>();
            if(damageable != null) { damageable.Hit(damage); }
        }
    }
}

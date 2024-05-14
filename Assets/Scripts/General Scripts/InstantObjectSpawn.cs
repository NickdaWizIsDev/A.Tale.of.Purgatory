using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantObjectSpawn : MonoBehaviour
{
    public void SpawnOnActivate(GameObject gameObject, Transform transform)
    {
        Instantiate(gameObject, transform.position, Quaternion.identity);
    }
}

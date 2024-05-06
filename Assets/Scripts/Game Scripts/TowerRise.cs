using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TowerRise : MonoBehaviour
{
    public Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
}

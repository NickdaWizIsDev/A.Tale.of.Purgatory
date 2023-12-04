using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TowerRise : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void Rise()
    {
        animator.SetTrigger(AnimationStrings.trigger1);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevel : MonoBehaviour
{
    Transition transition;

    // Start is called before the first frame update
    void Start()
    {
        transition = GetComponentInParent<Transition>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            transition.NextScene();
        }
    }
}

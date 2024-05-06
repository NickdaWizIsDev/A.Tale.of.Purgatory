using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Indicator")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink Files")]
    [SerializeField] private TextAsset inkFile1;
    [SerializeField] private TextAsset inkFile2;
    [SerializeField] private TextAsset inkFile3;
    public int nextFile = 1;

    private bool playerInRange;

    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && !DialogueManager.GetInstance().DialogueIsPlaying)
        {
            visualCue.SetActive(true);

            if (Input.GetMouseButtonDown(1) && nextFile == 1)
            {
                nextFile = 2;
                DialogueManager.GetInstance().EnterDialogueMode(inkFile1);
            }
            else if(Input.GetMouseButtonDown(1) && nextFile == 2)
            {
                nextFile = 3;
                DialogueManager.GetInstance().EnterDialogueMode(inkFile2);
            }
            else if (Input.GetMouseButtonDown(1) && nextFile == 3)
            {
                DialogueManager.GetInstance().EnterDialogueMode(inkFile3);
            }
        }
        else if(!playerInRange) { visualCue.SetActive(false); }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        playerInRange = true;
    }

    private void OnTriggerExit(Collider other) { playerInRange = false; }
}

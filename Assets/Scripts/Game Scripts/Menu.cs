using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    Animator animator;
    private void Start()
    {
        Time.timeScale = 1f;
        animator = GetComponent<Animator>();
    }

    public void LoadSceneOne()
    {
        if (!DataManager.Instance.hasSeenIntro)
            SceneManager.LoadScene(1);
        else if (DataManager.Instance.hasSeenIntro)
            SceneManager.LoadScene(2);
    }

    public void Trigger()
    {
        animator.SetTrigger(AnimationStrings.trigger1);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

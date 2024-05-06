using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public Button restart;
    public GameObject panel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = Mathf.Lerp(0, 1, 2);
            gameObject.SetActive(false);
        }

        if (!DataManager.Instance.hasSeenIntro)
            restart.interactable = false;
        else if (DataManager.Instance.hasSeenIntro)
            restart.interactable = true;
    }

    public void Back()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene("Floor 1");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Trigger(bool inOptions)
    {
        Vector2 startPos = panel.GetComponent<RectTransform>().anchoredPosition;
        if (!inOptions)
        {
            panel.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(startPos, new(-800f, 0f), 1f);
            Debug.Log("Moving Canvases");
        }
        else if (inOptions)
        {
            panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
            Debug.Log("Moving Canvases");
        }
    }
}

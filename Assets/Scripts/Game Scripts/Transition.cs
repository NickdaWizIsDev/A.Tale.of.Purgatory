using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    int sceneNumber;
    public string sceneName;
    public string nextScene;

    // Start is called before the first frame update
    void Start()
    {
        sceneNumber = SceneManager.GetActiveScene().buildIndex;
        
    }

    private void Update()
    {
        sceneName = SceneManager.GetActiveScene().name;
        nextScene = SceneManager.GetSceneByBuildIndex(sceneNumber + 1).name;
    }

    public void NextScene()
    {
        SceneManager.LoadScene(sceneNumber + 1);
    }
}

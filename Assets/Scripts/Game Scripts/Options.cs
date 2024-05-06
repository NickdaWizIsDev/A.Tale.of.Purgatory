using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public Pause pause;

    public AudioMixer audioMixer;
    public float volume;
    public Slider volumeSlider;

    private void Awake()
    {
        SetVolume(DataManager.Instance.volume);
    }


    private void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex > 1)
        {
            DataManager.Instance.hasSeenIntro = true;
        }
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
        DataManager.Instance.volume = volume;
        if (volume <= -40f)
        {
            audioMixer.SetFloat("Volume", -80f);
            DataManager.Instance.volume = -80f;
        }
    }
}

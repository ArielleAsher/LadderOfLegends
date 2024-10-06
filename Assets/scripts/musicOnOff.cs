using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicOnOff : MonoBehaviour
{
    public GameObject musicOffImage;
    private AudioManager audioManager;


    private void Start()
    {
        audioManager = AudioManager.instance;
    }
    public void Update()
    {
        if (AudioManager.instance.isMusicOn)
        {
            musicOffImage.SetActive(false);
        }
        else
        {
            musicOffImage.SetActive(true);
        }

    }
    public void ToggleMusic()
    {
        if (audioManager != null)
        {
            audioManager.ToggleMusic();
            musicOffImage.SetActive(AudioManager.instance.isMusicOn);
        }
    }
}

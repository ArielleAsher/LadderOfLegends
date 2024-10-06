using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class soundManager : MonoBehaviour
{
    public List<Button> clickableButtons;
    public AudioSource clickSound;
    public Button soundToggleButton;
    public Image crossMarkImage;
    public movingSnakes_GameManager gameManager;
    private AudioManager audioManager;
    public bool isSoundEnabled = true;

    void Start()
    {
        audioManager = AudioManager.instance;

        foreach (Button button in clickableButtons)
        {
            AudioSource audioSource = button.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = button.gameObject.AddComponent<AudioSource>();
            }

            button.onClick.AddListener(() => PlayClickSound(audioSource));
        }
        soundToggleButton.onClick.AddListener(ToggleSounds);

        UpdateCrossMarkImage();
    }

    public void Update()
    {
        if (audioManager.isSoundOn)
        {
            isSoundEnabled = true;
            if (crossMarkImage != null)
            {
                crossMarkImage.gameObject.SetActive(!isSoundEnabled);
            }
        }
        else
        {
            isSoundEnabled= false;
            if (crossMarkImage != null)
            {
                crossMarkImage.gameObject.SetActive(!isSoundEnabled);
            }
        }
    }

    void PlayClickSound(AudioSource audioSource)
    {
        if (isSoundEnabled && audioSource != null)
        {
            audioSource.Play();
        }
    }

    void ToggleSounds()
    {
        isSoundEnabled = !isSoundEnabled;
        audioManager.isSoundOn=!audioManager.isSoundOn;
        foreach (Button button in clickableButtons)
        {
            AudioSource audioSource = button.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.mute = !isSoundEnabled;
            }
        }
        UpdateCrossMarkImage();
    }

    void UpdateCrossMarkImage()
    {
        if (AudioManager.instance.isSoundOn)
        {
            crossMarkImage.gameObject.SetActive(!isSoundEnabled);
        }
        else
        {
            crossMarkImage.gameObject.SetActive(!isSoundEnabled);
        }

    }


}

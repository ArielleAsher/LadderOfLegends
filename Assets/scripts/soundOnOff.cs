using UnityEngine;
using UnityEngine.UI;

public class soundOnOff : MonoBehaviour
{
    public GameObject soundOffImage;

    private bool isSoundOn = true;
    private AudioManager audioManager;


    private void Start()
    {
        audioManager = AudioManager.instance;
        soundOffImage.SetActive(!isSoundOn);
    }
    public void Update()
    {
        if (AudioManager.instance.isSoundOn)
        {
            soundOffImage.SetActive(false);
        }
        else
        {
            soundOffImage.SetActive(true);
        }

    }

    public void ToggleSound()
    {
        isSoundOn = !isSoundOn;
        soundOffImage.SetActive(!isSoundOn);
    }
}

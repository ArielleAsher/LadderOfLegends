using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceAudio : MonoBehaviour
{
    AudioSource adio;

    private void Start()
    {
        adio= GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        adio.Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSettingsPopUp : MonoBehaviour
{
    public GameObject panel1;

    public void TogglePanels()
    {
        panel1.SetActive(true);
    }
}

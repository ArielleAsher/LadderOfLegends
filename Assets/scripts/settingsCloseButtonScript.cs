using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class settingsCloseButtonScript : MonoBehaviour
{

    public GameObject panel1;
    public void closeSettings()
    {
        panel1.SetActive(false);
    }
}

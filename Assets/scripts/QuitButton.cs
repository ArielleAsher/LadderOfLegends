using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitButton : MonoBehaviour
{
    public GameObject confirmationDialog;
    public Button quitButton;
    public Button noButton;

    void Start()
    {
        quitButton.onClick.AddListener(OnQuitButtonClicked);
        noButton.onClick.AddListener(OnNoButtonClicked);

        confirmationDialog.SetActive(false); 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            confirmationDialog.SetActive(true);
        }
    }

    void OnQuitButtonClicked()
    {
        confirmationDialog.SetActive(true);
    }

    void OnNoButtonClicked()
    {
        confirmationDialog.SetActive(false); // Hide the confirmation dialog
    }

}
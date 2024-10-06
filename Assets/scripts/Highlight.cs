using UnityEngine;
using System.Collections.Generic;

public class Highlight : MonoBehaviour
{
    public static Highlight Instance;

    public GameObject[] objects;
    public int selectedWaypoint;
    public GameObject gameDice;
    private GameObject player;

    //public GameObject Chess;


    void Awake()
    {
        // Singleton pattern to allow easy access to the Highlight instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        HighlightOff();
        // Assign unique indices and attach click handlers
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].AddComponent<ObjectClickHandler>().objectIndex = i;
        }
    }

    public void HighlightOn(List<int> indices)
    {
        if(indices.Count==0){
            gameDice.SetActive(true);
            return;
        }
        for (int i = 0; i < objects.Length; i++)
        {
            if (indices.Contains(i))
            {
                objects[i].SetActive(true);
            }
        }
    }

    public void HighlightOff()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(false);
        }
    }

    public void ObjectClicked(int index)
{
    selectedWaypoint = index;
    Debug.Log("Selected Waypoint: " + selectedWaypoint);
    HighlightOff();
    
    // Assuming GameControl is a singleton or can be found via FindObjectOfType
    GameControl gameControlInstance = GameControl.Instance; // Adjust this line based on your implementation
    
    if (gameControlInstance != null)
    {
        int currentPlayer = Chess.Instance.currentPlayer; // Assuming Chess.Instance is also accessible
        gameControlInstance.moveToWaypoint(currentPlayer, selectedWaypoint);
        gameDice.SetActive(true);
    }
    else
    {
        Debug.LogError("GameControl instance not found!");
    }
    
    // Additional logic for handling the object click
}
}

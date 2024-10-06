using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject gamePanel;
    public GameObject mainPanel;
    public GameObject completePanel;

    public PlayerScript bluePlayer;
    public PlayerScript redPlayer;
    public PlayerScript greenPlayer;
    public PlayerScript yellowPlayer;

    public GameObject blueRollingPlace;
    public GameObject redRollingPlace;
    public GameObject greenRollingPlace;
    public GameObject yellowRollingPlace;

    [SerializeField] TMP_Text[] playerStatusFields;
    [SerializeField] GameObject[] resultBars;
    [SerializeField] GameObject[] firstPosition;
    [SerializeField] GameObject[] secondPosition;
    [SerializeField] GameObject[] thirdPosition;
    [SerializeField] GameObject[] defeated;

    public AudioSource audio;
    public int gametype;

    public playerElimination playerEliminationManager;

    public GameObject confettiEffect;

    private void Start()
    {

    }
    public void Awake()
    {

    }

    void Update()
    {
        SoundToggle();
    }
    public void Game1()
    {
        gametype = 2;
        gamePanel.SetActive(true);
        mainPanel.SetActive(false);
        movingSnakes_GameManager.gm.TotalPlayerCanPlay = 2;
        greenPlayer.gameObject.SetActive(false);
        greenRollingPlace.gameObject.SetActive(false);
        yellowPlayer.gameObject.SetActive(false);
        yellowRollingPlace.gameObject.SetActive(false);
    }

    public void Game2()
    {
        gametype = 3;
        gamePanel.SetActive(true);
        mainPanel.SetActive(false);
        movingSnakes_GameManager.gm.TotalPlayerCanPlay = 3;
        yellowPlayer.gameObject.SetActive(false);
        yellowRollingPlace.gameObject.SetActive(false);
    }

    public void Game3()
    {
        gametype = 4;
        gamePanel.SetActive(true);
        mainPanel.SetActive(false);
        movingSnakes_GameManager.gm.TotalPlayerCanPlay = 4;
    }

    public void Game4()
    {

        gametype = 1;
        gamePanel.SetActive(true);
        mainPanel.SetActive(false);
        movingSnakes_GameManager.gm.TotalPlayerCanPlay = 1;
        greenPlayer.gameObject.SetActive(false);
        greenRollingPlace.gameObject.SetActive(false);
        yellowPlayer.gameObject.SetActive(false);
        yellowRollingPlace.gameObject.SetActive(false);
    }

    public void endGameRanks()
    {
        completePanel.SetActive(true);
        audio.Play();
        //gamePanel.SetActive(false);

        // Activate confetti effect
        ActivateConfetti();

        if (gametype == 1)
        {
            computerEnd();
        }
        else if (gametype == 2)
        {
            twoPlayerEnd();
        }
        else if (gametype == 3)
        {
            threePlayerEnd();
        }
        else if (gametype == 4)
        {
            fourPlayerEnd();
        }
        else
        {
            return;
        }

    }
    public void twoPlayerEnd()
    {
        resultBars[2].SetActive(false);
        resultBars[3].SetActive(false);
        if (playerEliminationManager.eliminatedDictionary.ContainsKey("Blue Player") && playerEliminationManager.eliminatedDictionary.ContainsKey("Red Player"))
        {
            playerStatusFields[0].text = "Eliminated";
            defeated[0].SetActive(true);
            playerStatusFields[1].text = "Eliminated";
            defeated[1].SetActive(true);
        }
        else if (playerEliminationManager.eliminatedDictionary.ContainsKey("Blue Player") && !playerEliminationManager.eliminatedDictionary.ContainsKey("Red Player"))
        {
            playerStatusFields[0].text = "Eliminated";
            defeated[0].SetActive(true);
            playerStatusFields[1].text = "First";
            firstPosition[1].SetActive(true);
        }
        else if (playerEliminationManager.eliminatedDictionary.ContainsKey("Red Player") && !playerEliminationManager.eliminatedDictionary.ContainsKey("Blue Player"))
        {
            playerStatusFields[0].text = "First";
            firstPosition[0].SetActive(true);
            playerStatusFields[1].text = "Eliminated";
            defeated[1].SetActive(true);
        }
        else
        {
            if (playerEliminationManager.completedPlayerList["Blue Player"] == 1)
            {
                playerStatusFields[0].text = "First";
                firstPosition[0].SetActive(true);
                playerStatusFields[1].text = "Lost";
                defeated[1].SetActive(true);
            }
            else
            {
                playerStatusFields[0].text = "Lost";
                defeated[0].SetActive(true);
                playerStatusFields[1].text = "First";
                firstPosition[1].SetActive(true);
            }
        }
        Destroy(gamePanel);
    }

    public void threePlayerEnd()
    {
        resultBars[3].SetActive(false);
        if (!playerEliminationManager.eliminatedDictionary.ContainsKey("Blue Player") && !playerEliminationManager.completedPlayerList.ContainsKey("Blue Player"))
        {
            //firstPosition[0].SetActive(true);
            movingSnakes_GameManager.gm.totalPlayersCompleted++;
            playerEliminationManager.completedPlayerList.Add("Blue Player", movingSnakes_GameManager.gm.totalPlayersCompleted);
        }
        if (!playerEliminationManager.eliminatedDictionary.ContainsKey("Blue Player"))
        {
            int number = playerEliminationManager.completedPlayerList["Blue Player"];
            string positionInString = "Error";
            switch (number)
            {
                case 0:
                    positionInString = "Eliminated";
                    defeated[0].SetActive(true);
                    break;
                case 1:
                    positionInString = "First";
                    firstPosition[0].SetActive(true);
                    break;
                case 2:
                    positionInString = "Second";
                    secondPosition[0].SetActive(true);
                    break;
                case 3:
                    positionInString = "Lost";
                    defeated[0].SetActive(true);
                    break;
                case 4:
                    break;
            }
            playerStatusFields[0].text = positionInString;
        }
        else
        {
            playerStatusFields[0].text = "Eliminated";
            defeated[0].SetActive(true);
        }
        if (!playerEliminationManager.eliminatedDictionary.ContainsKey("Red Player") && !playerEliminationManager.completedPlayerList.ContainsKey("Red Player"))
        {
            //firstPosition[1].SetActive(true);
            movingSnakes_GameManager.gm.totalPlayersCompleted++;
            playerEliminationManager.completedPlayerList.Add("Red Player", movingSnakes_GameManager.gm.totalPlayersCompleted);
        }
        if (!playerEliminationManager.eliminatedDictionary.ContainsKey("Red Player"))
        {
            int number = playerEliminationManager.completedPlayerList["Red Player"];
            string positionInString = "Error";
            switch (number)
            {
                case 0:
                    positionInString = "Eliminated";
                    defeated[1].SetActive(true);
                    break;
                case 1:
                    positionInString = "First";
                    firstPosition[1].SetActive(true);
                    break;
                case 2:
                    positionInString = "Second";
                    secondPosition[1].SetActive(true);
                    break;
                case 3:
                    positionInString = "Lost";
                    defeated[1].SetActive(true);
                    break;
                case 4:
                    break;

            }
            playerStatusFields[1].text = positionInString;
        }
        else
        {
            playerStatusFields[1].text = "Eliminated";
            defeated[1].SetActive(true);
        }
        if (!playerEliminationManager.eliminatedDictionary.ContainsKey("Green Player") && !playerEliminationManager.completedPlayerList.ContainsKey("Green Player"))
        {
            //firstPosition[2].SetActive(true);
            movingSnakes_GameManager.gm.totalPlayersCompleted++;
            playerEliminationManager.completedPlayerList.Add("Blue Player", movingSnakes_GameManager.gm.totalPlayersCompleted);
        }
        if (!playerEliminationManager.eliminatedDictionary.ContainsKey("Green Player"))
        {
            int number = playerEliminationManager.completedPlayerList["Green Player"];
            string positionInString = "Error";
            switch (number)
            {
                case 0:
                    positionInString = "Eliminated";
                    defeated[2].SetActive(true);
                    break;
                case 1:
                    positionInString = "First";
                    firstPosition[2].SetActive(true);
                    break;
                case 2:
                    positionInString = "Second";
                    secondPosition[2].SetActive(true);
                    break;
                case 3:
                    positionInString = "Lost";
                    defeated[2].SetActive(true);
                    break;
                case 4:
                    break;

            }
            playerStatusFields[2].text = positionInString;
        }
        else
        {
            playerStatusFields[2].text = "Eliminated";
            defeated[2].SetActive(true);
        }
        Destroy(gamePanel);
    }

    public void fourPlayerEnd()
    {
        if (!playerEliminationManager.eliminatedDictionary.ContainsKey("Blue Player") && !playerEliminationManager.completedPlayerList.ContainsKey("Blue Player"))
        {
            //firstPosition[0].SetActive(true);
            movingSnakes_GameManager.gm.totalPlayersCompleted++;
            playerEliminationManager.completedPlayerList.Add("Blue Player", movingSnakes_GameManager.gm.totalPlayersCompleted);
        }
        if (!playerEliminationManager.eliminatedDictionary.ContainsKey("Blue Player"))
        {
            int number = playerEliminationManager.completedPlayerList["Blue Player"];
            string positionInString = "Error";
            switch (number)
            {
                case 0:
                    positionInString = "Eliminated";
                    defeated[0].SetActive(true);
                    break;
                case 1:
                    positionInString = "First";
                    firstPosition[0].SetActive(true);
                    break;
                case 2:
                    positionInString = "Second";
                    secondPosition[0].SetActive(true);
                    break;
                case 3:
                    positionInString = "Third";
                    thirdPosition[0].SetActive(true);
                    break;
                case 4:
                    positionInString = "Lost";
                    defeated[0].SetActive(true);
                    break;
                case 5:
                    break;
            }
            playerStatusFields[0].text = positionInString;
        }
        else
        {
            playerStatusFields[0].text = "Eliminated";
            defeated[0].SetActive(true);
        }
        if (!playerEliminationManager.eliminatedDictionary.ContainsKey("Red Player") && !playerEliminationManager.completedPlayerList.ContainsKey("Red Player"))
        {
            //firstPosition[1].SetActive(true);
            movingSnakes_GameManager.gm.totalPlayersCompleted++;
            playerEliminationManager.completedPlayerList.Add("Red Player", movingSnakes_GameManager.gm.totalPlayersCompleted);
        }
        if (!playerEliminationManager.eliminatedDictionary.ContainsKey("Red Player"))
        {
            int number = playerEliminationManager.completedPlayerList["Red Player"];
            string positionInString = "Error";
            switch (number)
            {
                case 0:
                    positionInString = "Eliminated";
                    defeated[1].SetActive(true);
                    break;
                case 1:
                    positionInString = "First";
                    firstPosition[1].SetActive(true);
                    break;
                case 2:
                    positionInString = "Second";
                    secondPosition[1].SetActive(true);
                    break;
                case 3:
                    positionInString = "Third";
                    thirdPosition[1].SetActive(true);
                    break;
                case 4:
                    positionInString = "Lost";
                    defeated[1].SetActive(true);
                    break;
                case 5:
                    break;

            }
            playerStatusFields[1].text = positionInString;
        }
        else
        {
            playerStatusFields[1].text = "Eliminated";
            defeated[1].SetActive(true);
        }
        if (!playerEliminationManager.eliminatedDictionary.ContainsKey("Green Player") && !playerEliminationManager.completedPlayerList.ContainsKey("Green Player"))
        {
            //firstPosition[2].SetActive(true);
            movingSnakes_GameManager.gm.totalPlayersCompleted++;
            playerEliminationManager.completedPlayerList.Add("Green Player", movingSnakes_GameManager.gm.totalPlayersCompleted);
        }
        if (!playerEliminationManager.eliminatedDictionary.ContainsKey("Green Player"))
        {
            int number = playerEliminationManager.completedPlayerList["Green Player"];
            string positionInString = "Error";
            switch (number)
            {
                case 0:
                    positionInString = "Eliminated";
                    defeated[2].SetActive(true);
                    break;
                case 1:
                    positionInString = "First";
                    firstPosition[2].SetActive(true);
                    break;
                case 2:
                    positionInString = "Second";
                    secondPosition[2].SetActive(true);
                    break;
                case 3:
                    positionInString = "ThirdPosition";
                    thirdPosition[2].SetActive(true);
                    break;
                case 4:
                    positionInString = "Lost";
                    defeated[2].SetActive(true);
                    break;
                case 5:
                    break;

            }
            playerStatusFields[2].text = positionInString;
        }
        else
        {
            playerStatusFields[2].text = "Eliminated";
            defeated[2].SetActive(true);
        }
        if (!playerEliminationManager.eliminatedDictionary.ContainsKey("Yellow Player") && !playerEliminationManager.completedPlayerList.ContainsKey("Yellow Player"))
        {
            //firstPosition[3].SetActive(true);
            movingSnakes_GameManager.gm.totalPlayersCompleted++;
            playerEliminationManager.completedPlayerList.Add("Yellow Player", movingSnakes_GameManager.gm.totalPlayersCompleted);
        }
        if (!playerEliminationManager.eliminatedDictionary.ContainsKey("Yellow Player"))
        {
            int number = playerEliminationManager.completedPlayerList["Yellow Player"];
            string positionInString = "Error";
            switch (number)
            {
                case 0:
                    positionInString = "Eliminated";
                    defeated[3].SetActive(true);
                    break;
                case 1:
                    positionInString = "First";
                    firstPosition[3].SetActive(true);
                    break;
                case 2:
                    positionInString = "Second";
                    secondPosition[3].SetActive(true);
                    break;
                case 3:
                    positionInString = "Third";
                    thirdPosition[3].SetActive(true);
                    break;
                case 4:
                    positionInString = "Lost";
                    defeated[3].SetActive(true);
                    break;
                case 5:
                    break;

            }
            playerStatusFields[3].text = positionInString;
        }
        else
        {
            playerStatusFields[3].text = "Eliminated";
            defeated[3].SetActive(true);
        }
        Destroy(gamePanel);
    }

    public void computerEnd()
    {
        resultBars[2].SetActive(false);
        resultBars[3].SetActive(false);
        if (playerEliminationManager.eliminatedDictionary.ContainsKey("Blue Player") && playerEliminationManager.eliminatedDictionary.ContainsKey("Red Player"))
        {
            playerStatusFields[0].text = "You Lost";
            defeated[0].SetActive(true);
            playerStatusFields[1].text = "Computer Lost";
            defeated[1].SetActive(true);
        }
        else if (playerEliminationManager.eliminatedDictionary.ContainsKey("Blue Player") && !playerEliminationManager.eliminatedDictionary.ContainsKey("Red Player"))
        {
            playerStatusFields[0].text = "You Lost";
            defeated[0].SetActive(true);
            playerStatusFields[1].text = "Computer Won";
            firstPosition[1].SetActive(true);
        }
        else if (playerEliminationManager.eliminatedDictionary.ContainsKey("Red Player") && !playerEliminationManager.eliminatedDictionary.ContainsKey("Blue Player"))
        {
            playerStatusFields[0].text = "You Won";
            firstPosition[0].SetActive(true);
            playerStatusFields[1].text = "Computer Lost";
            defeated[1].SetActive(true);
        }
        else
        {
            if (playerEliminationManager.completedPlayerList.ContainsKey("Blue Player"))
            {
                playerStatusFields[0].text = "You Won";
                firstPosition[0].SetActive(true);
                playerStatusFields[1].text = "Computer Lost";
                defeated[1].SetActive(true);
            }
            else
            {
                playerStatusFields[0].text = "You Lost";
                defeated[0].SetActive(true);
                playerStatusFields[1].text = "Computer Won";
                firstPosition[1].SetActive(true);
            }
        }
        Destroy(gamePanel);
    }

    private void ActivateConfetti()
    {
        if (confettiEffect != null)
        {
            confettiEffect.SetActive(true);
        }
    }

    public void SoundToggle()
    {
        if (AudioManager.instance.isSoundOn)
        {
            audio.mute = false;
        }
        else
        {
            audio.mute = true;
        }
    }
}



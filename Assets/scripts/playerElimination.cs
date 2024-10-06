using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerElimination : MonoBehaviour
{
    public Animator playerAnimator;
    [SerializeField] GameObject[] playerGreen1;
    [SerializeField] GameObject[] playerGreen2;
    [SerializeField] GameObject[] playerGreen3;
    [SerializeField] GameObject[] playerGreen4;
    [SerializeField] public GameObject[] hearts;
    [SerializeField] public GameObject liveMsgPopUp;
    public UIManager winningGameManager;
    public rollingDice rollingDiceManager;

    public Dictionary<string, string> eliminatedDictionary = new Dictionary<string, string>();
    public Dictionary<string,int> completedPlayerList = new Dictionary<string,int>();

    [SerializeField] public GameObject[] eliminatedTag;
    [SerializeField] public GameObject[] player1PositionTag;
    [SerializeField] public GameObject[] player2PositionTag;
    [SerializeField] public GameObject[] player3PositionTag;
    [SerializeField] public GameObject[] player4PositionTag;

    public PlayerScript bluePlayer;
    public PlayerScript redPlayer;
    public PlayerScript greenPlayer;
    public PlayerScript yellowPlayer;
    
    public void Start()
    {
    }

    public void Awake()
    {
        rollingDiceManager = FindObjectOfType<rollingDice>();
        winningGameManager = FindObjectOfType<UIManager>();
    }

    public void Update()
    {

        
    }

    public void reducePlayerLive(int playerNo,int liveNo)
    {
        if (playerNo == 1)
        {
            playerGreen1[liveNo].SetActive(false);
        }
        if (playerNo == 2)
        {
            playerGreen2[liveNo].SetActive(false);
        }
        if(playerNo == 3)
        {
            playerGreen3[liveNo].SetActive(false);
        }
        if(playerNo == 4)
        {
            playerGreen4[liveNo].SetActive(false);
        }
    }
    public void removePlayerFromGame(int playerNo,string eliminatedPlayer)
    {

        switch (eliminatedPlayer)
        {
           case "BlueRollingDice":
                playerGreen1[0].SetActive(false) ;
                eliminatedTag[0].SetActive(true);
                movingSnakes_GameManager.gm.BlueCompletePlayer+=1;
                eliminatedDictionary.Add("Blue Player", "Eliminated");
                if (!checkGameEnd())
                {
                    movingSnakes_GameManager.gm.selfDice = false;
                    movingSnakes_GameManager.gm.transferDice = true;
                    bluePlayer.gameObject.SetActive(false);
                }
                else
                {
                    rollingDiceManager.StopAllCoroutines();
                    movingSnakes_GameManager.gm.selfDice = false;
                    movingSnakes_GameManager.gm.transferDice =false;
                    bluePlayer.gameObject.SetActive(false);
                }
                
                break;
           case "GreenRollingDice":
                playerGreen2[0].SetActive(false);
                eliminatedTag[1].SetActive(true);
                movingSnakes_GameManager.gm.GreenCompletePlayer+=1;
                eliminatedDictionary.Add("Green Player", "Eliminated");
                if (!checkGameEnd())
                {
                    movingSnakes_GameManager.gm.selfDice = false;
                    movingSnakes_GameManager.gm.transferDice = true;
                    greenPlayer.gameObject.SetActive(false);
                }
                else
                {
                    rollingDiceManager.StopAllCoroutines();
                    movingSnakes_GameManager.gm.selfDice = false;
                    movingSnakes_GameManager.gm.transferDice = false;
                    greenPlayer.gameObject.SetActive(false);
                }
                break;
           case "RedRollingDice":
                playerGreen3[0].SetActive(false);
                eliminatedTag[2].SetActive(true);
                movingSnakes_GameManager.gm.RedCompletePlayer+=1;
                eliminatedDictionary.Add("Red Player", "Eliminated");
                if (!checkGameEnd())
                {
                    movingSnakes_GameManager.gm.selfDice = false;
                    movingSnakes_GameManager.gm.transferDice = true;
                    redPlayer.gameObject.SetActive(false);
                }
                else
                {
                    rollingDiceManager.StopAllCoroutines();
                    movingSnakes_GameManager.gm.selfDice = false;
                    movingSnakes_GameManager.gm.transferDice = false;
                    redPlayer.gameObject.SetActive(false);
                }
                break;
           case "YellowRollingDice":
                playerGreen4[0].SetActive(false);
                eliminatedTag[3].SetActive(true);
                movingSnakes_GameManager.gm.YellowCompletePlayer+=1;
                eliminatedDictionary.Add("Orange Player", "Eliminated");
                if (!checkGameEnd())
                {
                    movingSnakes_GameManager.gm.selfDice = false;
                    movingSnakes_GameManager.gm.transferDice = true;
                    yellowPlayer.gameObject.SetActive(false);
                }
                else
                {
                    rollingDiceManager.StopAllCoroutines();
                    movingSnakes_GameManager.gm.selfDice = false;
                    movingSnakes_GameManager.gm.transferDice = false;
                    yellowPlayer.gameObject.SetActive(false);
                }
                break;
        }
        
    }
    public bool checkGameEnd()
    {
        int numberOfCompletedPlayers = movingSnakes_GameManager.gm.BlueCompletePlayer + movingSnakes_GameManager.gm.RedCompletePlayer + movingSnakes_GameManager.gm.GreenCompletePlayer + movingSnakes_GameManager.gm.YellowCompletePlayer;
        if (numberOfCompletedPlayers == 1 && movingSnakes_GameManager.gm.TotalPlayerCanPlay == 1)
        {
            winningGameManager.endGameRanks();
            return true;
        }
        else if (numberOfCompletedPlayers == 1 && movingSnakes_GameManager.gm.TotalPlayerCanPlay == 2)
        {
            winningGameManager.endGameRanks();
            return true;
        }
        else if (numberOfCompletedPlayers == 2 && movingSnakes_GameManager.gm.TotalPlayerCanPlay == 3)
        {
            winningGameManager.endGameRanks();
            return true;
        }
        else if (numberOfCompletedPlayers == 3 && movingSnakes_GameManager.gm.TotalPlayerCanPlay == 4)
        {
            winningGameManager.endGameRanks();
            return true;
        }

        return false;

    }

    public void PlayAnimationAndDeactivateHelper(int barIndex){
        StartCoroutine(PlayAnimationAndDeactivate(barIndex));
    }

    public IEnumerator PlayAnimationAndDeactivate(int barIndex)
    {

        hearts[barIndex - 1].SetActive(true);
        liveMsgPopUp.SetActive(true);
        playerAnimator = hearts[barIndex - 1].GetComponent<Animator>();
        if (playerAnimator != null)
        {
            playerAnimator.Play("heartBreakAnimation");
            yield return new WaitForSeconds(1f);
        }
        hearts[barIndex - 1].SetActive(false);
        liveMsgPopUp.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingSnakes_GameManager : MonoBehaviour
{
    public movingSnakes_PathObjectParent pathParent;
    public static movingSnakes_GameManager gm;
    public rollingDice rollingDice;
    public soundManager soundmanager;
    public timerLoader loadTimer;
    public int numberOfStepsToMove;

    List<movingSnakes_PathPoint> playerOnPathPointList = new List<movingSnakes_PathPoint>();

    public bool canDiceRoll = true;
    public bool transferDice = false;
    public bool selfDice = true;
    public bool hasPlayerAlreadyMoved = true;
    public rollingDice[] manageRollingDice;
    public PlayerScript[] managePlayerScript;
    public UIManager winningGameManager;
    playerElimination playerEliminate;

    public int BlueOutPlayer = 0;
    public int GreenOutPlayer = 0;
    public int RedOutPlayer = 0;
    public int YellowOutPlayer = 0;
    public int completePlayerCount = 0;

    public int BlueCompletePlayer = 0;
    public int GreenCompletePlayer = 0;
    public int RedCompletePlayer = 0;
    public int YellowCompletePlayer = 0;

    public GameObject gamePanel;
    public GameObject CompleteGame;

    public int TotalPlayerCanPlay;
    public int totalPlayersCompleted;

    public AudioClip mainMenuMusic;
    public AudioClip gameplayMusic;
    public bool sound;

    public AudioSource ads;
    private bool isGamePanelActive = false;
    public int totalSix;

    private void Start()
    {
        TotalPlayerCanPlay = winningGameManager.gametype;
        totalPlayersCompleted = 0;
        AudioManager.instance.PlayBGM(mainMenuMusic);
    }
    private void Awake()
    {
        gm = this;
        ads = GetComponent<AudioSource>();
        soundmanager = GetComponent<soundManager>();
        rollingDice = FindObjectOfType<rollingDice>();
        loadTimer = FindObjectOfType<timerLoader>();
        winningGameManager = FindObjectOfType<UIManager>();
        playerEliminate = FindFirstObjectByType<playerElimination>();
    }

    private void Update()
    {
        SoundToggle();
        bool isGamePanelActiveNow = gamePanel.activeSelf;

        if (isGamePanelActive != isGamePanelActiveNow)
        {
            isGamePanelActive = isGamePanelActiveNow;

            if (isGamePanelActive)
            {
                AudioManager.instance.PlayBGM(gameplayMusic);
            }
            else
            {
                AudioManager.instance.PlayBGM(mainMenuMusic);
            }
        }
    }
    public void AddPathPoint(movingSnakes_PathPoint pathPoint)
    {
        playerOnPathPointList.Add(pathPoint);
    }

    public void RemovePathPoint(movingSnakes_PathPoint pathPoint)
    {
        if (playerOnPathPointList.Contains(pathPoint))
        {
            playerOnPathPointList.Remove(pathPoint);
        }
    }

    public void RollingDiceManager()
    {
        int nextDice;
        if (movingSnakes_GameManager.gm.transferDice)
        {
            if (movingSnakes_GameManager.gm.TotalPlayerCanPlay == 1)
            {
                if (movingSnakes_GameManager.gm.rollingDice.name == "BlueRollingDice")
                {
                    movingSnakes_GameManager.gm.manageRollingDice[0].gameObject.SetActive(false);
                    movingSnakes_GameManager.gm.manageRollingDice[2].gameObject.SetActive(true);
                    movingSnakes_GameManager.gm.manageRollingDice[2].OnMouseDown();
                }
                else
                {
                    movingSnakes_GameManager.gm.manageRollingDice[2].gameObject.SetActive(false);
                    movingSnakes_GameManager.gm.manageRollingDice[0].gameObject.SetActive(true);
                }
            }
            else if (movingSnakes_GameManager.gm.TotalPlayerCanPlay == 2)
            {
                if (movingSnakes_GameManager.gm.rollingDice.name == "BlueRollingDice")
                {
                    movingSnakes_GameManager.gm.manageRollingDice[0].gameObject.SetActive(false);
                    movingSnakes_GameManager.gm.manageRollingDice[2].gameObject.SetActive(true);
                }
                else
                {
                    movingSnakes_GameManager.gm.manageRollingDice[2].gameObject.SetActive(false);
                    movingSnakes_GameManager.gm.manageRollingDice[0].gameObject.SetActive(true);
                }
            }
            else if (movingSnakes_GameManager.gm.TotalPlayerCanPlay == 3)
            {
                for (int i = 0; i < 3; i++)
                {


                    if (movingSnakes_GameManager.gm.rollingDice == movingSnakes_GameManager.gm.manageRollingDice[i])
                    {
                        nextDice = threePlayerPassout(i);
                        movingSnakes_GameManager.gm.manageRollingDice[i].gameObject.SetActive(false);
                        movingSnakes_GameManager.gm.manageRollingDice[nextDice].gameObject.SetActive(true);
                        break;
                    }

                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {


                    if (movingSnakes_GameManager.gm.rollingDice == movingSnakes_GameManager.gm.manageRollingDice[i])
                    {
                        nextDice = passout(i);
                        movingSnakes_GameManager.gm.manageRollingDice[i].gameObject.SetActive(false);
                        movingSnakes_GameManager.gm.manageRollingDice[nextDice].gameObject.SetActive(true);
                        break;
                    }

                }
            }
            movingSnakes_GameManager.gm.canDiceRoll = true;
        }
        else
        {
            if (movingSnakes_GameManager.gm.selfDice)
            {
                movingSnakes_GameManager.gm.canDiceRoll = true;
                movingSnakes_GameManager.gm.selfDice = false;
                movingSnakes_GameManager.gm.hasPlayerAlreadyMoved = true;
                if (movingSnakes_GameManager.gm.TotalPlayerCanPlay == 1 && movingSnakes_GameManager.gm.rollingDice == movingSnakes_GameManager.gm.manageRollingDice[2])
                {
                    movingSnakes_GameManager.gm.manageRollingDice[2].OnMouseDown();
                }
            }
        }

    }
    int threePlayerPassout(int i)
    {
        if (i == 2) { i = 0; }
        else { i += 1; }
        if (i == 0) { if (movingSnakes_GameManager.gm.BlueCompletePlayer > 0) { ++i; } }
        if (i == 1) { if (movingSnakes_GameManager.gm.GreenCompletePlayer > 0) { ++i; } }
        if (i == 2)
        {
            if (movingSnakes_GameManager.gm.YellowCompletePlayer > 0)
            {
                i = 0;
                if (i == 0) { if (movingSnakes_GameManager.gm.BlueCompletePlayer > 0) { ++i; } }
                if (i == 1) { if (movingSnakes_GameManager.gm.GreenCompletePlayer > 0) { ++i; } }
            }
        }

        return i;
    }
    int passout(int i)
    {
        if (i == 3) { i = 0; } else { i += 1; }
        if (i == 0) { if (movingSnakes_GameManager.gm.BlueCompletePlayer > 0) { ++i; } }
        if (i == 1) { if (movingSnakes_GameManager.gm.GreenCompletePlayer > 0) { ++i; } }
        if (i == 2) { if (movingSnakes_GameManager.gm.RedCompletePlayer > 0) { ++i; } }
        if (i == 3)
        {
            if (movingSnakes_GameManager.gm.YellowCompletePlayer > 0)
            {
                i = 0;
                if (i == 0) { if (movingSnakes_GameManager.gm.BlueCompletePlayer > 0) { ++i; } }
                if (i == 1) { if (movingSnakes_GameManager.gm.GreenCompletePlayer > 0) { ++i; } }
                if (i == 2) { if (movingSnakes_GameManager.gm.RedCompletePlayer > 0) { ++i; } }

            }
        }
        return i;
    }

    public void SoundToggle()
    {
        if (AudioManager.instance.isSoundOn)
        {
            movingSnakes_GameManager.gm.sound = true;
        }
        else
        {
            movingSnakes_GameManager.gm.sound = false;
        }
    }

}
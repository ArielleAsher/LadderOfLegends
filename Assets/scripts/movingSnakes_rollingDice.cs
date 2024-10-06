using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DentedPixel;
using UnityEngine.UIElements;

public class rollingDice : MonoBehaviour
{
    [SerializeField] Sprite[] numberSprite;
    [SerializeField] SpriteRenderer numberSpriteHolder;
    [SerializeField] SpriteRenderer rollingDiceAnimation;
    [SerializeField] int numberGot;
    public int playerIndex;

    public int lives = 3;
    public AudioSource playerWinaudio;
    //public Animator playerAnimator;
    int outPieces;

    public GameObject confettiEffect;

    PlayerScript currentPlayerScript;
    timerLoader timeLoader;
    playerElimination playerEliminate;
    UIManager winningGameManager;
    Coroutine generateRandomNumberDice;
    Coroutine MovePlayerPiece;


    public int maxNum;
    public DiceAudio diceSound;
    private bool isTimerRunning = true;
    int barIndex;
    Coroutine timerCoroutine;
    public bool isRollingDiceOutOfGame = false;
    public int recentlyMovedPathPoint;
    public AudioSource audio;
    public void Start()
    {

    }
    public void Update()
    {
        if (!isTimerRunning)
        {
            stopTimer();
        }
        SoundToggle();
    }
    private void OnEnable()
    {
        StartTimer();
    }

    public void Awake()
    {
        timeLoader = FindObjectOfType<timerLoader>();
        playerEliminate = FindFirstObjectByType<playerElimination>();
        winningGameManager = FindObjectOfType<UIManager>();
    }

    public void OnMouseDown()
    {
        StopCoroutine(StartTimerCoroutine());
        isTimerRunning = false;
        LeanTween.cancelAll(true);
        generateRandomNumberDice = StartCoroutine(RollingDice());
    }

    IEnumerator RollingDice()
    {
        yield return new WaitForEndOfFrame();

        if (movingSnakes_GameManager.gm.canDiceRoll)
        {
            if (movingSnakes_GameManager.gm.sound)
            {
                diceSound.PlaySound();
            }
            movingSnakes_GameManager.gm.canDiceRoll = false;
            numberSpriteHolder.gameObject.SetActive(false);
            rollingDiceAnimation.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.6f);

            if (movingSnakes_GameManager.gm.totalSix == 2) { movingSnakes_GameManager.gm.totalSix = 0; maxNum = 5; } else { maxNum = 6; }

            numberGot = Random.Range(0, maxNum);
            numberSpriteHolder.sprite = numberSprite[numberGot];
            numberGot += 1;
            if (numberGot == 6)
            {
                movingSnakes_GameManager.gm.totalSix += 1;
                //Debug.Log(" 6 Timer called again!!");
                StartTimer();

            }
            movingSnakes_GameManager.gm.numberOfStepsToMove = numberGot;
            movingSnakes_GameManager.gm.rollingDice = this;

            numberSpriteHolder.gameObject.SetActive(true);
            rollingDiceAnimation.gameObject.SetActive(false);

            yield return new WaitForEndOfFrame();

            if (movingSnakes_GameManager.gm.rollingDice == movingSnakes_GameManager.gm.manageRollingDice[0]) { outPieces = movingSnakes_GameManager.gm.BlueOutPlayer; currentPlayerScript = movingSnakes_GameManager.gm.managePlayerScript[0]; }
            else if (movingSnakes_GameManager.gm.rollingDice == movingSnakes_GameManager.gm.manageRollingDice[1]) { outPieces = movingSnakes_GameManager.gm.GreenOutPlayer; currentPlayerScript = movingSnakes_GameManager.gm.managePlayerScript[1]; }
            else if (movingSnakes_GameManager.gm.rollingDice == movingSnakes_GameManager.gm.manageRollingDice[2]) { outPieces = movingSnakes_GameManager.gm.RedOutPlayer; currentPlayerScript = movingSnakes_GameManager.gm.managePlayerScript[2]; }
            else if (movingSnakes_GameManager.gm.rollingDice == movingSnakes_GameManager.gm.manageRollingDice[3]) { outPieces = movingSnakes_GameManager.gm.YellowOutPlayer; currentPlayerScript = movingSnakes_GameManager.gm.managePlayerScript[3]; }

            if (movingSnakes_GameManager.gm.numberOfStepsToMove != 6 && outPieces == 0)
            {
                movingSnakes_GameManager.gm.selfDice = false;
                movingSnakes_GameManager.gm.transferDice = true;

                yield return new WaitForSeconds(0.6f);
                movingSnakes_GameManager.gm.RollingDiceManager();
            }
            else
            {
                if (movingSnakes_GameManager.gm.numberOfStepsToMove == 6 && outPieces == 0)
                {
                    outPieces = 1;
                    MakePlayerReadyToMove();
                }
                else if (outPieces > 0)
                {
                    MovePlayerPiece = StartCoroutine(MoveStep_Enum());

                }
            }
            if (generateRandomNumberDice != null)
            {
                StopCoroutine(RollingDice());
            }
        }

    }

    public void MakePlayerReadyToMove()
    {

        currentPlayerScript.isReady = true;
        currentPlayerScript.transform.position = currentPlayerScript.pathParent.commonPathPoint[0].transform.position;
        currentPlayerScript.numberOfStepsAlreadyMoved = 1;

        currentPlayerScript.previousPathPoint = currentPlayerScript.pathParent.commonPathPoint[0];
        currentPlayerScript.currentPathPoint = currentPlayerScript.pathParent.commonPathPoint[0];
        movingSnakes_GameManager.gm.AddPathPoint(currentPlayerScript.currentPathPoint);
        currentPlayerScript.currentPathPoint.AddPlayerPiece(currentPlayerScript);

        movingSnakes_GameManager.gm.canDiceRoll = true;
        movingSnakes_GameManager.gm.transferDice = false;
        movingSnakes_GameManager.gm.selfDice = true;

        Debug.Log(gameObject.name + " : Player Unlocked");

        movingSnakes_GameManager.gm.RollingDiceManager();
        movingSnakes_GameManager.gm.numberOfStepsToMove = 0;
        outPlayer();
    }
    public void outPlayer()
    {
        if (movingSnakes_GameManager.gm.rollingDice == movingSnakes_GameManager.gm.manageRollingDice[0]) { movingSnakes_GameManager.gm.BlueOutPlayer += 1; }
        else if (movingSnakes_GameManager.gm.rollingDice == movingSnakes_GameManager.gm.manageRollingDice[1]) { movingSnakes_GameManager.gm.GreenOutPlayer += 1; }
        else if (movingSnakes_GameManager.gm.rollingDice == movingSnakes_GameManager.gm.manageRollingDice[2]) { movingSnakes_GameManager.gm.RedOutPlayer += 1; }
        else if (movingSnakes_GameManager.gm.rollingDice == movingSnakes_GameManager.gm.manageRollingDice[3]) { movingSnakes_GameManager.gm.YellowOutPlayer += 1; }

    }

    IEnumerator MoveStep_Enum()
    {
        currentPlayerScript.numberOfStepsToMove = movingSnakes_GameManager.gm.numberOfStepsToMove;
        for (int i = currentPlayerScript.numberOfStepsAlreadyMoved; i < (currentPlayerScript.numberOfStepsAlreadyMoved + currentPlayerScript.numberOfStepsToMove); i++)
        {
            if (isPathAvailableToMove(currentPlayerScript.numberOfStepsToMove, currentPlayerScript.numberOfStepsAlreadyMoved))
            {
                movingSnakes_GameManager.gm.hasPlayerAlreadyMoved = false;
                if (movingSnakes_GameManager.gm.sound) { movingSnakes_GameManager.gm.ads.Play(); }
                recentlyMovedPathPoint = i;
                currentPlayerScript.transform.position = currentPlayerScript.pathParent.commonPathPoint[i].transform.position;
                yield return new WaitForSeconds(0.3f);
            }
        }
        movingSnakes_GameManager.gm.hasPlayerAlreadyMoved = true;
        if (isPathAvailableToMove(currentPlayerScript.numberOfStepsToMove, currentPlayerScript.numberOfStepsAlreadyMoved))
        {
            currentPlayerScript.numberOfStepsAlreadyMoved += currentPlayerScript.numberOfStepsToMove;

            movingSnakes_GameManager.gm.RemovePathPoint(currentPlayerScript.previousPathPoint);
            currentPlayerScript.previousPathPoint.RemovePlayerPiece(currentPlayerScript);
            currentPlayerScript.currentPathPoint = currentPlayerScript.pathParent.commonPathPoint[currentPlayerScript.numberOfStepsAlreadyMoved - 1];

            movingSnakes_GameManager.gm.AddPathPoint(currentPlayerScript.currentPathPoint);
            currentPlayerScript.currentPathPoint.AddPlayerPiece(currentPlayerScript);
            currentPlayerScript.previousPathPoint = currentPlayerScript.currentPathPoint;

            if (movingSnakes_GameManager.gm.numberOfStepsToMove != 6)
            {
                movingSnakes_GameManager.gm.selfDice = false;
                movingSnakes_GameManager.gm.transferDice = true;
            }
            else
            {
                movingSnakes_GameManager.gm.selfDice = true;
                movingSnakes_GameManager.gm.transferDice = false;
            }
            SnakeAndLadderPoint();
            movingSnakes_GameManager.gm.numberOfStepsToMove = 0;
        }
        else
        {
            movingSnakes_GameManager.gm.selfDice = false;
            movingSnakes_GameManager.gm.transferDice = true;
        }

        if (currentPlayerScript.numberOfStepsAlreadyMoved == 100)
        {
            ActivateConfetti();
            playerWinaudio.Play();
            string playerPieceName = currentPlayerScript.name;
            int completePosition = movingSnakes_GameManager.gm.totalPlayersCompleted;
            if (playerPieceName == "bluePlayerPiece")
            {
                movingSnakes_GameManager.gm.totalPlayersCompleted++;
                playerEliminate.player1PositionTag[completePosition].SetActive(true);
                playerEliminate.completedPlayerList.Add("Blue Player", movingSnakes_GameManager.gm.totalPlayersCompleted);
                movingSnakes_GameManager.gm.BlueCompletePlayer += 1;
            }
            else if (playerPieceName == "redPlayerPiece")
            {
                movingSnakes_GameManager.gm.totalPlayersCompleted++;
                playerEliminate.player3PositionTag[completePosition].SetActive(true);
                playerEliminate.completedPlayerList.Add("Red Player", movingSnakes_GameManager.gm.totalPlayersCompleted);
                movingSnakes_GameManager.gm.RedCompletePlayer += 1;
            }
            else if (playerPieceName == "greenPlayerPiece")
            {
                movingSnakes_GameManager.gm.totalPlayersCompleted++;
                playerEliminate.player2PositionTag[completePosition].SetActive(true);
                playerEliminate.completedPlayerList.Add("Green Player", movingSnakes_GameManager.gm.totalPlayersCompleted);
                movingSnakes_GameManager.gm.GreenCompletePlayer += 1;
            }
            else if (playerPieceName == "yellowPlayerPiece")
            {
                movingSnakes_GameManager.gm.totalPlayersCompleted++;
                playerEliminate.player4PositionTag[completePosition].SetActive(true);
                playerEliminate.completedPlayerList.Add("Yellow Player", movingSnakes_GameManager.gm.totalPlayersCompleted);
                movingSnakes_GameManager.gm.YellowCompletePlayer += 1;
            }
            movingSnakes_GameManager.gm.selfDice = false;
            movingSnakes_GameManager.gm.transferDice = true;
        }

        int numberOfCompletedPlayers = movingSnakes_GameManager.gm.BlueCompletePlayer + movingSnakes_GameManager.gm.RedCompletePlayer + movingSnakes_GameManager.gm.GreenCompletePlayer + movingSnakes_GameManager.gm.YellowCompletePlayer;
        if (numberOfCompletedPlayers == 1 && movingSnakes_GameManager.gm.TotalPlayerCanPlay == 1)
        {
            StopAllCoroutines();
            winningGameManager.endGameRanks();

        }
        else if (numberOfCompletedPlayers == 1 && movingSnakes_GameManager.gm.TotalPlayerCanPlay == 2)
        {
            StopAllCoroutines();
            winningGameManager.endGameRanks();

        }
        else if (numberOfCompletedPlayers == 2 && movingSnakes_GameManager.gm.TotalPlayerCanPlay == 3)
        {
            StopAllCoroutines();
            winningGameManager.endGameRanks();

        }
        else if (numberOfCompletedPlayers == 3 && movingSnakes_GameManager.gm.TotalPlayerCanPlay == 4)
        {
            winningGameManager.endGameRanks();
            StopAllCoroutines();
        }

        yield return new WaitForSeconds(0.3f);

        movingSnakes_GameManager.gm.RollingDiceManager();

        if (MovePlayerPiece != null)
        {
            StopCoroutine(MoveStep_Enum());
        }
    }

    public void SnakeAndLadderPoint()
    {
        int[] ladderEntryPoint = { 13, 22, 47, 71, 82 };
        int[] ladderExitPoint = { 27, 60, 67, 91, 98 };

        if (ladderEntryPoint.Contains(currentPlayerScript.numberOfStepsAlreadyMoved))
        {
            audio.Play();
            int index = System.Array.IndexOf(ladderEntryPoint, currentPlayerScript.numberOfStepsAlreadyMoved);
            currentPlayerScript.numberOfStepsAlreadyMoved = ladderExitPoint[index];
            currentPlayerScript.transform.position = currentPlayerScript.pathParent.commonPathPoint[currentPlayerScript.numberOfStepsAlreadyMoved - 1].transform.position;
        }
    }

    bool isPathAvailableToMove(int numberOfStepsToMove, int numberOfStepsAlreadyMoved)
    {
        if (numberOfStepsToMove == 0)
        {
            return false;
        }

        int leftNumberOfPath = 100 - numberOfStepsAlreadyMoved;
        if (leftNumberOfPath >= numberOfStepsToMove)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void StartTimer()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
        timerCoroutine = StartCoroutine(StartTimerCoroutine());
    }

    IEnumerator StartTimerCoroutine()
    {
        isTimerRunning = true;
        switch (gameObject.name)
        {
            case "BlueRollingDice":
                barIndex = 1;
                break;
            case "GreenRollingDice":
                barIndex = 2;
                break;
            case "RedRollingDice":
                barIndex = 3;
                break;
            case "YellowRollingDice":
                barIndex = 4;
                break;
            default:
                barIndex = 0;
                break;
        }

        float timer = 7f;
        startTimer(barIndex);
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        lives--;

        if (lives > 0 && lives < 3)
        {
            playerEliminate.PlayAnimationAndDeactivateHelper(barIndex);
            // StartCoroutine(PlayAnimationAndDeactivate(barIndex));
            playerEliminate.reducePlayerLive(barIndex, lives);
        }
        if (lives == 0)
        {
            numberSpriteHolder.gameObject.SetActive(false);
            rollingDiceAnimation.gameObject.SetActive(false);
            playerEliminate.removePlayerFromGame(barIndex, gameObject.name);
        }

        isTimerRunning = false;
        timeLoader.ResetBars();
        OnMouseDown();
    }

    public void startTimer(int barIdex)
    {
        stopTimer();
        switch (barIndex)
        {
            case 1:
                timeLoader.barContainer1.SetActive(true);
                LeanTween.scaleX(timeLoader.bar1, 1, 7f);
                LeanTween.scaleX(timeLoader.bar2, 0, 0);
                timeLoader.barContainer2.SetActive(false);
                LeanTween.scaleX(timeLoader.bar3, 0, 0);
                timeLoader.barContainer3.SetActive(false);
                LeanTween.scaleX(timeLoader.bar4, 0, 0);
                timeLoader.barContainer4.SetActive(false);
                break;
            case 2:
                timeLoader.barContainer2.SetActive(true);
                LeanTween.scaleX(timeLoader.bar2, 1, 7f);
                LeanTween.scaleX(timeLoader.bar1, 0, 0);
                timeLoader.barContainer1.SetActive(false);
                LeanTween.scaleX(timeLoader.bar3, 0, 0);
                timeLoader.barContainer3.SetActive(false);
                LeanTween.scaleX(timeLoader.bar4, 0, 0);
                timeLoader.barContainer4.SetActive(false);
                break;
            case 3:
                timeLoader.barContainer3.SetActive(true);
                LeanTween.scaleX(timeLoader.bar3, 1, 7f);
                LeanTween.scaleX(timeLoader.bar2, 0, 0);
                timeLoader.barContainer2.SetActive(false);
                LeanTween.scaleX(timeLoader.bar1, 0, 0);
                timeLoader.barContainer1.SetActive(false);
                LeanTween.scaleX(timeLoader.bar4, 0, 0);
                timeLoader.barContainer4.SetActive(false);
                break;
            case 4:
                timeLoader.barContainer4.SetActive(true);
                LeanTween.scaleX(timeLoader.bar4, 1, 7f);
                LeanTween.scaleX(timeLoader.bar2, 0, 0);
                timeLoader.barContainer2.SetActive(false);
                LeanTween.scaleX(timeLoader.bar1, 0, 0);
                timeLoader.barContainer1.SetActive(false);
                LeanTween.scaleX(timeLoader.bar3, 0, 0);
                timeLoader.barContainer3.SetActive(false);
                break;
            default:
                break;
        }
    }

    public void stopTimer()
    {
        LeanTween.scaleX(timeLoader.bar1, 0, 0);
        LeanTween.scaleX(timeLoader.bar2, 0, 0);
        LeanTween.scaleX(timeLoader.bar3, 0, 0);
        LeanTween.scaleX(timeLoader.bar4, 0, 0);

        timeLoader.barContainer1.SetActive(false);
        timeLoader.barContainer2.SetActive(false);
        timeLoader.barContainer3.SetActive(false);
        timeLoader.barContainer4.SetActive(false);
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

    // public IEnumerator PlayAnimationAndDeactivate(int barIndex)
    // {

    //     playerEliminate.hearts[barIndex - 1].SetActive(true);
    //     playerAnimator = playerEliminate.hearts[barIndex - 1].GetComponent<Animator>();
    //     if (playerAnimator != null)
    //     {
    //         playerAnimator.Play("heartBreakAnimation");
    //         yield return new WaitForSeconds(2f);
    //     }
    //     playerEliminate.hearts[barIndex - 1].SetActive(false);
    // }
    
}

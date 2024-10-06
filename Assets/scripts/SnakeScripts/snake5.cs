using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class snake5 : MonoBehaviour
{
    PlayerScript[] managePlayerScript;
    public snakeManager managerSnakes;
    public movingSnakes_PathObjectParent pathParent;
    bool trackerFollowing = false;
    GameObject tracker1;
    PlayerScript currentlyFollowingPlayer = null;
    public float followSpeed;
    public float moveBackSpeed;
    int currentPathPoint = 97;
    Coroutine followCoroutine;
    public AudioSource audio;
    private CircleCollider2D snakeCollider;


    void Start()
    {
        snakeCollider = GetComponent<CircleCollider2D>();
        if (snakeCollider == null)
        {
            snakeCollider = gameObject.AddComponent<CircleCollider2D>();
            snakeCollider.isTrigger = true;
        }
        tracker1 = gameObject;
        tracker1.transform.position = pathParent.commonPathPoint[currentPathPoint - 1].transform.position;
        setSnakeSpeed();
    }

    void Awake()
    {
        managerSnakes = FindObjectOfType<snakeManager>();
        managePlayerScript = FindObjectsOfType<PlayerScript>();
        pathParent = FindObjectOfType<movingSnakes_PathObjectParent>();
    }

    void Update()
    {
        PrintPlayerPositions();
        SoundToggle();
    }

    public void PrintPlayerPositions()
    {
        foreach (PlayerScript playerScript in managePlayerScript)
        {
            int numSteps = playerScript.numberOfStepsAlreadyMoved;
            if (numSteps >= 93 && numSteps <= 97)
            {
                trackerFollowing = true;
                if (currentlyFollowingPlayer == null)
                {
                    currentlyFollowingPlayer = playerScript;
                    StartFollowingPlayer(playerScript);
                }
                if (!IsPlayerInRange(currentlyFollowingPlayer))
                {
                    followCoroutine = null;
                    currentlyFollowingPlayer = null;
                    ReturnToOriginalPosition();
                    trackerFollowing = false;
                }

            }
            else
            {
                if (playerScript == currentlyFollowingPlayer)
                {
                    followCoroutine = null;
                    ReturnToOriginalPosition();
                    currentlyFollowingPlayer = null;
                    trackerFollowing = false;
                    managerSnakes.animator[0].SetBool("SnakeMove", false);

                }
                else
                {
                    managerSnakes.animator[4].SetBool("SnakeMove", false);
                }
            }
        }
    }

    public void StartFollowingPlayer(PlayerScript playerScript)
    {
        setSnakeSpeed();
        followCoroutine = StartCoroutine(FollowPlayer(playerScript));
    }

    private IEnumerator FollowPlayer(PlayerScript playerScript)
    {
        while (currentlyFollowingPlayer == playerScript)
        {
            Vector3 targetPosition = pathParent.commonPathPoint[playerScript.numberOfStepsAlreadyMoved - 1].transform.position;

            while (Vector3.Distance(tracker1.transform.position, targetPosition) > 0.1f)
            {
                if (!IsPlayerInRange(playerScript))
                {
                    ReturnToOriginalPosition();
                    yield break;
                }
                managerSnakes.animator[4].SetBool("SnakeMove", true);
                if (playerScript.numberOfStepsAlreadyMoved >= 0f)
                {
                    targetPosition = pathParent.commonPathPoint[playerScript.numberOfStepsAlreadyMoved - 1].transform.position;
                }
                else
                {
                    ReturnToOriginalPosition();
                    yield break;
                }
                Vector3 newPosition = new Vector3(
                    Mathf.MoveTowards(tracker1.transform.position.x, targetPosition.x, followSpeed * Time.deltaTime),
                    tracker1.transform.position.y,
                    tracker1.transform.position.z
                );

                tracker1.transform.position = newPosition;

                // float distanceToTarget = Vector3.Distance(tracker1.transform.position, targetPosition);
                // if ((distanceToTarget < 0.1f && movingSnakes_GameManager.gm.hasPlayerAlreadyMoved))
                // {
                //     KillPlayer(playerScript);
                //     audio.Play();
                //     ReturnToOriginalPosition();
                //     yield break;
                // }

                yield return null;
            }
            tracker1.transform.position = targetPosition;
            yield return null;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        PlayerScript playerScript = other.GetComponent<PlayerScript>();
        if (playerScript != null)
        {
            if (IsPlayerInRange(playerScript))
            {
                float distance = Vector2.Distance(transform.position, pathParent.commonPathPoint[playerScript.numberOfStepsAlreadyMoved - 1].transform.position);
                if (distance <= 0.1)
                {
                    audio.Play();
                    KillPlayer(playerScript);
                    ReturnToOriginalPosition();
                }
            }
        }
    }

    public void ReturnToOriginalPosition()
    {
        if (!checkSnakeInOriginalPosition())
        {
            StartCoroutine(MoveToOriginalPosition());
        }
        managerSnakes.animator[4].SetBool("SnakeMove", false);
    }

    private IEnumerator MoveToOriginalPosition()
    {
        Vector3 originalPosition = pathParent.commonPathPoint[currentPathPoint - 1].transform.position;
        float duration = 4.0f;
        float elapsedTime = 0.0f;
        Vector3 startingPosition = transform.position;

        while (elapsedTime < duration)
        {
            managerSnakes.animator[4].SetBool("SnakeMove", true);
            Vector3 newPosition = new Vector3(
                Mathf.Lerp(startingPosition.x, originalPosition.x, elapsedTime / duration),
                transform.position.y,
                transform.position.z
            );

            transform.position = newPosition;
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.position = new Vector3(originalPosition.x, transform.position.y, transform.position.z);
        managerSnakes.animator[4].SetBool("SnakeMove", false);

    }

    public void KillPlayer(PlayerScript currentPlayerScript)
    {
        int playerPosition = currentPlayerScript.numberOfStepsAlreadyMoved;
        int positionToMove = 79 - (97 - playerPosition);
        Vector3 newPosition = pathParent.commonPathPoint[positionToMove - 1].transform.position;


        currentPlayerScript.transform.position = newPosition;
        currentPlayerScript.numberOfStepsAlreadyMoved = positionToMove;
        movingSnakes_GameManager.gm.RemovePathPoint(currentPlayerScript.previousPathPoint);
        currentPlayerScript.previousPathPoint.RemovePlayerPiece(currentPlayerScript);
        currentPlayerScript.currentPathPoint = currentPlayerScript.pathParent.commonPathPoint[currentPlayerScript.numberOfStepsAlreadyMoved - 1];

        movingSnakes_GameManager.gm.AddPathPoint(currentPlayerScript.currentPathPoint);
        currentPlayerScript.currentPathPoint.AddPlayerPiece(currentPlayerScript);
        currentPlayerScript.previousPathPoint = currentPlayerScript.currentPathPoint;
        LadderPointCheck(currentPlayerScript);

        Debug.Log("Player Killed. Moved to position: " + newPosition);

        currentlyFollowingPlayer = null;
        trackerFollowing = false;
    }

    private bool IsPlayerInRange(PlayerScript playerScript)
    {
        int numSteps = playerScript.numberOfStepsAlreadyMoved;
        return (numSteps >= 93 && numSteps <= 97);
    }

    public void setSnakeSpeed()
    {
        int players = movingSnakes_GameManager.gm.TotalPlayerCanPlay - movingSnakes_GameManager.gm.totalPlayersCompleted;
        switch (players)
        {
            case 0:
                followSpeed = 0f;
                break;
            case 1:
                followSpeed = 0.4f;
                break;
            case 2:
                followSpeed = 0.4f;
                break;
            case 3:
                followSpeed = 0.4f;
                break;
            case 4:
                followSpeed = 0.2f;
                break;
            default:
                followSpeed = 0f;
                break;
        }
    }

    public void LadderPointCheck(PlayerScript currentPlayerScript)
    {
        int[] ladderEntryPoint = { 13, 22, 47, 71, 82 };
        int[] ladderExitPoint = { 27, 60, 67, 91, 98 };

        if (ladderEntryPoint.Contains(currentPlayerScript.numberOfStepsAlreadyMoved))
        {
            int index = System.Array.IndexOf(ladderEntryPoint, currentPlayerScript.numberOfStepsAlreadyMoved);
            currentPlayerScript.numberOfStepsAlreadyMoved = ladderExitPoint[index];
            currentPlayerScript.transform.position = currentPlayerScript.pathParent.commonPathPoint[currentPlayerScript.numberOfStepsAlreadyMoved - 1].transform.position;
        }
    }

    public bool checkSnakeInOriginalPosition()
    {
        Vector3 targetPosition = pathParent.commonPathPoint[currentPathPoint - 1].transform.position;
        float threshold = 0.01f;
        if (Vector3.Distance(tracker1.transform.position, targetPosition) < threshold)
        {
            managerSnakes.animator[0].SetBool("SnakeMove", false);
        }
        return Vector3.Distance(tracker1.transform.position, targetPosition) < threshold;
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

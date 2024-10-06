using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public bool moveNow;
    public bool isReady;

    public int numberOfStepsToMove;
    public int numberOfStepsAlreadyMoved;


    public movingSnakes_PathObjectParent pathParent;
    public rollingDice Dice;

    Coroutine MovePlayerPiece;

    public movingSnakes_PathPoint previousPathPoint;
    public movingSnakes_PathPoint currentPathPoint;

    public bool hasPlayerAlreadyMoved;

    public int totalLives = 3;

    private void Awake()
    {
        pathParent=FindObjectOfType<movingSnakes_PathObjectParent>();
        hasPlayerAlreadyMoved = false;
    }

    public void MakePlayerReadyToMove()
    {
        
    }

    public void MoveSteps()
    {
    }
    
}

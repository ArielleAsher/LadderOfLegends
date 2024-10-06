//changed if condition in boolToInt(19/6/24)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chess : MonoBehaviour
{
    public static Chess Instance { get; private set; }
    public int currentI; //representing the current index of the player // setter for setting the index and getter for getting the index
    public int r;
    public int targetWaypoint;
    private Dictionary<int, System.Func<List<int>>> pieceMoves;

    public int waypointIndex;
    int spinningWheelResult;

    //public GameObject spinningWheel; // This should be assigned in the Unity Editor //spinningWheel
    public GameObject player;

    public List<int> possibleMoves;
    public int currentPlayer;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: if you want the instance to persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }
    //methods:
    public void setPlayer(int currPlayer){
        currentPlayer = currPlayer;
    }
    public void ExecuteChessLogic(int spinningWheelResult)
    {
        // Initialize the piece move methods
        //this is the current index of player

        pieceMoves = new Dictionary<int, System.Func<List<int>>>()
        {
            { 1, Bishop },
            { 2, Queen },
            { 3, Pawn },
            { 4, Knight },
            { 5, Rook },
            { 6, King }
        };

        switch (currentPlayer)
        {
            case 1:
                player = GameObject.Find("Player1");
                break;
            case 2:
                player = GameObject.Find("Player2");
                break;
            case 3:
                player = GameObject.Find("Player3");
                break;
            case 4:
                player = GameObject.Find("Player4");
                break;
            default:
                Debug.LogError("Unexpected Dice.whosTurn value: " + Dice.whosTurn);
                break;
        }

        // Check if player GameObject was found
        if (player == null)
        {
            Debug.LogError("Player GameObject not found for Dice.whosTurn: " + Dice.whosTurn);
            return;
        }
        
        waypointIndex = player.GetComponent<FollowThePath>().waypointIndex;
        this.spinningWheelResult = spinningWheelResult;
        HandleMove(waypointIndex, spinningWheelResult);    //removed plus one here(19/6/2024)
    }
    //The function below has diceRoll result which is basically player abhi kaha hein, and spinning wheel se result
    public void HandleMove(int currentInd, int spinningWheelResult)       //GET SPINNING WHEEL RESULT AND DICE ROLL RESULT
    {
        

        // Set the current position based on dice roll result
        setPosition(currentInd);   

        // Get the possible moves for the chosen piece
        possibleMoves = GetPossibleMoves(spinningWheelResult); //HERE IS WHERE THE FINAL MOVES FOR ANY CHESSPIECE IS BEING SAVED
        //Debug.Log("Possible Moves: " + string.Join(", ", possibleMoves));
    }

    // Get the possible moves for the chosen piece
    private List<int> GetPossibleMoves(int pieceIndex)
    {
        if (pieceMoves.ContainsKey(pieceIndex))
        {
            return pieceMoves[pieceIndex]();
        }
        else
        {
            Debug.LogError("Invalid piece index: " + pieceIndex);
            return new List<int>();
        }
    }

    public List<int> boolToInt(bool[] moves)
    {
        List<int> arr = new List<int>();
        for (int i = 1; i < 101; i++)
        {
            if (moves[i] == true && i!=currentI)
            {
                arr.Add(i-1);
            }
        }
        return arr;
    }
    public void setPosition(int p)
    {
        currentI = p;
    }

    //KING
    private List<int> King()          //declarig all chesspieces private
    {
        //Debug.Log("possible move king");
        bool[] validMoves = new bool[101];
        //right
        targetWaypoint = currentI + 1;
        if(targetWaypoint>=0 && targetWaypoint<100)validMoves[targetWaypoint] = true;
        //left
        targetWaypoint = currentI - 1;
        if(targetWaypoint>=0 && targetWaypoint<100)validMoves[targetWaypoint] = true;
        //up
        if (currentI % 10 != 0 && currentI<91)
        {
            targetWaypoint = currentI + ((10 - (currentI % 10)) * 2) + 1;
        }
        else
        {
            targetWaypoint = currentI + 1;
        }
        if(targetWaypoint>=0 && targetWaypoint<100)validMoves[targetWaypoint] = true;
        //down
        if (currentI % 10 != 0 && currentI>10)
        {// if(currentI
            targetWaypoint = currentI - (((currentI % 10 - 1) * 2) + 1);
        }
        else
        {
            targetWaypoint = currentI - 19;
        }
        if(targetWaypoint>=0 && targetWaypoint<100)validMoves[targetWaypoint] = true;
        List<int> numbers = boolToInt(validMoves);
        
        return numbers;
    }
    //king over

    //PAWN
    private List<int> Pawn()
    {
        //Debug.Log("possible move queen");
        bool[] validMoves = new bool[101];
        if (currentI % 10 != 0)
        {
            targetWaypoint = currentI + ((10 - (currentI % 10)) * 2) + 1;
        }
        else
        {
            targetWaypoint = currentI + 1;
        }
        if(targetWaypoint>=0 && targetWaypoint<100)validMoves[targetWaypoint] = true;
        List<int> numbers = boolToInt(validMoves);
        
        return numbers;
    }
    //pawn over

    //ROOK
    private List<int> Rook()
    {
        r = currentI / 10;
        //Debug.Log("possible move queen");
        bool[] validMoves = new bool[101];
        //right
        int temp = currentI;
        //left or right
        r = currentI / 10;
        if (currentI % 10 == 0)//different methods for %10!=0 and %10==0{
        {
            targetWaypoint = currentI - 1;
            if (targetWaypoint <= (r + 1) * 10 && targetWaypoint >= (r * 10) && targetWaypoint >= 0 && targetWaypoint < 100) validMoves[targetWaypoint] = true;
            targetWaypoint--;
            if (targetWaypoint <= (r + 1) * 10 && targetWaypoint >= (r * 10) && targetWaypoint >= 0 && targetWaypoint < 100) validMoves[targetWaypoint] = true;

        }
        else if (currentI == 11 || currentI == 21 || currentI == 31 || currentI == 41 || currentI == 51 || currentI == 61 || currentI == 71 || currentI == 81 || currentI == 91)
        {
            targetWaypoint = currentI + 1;
            if (targetWaypoint <= (r + 1) * 10 && targetWaypoint >= (r * 10) && targetWaypoint >= 0 && targetWaypoint < 100) validMoves[targetWaypoint] = true;
            targetWaypoint++;
            if (targetWaypoint <= (r + 1) * 10 && targetWaypoint >= (r * 10) && targetWaypoint >= 0 && targetWaypoint < 100) validMoves[targetWaypoint] = true;
        }
        else
        {
            targetWaypoint = currentI - 1;
            if (targetWaypoint <= (r + 1) * 10 && targetWaypoint >= (r * 10) && targetWaypoint >= 0 && targetWaypoint < 100) validMoves[targetWaypoint] = true;
            targetWaypoint--;
            if (targetWaypoint <= (r + 1) * 10 && targetWaypoint >= (r * 10) && targetWaypoint >= 0 && targetWaypoint < 100) validMoves[targetWaypoint] = true;
            targetWaypoint = currentI + 1;
            if (targetWaypoint <= (r + 1) * 10 && targetWaypoint >= (r * 10) && targetWaypoint >= 0 && targetWaypoint < 100) validMoves[targetWaypoint] = true;
            targetWaypoint++;
            if (targetWaypoint <= (r + 1) * 10 && targetWaypoint >= (r * 10) && targetWaypoint >= 0 && targetWaypoint < 100) validMoves[targetWaypoint] = true;

        }
        for (int i = 1; i < 3; i++)
        {
            if (temp % 10 != 0)
            {
                temp = temp + ((10 - (temp % 10)) * 2) + 1;
            }
            else
            {
                temp = temp + 1;
            }
            if(temp>=0 && temp<100)validMoves[temp] = true;
        }
        List<int> numbers = boolToInt(validMoves);
        
        return numbers;
    }
    //rook over

    //QUEEN
    private List<int> Queen()
    {
        bool[] validMoves = new bool[101];
        //left or right
        r = currentI / 10;
        if(currentI%10==0)//different methods for %10!=0 and %10==0{
        {
            targetWaypoint = currentI - 1;
            if (targetWaypoint <= (r + 1) * 10 && targetWaypoint > (r * 10) && targetWaypoint >= 0 && targetWaypoint < 100) validMoves[targetWaypoint] = true;
            targetWaypoint--;
            if (targetWaypoint <= (r + 1) * 10 && targetWaypoint > (r * 10) && targetWaypoint >= 0 && targetWaypoint < 100) validMoves[targetWaypoint] = true;

        }
        else if (currentI==11 || currentI==21 || currentI==31 || currentI==41 || currentI==51 || currentI==61 || currentI==71 || currentI==81 || currentI == 91)
        {
            targetWaypoint = currentI + 1;
            if (targetWaypoint <= (r + 1) * 10 && targetWaypoint > (r * 10) && targetWaypoint >= 0 && targetWaypoint < 100) validMoves[targetWaypoint] = true;
            targetWaypoint++;
            if (targetWaypoint <= (r + 1) * 10 && targetWaypoint > (r * 10) && targetWaypoint >= 0 && targetWaypoint < 100) validMoves[targetWaypoint] = true;
        }
        else
        {
            targetWaypoint = currentI - 1;
            if (targetWaypoint <= (r + 1) * 10 && targetWaypoint > (r * 10) && targetWaypoint >= 0 && targetWaypoint < 100) validMoves[targetWaypoint] = true;
            targetWaypoint--;
            if (targetWaypoint <= (r + 1) * 10 && targetWaypoint > (r * 10) && targetWaypoint >= 0 && targetWaypoint < 100) validMoves[targetWaypoint] = true;
            targetWaypoint = currentI + 1;
            if (targetWaypoint <= (r + 1) * 10 && targetWaypoint > (r * 10) && targetWaypoint >= 0 && targetWaypoint < 100) validMoves[targetWaypoint] = true;
            targetWaypoint++;
            if (targetWaypoint <= (r + 1) * 10 && targetWaypoint > (r * 10) && targetWaypoint >= 0 && targetWaypoint < 100) validMoves[targetWaypoint] = true;

        }
        //up
        int temp = currentI;
        int tempr;// = temp / 10 +1;
        int diag1;
        int diag2;
        for (int i = 1; i < 3; i++)
        {
            if (temp % 10 != 0)
            {
                temp = temp + ((10 - (temp % 10)) * 2) + 1;
                tempr = temp / 10;
                if (temp>=0 && temp<100)validMoves[temp] = true;//up
                diag1 = temp + i;
                if (diag1<= (tempr+1)*10 && diag1 > tempr*10)//tempr jo bhi h +1 is the upper limit
                { //diag1<=(tempr+1)*10 &&
                    if(diag1>=0 && diag1<100)validMoves[diag1] = true;//right or left up
                }
                diag2 = temp - i;
                if (diag2 <= (tempr + 1) * 10 && diag2 > tempr * 10) //diag2 >= (tempr) * 10
                { //diag2<=(tempr+1)*10 &&
                    if(diag2>=0 && diag2<100)validMoves[diag2] = true;//right or left up
                }
                //tempr = temp / 10;
            }
            else
            {                
                temp = temp + 1;
                tempr = temp / 10;
                if (temp>=0 && temp<100)validMoves[temp] = true;//up
                diag1 = temp + i;
                if (diag1 <=tempr*10 && diag1> (tempr-1)*10)
                { //diag1<=(tempr+1)*10 &&
                    if(diag1>=0 && diag1<100)validMoves[diag1] = true;//right or left up
                }
                diag2 = temp - i;
                if (diag2 <= tempr * 10 && diag2 > (tempr - 1) * 10)
                { //diag2<=(tempr+1)*10 &&
                    if(diag2>=0 && diag2<100)validMoves[diag2] = true;//right or left up
                }
                //tempr = temp / 10;
            }

        }
        
        List<int> numbers = boolToInt(validMoves);
        
        return numbers;
    }
    //Queen over

    //BISHOP
    private List<int> Bishop()
    {
        //Debug.Log("possible move bishop");
        bool[] validMoves = new bool[101];                //The array r will be used to store the possible move positions for the bishop.	
        //up
        int temp = currentI;
        int tempr;  //remove if works well
        int diag1;
        int diag2;
        if (temp % 10 != 0)
        {
            tempr = (temp / 10)+1;
            temp = temp + ((10 - (temp % 10)) * 2) + 1;
            diag1 = temp + 1;
            if (diag1 > tempr*10 && diag1<= (tempr+1)* 10) //diag1 >= (tempr - 1) * 10
            { //diag1<=(tempr+1)*10 &&
                if (diag1 >= 0 && diag1 < 100) validMoves[diag1] = true;//right or left up
            }
            diag2 = temp - 1;
            if (diag2 > tempr * 10 && diag2 <= (tempr + 1) * 10)      //diag2 >= (temp / 10) * 10
            { //diag2<=(tempr+1)*10 &&
                if (diag2 >= 0 && diag2 < 100) validMoves[diag2] = true;//right or left up
            }
            if(temp % 10 != 0)
            {
                tempr = (temp / 10) + 1;
                temp = temp + ((10 - (temp % 10)) * 2) + 1;
                diag1 = temp + 2;
                if (diag1 > tempr * 10 && diag1 <= (tempr + 1) * 10)
                { //diag1<=(tempr+1)*10 &&
                    if (diag1 >= 0 && diag1 < 100) validMoves[diag1] = true;//right or left up
                }
                diag2 = temp - 2;
                if (diag2 > tempr * 10 && diag2 <= (tempr + 1) * 10)
                { //diag2<=(tempr+1)*10 &&
                    if (diag2 >= 0 && diag2 < 100) validMoves[diag2] = true;//right or left up
                }
            }
            else
            {

                temp = temp +1;
                tempr = (temp / 10) + 1;
                diag1 = temp + 2; 
                if (diag1<=tempr * 10 && diag1 > (tempr - 1) * 10)   //diag1 > tempr * 10 && diag1 <= (tempr + 1) * 10
                { //diag1<=(tempr+1)*10 &&
                    if (diag1 >= 0 && diag1 < 100) validMoves[diag1] = true;//right or left up
                }

            }
            //tempr = temp / 10;  //remove if works well
        }
        else
        {
            temp = temp + 1;
            tempr = (temp / 10) + 1;
            diag1 = temp + 1;
            if (diag1 <= tempr * 10 && diag1 > (tempr - 1) * 10)
            { //diag1<=(tempr+1)*10 &&
                if (diag1 >= 0 && diag1 < 100) validMoves[diag1] = true;//right or left up
            }
            temp= temp + ((10 - (temp % 10)) * 2) + 1;
            tempr = temp / 10;
            diag1 = temp - 2;
            if(diag1<=tempr*10 && diag1 > (tempr - 1) * 10)
            {
                if (diag1 >= 0 && diag1 < 100) validMoves[diag1] = true;
            }
        }

        
        
        List<int> numbers = boolToInt(validMoves);
        
        return numbers;
    }
    //Bishop over

    private List<int> Knight()
    {
        //Debug.Log("possible move bishop");
        bool[] validMoves = new bool[101];                 //The array r will be used to store the possible move positions for the bishop.	
                                                           //up
        int temp = currentI;
        int tempr; 
        int diag11;
        int diag12;
        int diag21;
        int diag22;
        if (temp % 10 != 0)
        {
            temp = temp + ((10 - (temp % 10)) * 2) + 1;
            tempr = temp / 10;
            diag11 = temp + 2;
            if (diag11 >= (tempr - 1) * 10 )
            { //diag1<=(tempr+1)*10 &&
                if(diag11>=0 && diag11<100)validMoves[diag11] = true;//right or left up
            }
            diag12 = temp - 2;
            if (diag12 > (tempr) * 10 && diag12<= (tempr+1)*10)
            { //diag1<=(tempr+1)*10 &&
                if(diag12>=0 && diag12<100)validMoves[diag12] = true;//right or left up
            }
            if (temp % 10 != 0) { temp = temp + ((10 - (temp % 10)) * 2) + 1; diag21 = temp + 1; }
            else { temp = temp + 1; diag21 = temp + 1; }
            if (diag21 >= (tempr) * 10)
            { //diag2<=(tempr+1)*10 &&
                if (diag21 >= 0 && diag21 < 100) validMoves[diag21] = true;//right or left up
            }

            diag22 = temp - 1;
            if (diag22 >= (tempr) * 10)
            { //diag2<=(tempr+1)*10 &&
                if(diag22>=0 && diag22<100)validMoves[diag22] = true;//right or left up
            }
            tempr = temp / 10;
        }
        else
        {
            temp = temp + 1;
            tempr = temp / 10;
            diag11 = temp +2;
            if (diag11 >= (tempr - 1) * 10)
            { //diag1<=(tempr+1)*10 &&
                if(diag11>=0 && diag11<100)validMoves[diag11] = true;//right or left up
            }
            temp= temp + ((10 - (temp % 10)) * 2) + 1;
            diag22 = temp -1;
            if (diag22 >= (tempr) * 10)
            { //diag2<=(tempr+1)*10 &&
                if(diag22>=0 && diag22<100)validMoves[diag22] = true;//right or left up
            }
            tempr = temp / 10;
        }
        List<int> numbers = boolToInt(validMoves);
        
        return numbers;
    }
    //Knight over
    //All chess pieces function initialisation over

    
    // Update is called once per frame
    
}

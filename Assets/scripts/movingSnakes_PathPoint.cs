using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingSnakes_PathPoint : MonoBehaviour
{
    public movingSnakes_PathObjectParent pathObjectParent;
    public List<PlayerScript> playerPiece = new List<PlayerScript>();
    public AudioSource audio;

    private void Start()
    {
        pathObjectParent = GetComponentInParent<movingSnakes_PathObjectParent>();
    }

    void Update()
    {
        SoundToggle();
    }
    public void AddPlayerPiece(PlayerScript pathPoint)
    {
        if (playerPiece.Count > 0)
        {
            killPlayer();
            audio.Play();
        }
        playerPiece.Add(pathPoint);
    }

    public void killPlayer()
    {
        if (playerPiece[0].numberOfStepsAlreadyMoved < 100)
        {
            playerPiece[0].isReady = false;
            playerPiece[0].numberOfStepsAlreadyMoved = 0;
            string playerPieceName = playerPiece[0].name;
            Vector3 newPosition = Vector3.zero;

            if (playerPieceName == "bluePlayerPiece")
            {
                movingSnakes_GameManager.gm.BlueOutPlayer = 0;
                newPosition = new Vector3(-0.854868f, -4.372014f,0);
                playerPiece[0].transform.position = newPosition;
            }
            else if (playerPieceName == "redPlayerPiece")
            {
                movingSnakes_GameManager.gm.RedOutPlayer = 0;
                newPosition = new Vector3(0.3248296f,-4.359261f,0);
                playerPiece[0].transform.position = newPosition;
            }
            else if (playerPieceName == "greenPlayerPiece")
            {
                movingSnakes_GameManager.gm.GreenOutPlayer = 0;
                newPosition = new Vector3(-0.3064683f,-4.352883f,0);
                playerPiece[0].transform.position = newPosition;
            }
            else if (playerPieceName == "yellowPlayerPiece")
            {
                movingSnakes_GameManager.gm.YellowOutPlayer = 0;
                newPosition = new Vector3(0.943374f,-4.372014f,0);
                playerPiece[0].transform.position = newPosition;
            }

            // playerPiece[0].transform.position = newPosition;
            // Debug.Log(playerPieceName + " position is : " + newPosition);

            RemovePlayerPiece(playerPiece[0]);
        }
    }


    public void RemovePlayerPiece(PlayerScript pathPoint)
    {
        if (playerPiece.Contains(pathPoint))
        {
            playerPiece.Remove(pathPoint);
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

    // public void RescallingAndRepositioningForAllPlayerPiece()
    // {
    //     int plsCount = playerPiece.Count;
    //     bool isOdd = plsCount % 2 != 0;

    //     int extent = plsCount / 2;
    //     int counter = 0;
    //     int spriteLayer = 0;

    //     if (isOdd)
    //     {
    //         for (int i = -extent; i <= extent; i++)
    //         {
    //             playerPiece[counter].transform.localScale = new Vector3(pathObjectParent.scales[plsCount - 1], pathObjectParent.scales[plsCount - 1], 1f);
    //             playerPiece[counter].transform.position = new Vector3(transform.position.x + (i * pathObjectParent.positionDifference[plsCount - 1]), transform.position.y, 0f);
    //             counter++;
    //         }
    //     }
    //     else
    //     {
    //         for (int i = -extent; i < extent; i++)
    //         {
    //             playerPiece[counter].transform.localScale = new Vector3(pathObjectParent.scales[plsCount - 1], pathObjectParent.scales[plsCount - 1], 1f);
    //             playerPiece[counter].transform.position = new Vector3(transform.position.x + (i * pathObjectParent.positionDifference[plsCount - 1]), transform.position.y, 0f);
    //             counter++;
    //         }
    //     }
    //     for (int i = 0; i < playerPiece.Count; i++)
    //     {
    //         playerPiece[i].GetComponentInChildren<SpriteRenderer>().sortingOrder = spriteLayer;
    //         spriteLayer++;
    //     }
    // }

}

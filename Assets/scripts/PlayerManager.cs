using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public UIManager UImanager;
    int gameNo = -1;


    private void Start()
    {
        UImanager = GetComponent<UIManager>();
        gameNo = UImanager.gametype;
    }

    public void selectGameType(int totalPlayers)
    {

    }



}

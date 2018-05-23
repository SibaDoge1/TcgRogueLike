﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public GameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            Debug.LogError("SingleTone Error");
        }
    }


    private Map currentFloor;

    private void Start()    //Start of Everything
    {
        currentFloor = MapGenerator.instance.GetNewMap(0);

        //Locate Player
        //Player Init

        //Turn loop


    }


    public void OnEndPlayerTurn()
    {
        //Enemy's turn
    }

    public void OnEndEnemyTurn()
    {
        //Enable Input System
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {

    public static TurnManager instance;
    private void Awake()
    {
        instance = this;
    }
    private Room _currentRoom;
    public Room currentRoom
    {
        get
        {
            return _currentRoom;
        }
        set
        {
            _currentRoom = value;
            currentTurn = 0;
        }
    }
    public int currentTurn;



    public void MoveNextTurn()
    {
        currentRoom.DoTurn(currentTurn);
        currentTurn++;
    }
}

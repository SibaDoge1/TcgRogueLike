using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    private void Awake(){
		if (instance == null) {
			instance = this;
		} else {
			Destroy (this);
			Debug.LogError ("SingleTone Error");
		}
	}

	private void Start() {	//Start of Everything
		currentFloor = MapGenerator.GetNewMap(0);
        player = Instantiate(Resources.Load("Player") as GameObject).GetComponent<Player>();
        player.SetRoom(currentFloor.StartRoom, new Vector2Int(3,3) );

		//Turn loop
	}

    
    private Map currentFloor;
	public Room GetCurrentRoom(){
		return currentFloor.CurrentRoom;
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

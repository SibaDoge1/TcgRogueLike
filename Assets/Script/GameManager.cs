using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
		//DECK CONSTRUCTION
		PlayerData.deck.Clear();
		for (int i = 0; i < 7; i++) {
			PlayerData.deck.Add (new CardData_Sword (1));
		}
		PlayerData.deck.Add (new CardData_BFSword (3));
		PlayerData.deck.Add (new CardData_Tumble (4));
		PlayerData.deck.Add (new CardData_Arrow (7));
        currentFloor = MapGenerator.GetNewMap(0, new Vector2Int(5, 5), 10);
        PlayerControl.instance.ReLoadDeck();
        PlayerControl.instance.InitPlayer(currentFloor.StartRoom);
		EnemyControl.instance.InitEnemy (currentFloor.StartRoom);
		MinimapRenderer.instance.Init (currentFloor);
	}

    
    private Map currentFloor;
	public Room GetCurrentRoom(){
		return currentFloor.CurrentRoom;
	}
	public void SetCurrentRoom(Room room_){
		currentFloor.CurrentRoom = room_;
	}
    
	public void OnPlayerEnterNewRoom(){
		if (currentFloor.CurrentRoom.IsCleares == false) {
			EnemyControl.instance.InitEnemy (currentFloor.CurrentRoom);
			MinimapRenderer.instance.RenderRoom (currentFloor.CurrentRoom);
		}
	}

	public void OnPlayerClearRoom(){
		MinimapRenderer.instance.DoorOpen (currentFloor.CurrentRoom);
	}

    public void OnEndPlayerTurn()
    {
		EnemyControl.instance.EnemyTurn ();
		MinimapRenderer.instance.PlayerTileRefresh (currentFloor.CurrentRoom);
    }

    public void OnEndEnemyTurn()
    {
		if (PlayerControl.instance.GetRemainAction () <= 0) {
			InputModule.IsPlayerTurn = true;
		}
        else if (PlayerControl.instance.MoveReserveResume() == false) {
			InputModule.IsPlayerTurn = true;
		}
    }

    public void ReGame()
    {
        SceneManager.LoadScene(0);
    }
}

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
		for (int i = 0; i < 3; i++) {
			PlayerData.deck.Add (new CardData_Sword (1,PlayerControl.instance.PlayerObject, Attribute.APAS));
		}
        for (int i = 0; i < 3; i++)
        {
            PlayerData.deck.Add(new CardData_Sword(1, PlayerControl.instance.PlayerObject, Attribute.PRITHVI));
        }
        for (int i = 0; i < 3; i++)
        {
            PlayerData.deck.Add(new CardData_Sword(1, PlayerControl.instance.PlayerObject, Attribute.TEJAS));
        }
        PlayerData.deck.Add (new CardData_BFSword (3, PlayerControl.instance.PlayerObject,Attribute.AK));
		PlayerData.deck.Add (new CardData_Tumble (4, PlayerControl.instance.PlayerObject));
		PlayerData.deck.Add (new CardData_Arrow (7, PlayerControl.instance.PlayerObject, Attribute.AK));
       
        currentFloor = MapGenerator.GetNewMap(Config.instance.floorNum,Config.instance.roomNum);
        PlayerControl.instance.ReLoadDeck();
        PlayerControl.instance.InitPlayer(currentFloor.StartRoom);
        EnemyControl.instance.InitEnemy (currentFloor.StartRoom);
        //MinimapRenderer.instance.Init (currentFloor);
    }


    public Map currentFloor;
	public Room GetCurrentRoom(){
		return currentFloor.CurrentRoom;
	}
	public void SetCurrentRoom(Room room_){
		currentFloor.CurrentRoom = room_;
	}
    
	public void OnPlayerEnterNewRoom(){
		if (currentFloor.CurrentRoom.IsCleares == false) {
			EnemyControl.instance.InitEnemy (currentFloor.CurrentRoom);
			//MinimapRenderer.instance.RenderRoom (currentFloor.CurrentRoom);
		}
	}

	public void OnPlayerClearRoom(){
        PlayerControl.instance.ReLoadDeck();
        //MinimapRenderer.instance.DoorOpen (currentFloor.CurrentRoom);
	}

    public void OnEndPlayerTurn()
    {
		EnemyControl.instance.EnemyTurn ();
		//MinimapRenderer.instance.PlayerTileRefresh (currentFloor.CurrentRoom);
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

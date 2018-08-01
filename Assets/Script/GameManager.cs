using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Turn
{
    PLAYER,
    ENEMY
}
public class GameManager : MonoBehaviour {

    private Turn currentTurn;
    public Turn CurrentTurn
    {
        get { return currentTurn; }
        set { currentTurn = value; }
    }


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
		PlayerData.Clear();
		for (int i = 0; i < 18; i++) {
			PlayerData.Deck.Add (new Card_Sword (1,PlayerControl.Player,(Attribute)Random.Range(0,4)));
		}
        PlayerData.Deck.Add (new Card_BFSword (3, PlayerControl.Player,Attribute.AK));
		PlayerData.Deck.Add (new CardData_Tumble (4, PlayerControl.Player));
		PlayerData.Deck.Add (new Card_Arrow (7, PlayerControl.Player, Attribute.AK));
       
        for(int i=0; i<4; i++)
        {
            PlayerData.AttainCards.Add(new Card_BFSword(3, PlayerControl.Player, Attribute.AK));
        }

        currentFloor = MapGenerator.GetNewMap(Config.instance.floorNum,Config.instance.roomNum);

        MinimapTexture.Init(currentFloor);

        PlayerControl.instance.ReLoadDeck();
        PlayerControl.instance.InitPlayer(currentFloor.StartRoom);
        EnemyControl.instance.SetRoom (currentFloor.StartRoom);

        MinimapTexture.DrawDoors(GetCurrentRoom().transform.position, GetCurrentRoom().doorList);
        MinimapTexture.DrawPlayerPos(GetCurrentRoom().transform.position, PlayerControl.Player.pos);

        UIManager.instance.AkashaCountUpdate(PlayerData.AkashaCount);
        UIManager.instance.AkashaUpdate(PlayerData.AkashaGage, 10);
    }


    public Map currentFloor;
	public Room GetCurrentRoom(){
		return currentFloor.CurrentRoom;
	}
	public void SetCurrentRoom(Room room_){
		currentFloor.CurrentRoom = room_;
	}
    
	public void OnPlayerEnterRoom()
    {
		if (currentFloor.CurrentRoom.IsVisited == false)
        {
            currentFloor.CurrentRoom.IsVisited = true;
            EnemyControl.instance.SetRoom (currentFloor.CurrentRoom);
			MinimapTexture.DrawRoom (currentFloor.CurrentRoom);
		}
    }

	public void OnPlayerClearRoom(){
        //PlayerData.AkashaGage = 0;
        PlayerControl.instance.ReLoadDeck();
        MinimapTexture.DrawDoors (GetCurrentRoom().transform.position, GetCurrentRoom().doorList);
	}


    /// <summary>
    /// 호출하자마자 Turn은 EnemyTurn으로 바꾸고
    /// float나 코루틴 삽입시 그만큼 기달리고 적행동 시작 (기본=0.17f)
    /// +)반드시 PlayerControler 클래스에서만 호출하도록 할것
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public void OnEndPlayerTurn()
    {
        CurrentTurn = Turn.ENEMY;
        MinimapTexture.DrawPlayerPos(GetCurrentRoom().transform.position, PlayerControl.Player.pos);
        EnemyControl.instance.EnemyTurn();
    }




    /// <summary>
    /// 반드시 EnemyControler 클래스에서만 호출하도록 할것
    /// </summary>
    public void OnEndEnemyTurn()
    {
        currentTurn = Turn.PLAYER;
        PlayerControl.instance.CountDebuff();
        MinimapTexture.DrawEnemies(GetCurrentRoom().transform.position, GetCurrentRoom().GetEnemyPoses());
    }

    public void GameOver()
    {
        UIManager.instance.GameOver();
    }
    public void GameWin()
    {
        UIManager.instance.GameWin();
    }
    public void ReGame()
    {
        SceneManager.LoadScene(0);
    }

    private bool isGamePaused;
    public bool IsGamePaused
    {
        get { return isGamePaused; }
        set { isGamePaused = value; }
    }
    public void GamePauseOn()
    {
        isGamePaused = true;
        Time.timeScale = 0;
    }
    public void GamePauseOff()
    {
        isGamePaused = false;
        Time.timeScale = 1;
    }

}

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
		for (int i = 0; i < 9; i++) {
			PlayerData.PlayerCards.Add (new Card_Sword (1,PlayerControl.Player,(Attribute)Random.Range(0,4)));
		}
        PlayerData.PlayerCards.Add (new Card_BFSword (3, PlayerControl.Player,Attribute.AK));
		PlayerData.PlayerCards.Add (new CardData_Tumble (4, PlayerControl.Player));
		PlayerData.PlayerCards.Add (new Card_Arrow (7, PlayerControl.Player, Attribute.AK));
       
        currentFloor = MapGenerator.GetNewMap(Config.instance.floorNum,Config.instance.roomNum);

        MinimapTexture.Init(currentFloor);

        PlayerControl.instance.ReLoadDeck();
        PlayerControl.instance.InitPlayer(currentFloor.StartRoom);
        EnemyControl.instance.SetRoom (currentFloor.StartRoom);

        MinimapTexture.DrawDoors(currentFloor.CurrentRoom);
        MinimapTexture.DrawPlayerPos(PlayerControl.Player.transform.position);

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
        PlayerData.AkashaGage = 0;
        PlayerControl.instance.ReLoadDeck();
        MinimapTexture.DrawDoors (currentFloor.CurrentRoom);
	}


    /// <summary>
    /// 반드시 PlayerControler 클래스에서만 호출하도록 할것
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public void OnEndPlayerTurn(float time = 0.12f)
    {
        CurrentTurn = Turn.ENEMY;
        if (playerTurnDelay != null)
        {
            StopCoroutine(playerTurnDelay);
        }
        playerTurnDelay = StartCoroutine(PlayerTurnDelay(time));
    }
    Coroutine playerTurnDelay;
    IEnumerator PlayerTurnDelay(float time)
    {
        yield return new WaitForSeconds(time);
        EnemyControl.instance.EnemyTurn();
        MinimapTexture.DrawPlayerPos(PlayerControl.Player.transform.position);
    }

    /// <summary>
    /// 반드시 EnemyControler 클래스에서만 호출하도록 할것
    /// </summary>
    public void OnEndEnemyTurn()
    {
        currentTurn = Turn.PLAYER;
        PlayerControl.instance.CountDebuff();

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

}

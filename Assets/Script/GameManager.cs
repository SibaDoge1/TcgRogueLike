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
			UnityEngine.Debug.LogError ("SingleTone Error");
		}
	}

	private void Start() {  //Start of Everything
        PlayerData.Clear();
        BuildDeck();
        for(int i=0; i<10;i++)
        {
            PlayerData.AttainCards.Add(new Card_CroAtt());
        }

        #region AttainPool
        for (int i=0; i<4; i++)
        {
            PlayerData.AttainCards.Add(new Card_SquAtt());
        }
        #endregion

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
        //TODO : Room CLear CardPool
        PlayerData.AttainCards.Add(new Card_CroAtt());
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

    #region private
    private void BuildDeck()
    {
        if(Config.instance.UseCustomDeck)
        {
            string[] temp = Config.instance.CustomDeck;
            for (int i = 0; i < temp.Length; i++)
            {
                if(System.Type.GetType(temp[i]) != null)
                {
                    var c = System.Activator.CreateInstance(System.Type.GetType(temp[i]));
                    if(c is CardData)
                    {
                        PlayerData.Deck.Add(c as CardData);
                    }else
                    {
                        UnityEngine.Debug.Log("Card String Error");
                    }
                }
                else
                {
                    UnityEngine.Debug.Log("Card String Error");
                }
            }
        }
        else
        {
            for (int i = 0; i < 2; i++)
            {
                PlayerData.Deck.Add(new Card_SquAtt());
            }
            for (int i = 0; i < 2; i++)
            {
                PlayerData.Deck.Add(new Card_CroAtt());
            }
            for (int i = 0; i < 2; i++)
            {
                PlayerData.Deck.Add(new Card_XAtt());
            }
        }
    }

    #endregion
}

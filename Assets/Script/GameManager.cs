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
    private bool isInputOk = true;
    public bool IsInputOk
    {
        get { return isInputOk; }
        set { isInputOk = value; }
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
    //Start of Everything
    private void Start()
    {
        SetSeed();

        PlayerControl player = MakePlayer();
        PlayerData.Clear();
        BuildDeck();

        currentMap = GetMap();
        MinimapTexture.Init(CurrentMap);
        SetingtPlayer(player);

        EnemyControl.instance.SetRoom (CurrentMap.StartRoom);

        MinimapTexture.DrawPlayerPos(CurrentRoom().transform.position, PlayerControl.Player.pos);

        UIManager.instance.AkashaCountUpdate(PlayerData.AkashaCount);
        UIManager.instance.AkashaUpdate(PlayerData.AkashaGage, 10);
        IsInputOk = false;
        UIManager.instance.ShowTextUI("GameStart!", new CallBack(delegate () { IsInputOk = true; }));
    }

    private Map currentMap;
    public Map CurrentMap
    {
        get { return currentMap; }
    }
	public Room CurrentRoom(){
		return CurrentMap.CurrentRoom;
	}
	public void SetCurrentRoom(Room room_)
    {
        if(currentMap.CurrentRoom != null)
        {
            currentMap.SetRoomOff(currentMap.CurrentRoom);
        }
        CurrentMap.CurrentRoom = room_;
        currentMap.SetRoomOn(CurrentMap.CurrentRoom);
    }

    public void OnPlayerEnterRoom(Room newRoom)
    {

        if (newRoom.IsVisited == false)
        {
            newRoom.IsVisited = true;
            EnemyControl.instance.SetRoom (newRoom);
			MinimapTexture.DrawRoom (newRoom);
		}
       SetCurrentRoom(newRoom);
    }

    public void OnPlayerClearRoom(){
        PlayerControl.instance.ReLoadDeck();
        if(CurrentRoom().roomType == RoomType.BATTLE)
        {
            GetRandomCardToAttain(CurrentMap.Floor);
        }
    }


    /// <summary>
    /// 호출하자마자 Turn은 EnemyTurn으로 바꾸고
    /// float나 코루틴 삽입시 그만큼 기달리고 적행동 시작 (기본=0.17f)
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public void OnEndPlayerTurn(float time = 0.1f)
    {
        if(act!=null)
        {
            StopCoroutine(act);
        }
       act = StartCoroutine(EndTurnDelay(time));
    }
    Coroutine act;
    IEnumerator EndTurnDelay(float time)
    {
        //PlayerControl.instance.EnableCards(false);
        CurrentTurn = Turn.ENEMY;
        yield return new WaitForSeconds(time);
        MinimapTexture.DrawPlayerPos(CurrentRoom().transform.position, PlayerControl.Player.pos);
        EnemyControl.instance.EnemyTurn();
    }



    /// <summary>
    /// 반드시 EnemyControler 클래스에서만 호출하도록 할것
    /// </summary>
    public void OnEndEnemyTurn()
    {
        //PlayerControl.instance.EnableCards(true);
        currentTurn = Turn.PLAYER;
        PlayerControl.instance.CountDebuff();
        MinimapTexture.DrawEnemies(CurrentRoom().transform.position, CurrentRoom().GetEnemyPoses());
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
                var c = CardData.GetCardByName(temp[i]);
                if(c == null)
                {
                    Debug.Log("ERROR : 커스텀덱의 카드명을 확인해주세요");
                }else
                {
                    PlayerData.Deck.Add(CardData.GetCardByName(temp[i]));
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
    private void GetRandomCardToAttain(int floor)
    {
        if (floor == 1)
        {
            if (MyRandom.RandomEvent(Config.instance.DropRate[0].lowerCard, Config.instance.DropRate[0].higherCard) == 1) // R5풀,R4풀 17:3 확률
            {
                PlayerControl.instance.AddToAttain(CardData.GetCardByName(CardDatabase.R5Pool[Random.Range(0, CardDatabase.R5Pool.Length)]));
            }
            else
            {
                PlayerControl.instance.AddToAttain(CardData.GetCardByName(CardDatabase.R4Pool[Random.Range(0, CardDatabase.R4Pool.Length)]));
            }
        }
    }
    private PlayerControl MakePlayer()
    {
        return Instantiate(ResourceLoader.instance.LoadPlayer()).GetComponent<PlayerControl>();
    }
    private void SetingtPlayer(PlayerControl pc)
    {
        pc.deck = UIManager.instance.GetDeck();
        pc.hand = UIManager.instance.GetHand();
        pc.ReLoadDeck();
        pc.InitPlayer(CurrentMap.StartRoom);
    }
    private void SetSeed()
    {
        if (Config.instance.UseRandomSeed)
        {
            MyRandom.SetSeed(Random.Range(int.MinValue, int.MaxValue));
        }
        else
        {
            MyRandom.SetSeed(Config.instance.Seed);
        }
    }
    private Map GetMap()
    {
        if (Config.instance.RoomTestMode)
        {
              return MapGenerator.instance.GetTestMap(Config.instance.floorNum,
                Config.instance.TestRoomType, Config.instance.TestRoomName);
        }
        else
        {
            return MapGenerator.instance.GetMap(Config.instance.floorNum,
                Config.instance.battleRoomNum, Config.instance.eventRoomNum, Config.instance.shopRoomNum);
        }
    }
    #endregion
}

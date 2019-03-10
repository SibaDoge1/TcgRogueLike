using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Turn
{
    PLAYER,
    ENEMY
}

public delegate void OnRoomClearDelegater();

public class GameManager : MonoBehaviour
{
    private Player player;
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
    private Dictionary<string, bool> endingConditions;
    public Dictionary<string, bool> EndingConditions
    {
        get
        {
            return endingConditions;
        }
        set
        {
            endingConditions = value;
        }
    }

    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            UnityEngine.Debug.LogError("SingleTone Error : " + this.name);
            Destroy(this);
        }
    }
    //Start of Everything
    private void Start()
    {
        SetSeed();
        EndingConditions = new Dictionary<string, bool>();
        EndingConditions.Add("Pablus", false);
        EndingConditions.Add("Xynus", false);

        if(!ArchLoader.instance.IsCached)
        {
            ArchLoader.instance.StartCache();
            Database.ReadDatas();
        }

        player = ArchLoader.instance.GetPlayer();
        PlayerData.Clear();
        BuildDeck();

        LoadLevel(Config.instance.floorNum);
        
    }
    
    public void LoadLevel(int level)
    {
        StopAllCoroutines();
        DestroyMap();
        currentMap = Instantiate(Resources.Load<GameObject>("Map")).GetComponent<Map>();
        SetMap(level);

        MinimapTexture.Init(currentMap);
        SettingtPlayer();

        MinimapTexture.DrawPlayerPos(CurrentRoom().transform.position, PlayerControl.player.pos);

        UIManager.instance.AkashaUpdate(PlayerData.AkashaGage);
        UIManager.instance.FloorCount(level);
    }
  

    private Map currentMap;
    public Map CurrentMap
    {
        get { return currentMap; }
    }
    public Room CurrentRoom()
    {
        return CurrentMap.CurrentRoom;
    }
    public void SetCurrentRoom(Room room_)
    {
        if (currentMap.CurrentRoom != null)
        {
            currentMap.SetRoomOff(currentMap.CurrentRoom);
        }
        CurrentMap.CurrentRoom = room_;
        UIManager.instance.RoomDebugText(CurrentRoom().RoomName);
        currentMap.SetRoomOn(CurrentMap.CurrentRoom);
    }

    public void OnPlayerEnterRoom(Room newRoom)
    {

        if (newRoom.roomType == RoomType.BOSS && newRoom.IsVisited == false)
        {
            PlayBossBGM(currentMap.Floor);
        }
        else
        {
            PlayBGM(currentMap.Floor);
        }

        if (newRoom.IsVisited == false)
        {
            newRoom.IsVisited = true;
            EnemyControl.instance.SetRoom(newRoom);
            MinimapTexture.DrawRoom(newRoom);
            //SoundDelegate.instance.PlayEffectSound(EffectSound.MOVE, Camera.main.transform.position);

            if (newRoom.roomType == RoomType.START)
            {
                newRoom.OpenDoors();
            }
        }

        SetCurrentRoom(newRoom);
    }

    public void OnPlayerClearRoom()
    {
        PlayerControl.instance.OnRoomClear();
        SoundDelegate.instance.PlayEffectSound(SoundEffect.ROOMCLEAR,player.transform.position);
        PlayerData.AkashaGage = 0;
        if (CurrentRoom().roomType == RoomType.BATTLE || CurrentRoom().roomType == RoomType.BOSS || CurrentRoom().roomType == RoomType.EVENT)
        {
            GetRandomCardToAttain(CurrentRoom().RoomName);
        }
    }


    /// <summary>
    /// 호출하자마자 Turn은 EnemyTurn으로 바꾸고
    /// float 삽입시 그만큼 기달리고 적행동 시작 (기본=0.17f)
    /// </summary>
    public void OnEndPlayerTurn(float time = 0.1f)
    {
        CurrentTurn = Turn.ENEMY;
        StartCoroutine(EndTurnDelay(time));
    }
    IEnumerator EndTurnDelay(float time)
    {
        yield return null;
        if(PlayerControl.player.PlayerAnim == null)
        {
            yield return new WaitForSeconds(time);
        }else
        {
            yield return PlayerControl.player.PlayerAnim;
        }
        MinimapTexture.DrawPlayerPos(CurrentRoom().transform.position, PlayerControl.player.pos);
        EnemyControl.instance.EnemyTurn();
    }



    /// <summary>
    /// 반드시 EnemyControler 클래스에서만 호출하도록 할것
    /// </summary>
    public void OnEndEnemyTurn()
    {
        currentTurn = Turn.PLAYER;
        PlayerControl.playerBuff.OnPlayerTurn();
        PlayerData.PlayerTurnStart();
        MinimapTexture.DrawEnemies(CurrentRoom().transform.position, CurrentRoom().GetEnemyPoses());
    }

    public void GameOver()
    {
        FadeTool.FadeOutIn(3.5f,2,UIManager.instance.GameOverUIOn);
        SoundDelegate.instance.PlayGameOverSound(BGM.NONE,3.5f);
        IsInputOk = false;
    }

    public void ReGame(int i)
    {
        SceneManager.LoadScene(i);
    }


    private bool isGamePaused;
    public bool IsGamePaused
    {
        get { return isGamePaused; }
        set { isGamePaused = value; }
    }
    public void GamePauseOn()
    {
        isInputOk = false;
        Time.timeScale = 0;
    }
    public void GamePauseOff()
    {
        isGamePaused = true;
        Time.timeScale = 1;
    }

    #region private
    /// <summary>
    /// 게임 시작시 덱 빌드
    /// </summary>
    private void BuildDeck()
    {
        if(Config.instance.CardTestMode)
        {
            for( int i=0; i<Config.instance.cardList.Length;i++)
            {
                PlayerData.Deck.Add(Card.GetCardByNum(Config.instance.cardList[i]));
            }
        }
        else
        {
            for (int i = 0; i < 10; i++)//노말카드 랜덤 12장 생성
            {
                PlayerData.Deck.Add(Card.GetCardByNum(91));
            }
            PlayerData.Deck.Add(Card.GetCardByNum(92));
            PlayerData.Deck.Add(Card.GetCardByNum(92));
            PlayerData.Deck.Add(Card.GetCardByNum(93));
            PlayerData.Deck.Add(Card.GetCardByNum(93));
            PlayerData.Deck.Add(Card.GetCardByNum(94));
        }
    }
    private void GetRandomCardToAttain(string name)
    {
        PlayerControl.instance.AddToAttain(Database.GetCardPool(name).GetRandomCard());       
    }
    
    private void SettingtPlayer()
    {
        player.gameObject.SetActive(true);
        PlayerControl pc = player.GetComponent<PlayerControl>();
        pc.deck = UIManager.instance.GetDeck();
        pc.hand = UIManager.instance.GetHand();
        pc.OnRoomClear();
        Card.SetPlayer(player);
        MyCamera.instance.PlayerTrace(player);
        player.EnterRoom(CurrentMap.StartRoom);
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

    private void SetMap(int level)
    {
        MapGenerator mapGenerator = currentMap.GetComponent<MapGenerator>();

            mapGenerator.GetMap(level,
                Config.instance.LevelSettings[level-1].battleRoomNum, Config.instance.LevelSettings[level-1].eventRoomNum,
                Config.instance.LevelSettings[level-1].bossRoom,Config.instance.LevelSettings[level-1].endRoom);        
    }
    private void DestroyMap()
    {
        PlayerControl.instance.transform.SetParent(null);
        if(currentMap != null)
        {
            //GameObject old = currentMap.gameObject;
            Destroy(currentMap.gameObject);
        }
    }

    private void PlayBGM(int floor)
    {
        switch (floor)
        {
            case 1:
                SoundDelegate.instance.PlayBGM(BGM.FIELD1);
                break;
            case 2:
                SoundDelegate.instance.PlayBGM(BGM.FIELD2);
                break;
            case 3:
                SoundDelegate.instance.PlayBGM(BGM.FIELD3);
                break;
            case 4:
                SoundDelegate.instance.PlayBGM(BGM.FIELD4);
                break;
            case 5:
                SoundDelegate.instance.PlayBGM(BGM.BOSSSPIDER);
                break;
        }
    }

    private void PlayBossBGM(int floor)
    {
        switch(floor)
        {
            case 2:
                SoundDelegate.instance.PlayBGM(BGM.BOSSFIRE);
                break;
            case 3:
                SoundDelegate.instance.PlayBGM(BGM.BOSSFIRE);
                break;
            case 4:
                SoundDelegate.instance.PlayBGM(BGM.BOSSROBOT);
                break;
            case 5:
                SoundDelegate.instance.PlayBGM(BGM.BOSSSPIDER);
                break;
        }
    }
    #endregion

}

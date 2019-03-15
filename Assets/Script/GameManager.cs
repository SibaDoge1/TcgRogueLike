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
    private bool isLoaded; //인게임세이브가 로드됬는가를 나타냄
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
    private bool xynus;
    public bool Xynus
    {
        get { return xynus; }
        set { xynus = value; }
    }
    private bool pablus;
    public bool Pablus
    {
        get { return pablus; }
        set { pablus = value; }
    }
    private int buildSeed;
    public int BuildSeed
    {
        get { return buildSeed; }
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
#if UNITY_EDITOR
        Application.targetFrameRate = 60;
        #endif
    }
    private int startLevel;
    private int startHp;
    List<Card> startDeck = new List<Card>();
    List<Card> startAttain = new List<Card>();

    //Start of Everything
    private void Start()
    {

        if(!ArchLoader.instance.IsCached)
        {
            ArchLoader.instance.StartCache();
            Database.ReadDatas();
        }

        player = ArchLoader.instance.GetPlayer();
        player.gameObject.SetActive(true);

        ObjectPoolManager.instance.MakeEffects();
        LoadIngameSaveData();
        LoadLevel(startLevel);       
    }
    
    public void LoadLevel(int level)
    {

        StopAllCoroutines();
        DestroyMap();
        currentMap = Instantiate(Resources.Load<GameObject>("Map")).GetComponent<Map>();
        if(isLoaded)
        {         
            Random.InitState(buildSeed);
            isLoaded = false;//초기화
        }else
        {
            buildSeed = Random.Range(int.MinValue, int.MaxValue);
            Random.InitState(buildSeed);
        }
        Debug.Log("Seed : "+buildSeed);
        BuildMap(level);
        MinimapTexture.Init(currentMap);

        player.EnterRoom(CurrentMap.StartRoom);
        MinimapTexture.DrawPlayerPos(CurrentRoom().transform.position, player.pos);
        UIManager.instance.FloorCount(level);
        PlayerControl.instance.AkashaGage = 0;
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
    public void SetCurrentRoom(Room newRoom)
    {
        if (currentMap.CurrentRoom != null)
        {
            currentMap.SetRoomOff(currentMap.CurrentRoom);
        }
        CurrentMap.CurrentRoom = newRoom;
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
        PlayerControl.instance.AkashaGage = 0;
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
        PlayerControl.instance.OnPlayerTurn();
        MinimapTexture.DrawEnemies(CurrentRoom().transform.position, CurrentRoom().GetEnemyPoses());
    }

    public void GameOver()
    {
        FadeTool.FadeOutIn(3.5f,2,UIManager.instance.GameOverUIOn);
        SoundDelegate.instance.PlayGameOverSound(BGM.NONE,3.5f);
        IsInputOk = false;
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
    private void GetRandomCardToAttain(string name)
    {
        PlayerControl.instance.AddToAttain(Database.GetCardPool(name).GetRandomCard());       
    }
    
    private void LoadIngameSaveData()
    {
        PlayerControl pc = player.GetComponent<PlayerControl>();
        pc.DeckManager = new DeckManager();
        pc.HandManager = UIManager.instance.GetHand();

        pc.DeckManager.Deck = startDeck;
        pc.DeckManager.AttainCards = startAttain;
        pc.HandManager.MakeCards(5);


        if (InGameSaveManager.CheckSaveData())
        {
            List<int> deckData = InGameSaveManager.DeckCards;
            List<int> attainData = InGameSaveManager.AttainCards;

            for (int i = 0; i < deckData.Count; i++)
            {
                startDeck.Add(Card.GetCardByNum(deckData[i]));
            }
            for (int i = 0; i < attainData.Count; i++)
            {
                startAttain.Add(Card.GetCardByNum(attainData[i]));
            }

            Pablus = InGameSaveManager.Pablus;
            Xynus = InGameSaveManager.Xynus;
            startLevel = InGameSaveManager.Floor;
            startHp = InGameSaveManager.Hp;
            buildSeed = InGameSaveManager.Seed;
            isLoaded = true;

            InGameSaveManager.ClearSaveData();
        }
        else
        {
            for (int i = 0; i < 10; i++)//노말카드 랜덤 12장 생성
            {
                startDeck.Add(Card.GetCardByNum(91));
            }
            startDeck.Add(Card.GetCardByNum(92));
            startDeck.Add(Card.GetCardByNum(92));
            startDeck.Add(Card.GetCardByNum(93));
            startDeck.Add(Card.GetCardByNum(93));
            startDeck.Add(Card.GetCardByNum(94));

            Pablus = false;
            Xynus = false;
            startLevel = 1;
            startHp = 10;

            isLoaded = false;
        }


        pc.ReLoadDeck();

        player.SetHp(startHp);
        Card.SetPlayer(player);
        MyCamera.instance.PlayerTrace(player);
    }

    private void BuildMap(int level)
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

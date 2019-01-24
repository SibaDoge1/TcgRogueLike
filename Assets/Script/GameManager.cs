using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Turn
{
    PLAYER,
    ENEMY
}

public class GameManager : MonoBehaviour
{

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
    private void Awake()
    {
        #region 안드로이드 설정
        Input.multiTouchEnabled = false;
        Application.targetFrameRate = 60;
        Screen.orientation = ScreenOrientation.Landscape;
        #endregion

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            UnityEngine.Debug.LogError("SingleTone Error");
        }
    }
    //Start of Everything
    private void Start()
    {
        SetSeed();

        ReadDatas();
        ArchLoader.instance.GetPlayer();
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
        SettingtPlayer(PlayerControl.instance);

        EnemyControl.instance.SetRoom(CurrentMap.StartRoom);

        MinimapTexture.DrawPlayerPos(CurrentRoom().transform.position, PlayerControl.Player.pos);

        UIManager.instance.AkashaUpdate(PlayerData.AkashaGage);

        if (currentMap.Floor == 1)
            SoundDelegate.instance.PlayBGM(BGM.FLOOR1);
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
        currentMap.SetRoomOn(CurrentMap.CurrentRoom);
    }

    public void OnPlayerEnterRoom(Room newRoom)
    {

        if (newRoom.IsVisited == false)
        {
            newRoom.IsVisited = true;
            EnemyControl.instance.SetRoom(newRoom);
            MinimapTexture.DrawRoom(newRoom);
            SoundDelegate.instance.PlayEffectSound(EffectSoundType.RoomMove, Camera.main.transform.position);

            if (!(newRoom.roomType == RoomType.BATTLE) && !(newRoom.roomType == RoomType.BOSS))
            {
                newRoom.OpenDoors();
            }

            if (newRoom.roomType == RoomType.BOSS)
            {
                PlayBossBGM(currentMap.Floor);
            }
        }
        SetCurrentRoom(newRoom);
    }

    public void OnPlayerClearRoom()
    {
        PlayerControl.instance.ReLoadDeck();
        SoundDelegate.instance.PlayEffectSound(EffectSoundType.RoomClear, Camera.main.transform.position);
        PlayerData.AkashaGage = 0;
        if (CurrentRoom().roomType == RoomType.BATTLE)
        {
            GetRandomCardToAttain(CurrentRoom().RoomValue);
        }
    }


    /// <summary>
    /// 호출하자마자 Turn은 EnemyTurn으로 바꾸고
    /// float나 코루틴 삽입시 그만큼 기달리고 적행동 시작 (기본=0.17f)
    /// </summary>
    public void OnEndPlayerTurn(float time = 0.1f)
    {
        CurrentTurn = Turn.ENEMY;
        StartCoroutine(EndTurnDelay(time));
    }
    IEnumerator EndTurnDelay(float time)
    {
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
        PlayerData.PlayerTurnStart();
        MinimapTexture.DrawEnemies(CurrentRoom().transform.position, CurrentRoom().GetEnemyPoses());
    }

    public void GameOver()
    {
        SoundDelegate.instance.PlayEffectSound(EffectSoundType.GameOver, Camera.main.transform.position);
        UIManager.instance.GameOverUIOn();
        IsInputOk = false;
    }
    public void GameWin()
    {
        IsInputOk = false;
        UIManager.instance.GameWinUIOn();
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
        for(int i=0; i<12;i++) // 현재 랜덤으로 12장 생성
          PlayerData.Deck.Add(Card.GetCardByNum(0));     
    }
    private void GetRandomCardToAttain(int value)
    {
        PlayerControl.instance.AddToAttain(Database.GetCardPoolByValue(value).GetRandomCard());       
    }
    
    private void SettingtPlayer(PlayerControl pc)
    {
        pc.gameObject.SetActive(true);
        pc.deck = UIManager.instance.GetDeck();
        pc.hand = UIManager.instance.GetHand();
        pc.ReLoadDeck();
        Player player = pc.GetComponent<Player>();
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

        if (Config.instance.RoomTestMode)
        {
            mapGenerator.GetTestMap(level,
              Config.instance.TestRoomType, Config.instance.TestRoomName);
        }
        else
        {
            mapGenerator.GetMap(level,
                Config.instance.LevelSettings[level].battleRoomNum, Config.instance.LevelSettings[level].eventRoomNum, Config.instance.LevelSettings[level].shopRoomNum);
        }
    }
    private void PlayBossBGM(int floor)
    {
        switch(floor)
        {
            case 1:
              SoundDelegate.instance.PlayBGM(BGM.Floor1_BOSS);
                break;
            default:
                break;
        }
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
    private void ReadDatas()
    {
        Database.ReadDatas();//todo : 룸데이터도 여기서 아예 읽어오자
        ArchLoader.instance.StartCache();
    }
    #endregion
}

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
        Application.targetFrameRate = 60;
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

        MakePlayer();
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
        SetingtPlayer(PlayerControl.instance);

        EnemyControl.instance.SetRoom(CurrentMap.StartRoom);

        MinimapTexture.DrawPlayerPos(CurrentRoom().transform.position, PlayerControl.Player.pos);

        UIManager.instance.AkashaUpdate(PlayerData.AkashaGage, 10);

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
            GetRandomCardToAttain(CurrentMap.Floor);
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
    private void BuildDeck()
    {
        if (Config.instance.UseCustomDeck)
        {
            string[] temp = Config.instance.CustomDeck;
            for (int i = 0; i < temp.Length; i++)
            {
                var c = CardData.GetCardByName(temp[i]);
                if (c == null)
                {
                    Debug.Log("ERROR : 커스텀덱의 카드명을 확인해주세요");
                }
                else
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
        if (floor <= 1)
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
        pc.gameObject.SetActive(true);
        pc.deck = UIManager.instance.GetDeck();
        pc.hand = UIManager.instance.GetHand();
        pc.ReLoadDeck();
        Player player = pc.GetComponent<Player>();
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
        else if (level == 0)
        {
            mapGenerator.GetTutorialMap();
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

    #endregion
}

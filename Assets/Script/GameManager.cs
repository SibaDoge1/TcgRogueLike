﻿using System.Collections;
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

    public static GameManager instance;
    private void Awake()
    {
        //Changes: 안드로이드 설정 메인메뉴씬으로 옮김

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            UnityEngine.Debug.LogError("SingleTone Error: GameManager");
            Destroy(this);
        }
    }
    //Start of Everything
    private void Start()
    {
        SetSeed();

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

        //TODO : 방마다 BGM 바꾸게 변경
        PlayBGM(currentMap.Floor);
    }

    public void OnPlayerClearRoom()
    {
        PlayerControl.instance.OnRoomClear();
        SoundDelegate.instance.PlayEffectSound(EffectSoundType.RoomClear, Camera.main.transform.position);
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
        for(int i=0; i<10;i++) //노말카드 랜덤 9장 생성
          PlayerData.Deck.Add(Card.GetCardByNum(0));

        for(int i=0; i<3;i++)//특수카드 3장 생성
        {
            PlayerData.Deck.Add(Card.GetCardByNum(Random.Range(1,20)));
        }
        PlayerData.Deck.Add(Card.GetCardByNum(99));//Reload
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
                SoundDelegate.instance.PlayBGM(BGM.FLOOR1);
                break;
        }
    }

    private void PlayBossBGM(int floor)
    {
        switch(floor)
        {
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
        }
    }
    #endregion

}

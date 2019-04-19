using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private Transform canvas;
    private Tutorial tutorial;
    private GameObject _new;
    private GameObject loadPanel;
    private GameObject startPanel;
    private Option option;
    private Exit exitPanel;
    private Diary diary;
    private Intro intro;
    public delegate void voidFunc();
    public static bool isBtnEnable = false;

    void Awake()
    {
        //FadeTool.FadeIn(1f, ()=> { isBtnEnable = true; });
        #region 안드로이드 설정
        Input.multiTouchEnabled = false;
        Application.targetFrameRate = 60;
        #endregion
        canvas = GameObject.Find("Canvas").transform;
        tutorial = canvas.Find("Tutorial").gameObject.GetComponent<Tutorial>();
        _new = canvas.Find("NewIcon").gameObject ;
        option = canvas.Find("Option").gameObject.GetComponent<Option>();
        exitPanel = canvas.Find("ExitPanel").gameObject.GetComponent<Exit>();
        diary = canvas.Find("Diary").gameObject.GetComponent<Diary>();
        intro = canvas.Find("Intro").gameObject.GetComponent<Intro>();
        loadPanel = canvas.Find("LoadPanel").gameObject;
        startPanel = canvas.Find("StartPanel").gameObject;
        if (!SaveManager.isintroSeen)
        {
            SaveManager.isintroSeen = true;
            Database.ReadDatas();
            ArchLoader.instance.StartCache();
            SaveManager.LoadAll(false);
            loadPanel.SetActive(true);
            GooglePlayManager.Init();
            GooglePlayManager.LogIn(OnLogInComplete, OnLogInComplete);
                intro.On(() =>
                {
                    isBtnEnable = true;
                });
            
        }
#if UNITY_EDITOR
        OnLogInComplete();
#endif

    }

    void Start()
    {
        SoundDelegate.instance.PlayBGM(BGM.FIELDTITLECUT);
        if (!InGameSaveManager.CheckSaveData())//GTS : 인게임 세이브 체크 추가
        {
            canvas.Find("Btn_Continue").gameObject.SetActive(false);
        }
        CheckNew();
    }

    public void CheckNew()
    {
        if (SaveManager.CheckNew())
            _new.SetActive(true);
        else
            _new.SetActive(false);

    }

    public static void ButtonDown()
    {
        SoundDelegate.instance.PlayMono(MonoSound.BUTTONTITLE);
    }
    /// <summary>
    /// GTS : 이어하기 버튼 추가
    /// </summary>
    public void OnContinueButtonDown()
    {
        ButtonDown();
        LoadingManager.LoadScene("Levels/Floor0");
    }
    public void OnStartButtonDown()
    {
        if (!isBtnEnable) return;
        ButtonDown();
        //SceneManager.LoadScene("Levels/LoadingScene");
        if (InGameSaveManager.CheckSaveData())
        {
            startPanel.SetActive(true);
            return;
        }
        InGameSaveManager.ClearSaveData();//GTS : 인게임 세이브 데이터 초기화
        LoadingManager.LoadScene("Levels/Floor0");
    }

    public void GameStart()
    {
        InGameSaveManager.ClearSaveData();//GTS : 인게임 세이브 데이터 초기화
        LoadingManager.LoadScene("Levels/Floor0");
    }

    public void StartPanelOff()
    {
        startPanel.SetActive(false);
    }

    public void OnTutorialButtonDown()
    {
        if (!isBtnEnable) return;
        ButtonDown();
        tutorial.On();
    }

    public void OnDiaryButtonDown()
    {
        if (!isBtnEnable) return;
        ButtonDown();
        voidFunc checkNew = new voidFunc(CheckNew);
        diary.On(checkNew);
    }

    public void OnOptionButtonDown()
    {
        if (!isBtnEnable) return;
        ButtonDown();
        option.On();
    }

    public void OnExitButtonDown()
    {
        if (!isBtnEnable) return;
        ButtonDown();
        exitPanel.on();
    }

    public void OnLogInComplete()
    {
        loadPanel.SetActive(false);
    }


    public void SetDiaryAllTrue()
    {
        SaveManager.SetDiaryAllTrue();
    }
    public void SetCardAllTrue()
    {
        SaveManager.SetCardAllTrue();
    }
}

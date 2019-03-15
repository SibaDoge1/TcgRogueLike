using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private Tutorial tutorial;
    private GameObject _new;
    private Option option;
    private Exit exitPanel;
    private Diary diary;
    public delegate void voidFunc();
    public bool isBtnEnable;

    void Awake()
    {
        isBtnEnable = false;
        FadeTool.FadeIn(1f, ()=> { isBtnEnable = true; });
        #region 안드로이드 설정
        Input.multiTouchEnabled = false;
        Application.targetFrameRate = 60;
        #endregion
        tutorial = GameObject.Find("Canvas").transform.Find("Tutorial").gameObject.GetComponent<Tutorial>();
        _new = GameObject.Find("Canvas").transform.Find("NewIcon").gameObject ;
        option = GameObject.Find("Canvas").transform.Find("Option").gameObject.GetComponent<Option>();
        exitPanel = GameObject.Find("Canvas").transform.Find("ExitPanel").gameObject.GetComponent<Exit>();
        diary = GameObject.Find("Canvas").transform.Find("Diary").gameObject.GetComponent<Diary>();
        Database.ReadDatas();
        ArchLoader.instance.StartCache();
        GooglePlayManager.Init();
        
    }

    void Start()
    {
        SaveManager.LoadAll();
        CheckNew();
        SoundDelegate.instance.PlayBGM(BGM.FIELDTITLECUT);

        if(!InGameSave.SaveManager.CheckSaveData())//GTS : 인게임 세이브 체크 추가
        {
            GameObject.Find("Canvas").transform.Find("Btn_Continue").gameObject.SetActive(false);
        }
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
        InGameSave.SaveManager.ClearSaveData();//GTS : 인게임 세이브 데이터 초기화
        LoadingManager.LoadScene("Levels/Floor0");
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


    public void SetDiaryAllTrue()
    {
        SaveManager.SetDiaryAllTrue();
    }
    public void SetCardAllTrue()
    {
        SaveManager.SetCardAllTrue();
    }
}

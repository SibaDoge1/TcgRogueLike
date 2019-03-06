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

    void Awake()
    {
        FadeTool.FadeIn(1f, null);
        #region 안드로이드 설정
        Input.multiTouchEnabled = false;
        Application.targetFrameRate = 60;
        Screen.orientation = ScreenOrientation.Landscape;
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

    public void OnStartButtonDown()
    {
        ButtonDown();
        //SceneManager.LoadScene("Levels/LoadingScene");
        LoadingManager.LoadScene("Levels/Floor0");
    }

    public void OnTutorialButtonDown()
    {
        ButtonDown();
        tutorial.On();
    }

    public void OnDiaryButtonDown()
    {
        ButtonDown();
        voidFunc checkNew = new voidFunc(CheckNew);
        diary.On(checkNew);
    }

    public void OnOptionButtonDown()
    {
        ButtonDown();
        option.On();
    }

    public void OnExitButtonDown()
    {
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

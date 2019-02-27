using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private LoadingManager loadingPanel;
    private Tutorial tutorial;
    private GameObject _new;
    private Option option;
    private Exit exitPanel;
    private Diary diary;
    public delegate void voidFunc();

    void Awake()
    {
        #region 안드로이드 설정
        Input.multiTouchEnabled = false;
        Application.targetFrameRate = 60;
        Screen.orientation = ScreenOrientation.Landscape;
        #endregion
        loadingPanel = GameObject.Find("Canvas").transform.Find("LoadingPanel").GetComponent<LoadingManager>();
        tutorial = GameObject.Find("Canvas").transform.Find("Tutorial").gameObject.GetComponent<Tutorial>();
        _new = GameObject.Find("Canvas").transform.Find("NewIcon").gameObject ;
        option = GameObject.Find("Canvas").transform.Find("Option").gameObject.GetComponent<Option>();
        exitPanel = GameObject.Find("Canvas").transform.Find("ExitPanel").gameObject.GetComponent<Exit>();
        diary = GameObject.Find("Canvas").transform.Find("Diary").gameObject.GetComponent<Diary>();
        Database.ReadDatas();
        SaveData.FirstSetUp();
        GooglePlayManager.Init();
#if UNITY_ANDROID
        Debug.Log("안드로이드");
#endif
    }

    void Start()
    {
        //GooglePlayManager.LoadFromCloud();
        //GooglePlayManager.SaveToCloud();
        CheckNew();
    }

    public void CheckNew()
    {
        if (SaveData.CheckNew())
            _new.SetActive(true);
        else
            _new.SetActive(false);

    }

    public void OnStartButtonDown()
    {
        //SceneManager.LoadScene("Levels/LoadingScene");
        loadingPanel.LoadScene();
    }

    public void OnTutorialButtonDown()
    {
        tutorial.On();
    }

    public void OnDiaryButtonDown()
    {
        voidFunc checkNew = new voidFunc(CheckNew);
        diary.On(checkNew);
    }

    public void OnOptionButtonDown()
    {
        option.On();
    }

    public void OnExitButtonDown()
    {
        exitPanel.on();
    }


    public void SetDiaryAllTrue()
    {
        SaveData.SetDiaryAllTrue();
    }
    public void SetCardAllTrue()
    {
        SaveData.SetCardAllTrue();
    }

}

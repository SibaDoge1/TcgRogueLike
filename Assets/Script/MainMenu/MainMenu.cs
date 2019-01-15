using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    LoadingManager loadingPanel;
    Tutorial tutorial;
    GameObject diary;
    GameObject setting;

    void Awake()
    {
        #region 안드로이드 설정
        Input.multiTouchEnabled = false;
        Application.targetFrameRate = 60;
        Screen.orientation = ScreenOrientation.Landscape;
        #endregion
        loadingPanel = GameObject.Find("Canvas").transform.Find("LoadingPanel").GetComponent<LoadingManager>();
        tutorial = GameObject.Find("Canvas").transform.Find("Tutorial").gameObject.GetComponent<Tutorial>();
        //diary = GameObject.Find("Canvas").transform.Find("LoadingPanel").gameObject;
        //setting = GameObject.Find("Canvas").transform.Find("Setting").gameObject;
    }

    // Use this for initialization
    void Start()
    {
        Database.ReadDatas();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnStartButtonDown()
    {
        loadingPanel.LoadScene();
    }

    public void OnTutorialButtonDown()
    {
        tutorial.On();
    }

    public void OnDiaryButtonDown()
    {

    }

    public void OnSettingButtonDown()
    {

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    private Tutorial tuto;
    private Option opt;
    private Transform exit;
    void Awake()
    {
        opt = transform.Find("Option").GetComponent<Option>();
        tuto = transform.Find("Tutorial").GetComponent<Tutorial>();
        exit = transform.Find("ExitPanel");
    }

    public void On()
    {
        gameObject.SetActive(true);
    }

    public void Off()
    {
        gameObject.SetActive(false);
    }
    public void OnOptionButtonDown()
    {
        MainMenu.ButtonDown();
        opt.On();
    }

    public void OnTutoButtonDown()
    {
        MainMenu.ButtonDown();
        tuto.On();
    }

    public void OnTitleButtonDown()
    {
        MainMenu.ButtonDown();
        exit.gameObject.SetActive(true);
    }

    public void OnYesButtonDown()
    {
        MainMenu.ButtonDown();
        LoadingManager.LoadScene("Levels/MainMenu");
    }

    public void OnNoButtonDown()
    {
        MainMenu.ButtonDown();
        exit.gameObject.SetActive(false);
    }
    public void OnExitButtonDown()
    {
        MainMenu.ButtonDown();
        Off();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetUI : MonoBehaviour
{

    public void On()
    {
        gameObject.SetActive(true);
    }

    public void Off()
    {
        gameObject.SetActive(false);
    }

    public void OnYesButtonDown()
    {
        MainMenu.ButtonDown();
        SaveManager.CreateNew();
        NoticeTool.Notice("초기화 완료", 1.8f);
        Off();
    }

    public void OnNoButtonDown()
    {
        MainMenu.ButtonDown();
        Off();
    }
}

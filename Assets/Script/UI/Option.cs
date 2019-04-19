using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour {
    private Slider bgmSlider;
    private Slider fxSlider;
    private Slider UISlider;
    private ResetUI resetUI;

    void Awake()
    {
        bgmSlider = transform.Find("Slider_Bgm").GetComponent<Slider>();
        fxSlider = transform.Find("Slider_Fx").GetComponent<Slider>();
        UISlider = transform.Find("Slider_UI").GetComponent<Slider>();
        resetUI = transform.Find("Panel").GetComponent<ResetUI>();
    }

    void OnEnable()
    {
        SetSliderValues();
    }

    public void On()
    {
        gameObject.SetActive(true);
        SetSliderValues();
    }

    public void Off()
    {
        gameObject.SetActive(false);
    }

    public void SetSliderValues()
    {
        bgmSlider.value = SaveManager.GetBgmValue();
        fxSlider.value = SaveManager.GetFxValue();
        UISlider.value = SaveManager.GetUIValue();
    }

    public void OnBgmSliderChanged(Slider sld)
    {
        SaveManager.SetBgmValue(sld.value);
    }

    public void OnFxSliderChanged(Slider sld)
    {
        SaveManager.SetFxValue(sld.value);
    }

    public void OnUISliderChanged(Slider sld)
    {
        SaveManager.SetUIValue(sld.value);
    }

    public void OnExitButtonDown()
    {
        MainMenu.ButtonDown();
        Off();
    }

    public void OnAchiveButtonDown()
    {
        MainMenu.ButtonDown();
        GooglePlayManager.ShowAchievementUI();
    }

    public void OnCloudButtonDown()
    {
        MainMenu.ButtonDown();
        SaveManager.LoadAll(true, OnLoadComplete);
    }

    public void OnResetButtonDown()
    {
        MainMenu.ButtonDown();
        resetUI.On();
    }

    public void OnLoadComplete()
    {
        NoticeTool.Notice("Cloud Load Complete!", 2f);
    }
}

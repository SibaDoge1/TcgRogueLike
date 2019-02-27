using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour {
    private Slider bgmSlider;
    private Slider fxSlider;
    private Slider UISlider;

    void Awake()
    {
        bgmSlider = transform.Find("Slider_Bgm").GetComponent<Slider>();
        fxSlider = transform.Find("Slider_Fx").GetComponent<Slider>();
        UISlider = transform.Find("Slider_UI").GetComponent<Slider>();
    }

    void OnEnable()
    {
        SetSliderValues();
    }

    public void On()
    {
        gameObject.SetActive(true);
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
        gameObject.SetActive(false);
    }

}

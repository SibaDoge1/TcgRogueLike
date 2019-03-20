using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ending : MonoBehaviour {
    public float scrollTime = 4f;
    public List<Sprite> endImages;
    //public float scrollSpeed = 1f;
    ScrollRect view;
    Scrollbar bar;
    Transform content;
    Image image;

    // Use this for initialization
    void Awake()
    {
        FadeTool.FadeIn(1f, StartCredit);
        view = GameObject.Find("Canvas").transform.Find("Credit").GetComponent<ScrollRect>();
        bar = view.transform.Find("Scrollbar Vertical").GetComponent<Scrollbar>();
        //content = view.transform.Find("");
        image = GameObject.Find("Canvas").transform.Find("Image").GetComponent<Image>();
    }

    public void OnExitButtonDown()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void StartCredit()
    {
        StartCoroutine(CreditByTimeRoutine());
    }

    public void ShowImage()
    {
        switch (SaveManager.curEnding)
        {
            case 0: image.sprite = endImages[SaveManager.curEnding]; SoundDelegate.instance.PlayBGM(BGM.NORMALENDING); break;
            case 1: image.sprite = endImages[SaveManager.curEnding]; SoundDelegate.instance.PlayBGM(BGM.TRUEENDING); break;
            default: break;
        }
        image.gameObject.SetActive(true);
    }

    IEnumerator CreditByTimeRoutine()
    {
        bar.value = 1f;
        while (bar.value > 0f)
        {
            bar.value -= Time.deltaTime / scrollTime;
            yield return null;
        }
        FadeTool.FadeOutIn(1f, 1f, ShowImage);
    }
    /*
    IEnumerator CreditBySpeedRoutine()
    {
        bar.value = 1f;
        while (bar.value > 0f)
        {
            bar.value -= Time.deltaTime * scrollSpeed;
            yield return null;
        }
        FadeTool.FadeOutIn(0.5f, 0.5f, ShowImage);
    }
*/
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ending : MonoBehaviour {
    public float scrollTime = 10f;
    public List<Sprite> endImages;
    //public float scrollSpeed = 1f;
    ScrollRect view;
    Scrollbar bar;
    Transform content;
    Image image;
    GameObject BadImage;

    public List<Sprite> badPages;
    private int pageCount;

    // Use this for initialization
    void Awake()
    {
        ArchLoader.instance.StartCache();
        switch (SaveManager.curEnding)
        {
            case 1: SoundDelegate.instance.PlayBGM(BGM.NORMALENDINGCUT);
                FadeTool.FadeOutIn(0.5f, 1f, ShowBadImage); break;
            case 2: SoundDelegate.instance.PlayBGM(BGM.NORMALENDINGCUT);
                FadeTool.FadeIn(1f, StartCredit); break;
            case 3: SoundDelegate.instance.PlayBGM(BGM.TRUEENDINGCUT);
                FadeTool.FadeIn(1f, StartCredit); break;
            default: break;
        }
        view = GameObject.Find("Canvas").transform.Find("Credit").GetComponent<ScrollRect>();
        bar = view.transform.Find("Scrollbar Vertical").GetComponent<Scrollbar>();
        //content = view.transform.Find("");
        image = GameObject.Find("Canvas").transform.Find("Image").GetComponent<Image>();
        BadImage = GameObject.Find("Canvas").transform.Find("BadImage").gameObject;
    }

    public void OnExitButtonDown()
    {
        LoadingManager.LoadScene("MainMenu");
    }

    public void StartCredit()
    {
        StartCoroutine(CreditByTimeRoutine());
    }

    public void ShowImage()
    {
        image.sprite = endImages[SaveManager.curEnding];
        image.gameObject.SetActive(true);
    }
    public void ShowBadImage()
    {
        BadImage.SetActive(true);
    }

    IEnumerator CreditByTimeRoutine()
    {
        bar.value = 1f;
        while (bar.value > 0f)
        {
            bar.value -= Time.deltaTime / scrollTime;
            yield return null;
        }
        FadeTool.FadeOutIn(1.5f, 1f, ShowImage);
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

    IEnumerator BadCreditRoutine()
    {
        pageCount = 0;

        image.sprite = badPages[pageCount];
        FadeTool.FadeIn(0.4f);
        yield return new WaitForSeconds(4f);
        FadeTool.FadeOut(0.4f);
        yield return new WaitForSeconds(0.8f);
        pageCount++;
        while (pageCount < badPages.Count)
        {
            image.sprite = badPages[pageCount];
            FadeTool.FadeIn(0.4f);
            yield return new WaitForSeconds(2f);
            FadeTool.FadeOut(0.4f);
            yield return new WaitForSeconds(0.7f);
            pageCount++;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Intro : MonoBehaviour {

    public List<Sprite> pages;
    private Image image;
    private int pageCount;
    private Animator anim;
    private voidFunc onEnd;

    public void OnSkipButtonDown()
    {
        FadeTool.Reset();
        Off();
    }

    public void On(voidFunc OnEnd)
    {
        onEnd = OnEnd;
        image = transform.Find("Image").GetComponent<Image>();
        anim = transform.Find("Anim").GetComponent<Animator>();
        gameObject.SetActive(true);
        anim.Play("default");
        StartCoroutine(StartIntro());
    }

    public void Off()
    {
        pageCount = 0;
        image.sprite = pages[0];
        StopCoroutine(StartIntro());
        gameObject.SetActive(false);
        onEnd();
    }

    IEnumerator StartIntro()
    {
        pageCount = 0;

        image.sprite = pages[pageCount];
        FadeTool.FadeIn(0.4f);
        yield return new WaitForSeconds(4f);
        FadeTool.FadeOut(0.4f);
        yield return new WaitForSeconds(0.8f);
        pageCount++;
        while (pageCount < pages.Count)
        {
            image.sprite = pages[pageCount];
            FadeTool.FadeIn(0.4f);
            yield return new WaitForSeconds(2f);
            FadeTool.FadeOut(0.4f);
            yield return new WaitForSeconds(0.7f);
            pageCount++;
        } 
        FadeTool.FadeIn(0.01f);
        anim.Play("start");
        yield return new WaitForSeconds(2f);
        Off();
    }
    
}

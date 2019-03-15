using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Intro : MonoBehaviour {

    public List<Sprite> pages;
    private Image image;
    private Animator anim;
    private int pageCount;

    public void OnSkipButtonDown()
    {
        Off();
    }

    public void On()
    {
        image = transform.Find("Image").GetComponent<Image>();
        anim = transform.Find("Anim").GetComponent<Animator>();
        anim.Play("default");
        gameObject.SetActive(true);
        StartCoroutine(StartIntro());
    }

    public void Off()
    {
        pageCount = 0;
        image.sprite = pages[0];
        StopCoroutine(StartIntro());
        gameObject.SetActive(false);
    }

    IEnumerator StartIntro()
    {
        pageCount = 0;

        image.sprite = pages[pageCount];
        FadeTool.FadeIn(0.5f);
        yield return new WaitForSeconds(7f);
        FadeTool.FadeOut(0.5f);
        yield return new WaitForSeconds(1f);
        pageCount++;
        while (pageCount < pages.Count)
        {
            image.sprite = pages[pageCount];
            FadeTool.FadeIn(0.5f);
            yield return new WaitForSeconds(3.7f);
            FadeTool.FadeOut(0.5f);
            yield return new WaitForSeconds(0.8f);
            pageCount++;
        } 
        FadeTool.FadeIn(0.01f);
        anim.Play("start");
        yield return new WaitForSeconds(2f);
        Off();
    }
}

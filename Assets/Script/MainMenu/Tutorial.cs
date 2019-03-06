using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class Chapter
{
    public List<Sprite> page;
}

public class Tutorial : MonoBehaviour {
    private Image image;
    private Text text;
    private int curPage;
    private int curChapter;
    private int maxPage;
    public List<Chapter> pages;

    // Use this for initialization
    void Awake () {
        image = transform.Find("Image").GetComponent<Image>();
        text = transform.Find("Text").GetComponent<Text>();
        curPage = 0;
        curChapter = 0;
        maxPage = 0;
        for (int chap = 0; chap < pages.Count; chap++)
        {
            maxPage += pages[curChapter].page.Count;
        }

    }

    private void DisplayCount()
    {
        int totalPage = 0;
        for(int chap = 0; chap < curChapter; chap++)
        {
            totalPage += pages[chap].page.Count;
        }
        totalPage += curPage+1;
        text.text = totalPage + " / " + maxPage;
    }

    public void Next()
    {
        MainMenu.ButtonDown();
        curPage++;
        if (curPage < pages[curChapter].page.Count)
            image.sprite = pages[curChapter].page[curPage];
        else
        {
            curChapter++;
            if (curChapter < pages.Count)
            {
                curPage = 0;
                image.sprite = pages[curChapter].page[curPage];
            }
            else
            {
                Off();
            }
        }
        DisplayCount();
    }

    public void Back()
    {
        MainMenu.ButtonDown();
        curPage--;
        if (curPage >= 0)
            image.sprite = pages[curChapter].page[curPage];
        else
        {
            curChapter--;
            if (curChapter >= 0)
            {
                curPage = pages[curChapter].page.Count - 1;
                image.sprite = pages[curChapter].page[curPage];
            }
            else
            {
                Off();
            }
        }
        DisplayCount();
    }
    public void Skip()
    {
        MainMenu.ButtonDown();
        curChapter++;
        if (curChapter < pages.Count)
        {
            curPage = 0;
            image.sprite = pages[curChapter].page[curPage];
        }
        else
        {
            Off();
        }
        DisplayCount();
    }

    public void On()
    {
        gameObject.SetActive(true);
        image.sprite = pages[0].page[0];
    }
    public void Off()
    {
        curPage = 0;
        curChapter = 0;
        gameObject.SetActive(false);
    }
}

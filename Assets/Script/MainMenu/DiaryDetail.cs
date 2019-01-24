using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiaryDetail : MonoBehaviour {
    private Text title;
    private Text content;
    private Image image;
    private Diary diary;
    private MainMenu.voidFunc OffNew;

    void Awake()
    {
        title = transform.Find("Title").GetComponent<Text>();
        content = transform.Find("Text").GetComponent<Text>();
        image = transform.Find("Image").GetComponent<Image>();
        diary = transform.parent.GetComponent<Diary>();
    }

    public void On(DiaryData data, MainMenu.voidFunc func)
    {
        gameObject.SetActive(true);
        if (data == null) return;
        title.text = data.title;
        content.text = data.info;

        SaveData.diaryUnlockData[data.num][1] = false;
        OffNew = func;

        string imagePath;
        switch (data.category)
        {
            case Category.irregulars: imagePath = ""; break;
            case Category.raChips: imagePath = ""; break;
            case Category.records: imagePath = ""; break;
            case Category.humans: imagePath = ""; break;
        }
        //image.GetComponent<Image>().sprite = Resources.Load<Sprite>("");

    }

    public void Off()
    {
        OffNew();
        diary.CheckNew();
        gameObject.SetActive(false);
    }

    public void Zoom()
    {
        
    }
}

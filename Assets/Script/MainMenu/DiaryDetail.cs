using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiaryDetail : MonoBehaviour {
    public GameObject[] backGroundImages;
    private Text title;
    private Text content;
    private Image backGround;
    private Diary diary;
    private MainMenu.voidFunc OffNew;
    private GameObject noData;
    private GameObject zoomPanel;
    private DiaryData diaryData;
    private Transform categoryObject;
    private Scrollbar bar;

    private string objectPath;

    void Awake()
    {
        title = transform.Find("Title").GetComponent<Text>();
        content = transform.Find("TextBG").Find("Scroll View").Find("Viewport").Find("Content").GetComponent<Text>();
        diary = transform.parent.GetComponent<Diary>();
        noData = transform.Find("NO DATA").gameObject;
        zoomPanel = transform.Find("ZoomPanel").gameObject;
        bar = transform.Find("TextBG").Find("Scroll View").Find("Scrollbar Vertical").GetComponent<Scrollbar>();
        noData.SetActive(true);
    }

    public void On(DiaryData data, MainMenu.voidFunc func)
    {
        gameObject.SetActive(true);
        OffNew = func;
        diaryData = data;
        switch (diaryData.category)
        {
            case Category.irregulars: objectPath = "Monster"; break;
            case Category.raChips: objectPath = "Card"; break;
            case Category.records: objectPath = "Record"; break;
            case Category.humans: objectPath = "Human"; break;
            case Category.etc: objectPath = "Record"; break;
        }
        categoryObject = transform.Find(objectPath);

        if (SaveManager.GetDiaryUnlockData(diaryData.num)[0] == false)
        {
            Debug.Log(diaryData.num);
            noData.transform.Find("Text").GetComponent<Text>().text = Database.GetAchiveDataByDiary(diaryData.num).info;
            return;
        }
        Debug.Log(diaryData.category);
        SaveManager.ChangeNewToOld(diaryData.num);
        noData.SetActive(false);
        categoryObject.gameObject.SetActive(true);
        title.text = diaryData.title;
        content.text = "\n" + diaryData.info;
        content.rectTransform.sizeDelta = new Vector2(1111.6f, content.preferredHeight);
        if (categoryObject.Find("Name") != null)
            categoryObject.Find("Name").GetComponent<Text>().text = diaryData.title;


        string imagePath;
        switch (diaryData.category)
        {
            case Category.irregulars: imagePath = "Monster"; break;
            case Category.raChips: imagePath = "Card"; break;
            case Category.humans: imagePath = "Human"; break;
            default: imagePath = null; break;
        }
        if (imagePath != null)
        {
            Image image = categoryObject.Find("Image").GetComponent<Image>();
            Sprite spr = Resources.Load<Sprite>("Graphic/Diary/Images/" + imagePath + "/" + diaryData.spritePath + "_thumbnail");
            if (spr != null)
            {
                image.sprite = spr;
            }
            else
            {
                image.sprite = Resources.Load<Sprite>("Graphic/Diary/Images/" + "default_thumbnail");
            }
        }
        bar.value = 0.99f;
        //if (categoryObject.Find("Image") != null)
        // categoryObject.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Graphic/Diary/Images/" + objectPath + "/" + diaryData.spritePath);
    }

    public void Off()
    {
        MainMenu.ButtonDown();
        noData.SetActive(true);
        if(OffNew != null)
            OffNew();
        diary.CheckNew();
        categoryObject.gameObject.SetActive(false);
        bar.value = 1f;
        gameObject.SetActive(false);
    }

    public void OffWithOutUnlock()
    {
        noData.SetActive(true);
        diary.CheckNew();
        gameObject.SetActive(false);
    }

    public void Zoom()
    {
        MainMenu.ButtonDown();
        zoomPanel.SetActive(true);
        string imagePath;
        switch (diaryData.category)
        {
            case Category.irregulars: imagePath = "Monster"; break;
            case Category.raChips: imagePath = "Card"; break;
            case Category.humans: imagePath = "Human"; break;
            default: imagePath = null; break;
        }
        Image image = zoomPanel.transform.Find("Image").GetComponent<Image>();
        if (imagePath != null)
        {
            
            Sprite spr = Resources.Load<Sprite>("Graphic/Diary/Images/" + imagePath + "/" + diaryData.spritePath + "_full");
            if (spr != null)
            {
                image.sprite = spr;
            }
            else
            {
                image.sprite = Resources.Load<Sprite>("Graphic/Diary/Images/" + "default_full");
            }
        }
    }

    public void ZoomOff()
    {
        MainMenu.ButtonDown();
        zoomPanel.SetActive(false);
    }
}

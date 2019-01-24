using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiaryEntity : MonoBehaviour {

    private DiaryData diaryData;
    private GameObject _lock;
    private GameObject image;
    private GameObject NewIcon;
    private Text _name;
    private DiaryDetail detail;

    public void Awake()
    {
        _lock = transform.Find("Lock").gameObject;
        image = transform.Find("Image").gameObject;
        _name = transform.Find("Name").GetComponent<Text>();
        NewIcon = transform.Find("NewIcon").gameObject;
        detail = GameObject.Find("Canvas").transform.Find("Diary").Find("Detail").GetComponent<DiaryDetail>();
    }

    public void SetEntity(DiaryData data)
    {
        diaryData = data;
        if (diaryData == null) return;
        _name.text = diaryData.title;
        _lock.SetActive(false);
        image.SetActive(true);

        if (SaveData.diaryUnlockData[diaryData.num][1] == true) OnNewIcon();
        else OffNewIcon();

        string imagePath;
        switch (diaryData.category)
        {
            case Category.irregulars: imagePath = ""; break;
            case Category.raChips: imagePath = ""; break;
            case Category.records: imagePath = ""; break;
            case Category.humans: imagePath = ""; break;
        }
        //image.GetComponent<Image>().sprite = Resources.Load<Sprite>("");
    }
	
    public void OnClick()
    {
        detail.On(diaryData, new MainMenu.voidFunc(OffNewIcon));
    }

    public void OnNewIcon()
    {
        NewIcon.SetActive(true);
    }

    public void OffNewIcon()
    {
        NewIcon.SetActive(false);
    }
}

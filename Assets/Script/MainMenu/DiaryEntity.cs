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
        _name.text = diaryData.title;
        if (SaveManager.GetDiaryUnlockData(diaryData.num)[0] == false) return;
        _lock.SetActive(false);
        image.SetActive(true);

        if (SaveManager.GetDiaryUnlockData(diaryData.num)[1] == true) OnNewIcon();
        else OffNewIcon();

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
            Debug.Log("Graphic/Diary/Images/" + imagePath + "/" + diaryData.spritePath + "_list");
            Sprite spr = Resources.Load<Sprite>("Graphic/Diary/Images/" + imagePath + "/" + diaryData.spritePath + "_list");
            Debug.Log(diaryData.spritePath.Length);
            if (spr != null)
            {
                image.GetComponent<Image>().sprite = spr;
            }
            else
            {
                image.SetActive(false);
                //image.GetComponent<Image>().sprite = Resources.Load<Sprite>("Graphic/Diary/Images/" + "default_list");
            }
        }
    }
	
    public void OnClick()
    {
        MainMenu.ButtonDown();
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

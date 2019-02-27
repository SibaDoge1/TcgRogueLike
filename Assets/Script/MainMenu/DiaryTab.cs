using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiaryTab : MonoBehaviour {

    public Category category;
    private Image tab;
    private GameObject newIcon;
    
    void Awake()
    {
        tab = transform.Find("Tab").GetComponent<Image>();
        newIcon = transform.Find("New").gameObject;
    }

    public void CheckNew()
    {
        List<int> diaryList = transform.parent.GetComponent<Diary>().Diaries[category];
        for(int i=0; i < diaryList.Count; i++)
        {
            if (SaveManager.GetDiaryUnlockData(diaryList[i])[1] == true)
            {
                OnNewIcon();
                return;
            }
        }
        OffNewIcon();
    }
    public void SetTabColor(Color col)
    {
        tab.color = col;
    }

    public void OnNewIcon()
    {
        newIcon.SetActive(true);
    }

    public void OffNewIcon()
    {
        newIcon.SetActive(false);
    }
}

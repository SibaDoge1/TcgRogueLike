using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardInfoPanel : MonoBehaviour {

    private Image render;
    private Text text;
    private void Awake()
    {
        text = transform.Find("Text").GetComponent<Text>();
        render = transform.Find("Icon").GetComponent<Image>();
    }

    public void SetText(string name,string info)
    {
        text.text = "카드 이름 :"+name+"\n" + info;
    }
    public void SetUnknown()
    {
        text.text = "카드 이름 : 미지카드" + "\n" + "알수 없음";
    }
    public void SetRender(string path)
    {
        render.sprite = Resources.Load<Sprite>(CardDatabase.cardResourcePath + path);
    }

}

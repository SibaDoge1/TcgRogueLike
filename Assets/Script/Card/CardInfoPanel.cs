using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardInfoPanel : MonoBehaviour {

    private Image render;
    private Image cardAttribute;
    private Text text;
    private Text cardName;
    private void Awake()
    {
        cardName = transform.Find("CardName").GetComponent<Text>();
        text = transform.Find("Text").GetComponent<Text>();
        render = transform.Find("Graphic").GetComponent<Image>();
        cardAttribute = transform.Find("Attribute").GetComponent<Image>();
    }

    public void SetText(string name,string info)
    {
        cardName.text = name;
        text.text = info;
    }
    public void SetUnknown()
    {
        cardName.text = "미지카드";
        text.text = "알수 없음";
    }
    public void SetRender(string path)
    {
        render.sprite = Resources.Load<Sprite>(Database.cardResourcePath + path);
    }
    public void SetAttribute(Attribute attribute)
    {
        switch(attribute)
        {
            case Attribute.APAS:
                cardAttribute.sprite = Resources.Load<Sprite>("Attribute/apas1");
                break;
            case Attribute.PRITHVI:
                cardAttribute.sprite = Resources.Load<Sprite>("Attribute/prithivi1");
                break;
            case Attribute.TEJAS:
                cardAttribute.sprite = Resources.Load<Sprite>("Attribute/tejas1");
                break;
            case Attribute.VAYU:
                cardAttribute.sprite = Resources.Load<Sprite>("Attribute/vayu1");
                break;
        }
    }

}

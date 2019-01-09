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
        render.sprite = ArchLoader.instance.GetCardSprite(path);
    }
    public void SetAttribute(CardType attribute)
    {
        switch (attribute)
        {
            case CardType.A:
                cardAttribute.sprite = Resources.Load<Sprite>("Attribute/apas1");
                break;
            case CardType.P:
                cardAttribute.sprite = Resources.Load<Sprite>("Attribute/prithivi1");
                break;
            case CardType.T:
                cardAttribute.sprite = Resources.Load<Sprite>("Attribute/tejas1");
                break;
            case CardType.V:
                cardAttribute.sprite = Resources.Load<Sprite>("Attribute/vayu1");
                break;
            case CardType.NONE:
                cardAttribute.sprite = Resources.Load<Sprite>("Attribute/apas1"); //추후에 교체
                break;
            case CardType.AKASHA:
                cardAttribute.sprite = Resources.Load<Sprite>("Attribute/apas1");//추후에 교체
                break;
        }
        //TODO : Load방식 고쳐야함
    }

}

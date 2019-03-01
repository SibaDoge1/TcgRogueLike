using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardInfoPanel : MonoBehaviour {

    private Image render;
    private Image cardAttribute;
    private Image cardUpgrade;
    private Image cost;
    private Image[] currentCost;
    private Image range;


    private Text text;
    private Text cardName;
    private void Awake()
    {
        cardName = transform.Find("CardName").GetComponent<Text>();
        text = transform.Find("Text").GetComponent<Text>();
        render = transform.Find("Graphic").GetComponent<Image>();
        cardAttribute = transform.Find("Attribute").GetComponent<Image>();
        cardUpgrade = transform.Find("Upgrade").GetComponent<Image>();
        cost = transform.Find("Cost").GetComponent<Image>();
        currentCost = transform.Find("Cost").GetComponentsInChildren<Image>();
        range = transform.Find("Range").GetComponent<Image>();
    }

    public void SetUnknown()
    {
        cardName.text = "???";
        text.text = "카드 데이터를 읽어올 수 없습니다. 단말기에서 카드를 먼저 해독해주세요";
        render.sprite = ArchLoader.instance.GetCardSprite("error");
        cardAttribute.sprite = ArchLoader.instance.GetCardSprite("error");
        cardUpgrade.enabled = false;
        range.sprite = ArchLoader.instance.GetCardSprite("error");
        SetCost(0);
    }

    public void SetCard(Card data)
    {
        cardName.text = data.Name;
        text.text = data.Info;
        render.sprite = ArchLoader.instance.GetCardSprite(data.SpritePath);
        cardAttribute.sprite = ArchLoader.instance.GetCardAttribute(data.Type);
        range.sprite = ArchLoader.instance.GetCardRangeImage(data.Range);
        cardUpgrade.enabled = data.IsUpgraded;
        SetCost(data.Cost);
    }

    public void SetCost(int cost)
    {
        for(int i=1; i<currentCost.Length;i++)
        {
            if(i<=cost)
            {
                currentCost[i].enabled = true;
            }else
            {
                currentCost[i].enabled = false;
            }
        }
    }
}

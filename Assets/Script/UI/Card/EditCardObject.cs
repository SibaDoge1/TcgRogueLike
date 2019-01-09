using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 덱 수정에 있는 카드 오브젝트
/// </summary>

public class EditCardObject : Button
{

    private int index;
    public int Index
    {
        get { return index; }
        set { index = value; }
    }
    private bool isReavealed;
    public bool IsReavealed
    { get { return isReavealed; } }
    private bool isOnDeck;


    private Card data;
    public Card GetCard()
    {
        return data;
    }
    private DeckEditUI deckEditUI;
    private CardRender render;
    private Image highLightImage;
    bool isSelected;
    public bool IsSelected {
        get { return isSelected; }
        set {isSelected = value; }
    }
    protected  override void Awake()
    {
        onClick.AddListener(OnClickThis);
        highLightImage = transform.Find("HighLight").GetComponent<Image>();
        highLightImage.enabled = false;
        render = transform.Find("render").GetComponent<CardRender>();
    }

    public void SetCard(Card data_)
    {      
        data = data_;              
    }
    public void SetSpriteRender()
    {
        isReavealed = true;
        render.Name.text = data.Name;
        render.SetRank(data.Cost);
        render.SetAttribute(data.Type);
        render.SetGraphic(ArchLoader.instance.GetCardSprite(data.SpritePath));
        if (data is Card_Normal)
        {
            render.SetUpgrade((data as Card_Normal).IsUpgraded);
        }
    }


    public void SetDeckUI(DeckEditUI de)
    {
        deckEditUI = de;
    }

    public void SetParent(Transform vp, bool isDeck)
    {
        transform.SetParent(vp, false);
        isOnDeck = isDeck;
    }

    public void OnClickThis()
    {
        if (isReavealed)
        {
            deckEditUI.CardInfoOn(data);
        }
        else
        {
            deckEditUI.CardInfoUnknown();
        }

        if (deckEditUI.IsEditOk && IsReavealed)
        {
            if (isSelected)
            {
                if (isOnDeck)
                {
                    deckEditUI.DeckCardSelectOff(this);
                }
                else
                {
                    deckEditUI.AttainCardSelectOff(this);
                }
            }
            else
            {
                if (isOnDeck)
                {
                    deckEditUI.DeckCardSelect(this);
                }
                else
                {
                    deckEditUI.AttainCardSelect(this);
                }
            }
        }
    }
    public void HighLightOn()
    {
        isSelected = true;
        highLightImage.enabled = true;
    }
    public void HighLightOff()
    {
        isSelected = false;
        highLightImage.enabled = false;
    }

    public void Locate(int i)
    {
        transform.SetSiblingIndex(i);
    }


}

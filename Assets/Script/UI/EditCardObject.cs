using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



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


    private CardData data;
    public CardData GetCardData()
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

    public void SetCardData(CardData data_)
    {      
        data = data_;              
    }
    public void SetSpriteRender()
    {
        isReavealed = true;
        render.Img_Graphic.sprite = Resources.Load<Sprite>(CardDatabase.cardResourcePath + data.SpritePath);
        CardAbilityType a = data.GetCardAbilityType();
        switch (a)
        {
            case CardAbilityType.Attack:
                render.Img_Ability.sprite = Resources.Load<Sprite>("Card/Icon/iconAtk");
                {//속성 불러오기
                    render.Img_Attribute.enabled = true;
                    Attribute at = data.CardAtr;
                    switch (at)
                    {
                        case Attribute.AK:
                            render.Img_Attribute.sprite = Resources.Load<Sprite>("Attribute/akasha1");
                            break;
                        case Attribute.APAS:
                            render.Img_Attribute.sprite = Resources.Load<Sprite>("Attribute/apas1");
                            break;
                        case Attribute.PRITHVI:
                            render.Img_Attribute.sprite = Resources.Load<Sprite>("Attribute/prithivi1");
                            break;
                        case Attribute.TEJAS:
                            render.Img_Attribute.sprite = Resources.Load<Sprite>("Attribute/tejas1");
                            break;
                        case Attribute.VAYU:
                            render.Img_Attribute.sprite = Resources.Load<Sprite>("Attribute/vayu1");
                            break;
                    }
                }
                break;
            case CardAbilityType.Util:
                render.Img_Ability.sprite = Resources.Load<Sprite>("Card/Icon/iconUtil");
                break;
        }
    }

    public void SetDeckUI(DeckEditUI de)
    {
        deckEditUI = de;
    }

    public void SetParent(Transform vp, bool isDeck)
    {
        transform.SetParent(vp);
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

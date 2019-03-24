using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
/// <summary>
/// 덱 수정에 있는 카드 오브젝트
/// </summary>

public class EditCardObject : CardObject, IPointerClickHandler
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


    public Card GetData()
    {
        return data;
    }
    private DeckEditUI deckEditUI;
    private Image highLightImage;

    bool isSelected;
    public bool IsSelected {
        get { return isSelected; }
        set { isSelected = value; }
    }
    protected override void Awake()
    {
        base.Awake();
        highLightImage = transform.Find("HighLight").GetComponent<Image>();
        highLightImage.enabled = false;
    }

    public void SetRenderKnown()
    {
        isReavealed = true;
        render.SetRender(data);
    }
    public void SetRenderUnknown()
    {
        isReavealed = false;
        render.SetRender();
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isReavealed)
        {
            UIManager.instance.CardInfoPanel_On(data);
        }
        else
        {
            UIManager.instance.CardInfoPanel_On();
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
                UIManager.instance.CardInfoPanel_Off();
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
}

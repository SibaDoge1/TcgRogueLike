using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CheckCardObject : CardObject, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if(isAttain)
        {
            UIManager.instance.CardInfoPanel_On();
        }else
        {
            UIManager.instance.CardInfoPanel_On(data);
        }
    }
    public void SetRenderKnown()
    {
        isAttain = false;
        render.SetRender(data);
    }

    bool isAttain;
    public void SetRenderUnknown()
    {
        isAttain = true;
        render.SetRender();
    }

    public Card GetCardData
    {
        get { return data; }
    }

    public bool IsAttain
    {
        get
        {
            return isAttain;
        }
    }
    public void Locate(int locate)
    {
        transform.SetSiblingIndex(locate);
    }


}

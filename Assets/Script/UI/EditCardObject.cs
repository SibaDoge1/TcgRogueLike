using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class EditCardObject : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{

    private int index;
    private CardRender render;
    private CardData data;
    private Transform viewPort;
    private RectTransform rect;
    public CardData GetData()
    {
        return data;
    }
    private DeckEditUI deckEditUI;
    private Image thisImage;
    protected bool isOnDeck;
    protected bool isReavealed;

    protected  void Awake()
    {
        rect = GetComponent<RectTransform>();
        thisImage = GetComponent<Image>();
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
            case CardAbilityType.Heal:
                render.Img_Ability.sprite = Resources.Load<Sprite>("Card/Icon/iconHeal");
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
        isOnDeck = isDeck;
        viewPort = vp;
    }


    #region UserInput
    private Vector3 originPos;

    public void OnPointerDown(PointerEventData ped)
    {
        originPos = rect.anchoredPosition;
        if (isReavealed)
        {
            deckEditUI.CardInfoOn(data);
        }else
        {
            deckEditUI.CardInfoUnknown();
        }
    }

    public void OnPointerUp(PointerEventData ped)
    {
        if (deckEditUI.IsEditOk)
        {
            if (isOnDeck)
            {
                Vector3 v = deckEditUI.AttainViewPort.InverseTransformPoint(ped.position);    
                if (deckEditUI.AttainViewPort.rect.Contains(v))
                {
                    deckEditUI.MoveToAttain(this);
                }else
                {
                    ReturnToViewPort(index);
                }
            }else
            {
                Vector3 v = deckEditUI.DeckViewPort.InverseTransformPoint(ped.position);
                if (deckEditUI.DeckViewPort.rect.Contains(v))
                {
                    deckEditUI.MoveToDeck(this);
                }
                else
                {
                    ReturnToViewPort(index);
                }
            }
        }
    }

    public void OnDrag(PointerEventData ped)
    {
        if(deckEditUI.IsEditOk)
        {
            transform.position = ped.position;
            transform.SetParent(viewPort.parent.parent);
        }
    }
    #endregion


    private void EnableInteraction()
    {
        thisImage.raycastTarget = true;
    }
    private void DisableInteraction()
    {
        thisImage.raycastTarget = false;
    }
    public void SetLocation(int i)
    {
        index = i;
        if(locateRoutine != null)
        {
            StopCoroutine(locateRoutine);
        }
        locateRoutine = StartCoroutine(LocateRoutine(GetLocalPosition(i)));
    }
    #region Private

    float speed = 3.5f;
    private Coroutine locateRoutine;
    private IEnumerator LocateRoutine(Vector3 targetPosition)
    {
        rect.SetParent(viewPort);

        Vector3 oP = rect.anchoredPosition;
        float _time = 0f;
        while (true)
        {
            _time += Time.deltaTime * speed;
            if(_time<1f)
            {
                rect.anchoredPosition = Vector3.Lerp(oP, targetPosition, _time);
            }else
            {
                break;
            }
            yield return null;
        }
        rect.anchoredPosition = targetPosition;
    }
    private  Vector3 GetLocalPosition(int i)
    {
        Vector3 offset = new Vector3(120, -140);

        if (i==0)
        {
            return offset ;
        }
        return new Vector3(i%3*180,i/3*(-200)) + offset;
    }
    private void ReturnToViewPort(int i)
    {
        rect.SetParent(viewPort);
        rect.anchoredPosition = originPos;
        //StartCoroutine(ReturnToViewPortRoutine(i));
    }

    /*IEnumerator ReturnToViewPortRoutine(int i)
    {       
        rect.SetParent(viewPort.parent.parent);
        Vector3 oP = transform.position;
        
        float _time = 0f;
        while (true)
        {
            _time += Time.deltaTime * speed;
            if (_time < 1f)
            {
                transform.position = Vector3.Lerp(oP, originPos, _time);
            }
            else
            {
                break;
            }
            yield return null;
        }
        rect.SetParent(viewPort);
        rect.anchoredPosition = GetLocalPosition(i);
    }*/
    #endregion
}

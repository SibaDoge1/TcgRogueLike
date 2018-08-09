using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckEditUI : MonoBehaviour
{
    private RectTransform rect;
    private CardInfoPanel cardinfoPanel;
    private Vector3 offPos = new Vector3(0, 2000, 0);
    private bool isEditOk=false;
    public bool IsEditOk { get { return isEditOk; } }

    private List<EditCardObject> deckCardObjects;
    private List<EditCardObject> attainCardObjects;
    private List<CardData> deck;
    private List<CardData> attain;
    private List<EditCardObject> deckSelected;
    private List<EditCardObject> attainSelected;
    

    RectTransform deckViewPort;
    public RectTransform DeckViewPort { get { return deckViewPort; } }
    RectTransform attainViewPort;
    public RectTransform AttainViewPort { get { return attainViewPort; } }

    Text currentMode;
    Button ExitButton;
    Text buttonText;
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        deckViewPort = transform.Find("DeckPanel").Find("DeckCardPool").Find("viewport").GetComponent<RectTransform>();
        attainViewPort = transform.Find("DeckPanel").Find("AttainCardPool").Find("viewport").GetComponent<RectTransform>();
        cardinfoPanel = transform.Find("DeckPanel").Find("CardInfoPanel").GetComponent<CardInfoPanel>();

        currentMode = transform.Find("DeckPanel").Find("Texts").Find("currentMode").GetComponent<Text>();
        ExitButton = transform.Find("DeckPanel").Find("Buttons").Find("exit").GetComponent<Button>();
        buttonText = ExitButton.transform.Find("Text").GetComponent<Text>();
    }

    #region CardInfoPanel
    public void CardInfoOn(CardData c)
    {
        cardinfoPanel.gameObject.SetActive(true);
        cardinfoPanel.SetText(c.CardName,c.CardExplain);
        cardinfoPanel.SetRender(c.SpritePath);
    }
    public void CardInfoOff()
    {
        cardinfoPanel.gameObject.SetActive(false);
    }
    public void CardInfoUnknown()
    {
        cardinfoPanel.gameObject.SetActive(true);
        cardinfoPanel.SetUnknown();
    }
    #endregion

    public void On(bool ok)
    {
        isEditOk = ok;
        rect.anchoredPosition = Vector3.zero;
        SetTexts(isEditOk);
        MakeCardObjects();
        ExitButton.interactable = true;
        if (ok)
        {
            RevealAttainCards();
            deckSelected = new List<EditCardObject>();
            attainSelected = new List<EditCardObject>();
        }

    }
    public void Off()
    {
        StartCoroutine(OffRoutine());
        ExitButton.interactable = false;
    }
    bool isMoveable;

    public void ExchangeCards()
    {

            for(int i=0; i<deckSelected.Count;i++)
            {
                int di = deckSelected[i].Index;
                int ai = attainSelected[i].Index;
                deck.Remove(deckSelected[i].GetCardData());//덱에서 어테인으로
                deckCardObjects.Remove(deckSelected[i]);
                attain.Add(deckSelected[i].GetCardData());
                attainCardObjects.Add(deckSelected[i]);
                deckSelected[i].SetParent(attainViewPort, false);
                deckSelected[i].Index = ai;
                deckSelected[i].Locate(ai);

                attain.Remove(attainSelected[i].GetCardData());//어테인에서 덱으로
                attainCardObjects.Remove(attainSelected[i]);
                deck.Add(attainSelected[i].GetCardData());
                deckCardObjects.Add(attainSelected[i]);
                attainSelected[i].SetParent(deckViewPort, true);
                attainSelected[i].Index = di;
                attainSelected[i].Locate(di);
            }

            attainSelected.Clear();
            deckSelected.Clear();
        
    }


    public void RevealAttainCards()
    {
       StartCoroutine(RevealCardsRoutine());
    }
    public void DeckCardSelectOff(EditCardObject ec)
    {
        ec.HighLightOff();
        deckSelected.Remove(ec);
        CheckExchangeAvail();

    }
    public void AttainCardSelectOff(EditCardObject ec)
    {
        ec.HighLightOff();
        attainSelected.Remove(ec);
        CheckExchangeAvail();
    }
    public void DeckCardSelect(EditCardObject ec)
    {
        if(deckSelected.Count>=3)
        {
            int min = int.MaxValue;
            EditCardObject cardToRemove = null;
            for(int i=0; i<deckSelected.Count;i++)//제일 가까운 인덱스 번호 카드 지우기
            {
                int c = Mathf.Abs(deckSelected[i].Index - ec.Index);
                if(min>c)
                {
                    min = c;
                    cardToRemove = deckSelected[i];
                }
            }
            DeckCardSelectOff(cardToRemove);
        }
        ec.HighLightOn();
        deckSelected.Add(ec);
        CheckExchangeAvail();
    }
    public void AttainCardSelect(EditCardObject ec)
    {
        if (attainSelected.Count >=3)
        {
            int min = int.MaxValue;
            EditCardObject cardToRemove = null;
            for (int i = 0; i < attainSelected.Count; i++)//제일 가까운 인덱스 번호 카드 지우기
            {
                int c = Mathf.Abs(attainSelected[i].Index - ec.Index);
                if (min > c)
                {
                    min = c;
                    cardToRemove = attainSelected[i];
                }
            }
            AttainCardSelectOff(cardToRemove);
        }
        ec.HighLightOn();
        attainSelected.Add(ec);
        CheckExchangeAvail();
    }
    void CheckExchangeAvail()
    {
        if(attainSelected.Count==deckSelected.Count)
        {
            ExitButton.interactable = true;
        }else
        {
            ExitButton.interactable = false;
        }
    }

    #region private
    /// <summary>
    /// Temp
    /// </summary>
    private void SetTexts(bool bo)
    {
        if (bo)
        {
            currentMode.text = "수정모드";
            buttonText.text = "편성완료";
        }
        else
        {
            currentMode.text = "뷰 모드";
            buttonText.text = "나가기";
        }
    }
    private void MakeCardObjects()
    {
        deck = new List<CardData>(PlayerData.Deck);
        deckCardObjects = new List<EditCardObject>();
        for(int i=0; i<deck.Count;i++)
        {
            deckCardObjects.Add(deck[i].InstantiateDeckCard());
        }
        for (int i = 0; i < deckCardObjects.Count; i++)
        {
            deckCardObjects[i].SetParent(deckViewPort,true);
            deckCardObjects[i].SetDeckUI(this);
            deckCardObjects[i].SetSpriteRender();
        }

        attain = new List<CardData>(PlayerData.AttainCards);
        attainCardObjects = new List<EditCardObject>();
        for (int i = 0; i < attain.Count; i++)
        {
            attainCardObjects.Add(attain[i].InstantiateDeckCard());
        }
        for (int i = 0; i < attainCardObjects.Count; i++)
        {
            attainCardObjects[i].SetParent(attainViewPort,false);
            attainCardObjects[i].SetDeckUI(this);
        }
        SortCards();
    }
    private void DeleteAllObjects()
    {
        for(int i=deckCardObjects.Count-1;i>=0;i--)
        {
            Destroy(deckCardObjects[i].gameObject);
        }
        for (int i=attainCardObjects.Count-1;i>=0;i--)
        {           
            Destroy(attainCardObjects[i].gameObject);
        }
    }
    IEnumerator RevealCardsRoutine()
    {
        for (int i = 0; i < attainCardObjects.Count; i++)
        {
            if(!attainCardObjects[i].IsReavealed)
            {
                attainCardObjects[i].SetSpriteRender();
                yield return new WaitForSeconds(0.3f);
            }
        }
        yield return null;
    }
    IEnumerator OffRoutine()
    {
        if(IsEditOk)
        {
            yield return new WaitForSeconds(0.5f);
            ExchangeCards();
            yield return new WaitForSeconds(1f);
            PlayerData.Deck = deck;
            PlayerData.AttainCards.Clear();
            PlayerControl.instance.ReLoadDeck();
        }
        DeleteAllObjects();
        rect.anchoredPosition = offPos;
    }
    private void SortCards()
    {
        deckCardObjects.Sort(delegate (EditCardObject A, EditCardObject B)
        {
            int aIndex = (int)A.GetCardData().Rating * 40000 + A.GetCardData().Index * 200 + (int)A.GetCardData().CardAtr;
            int bIndex = (int)B.GetCardData().Rating * 40000 + B.GetCardData().Index * 1000 + (int)B.GetCardData().CardAtr;
            if (aIndex > bIndex)
                return 1;
            else if (aIndex < bIndex)
                return -1;
            return 0;
        });
        attainCardObjects.Sort(delegate (EditCardObject A, EditCardObject B)
        {           
            int aIndex = (int)A.GetCardData().Rating*40000+A.GetCardData().Index * 200 + (int)A.GetCardData().CardAtr;
            int bIndex = (int)B.GetCardData().Rating * 40000+B.GetCardData().Index * 1000 + (int)B.GetCardData().CardAtr;
            if (aIndex > bIndex)
                return 1;
            else if (aIndex < bIndex)
                return -1;
            return 0;
        });
        for (int i = 0; i < deckCardObjects.Count; i++)
        {
            deckCardObjects[i].Index = i;
            deckCardObjects[i].Locate(i);
        }
        for (int i = 0; i < attainCardObjects.Count; i++)
        {
            attainCardObjects[i].Index = i;
            attainCardObjects[i].Locate(i);
        }
    }



    #endregion
}

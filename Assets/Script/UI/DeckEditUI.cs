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
   
    

    RectTransform deckViewPort;
    public RectTransform DeckViewPort { get { return deckViewPort; } }
    RectTransform attainViewPort;
    public RectTransform AttainViewPort { get { return attainViewPort; } }

    Text currentMode;
    Text buttonText;
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        deckViewPort = transform.Find("DeckPanel").Find("DeckCardPool").Find("viewport").GetComponent<RectTransform>();
        attainViewPort = transform.Find("DeckPanel").Find("AttainCardPool").Find("viewport").GetComponent<RectTransform>();
        cardinfoPanel = transform.Find("DeckPanel").Find("CardInfoPanel").GetComponent<CardInfoPanel>();

        currentMode = transform.Find("DeckPanel").Find("Texts").Find("currentMode").GetComponent<Text>();
        buttonText = transform.Find("DeckPanel").Find("Buttons").Find("exit").Find("Text").GetComponent<Text>();
    }

    #region CardInfoPanel
    public void CardInfoOn(CardData c)
    {
        cardinfoPanel.gameObject.SetActive(true);
        cardinfoPanel.SetText(c.CardName,c.CardExplain);
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
        MakeCardObjects();
        SetTexts(isEditOk);
    }
    public void Off()
    {
        DeleteAllObjects();
        rect.anchoredPosition = offPos;
        if (isEditOk)
        {
            PlayerData.Deck = deck;
            PlayerData.AttainCards = GetUnReavealedCards(attainCardObjects);
            PlayerControl.instance.ReLoadDeck();
        }  
    }

    public void MoveToDeck(EditCardObject ec)
    {
        attain.Remove(ec.GetCardData());
        attainCardObjects.Remove(ec);
        deck.Add(ec.GetCardData());
        deckCardObjects.Add(ec);
        ec.SetParent(deckViewPort, true);
        SortCards();
    }
    public void MoveToAttain(EditCardObject ec)
    {
        deck.Remove(ec.GetCardData());
        deckCardObjects.Remove(ec);
        attain.Add(ec.GetCardData());
        attainCardObjects.Add(ec);
        ec.SetParent(AttainViewPort, false);
        SortCards();
    }

    public void RevealAllCards()
    {
        if (!isRunningRoutine && isEditOk)
        {
            StartCoroutine(RevealCardsRoutine());
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
    bool isRunningRoutine;
    IEnumerator RevealCardsRoutine()
    {
        isRunningRoutine = true;
        for (int i = 0; i < attainCardObjects.Count; i++)
        {
            if(!attainCardObjects[i].IsReavealed)
            {
                attainCardObjects[i].SetSpriteRender();
                yield return new WaitForSeconds(0.5f);
            }
        }
        isRunningRoutine = false;
        yield return null;
    }
    private void SortCards()
    {
        deckCardObjects.Sort(delegate (EditCardObject A, EditCardObject B)
        {
            int aIndex = A.GetCardData().Index * 1000 + (int)A.GetCardData().CardAtr;
            int bIndex = B.GetCardData().Index * 1000 + (int)B.GetCardData().CardAtr;
            if (aIndex > bIndex)
                return 1;
            else if (aIndex < bIndex)
                return -1;
            return 0;
        });
        attainCardObjects.Sort(delegate (EditCardObject A, EditCardObject B)
        {
            int aIndex = A.GetCardData().Index * 1000 + (int)A.GetCardData().CardAtr;
            int bIndex = B.GetCardData().Index * 1000 + (int)B.GetCardData().CardAtr;
            if (aIndex > bIndex)
                return 1;
            else if (aIndex < bIndex)
                return -1;
            return 0;
        });
        for (int i = 0; i < deckCardObjects.Count; i++)
        {
            deckCardObjects[i].SetLocation(i);
        }
        for (int i = 0; i < attainCardObjects.Count; i++)
        {
            attainCardObjects[i].SetLocation(i);
        }
    }
    private List<CardData> GetUnReavealedCards(List<EditCardObject> cd)
    {
        List<CardData> temp = new List<CardData>();
        for(int i=0; i<cd.Count;i++)
        {
            if(!cd[i].IsReavealed)
            {
                temp.Add(cd[i].GetCardData());
            }
        }
        return temp;

    }


    #endregion
}

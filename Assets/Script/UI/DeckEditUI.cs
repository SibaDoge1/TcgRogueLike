using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckEditUI : MonoBehaviour
{
    private RectTransform rect;
    private CardInfo cardinfo;
    private Vector3 offPos = new Vector3(0, 2000, 0);
    private bool isEditOk=false;
    public bool IsEditOk { get { return isEditOk; } }
    private bool isEdited=false;
    private List<EditCardObject> deckCardObjects;
    private List<EditCardObject> attainCardObjects;
    private List<CardData> deck;
    private List<CardData> attain;
   
    RectTransform deckViewPort;
    public RectTransform DeckViewPort { get { return deckViewPort; } }
    RectTransform attainViewPort;
    public RectTransform AttainViewPort { get { return attainViewPort; } }

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        deckViewPort = transform.Find("DeckPanel").Find("DeckCardPool").Find("viewport").GetComponent<RectTransform>();
        attainViewPort = transform.Find("DeckPanel").Find("AttainCardPool").Find("viewport").GetComponent<RectTransform>();

        cardinfo = transform.Find("DeckPanel").Find("CardInfoPanel").GetComponent<CardInfo>();
    }
    #region CardInfo
    public void CardInfoOn(CardData c)
    {
        cardinfo.gameObject.SetActive(true);
        cardinfo.SetText(c.CardName,c.CardExplain);
    }
    public void CardInfoOff()
    {
        cardinfo.gameObject.SetActive(false);
    }
    public void CardInfoUnknown()
    {
        cardinfo.gameObject.SetActive(true);
        cardinfo.SetUnknown();
    }
    #endregion
    public void On(bool b)
    {
        rect.anchoredPosition = Vector3.zero;
        
        MakeCardObjects();
        isEditOk = b;
        isEdited = false;
    }
    public void Off()
    {
        DeleteAllObjects();
        isEditOk = false;
        rect.anchoredPosition = offPos;
        if (isEdited)
        {
            PlayerData.Deck = new List<CardData>(deck);
            PlayerData.AttainCards.Clear();
            PlayerControl.instance.ReLoadDeck();
        }
    }

    public void MoveToDeck(EditCardObject ec)
    {
        isEdited = true;
        attain.Remove(ec.GetData());
        attainCardObjects.Remove(ec);

        deck.Add(ec.GetData());
        deckCardObjects.Add(ec);

        ec.SetParent(deckViewPort, true);

        SortCards();
    }
    public void MoveToAttain(EditCardObject ec)
    {
        isEdited = true;
        deck.Remove(ec.GetData());
        deckCardObjects.Remove(ec);

        attain.Add(ec.GetData());
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
            attainCardObjects[i].SetSpriteRender();
            yield return new WaitForSeconds(0.5f);
        }
        isRunningRoutine = false;
    }
    private void SortCards()
    {
        deckCardObjects.Sort(delegate (EditCardObject A, EditCardObject B)
        {
            int aIndex = A.GetData().Index * 1000 + (int)A.GetData().CardAtr;
            int bIndex = B.GetData().Index * 1000 + (int)B.GetData().CardAtr;
            if (aIndex > bIndex)
                return 1;
            else if (aIndex < bIndex)
                return -1;
            return 0;
        });
        attainCardObjects.Sort(delegate (EditCardObject A, EditCardObject B)
        {
            int aIndex = A.GetData().Index * 1000 + (int)A.GetData().CardAtr;
            int bIndex = B.GetData().Index * 1000 + (int)B.GetData().CardAtr;
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
    #endregion
}

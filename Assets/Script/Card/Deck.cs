using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckManager
{

    private List<Card> deck;
    public List<Card> Deck
    {
        get { return deck; }
        set
        {
            deck = value;
        }
    }
    List<Card> attainCards;
    public List<Card> AttainCards
    {
        get { return attainCards; }
        set { attainCards = value; }
    }

    int count = 0;
    public bool isDrawEnd
    {
        get
        {
            if (deck.Count <= count)
                return true;
            else
                return false;
        }
    }

	public HandCardObject Draw(){
		if (deck.Count <= count) {
			return Card.GetCardByNum(99).InstantiateHandCard();
		}
		Card c = deck [count];
        count++;

        DrawCallBack();
        return c.InstantiateHandCard ();
	}

    public void ReLoad()
    {
        count = 0;
        Shuffle();
        for(int i=0; i<deck.Count;i++)
        {
            Debug.Log(deck[i].Index);
            if(deck[i] is Card_Special)
            {
                ((Card_Special)deck[i]).CostReset();
            }
            deck[i].UpgradeReset();
        }     
        LoadCallBack();
    }

    public void OnCardReturned(Card card)
    {
        for(int i=0; i< deck.Count;i++)
        {
            if(deck[i] is Card_Special)
            {
                deck[i].CardReturnCallBack(card);
            }
        }
    }
	#region Private


	private void Shuffle()
    {
		Card temp;
		int randIndex;
		for (int i = 0; i < deck.Count; i++) {
			randIndex = Random.Range (0, deck.Count);
			temp = deck [i];
			deck [i] = deck [randIndex];
			deck [randIndex] = temp;
		}
	}

    public int DeckCount
    {
        get
        {
            return deck.Count-count;
        }
    }
    private void DrawCallBack()
    {
        UIManager.instance.DeckCont(deck.Count-count);
    }
    private void LoadCallBack()
    {
        UIManager.instance.DeckCont(deck.Count-count);
    }

    #endregion
}

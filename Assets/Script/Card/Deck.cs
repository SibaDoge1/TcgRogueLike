using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour {

    private List<Card> deck = PlayerData.Deck;
    public List<Card> playingDeck
    {
        get { return deck; }
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
			return null;
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
            if(deck[i] is Card_Special)
            {
                ((Card_Special)deck[i]).CostReset();
            }
        }     
        LoadCallBack();
    }


    public void OnRoomClear()
    {
        foreach(Card c in deck)
        {
            c.UpgradeReset();
        }

        ReLoad();
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

    /// <summary>
    /// 맨 마지막 카드 제외하고 셔플
    /// </summary>
	private void Shuffle()
    {
		Card temp;
		int randIndex;
		for (int i = 0; i < deck.Count - 2; i++) {
			randIndex = Random.Range (0, deck.Count-1);
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

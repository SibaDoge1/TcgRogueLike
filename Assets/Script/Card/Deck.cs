using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour {

	private List<Card> remainDeck;
    int count = 0;
    public bool isDrawEnd
    {
        get
        {
            if (remainDeck.Count <= count)
                return true;
            else
                return false;
        }
    }

	public HandCardObject Draw(){
		if (remainDeck.Count <= count) {
			return null;
		}
		Card c = remainDeck [count];
        count++;

        DrawCallBack();
        return c.InstantiateHandCard ();
	}

    public void ReLoad()
    {

        count = 0;
        Shuffle();     

        LoadCallBack();
    }


    public void OnRoomClear()
    {
		remainDeck = new List<Card> (PlayerData.Deck);
        remainDeck.Insert(remainDeck.Count, new Card_Reload(Database.GetCardData(99)));//1번은 리로드

        ReLoad();
	}

    public void OnCardReturned(Card card)
    {
        for(int i=0; i<remainDeck.Count;i++)
        {
            remainDeck[i].CardReturnCallBack(card);
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
		for (int i = 0; i < remainDeck.Count - 2; i++) {
			randIndex = Random.Range (0, remainDeck.Count-1);
			temp = remainDeck [i];
			remainDeck [i] = remainDeck [randIndex];
			remainDeck [randIndex] = temp;
		}
	}

    public int DeckCount
    {
        get
        {
            return remainDeck.Count;
        }
    }
    private void DrawCallBack()
    {
        UIManager.instance.DeckCont(remainDeck.Count);
    }
    private void LoadCallBack()
    {
        UIManager.instance.DeckCont(remainDeck.Count);
    }

    #endregion
}

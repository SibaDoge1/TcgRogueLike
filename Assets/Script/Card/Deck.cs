using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour {

    private List<Card> playingDeck = PlayerData.Deck;
    public List<Card> PlayingDeck
    {
        get { return playingDeck; }
    }

    int count = 0;
    public bool isDrawEnd
    {
        get
        {
            if (playingDeck.Count <= count)
                return true;
            else
                return false;
        }
    }

	public HandCardObject Draw(){
		if (playingDeck.Count <= count) {
			return Card.GetCardByNum(99).InstantiateHandCard();
		}
		Card c = playingDeck [count];
        count++;

        DrawCallBack();
        return c.InstantiateHandCard ();
	}

    public void ReLoad()
    {
        count = 0;
        Shuffle();
        for(int i=0; i<playingDeck.Count;i++)
        {
            if(playingDeck[i] is Card_Special)
            {
                ((Card_Special)playingDeck[i]).CostReset();
            }
        }     
        LoadCallBack();
    }


    public void OnRoomClear()
    {
        foreach(Card c in playingDeck)
        {
            c.UpgradeReset();
        }

        ReLoad();
	}

    public void OnCardReturned(Card card)
    {
        for(int i=0; i< playingDeck.Count;i++)
        {
            if(playingDeck[i] is Card_Special)
            {
                playingDeck[i].CardReturnCallBack(card);
            }
        }
    }
    public void OnDeckChanged(List<Card> deck)
    {
        playingDeck = deck;
        PlayerControl.instance.ReLoadDeck();
    }

	#region Private

    /// <summary>
    /// 맨 마지막 카드 제외하고 셔플
    /// </summary>
	private void Shuffle()
    {
		Card temp;
		int randIndex;
		for (int i = 0; i < playingDeck.Count - 1; i++) {
			randIndex = Random.Range (0, playingDeck.Count-1);
			temp = playingDeck [i];
			playingDeck [i] = playingDeck [randIndex];
			playingDeck [randIndex] = temp;
		}
	}

    public int DeckCount
    {
        get
        {
            return playingDeck.Count-count;
        }
    }
    private void DrawCallBack()
    {
        UIManager.instance.DeckCont(playingDeck.Count-count);
    }
    private void LoadCallBack()
    {
        UIManager.instance.DeckCont(playingDeck.Count-count);
    }

    #endregion
}

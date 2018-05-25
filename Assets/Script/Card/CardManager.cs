using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour {

    public static CardManager instance;
    private void Awake()
    {
        instance = this;

        deck = new Deck();
        grave = new Grave();
        hand = new Hand();
        TempMakeDeck();
    }

     public Deck deck;
     public Grave grave;
     public Hand hand;
     public List<PassiveCard> passiveCards;
 

    /// <summary>
    /// 임시용 덱 메이킹 함수
    /// </summary>
    void TempMakeDeck()
    {
        for(int i = 0; i<10; i++)
        {
            deck.AddCard(new Card_Sword((CardAttribute)Random.Range(1,4)));
        }
        for (int i=0; i<5; i++)
        {
            //deck.AddCard(new Card_Bandage());
        }
        deck.Shuffle();
    }

    /// <summary>
    /// 카드 획득 해서 목적지로 추가
    /// </summary>
    public void AddCard(ActiveCard card, CardPile target = null)
    {
        if (target == null)
        {
            deck.AddCard(card);
        }
        else
        {
            target.AddCard(card);
        }
    }
    /// <summary>
    /// 패시브 카드 추가
    /// </summary>
    public void AddCard(PassiveCard passi)
    {
        passiveCards.Add(passi);
    }
    /// <summary>
    /// 패시브카드 포함한 덱 리턴
    /// </summary>
    public List<CardData> GetAllDeck()
    {
        List<CardData> temp = new List<CardData>();
        for(int i=0; i<passiveCards.Count;i++)
        {
            temp.Add(passiveCards[i]);
        }
        List<ActiveCard> a;
        a = this.deck.GetPile();
        for(int i=0; i<a.Count;i++)
        {
            temp.Add(a[i]);
        }
        return temp;
    } 

    /// <summary>
    /// 덱에서 카드 드로우
    /// </summary>
    public ActiveCard CardDraw()
    {
        ActiveCard temp = deck.GetCard();
        deck.DelteCard();
        hand.AddCard(temp);
        return temp;
    }

    /// <summary>
    /// 핸드에서 카드 사용
    /// </summary>
    public void UseCard(ActiveCard card)
    {
        card.ActiveThis();
        hand.DelteCard(card);
        grave.AddCard(card);
    }

    /// <summary>
    /// 덱 리로드
    /// </summary>
    public void Reload()
    {
        int handCount = hand.GetPile().Count;

        deck.AddCardPile(hand.GetPile());
        deck.AddCardPile(grave.GetPile());
        hand.Reset();
        grave.Reset();
        for(int i=0;i<handCount;i++)
        {
            CardDraw();
        }
    }
    
}

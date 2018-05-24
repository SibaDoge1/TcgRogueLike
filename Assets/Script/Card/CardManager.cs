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
    }

    Deck deck;
    Grave grave;
    Hand hand;
    /// <summary>
    /// 카드 획득 해서 목적지로
    /// </summary>
    public void AddCard(CardData card,CardPile target = null)
    {
        if(target == null)
        {
            deck.AddCard(card);
        }else
        {
            target.AddCard(card);
        }
    }

    /// <summary>
    /// 임시용 덱 메이킹 함수
    /// </summary>
    void TempMakeDeck()
    {
        
    }
    /// <summary>
    /// 덱에서 카드 드로우
    /// </summary>
    public void CardDraw()
    {

    }

    /// <summary>
    /// 핸드에서 카드 사용
    /// </summary>
    public void UseCard()
    {

    }

    /// <summary>
    /// 덱 리로드
    /// </summary>
    public void Reload()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPile
{
    protected List<CardData> cardPile = new List<CardData>();


    /// <summary>
    /// 해당카드 삭제
    /// </summary>
    public virtual void DelteCard(CardData card)
    {
        cardPile.Remove(card);
    }
    public virtual void AddCard(CardData card)
    {
        cardPile.Add(card);
    }
    /// <summary>
    /// 랜덤으로 가져오기
    /// </summary>
    public virtual CardData GetRandomCard()
    {
        return cardPile[Random.Range(0, cardPile.Count)];
    }

}

public class Hand :CardPile
{
    
}

public class Deck : CardPile
{
    private List<CardData> battle;//패시브 카드 제외한 카드 리스트

    public override void AddCard(CardData card)
    {
        base.AddCard(card);
        if(card.Category != CardCategory.PASSI)
        {
            battle.Add(card);
        }
    }
    /// <summary>
    /// 제일 밑에 있는 카드 가져오기
    /// </summary>
    public CardData GetCard()
    {
        return cardPile[0];
    }
    public void Shuffle()
    {

    }
}
public class Grave : CardPile
{

}




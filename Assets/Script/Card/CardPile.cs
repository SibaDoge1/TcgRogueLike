using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ActiveCard만 들어가는 cardPile 자료구조입니다.
/// </summary>
public class CardPile
{
    protected List<ActiveCard> cardPile = new List<ActiveCard>();

    /// <summary>
    /// 덱가져오기
    /// </summary>
    /// <returns></returns>
    public virtual List<ActiveCard> GetPile()
    {
        return cardPile;
    }
    /// <summary>
    /// 해당카드 삭제
    /// </summary>
    public virtual void DelteCard(ActiveCard card)
    {
        cardPile.Remove(card);
    }
    public virtual void AddCard(ActiveCard card)
    {
        cardPile.Add(card);
    }
    /// <summary>
    /// 카드가져오기
    /// </summary>
    public virtual ActiveCard GetCard(int i=0)
    {
        return cardPile[i];
    }
    public virtual void DelteCard(int i = 0)
    {
        cardPile.RemoveAt(i);
    }
    /// <summary>
    /// 랜덤으로 가져오기
    /// </summary>
    public virtual ActiveCard GetRandomCard()
    {
        return cardPile[Random.Range(0, cardPile.Count)];
    }
    /// <summary>
    /// 카드뭉치 집어넣기
    /// </summary>
    public virtual void AddCardPile(List<ActiveCard> pile)
    {
        cardPile.AddRange(pile);
    }
    /// <summary>
    /// 카드 뭉치 리셋
    /// </summary>
    public virtual void Reset()
    {
        cardPile.Clear();
    }
}
public class Grave : CardPile
{
    public void Shuffle()
    {
        for (int i = 0; i < cardPile.Count; i++)
        {
            ActiveCard temp = cardPile[i];
            int rnd = Random.Range(i + 1, cardPile.Count);
            cardPile[i] = cardPile[rnd];
            cardPile[rnd] = temp;
        }
    }
}
public class Hand :CardPile
{
    public override void AddCard(ActiveCard card)
    {
        if(cardPile.Count<5)
        base.AddCard(card);
    }
    public override void AddCardPile(List<ActiveCard> pile)
    {
        for(int i=0; i<pile.Count; i++)
        {
            AddCard(pile[i]);
        }
    }
}

public class Deck : CardPile
{

    /// <summary>
    ///카드 섞기
    /// </summary>
    public void Shuffle()
    {
        for(int i=0; i<cardPile.Count;i++)
        {
            ActiveCard temp = cardPile[i];
            int rnd = Random.Range(i+1, cardPile.Count);
            cardPile[i] = cardPile[rnd];
            cardPile[rnd] = temp;
        }
    }
}





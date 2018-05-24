using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardCategory
{
    PASSI,//패시브
    DISPOSE,//일회성
    PERMAN//영구성
}
public enum CardType
{
    ATTACK,
    MAGIC
}
public enum CardAttribute
{
    NONE,
    FIRE,
    WATER,
    LEAF
}

public abstract class CardData
{
    protected CardCategory category;
    public CardCategory Category
    {
        get { return category; }
    }
    protected CardType type;
    public CardType Type
    {
        get { return type; }
    }
    protected CardAttribute attribute;
    public CardAttribute Attribute
    {
        get { return attribute; }
        set { attribute = value; }
    }

    protected string imageInfo;
    public string ImageInfo
    {
        get { return imageInfo; }
    }

    protected string cardInfo;
    public string CardInfo
    {
        get { return cardInfo; }
    }

    protected int atk;
    /// <summary>
    /// 효과 발동
    /// </summary>
    public abstract void ActiveThis();
}


public class Card0000 : CardData
{
    public Card0000()
    {
        category = CardCategory.PERMAN;
        type = CardType.ATTACK;         
    }
    public override void ActiveThis()
    {
    }
}
public class Card0001 : CardData
{
    public Card0001(Player player)
    {
        category = CardCategory.PERMAN;
        type = CardType.ATTACK;
    }
    public override void ActiveThis()
    {
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public abstract class CardData
{
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
}

public abstract class PassiveCard : CardData
{

}
#region ActiveCards
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
public abstract class ActiveCard : CardData
{
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

    /// <summary>
    /// 효과 발동
    /// </summary>
    public abstract void ActiveThis();

    /// <summary>
    /// 생성자 파라미터 = 속성
    /// </summary>
    public ActiveCard(CardAttribute atr = CardAttribute.NONE)
    {
        attribute = atr;
    }
}
public class Card_Sword : ActiveCard
{
    int range = 3;
    int damage = 5;
    public Card_Sword(CardAttribute atr) : base(atr)
    {
        type = CardType.ATTACK;
        //Set ImageInfo
        cardInfo = range + "의 범위에 적에게+" + damage + "의 데미지를 줍니다.";
    }
    public override void ActiveThis()
    {
        //todo : 플레이어 근처 오토타겟팅 어택 구현
    }
}
public class Card_Bandage : ActiveCard
{
    int heal = 3;
    Player player;
    public Card_Bandage(CardAttribute atr,Player _player) : base(atr)
    {
        type = CardType.ATTACK;
        player = _player;

        cardInfo = "자신의 hp를" + heal + "만큼 회복합니다.";
        //Set ImageInfo
    }

    public override void ActiveThis()
    {
        player.currentHp += heal;
    }
}
#endregion

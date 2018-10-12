using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Attribute
{
    PRITHVI,
    APAS,
    TEJAS,
    VAYU
}
public enum Rating
{
    R0,
    R1,
    R2,
    R3,
    R4,
    R5
}
public enum CardAbilityType{Attack, NonAttack}
public abstract class Card
{
    protected static Player player = PlayerControl.Player;
    #region CardValues;

    protected bool isDirectionCard = false;
    public bool IsDirectionCard
    {
        get { return isDirectionCard; }
    }

    protected int index;
    protected int val1;
    protected int val2;
    protected int val3;

    protected CardData cardData;
    public CardData CardData
     {
        get { return cardData; }
    }
    #endregion
    protected CardEffectType cardEffect = CardEffectType.Hit;
    protected CardSoundType cardSound = CardSoundType.Hit;

    public Card()
    {
        SetIndex();
        cardData = Database.GetCardData(index);
        ValueReset();
    }
    /// <summary>
    /// 이 카드 객체의 value를 초기화
    /// </summary>
    public void ValueReset()
    {
        val1 = cardData.val1;
        val2 = cardData.val2;
        val3 = cardData.val3;
    }
    /// <summary>
    /// 인덱스 처음에 초기화
    /// </summary>
    protected abstract void SetIndex();

	public CardObject InstantiateHandCard(){
		CardObject cardObject;
		cardObject = InstantiateDelegate.ProxyInstantiate (Resources.Load(Database.cardObjectPath)as GameObject).GetComponent<CardObject> ();
        cardObject.SetCardRender(this);
		return cardObject;
	}
    public EditCardObject InstantiateDeckCard()
    {
        EditCardObject cardObject;
        cardObject = InstantiateDelegate.ProxyInstantiate(Resources.Load(Database.editCardObjectPath) as GameObject).GetComponent<EditCardObject>();
        cardObject.SetCard(this);
        return cardObject;
    }

	protected virtual void CardActive()
    {
	}
    protected virtual void CardActive(Direction d)
    {
    }
    public void DoCard()
    { 
        ConsumeAkasha();
        CardActive();
    }
    public void DoCard(Direction d)
    {
        ConsumeAkasha();
        CardActive(d);
    }

    protected virtual void MakeEffect(Vector3 target)
    {
        EffectDelegate.instance.MadeEffect(cardEffect, target);
    }
    protected virtual void MakeSound(Vector3 target)
    {
        SoundDelegate.instance.PlayCardSound(cardSound, target);
    }

    protected virtual void ConsumeAkasha()
    {
        PlayerData.AkashaGage -= (int)cardData.cost;
    }
    public virtual bool IsAvailable()
    {
		return (PlayerData.AkashaGage>=(int)cardData.cost) ? true:false;
	}

	public virtual void CardEffectPreview(){		
	}
	public virtual void CancelPreview(){
	}
    

	public virtual bool IsConsumeTurn(){
		return true;
	}

    /// <summary>
    /// 클래스명으로 카드 가져오기, 나중에 번호로 가져오기도 만들것
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static Card GetCardByName(string s)
    {
        if (System.Type.GetType(s) != null)
        {
            var c = System.Activator.CreateInstance(System.Type.GetType(s));
            if (c is Card)
            {
                return (c as Card);
            }
            else
            {
                UnityEngine.Debug.Log("Card String Error");
                return null;
            }
        }
        else
        {
            UnityEngine.Debug.Log("Card String Error");
            return null;
        }
    }
    public static Card GetCardByNum(int i)
    {
        
        if (System.Type.GetType(Database.GetCardData(i).className) != null)
        {
            var c = System.Activator.CreateInstance(System.Type.GetType(Database.GetCardData(i).className));
            if (c is Card)
            {
                return (c as Card);
            }
            else
            {
                UnityEngine.Debug.Log("Card String Error");
                return null;
            }
        }
        else
        {
            UnityEngine.Debug.Log("Card String Error");
            return null;
        }
    }
}

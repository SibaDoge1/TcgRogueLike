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
public enum CardAbilityType{Attack, Util}
public  class CardData {
    protected Attribute cardAtr;
    public Attribute CardAtr { get { return cardAtr; } }
    protected Rating rating = Rating.R0;
    public Rating Rating {get { return rating; } }

    protected int index;
    public int Index
    {
        get { return index; }
    }
    protected Player player = PlayerControl.Player;
	protected string spritePath;
    public string SpritePath
    {
        get { return spritePath; }
    }
	protected string cardName;
	public string CardName {
		get { return cardName; }
	}

    protected string cardExplain;
	public string CardExplain {
		get { return cardExplain; }
	}
	protected virtual void SetData()
    {
        cardName = CardDatabase.cardNames[index];
        spritePath = CardDatabase.cardSpritePaths[index];
        cardExplain = CardDatabase.cardInfos[index];
    }
	protected CardEffectType effectType;

	public CardObject InstantiateHandCard(){
		CardObject cardObject;
		cardObject = InstantiateDelegate.ProxyInstantiate (Resources.Load(CardDatabase.cardObjectPath)as GameObject).GetComponent<CardObject> ();
        cardObject.SetCardRender(this);
		return cardObject;
	}
    public EditCardObject InstantiateDeckCard()
    {
        EditCardObject cardObject;
        cardObject = InstantiateDelegate.ProxyInstantiate(Resources.Load(CardDatabase.editCardObjectPath) as GameObject).GetComponent<EditCardObject>();
        cardObject.SetCardData(this);
        return cardObject;
    }

	public virtual void CardActive()
    {
        PlayerData.AkashaCount -= (int)rating;
	}

	public virtual bool IsAvailable()
    {
		return false;
	}

	public virtual void CardEffectPreview(){
		
	}
	public virtual void CancelPreview(){
	}

	public virtual bool IsConsumeTurn(){
		return true;
	}

	public virtual CardAbilityType GetCardAbilityType(){
		return CardAbilityType.Attack;
	}
    /// <summary>
    /// 클래스명으로 카드 가져오기, 나중에 번호로 가져오기도 만들것
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static CardData GetCardByName(string s)
    {
        if (System.Type.GetType(s) != null)
        {
            var c = System.Activator.CreateInstance(System.Type.GetType(s));
            if (c is CardData)
            {
                return (c as CardData);
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

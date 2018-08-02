using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Attribute
{
    NONE,
    AK,
    PRITHVI,
    APAS,
    TEJAS,
    VAYU
}
public enum CardAbilityType{Attack, Util}
public  class CardData {
    protected Attribute cardAtr;
    public Attribute CardAtr { get { return cardAtr; } }

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
        cardObject.SetCardData(this);
		return cardObject;
	}
    public EditCardObject InstantiateDeckCard()
    {
        EditCardObject cardObject;
        cardObject = InstantiateDelegate.ProxyInstantiate(Resources.Load(CardDatabase.editCardObjectPath) as GameObject).GetComponent<EditCardObject>();
        cardObject.SetCardData(this);
        return cardObject;
    }

	public virtual void CardActive(){//Room Parameter
		
	}

	public virtual bool IsAvailable(){//Room Parameter
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

}

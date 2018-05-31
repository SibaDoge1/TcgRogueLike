using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardAttribute
{
	NONE,
	FIRE,
	WATER,
	LEAF
}
public enum CardAbilityType{Attack, Heal, Util}
public class CardData {
	protected CardData(){}
	public CardData(int cardIndex){
		cardName = CardDatabase.cardNames [cardIndex];
		spritePath = CardDatabase.cardSpritePaths [cardIndex];
	}
		
	protected string spritePath;
	protected string cardName;
	public string CardName {
		get { return cardName; }
	}

	protected string cardExplain;
	public string CardExplain {
		get { return cardExplain; }
	}
		
	protected CardEffectType effectType;


	public CardObject Instantiate(){
		CardObject cardObject;
		cardObject = InstantiateDelegate.ProxyInstantiate (Resources.Load(CardDatabase.cardObjectPath)as GameObject).GetComponent<CardObject> ();
		cardObject.Init (this, Resources.Load<Sprite> (CardDatabase.cardResourcePath + spritePath));
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
	public virtual string GetCardAbilityValue(){
		return "0";
	}
}

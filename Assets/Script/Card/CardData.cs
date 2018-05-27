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
public enum CardAnimationType{FrontSlash, SelfBuff}
public enum CardEffectType{Slash, Heal}
public class CardData {
	protected CardData(){}
	public CardData(int cardIndex){

	}

	private const string cardObjectPath = "Card/CardBase";
	protected string spritePath = "Card/CardName";
	protected string cardName;
	protected string cardExplain;

	CardAnimationType animType;
	CardEffectType effectType;







	public CardObject Instantiate(){
		CardObject cardObject;
		cardObject = InstantiateDelegate.ProxyInstantiate (Resources.Load(cardObjectPath)as GameObject).GetComponent<CardObject> ();
		cardObject.Init (this, Resources.Load<Sprite> (spritePath));
		return cardObject;
	}


	public virtual void CardActive(){//Room Parameter
		
	}

	public virtual bool IsAvailable(){//Room Parameter
		return false;
	}
}

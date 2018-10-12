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
public  class Card
{
    protected bool isDirectionCard = false;
    public bool IsDirectionCard
    {
        get { return isDirectionCard; }
    }
    protected Attribute cardAtr;
    public Attribute CardAtr { get { return cardAtr; } }
    protected Rating rating = Rating.R0;
    public Rating Rating {get { return rating; } }

    protected CardEffectType cardEffect = CardEffectType.Hit;
    protected CardSoundType cardSound = CardSoundType.Hit;
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
        cardName = Database.cardNames[index];
        spritePath = Database.cardSpritePaths[index];
        cardExplain = Database.cardInfos[index];
    }

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
        PlayerData.AkashaGage -= (int)rating;
    }
    public virtual bool IsAvailable()
    {
		return (PlayerData.AkashaGage>=(int)rating)? true:false;
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
}

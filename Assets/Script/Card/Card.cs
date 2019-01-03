using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum CardAbilityType{Attack, NonAttack}
/// <summary>
/// 카드 데이터
/// </summary>
public abstract class Card
{
    protected static Player player;
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
    protected Figure figure;
    protected int range;

    protected List<GameObject> ranges = new List<GameObject>();
    protected virtual void SetRangeData() { }
    
    public static void SetPlayer(Player p)
    {
        player = p;
    }
    public Card()
    {
        SetIndex();
        cardData = Database.GetCardData(index);
        ValueReset();
        SetRangeData();
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
    /// Val값들 적용하여 Info string 리턴
    /// </summary>
    public string GetCardInfoString()
    {
        string[] s = string.Copy(cardData._info).Split('<','>');
       for(int i=0; i< s.Length;i++)
        {
            if(s[i] == "val1" || s[i] == "Val1")
            {
                s[i] = "" + val1;
            }else if(s[i] == "val2"  || s[i] == "Val2")
            {
                s[i] = "" + val2;
            }else if (s[i] == "val3" || s[i] == "Val3")
            {
                s[i] = "" + val3;
            }          
        }
        return string.Join("",s);
    }
    /// <summary>
    /// 인덱스 처음에 초기화
    /// </summary>
    protected abstract void SetIndex();

	public CardObject InstantiateHandCard(){
		CardObject cardObject;
        cardObject = ArchLoader.instance.GetCardObject();
        cardObject.SetCardRender(this);
		return cardObject;
	}
    public EditCardObject InstantiateDeckCard()
    {
        EditCardObject cardObject;
        cardObject = ArchLoader.instance.GetEditCard();
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
        EffectDelegate.instance.DestroyEffect(ranges);
        ranges.Clear();
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
        return GetCardByName(Database.GetCardData(i).className);      
    }
}

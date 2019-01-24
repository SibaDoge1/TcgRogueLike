using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType
{
    N,//기본카드 중 효과안붙음
    V,//기본카드 효과
    T,//기본카드 효과
    P,//기본카드 효과
    A,//기본카드 효과
    S
}

/// <summary>
/// 카드 데이터
/// </summary>
public abstract class Card
{
    protected static Player player;
    #region CardValues;
    protected bool isUpgraded = false;
    public bool IsUpgraded { get { return isUpgraded; } }

    protected bool isDirectionCard = false;
    public bool IsDirectionCard
    {
        get { return isDirectionCard; }
    }

    protected int index;
    public int Index { get { return index; } }

    protected string name;
    public string Name {get { return name; } }

    protected int cost;
    public int Cost { get { return cost; } }

    protected CardType cardType = CardType.N;
    public CardType Type { get { return cardType; } }

    protected int val1;
    protected int val2;
    protected int val3;

    protected string info;
    public string Info { get { return info; } }

    protected string spritePath;
    public string SpritePath { get { return spritePath; } }

    protected CardEffectType cardEffect = CardEffectType.Hit;
    protected CardSoundType cardSound = CardSoundType.Hit;
    #endregion


    protected List<GameObject> ranges = new List<GameObject>();
    
    public static void SetPlayer(Player p)
    {
        player = p;
    }


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
        PlayerData.AkashaGage -= cost;
    }
    public virtual bool IsAvailable()
    {
		return (PlayerData.AkashaGage>=cost) ? true:false;
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
    /// 스페셜 카드 가져오기
    /// </summary>
    private static Card GetSpecialCard(CardData cardData)
    {
        System.Type t = System.Type.GetType(cardData.className);
        if (t != null)
        {
            var c = System.Activator.CreateInstance(t,cardData);//해당 타입의 Instance 생성
            if (c is Card_Special)
            {
                return c as Card_Special;
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
        if(i == 0)
        {
            return new Card_Normal();
        }
        else
        {
            return GetSpecialCard(Database.GetCardData(i));
        }
    }

    /// <summary>
    /// 공격카드에서 데미지를 가할때는 이함수로 할것
    /// </summary>
    protected void DamageToTarget(Enemy target, float dam)
    {
        if (target != null)
        {
            MakeEffect(target.transform.position);
            MakeSound(target.transform.position);
            target.GetDamage(dam * player.Atk, player);
            PlayerData.AttackedTarget();
        }
    }
}

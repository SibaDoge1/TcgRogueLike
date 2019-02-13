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
    private bool isUpgraded = false;
    public bool IsUpgraded { get { return isUpgraded; } }

    protected bool isDirectionCard = false;
    public bool IsDirectionCard
    {
        get { return isDirectionCard; }
    }

    protected int index;
    public int Index { get { return index; } }

    protected string name;
    public string Name { get { return name; } }

    protected int cost;
    public int Cost
    {
        get { return cost; }
        set { cost = value; }
    }

    protected CardType cardType = CardType.N;
    public CardType Type { get { return cardType; } }

    protected int val1;
    protected int val2;
    protected int val3;

    protected string info;
    public string Info
    { get
        {
            string[] s = string.Copy(info).Split('<', '>');
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == "val1" || s[i] == "Val1")
                {
                    s[i] = "" + val1;
                }
                else if (s[i] == "val2" || s[i] == "Val2")
                {
                    s[i] = "" + val2;
                }
                else if (s[i] == "val3" || s[i] == "Val3")
                {
                    s[i] = "" + val3;
                }
            }

            return string.Join("", s);
        }
    }

    protected string spritePath;
    public string SpritePath { get { return spritePath; } }

    protected CardEffectType cardEffect = CardEffectType.Hit;
    protected CardSoundType cardSound = CardSoundType.Hit;

    public float effectTime = 0.1f;
    #endregion


    protected List<GameObject> ranges = new List<GameObject>();
    
    public static void SetPlayer(Player p)
    {
        player = p;
    }


	public HandCardObject InstantiateHandCard(){

		HandCardObject cardObject;
        cardObject = ArchLoader.instance.GetCardObject();
        cardObject.SetCardData(this);

		return cardObject;
	}
    public EditCardObject InstantiateEditCard()
    {
        EditCardObject cardObject;
        cardObject = ArchLoader.instance.GetEditCard();
        cardObject.SetCardData(this);
        return cardObject;
    }

    public CheckCardObject InstantiateCheckCard()
    {
        CheckCardObject cardObject;
        cardObject = ArchLoader.instance.GetCheckCard();
        cardObject.SetCardData(this);
        return cardObject;
    }

	protected virtual void CardActive()
    {
	}
    protected virtual void CardActive(Direction d)
    {
    }
    public virtual void OnCardPlayed()
    {
        if(isDirectionCard)
        {
            return;
        }else
        {
            ConsumeAkasha();
            CardActive();
        }
    }
    public virtual void OnCardPlayed(Direction d)
    {
        ConsumeAkasha();
        CardActive(d);
    }
    /// <summary>
    /// 이 카드가 반환되었을때 콜
    /// </summary>
    public virtual void OnCardReturned()
    {
        PlayerControl.instance.deck.OnCardReturned(this);
    }

    /// <summary>
    /// 덱의 어떤 카드가 반환됬을시 콜
    /// </summary>
    public virtual void CardReturnCallBack(Card data)
    {

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
    public virtual bool IsCostAvailable()
    {
            return (PlayerData.AkashaGage >= cost) ? true : false;
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

    public void UpgradeThis()
    {
        isUpgraded = true;
        val1 += 1;
    }



}

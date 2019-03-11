using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType
{
    V,//기본카드 효과
    T,//기본카드 효과
    P,//기본카드 효과
    A,//기본카드 무효과
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
    public string Name { get { return name; } }

    protected int cost;
    public int Cost
    {
        get { return cost; }
        set { cost = value; }
    }

    protected string cardRange = "range_0";
    public string Range { get { return cardRange; } }
    protected CardType cardType = CardType.A;
    public CardType Type { get { return cardType; } }
    protected Figure cardFigure = Figure.CIRCLE;
    public Figure CardFigure { get { return cardFigure; } }
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

    protected CardEffect cardEffect = CardEffect.HIT;
    public CardEffect CardEffect
    {
        get { return cardEffect; }
    }
    protected SoundEffect cardSound = SoundEffect.SFX1;
    public SoundEffect CardSound
    {
        get { return cardSound; }
    }
    public float effectTime = 0.1f;
    #endregion


    protected List<EffectObject> ranges = new List<EffectObject>();
    
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
        CardActive();

        if (!isDirectionCard)
        {
            ConsumeAkasha();
            //MakeSound(player.transform.position);
        }
    }
    public virtual void OnCardPlayed(Direction d)
    {
        ConsumeAkasha();
        //MakeSound(player.transform.position);
        CardActive(d);
    }
    /// <summary>
    /// 이 카드가 반환되었을때 콜
    /// </summary>
    public virtual void OnCardReturned()
    {
        PlayerControl.instance.DeckManager.OnCardReturned(this);
    }

    /// <summary>
    /// 덱의 어떤 카드가 반환됬을시 콜
    /// </summary>
    public virtual void CardReturnCallBack(Card data)
    {

    }


    protected virtual void MakeEffect(Vector3 target)
    {
        ObjectPoolManager.instance.PoolEffect(cardEffect, target);
    }
    protected virtual void MakeEffect(Arch.Tile target)
    {
        ObjectPoolManager.instance.PoolEffect(cardEffect, target);
    }

    protected virtual void MakeEffect(List<Arch.Tile> tiles)
    {
        for(int i=0; i<tiles.Count;i++)
        {
            ObjectPoolManager.instance.PoolEffect(cardEffect, tiles[i]);
        }
    }

    protected virtual void MakeSound(Vector3 target)
    {
        SoundDelegate.instance.PlayEffectSound(cardSound, target);
    }

    protected virtual void ConsumeAkasha()
    {
        PlayerControl.instance.AkashaGage -= cost;
    }
    public virtual bool IsCostAvailable()
    {
            return (PlayerControl.instance.AkashaGage >= cost) ? true : false;
    }

	public virtual void CardEffectPreview(){		
	}
	public virtual void CancelPreview(){
        ObjectPoolManager.instance.DeActiveRange(ranges);
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
                UnityEngine.Debug.Log("Card String Error On Card No." + cardData.index + "  Name : " + cardData.className);
                return null;
            }
        }
        else
        {

            UnityEngine.Debug.Log("Card String Error On Card No." + cardData.index + "  Name : " + cardData.className);
            return null;
        }
    }

    public static Card GetCardByNum(int i)
    {        

            switch(i)
            {
                case 90:
                    return new Card_Normal();
                case 91:
                    return new Card_Normal(CardType.A);
                case 92:
                    return new Card_Normal(CardType.P);
                case 93:
                    return new Card_Normal(CardType.T);
                case 94:
                    return new Card_Normal(CardType.V);
                case 99:
                    return new Card_Reload();
                default:
                    if(Database.CheckCardDataKey(i))
                    {
                    return GetSpecialCard(Database.GetCardData(i));
                    }else
                    {
                    Debug.Log("UnExpected CardNumber " + i);
                    return new Card_Normal();
                    }
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
            PlayerControl.instance.AttackedTarget();
            player.OnAttack();
        }
    }

    public virtual void UpgradeThis()
    {
        if(player.currentRoom.IsEnemyAlive())//전투중일때만
        {
            isUpgraded = true;
            val1++;
        }

    }
    /// <summary>
    /// 초기화
    /// </summary>
    public virtual void UpgradeReset()
    {
    } 

}

public class Card_Reload : Card
{
    public Card_Reload()
    {
        index = 99;
        name = "리로드";
        cost = 0;
        cardType = CardType.S;
        val1 = 0;
        val2 = 0;
        val3 = 0;
        info = "덱을 리로드한 뒤, AKS  3 감소, 이동불가 2턴 상태가 된다.";
        spritePath = "Card_Reload";
        cardEffect = CardEffect.FIRE;
        cardSound = SoundEffect.RELOADCARD;
        cardRange = "range_0";
    }
    public override void CardReturnCallBack(Card data)
    {
    }
    public override void UpgradeReset()
    {      
    }
    public override void OnCardReturned()
    {
        base.OnCardReturned();
        player.GetDamage(1, player,true);
    }

    protected override void CardActive()
    {
        PlayerControl.playerBuff.UpdateBuff(BUFF.MOVE, 3);
        PlayerControl.instance.AkashaGage -= 3;
        PlayerControl.instance.ReLoadDeck();
    }
}
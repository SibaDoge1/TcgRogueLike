using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;


public class Card_Attack : Card {
	public Card_Attack(){}
	public Card_Attack(int index,Player pl,Attribute atr) : base(index,pl){
		range = 1;
		target = 1;
        cardAtr = atr;
	}

	protected int range;
	protected int target;
	protected int damage;

    protected List<GameObject> ranges = new List<GameObject>();
    public override void CardActive (){
		int validTarget = target;
		//Check target in range
		for (int i = 0; i < validTarget; i++) {			
		}
	}

	public override CardAbilityType GetCardAbilityType (){
		return CardAbilityType.Attack;
	}
	public override string GetCardAbilityValue (){
		return damage.ToString();
	}

    /// <summary>
    /// 속성 계산 판정기
    /// </summary>
    private int AtrCompare(Attribute card, Attribute enemy)
    {
        if (card == enemy)
        {
            return 2;
        }
        else return 1;
    }

    /// <summary>
    /// AtkCard에서 데미지를 가할때는 이함수로 할것
    /// </summary>
    protected void DamageToTarget(Enemy target,float dam)
    {
        if(target != null)
        {
            int atr = AtrCompare(cardAtr, target.Atr);
            PlayerData.AkashaGage += atr;
            target.GetDamage(dam * atr * player.Atk, player);
        }
    }
}

public class Card_Sword : Card_Attack {
	public Card_Sword(int index,Player pl, Attribute atr) : base(index,pl,atr)
	{
		damage = 5;
		range = 1;
		//Set ImageInfo
		cardExplain = range + "의 범위중 한 적에게+" + damage + "의 데미지를 줍니다.";
	}
	public override void CardActive (){
		Enemy enemy = null;
		if (TileUtils.IsEnemyAround (player.currentTile, range)) {
            enemy = TileUtils.AutoTarget (player.currentTile, range);

            DamageToTarget(enemy, damage);
            EffectDelegate.instance.MadeEffect (effectType, enemy);
		}
	}

    protected List<Tile> targetTiles;
	public override void CardEffectPreview (){
		targetTiles = TileUtils.SquareRange (player.currentTile, range);
		for(int i = 0; i < targetTiles.Count; i++){
            ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.CARD, targetTiles[i]));
            if (ranges[i] != null)
            {
                ranges[i].transform.parent = player.transform;
            }
        }

	}
	public override void CancelPreview (){
		for(int i = 0; i < targetTiles.Count; i++){
            EffectDelegate.instance.DestroyEffect(ranges);
        }
    }
}
public class Card_BFSword : Card_Attack
{
    public Card_BFSword(int index, Player pl, Attribute atr) : base(index, pl,atr)
    {
        damage = 10;
        range = 1;
        //Set ImageInfo
        cardExplain = range + "의 범위의 모든 적에게+" + damage + "의 데미지를 줍니다.";
    }
    public override void CardActive()
    {

        if (TileUtils.IsEnemyAround(player.currentTile, range))
        {
            List<Enemy> targets = TileUtils.GetNearEnemies(player.currentTile, range);
            foreach (Enemy e in targets)
            {
                DamageToTarget(e,damage);
                EffectDelegate.instance.MadeEffect(CardEffectType.Slash, e.transform.position);
            }
        }
    }

    private List<Tile> targetTiles;
    public override void CardEffectPreview()
    {
        targetTiles = TileUtils.SquareRange(player.currentTile, range);
        for (int i = 0; i < targetTiles.Count; i++)
        {
            ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.CARD, targetTiles[i]));
            if (ranges[i] != null)
            {
                ranges[i].transform.parent = player.transform;
            }
        }
    }
    public override void CancelPreview()
    {
        for (int i = 0; i < targetTiles.Count; i++)
        {
            EffectDelegate.instance.DestroyEffect(ranges);
        }
    }
}


public class Card_Stone : Card_Attack
{
    public Card_Stone(int index, Player pl, Attribute atr) : base(index, pl,atr)
    {
        damage = 5;
        //Set ImageInfo
        range = 3;
        cardExplain = range + "의 범위중 한 적에게+" + damage + "의 데미지를 줍니다.";
    }
    public override void CardActive()
    {
        Enemy target = TileUtils.AutoTarget(player.currentTile, range);
        if (target != null)
        {
            DamageToTarget(target, damage);
            EffectDelegate.instance.MadeBullet(BulletType.Stone, PlayerControl.Player.transform.position, target.transform);
        }
    }

    private List<Tile> targetTiles;
    public override void CardEffectPreview()
    {
        targetTiles = TileUtils.SquareRange(player.currentTile, range);
        for (int i = 0; i < targetTiles.Count; i++)
        {
            ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.CARD, targetTiles[i]));
            if(ranges[i] != null)
            {
                ranges[i].transform.parent = player.transform;
            }
        }
    }
    public override void CancelPreview()
    {
        for (int i = 0; i < targetTiles.Count; i++)
        {
            EffectDelegate.instance.DestroyEffect(ranges);
        }
    }
}

public class Card_Arrow : Card_Attack
{
    public Card_Arrow(int index, Player pl, Attribute atr) : base(index, pl,atr)
    {
        damage = 10;
        //Set ImageInfo
        range = 5;
        cardExplain = range + "의 범위중 한 적에게+" + damage + "의 데미지를 줍니다.";
    }
    public override void CardActive()
    {
        Enemy target = TileUtils.AutoTarget(player.currentTile, range);
        if (target != null)
        {
            DamageToTarget(target, damage);
            EffectDelegate.instance.MadeBullet(BulletType.Arrow, player.transform.position, target.transform);
        }
    }
    private List<Tile> targetTiles;
    public override void CardEffectPreview()
    {
        targetTiles = TileUtils.SquareRange(player.currentTile, range);
        for (int i = 0; i < targetTiles.Count; i++)
        {
            ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.CARD, targetTiles[i]));
            if (ranges[i] != null)
            {
                ranges[i].transform.parent = player.transform;
            }
        }
    }
    public override void CancelPreview()
    {
        for (int i = 0; i < targetTiles.Count; i++)
        {
            EffectDelegate.instance.DestroyEffect(ranges);
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;


public class CardData_Attack : CardData {
	public CardData_Attack(){}
	public CardData_Attack(int index,Player pl,Attribute atr) : base(index,pl){
		range = 1;
		target = 1;
        cardAtr = atr;
	}

	protected int range;
	protected int target;
	protected int damage;

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

    private int AtrCounter(Attribute card, Attribute enemy)
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
    protected void DamageToTarget(Entity target,int dam)
    {
        Enemy etarget = target as Enemy;
        if(etarget != null)
        {
            etarget.GetDamage(dam * AtrCounter(cardAtr,etarget.Atr),player);
            //etarget.AtrCheck(cardAtr);
        }
        else
        {
            target.GetDamage(dam);
        }
    }
}

public class CardData_Sword : CardData_Attack {
	public CardData_Sword(int index,Player pl, Attribute atr) : base(index,pl,atr)
	{
		damage = 5;
		range = 1;
		//Set ImageInfo
		cardExplain = range + "의 범위중 한 적에게+" + damage + "의 데미지를 줍니다.";
	}
	public override void CardActive (){
		Entity target = null;
		if (TileUtils.IsHitableAround (player.currentTile, range)) {
			target = TileUtils.AutoTarget (player.currentTile, range);

            DamageToTarget(target,damage);
            EffectDelegate.instance.MadeEffect (effectType, target);
		}
	}

	private List<Tile> targetTiles;
	public override void CardEffectPreview (){
		targetTiles = TileUtils.SquareRange (player.currentTile, range);
		for(int i = 0; i < targetTiles.Count; i++){
			targetTiles [i].mySprite.color = Color.red;
		}
	}
	public override void CancelPreview (){
		for(int i = 0; i < targetTiles.Count; i++){
			targetTiles [i].mySprite.color = Color.white;
		}
	}
}
public class CardData_BFSword : CardData_Attack
{
    public CardData_BFSword(int index, Player pl, Attribute atr) : base(index, pl,atr)
    {
        damage = 10;
        range = 1;
        //Set ImageInfo
        cardExplain = range + "의 범위의 모든 적에게+" + damage + "의 데미지를 줍니다.";
    }
    public override void CardActive()
    {

        if (TileUtils.IsHitableAround(player.currentTile, range))
        {
            List<Entity> targets = TileUtils.GetNearEnemies(player.currentTile, range);
            foreach (Entity e in targets)
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
            targetTiles[i].mySprite.color = Color.red;
        }
    }
    public override void CancelPreview()
    {
        for (int i = 0; i < targetTiles.Count; i++)
        {
            targetTiles[i].mySprite.color = Color.white;
        }
    }
}


public class CardData_Stone : CardData_Attack
{
    public CardData_Stone(int index, Player pl, Attribute atr) : base(index, pl,atr)
    {
        damage = 5;
        //Set ImageInfo
        range = 3;
        cardExplain = range + "의 범위중 한 적에게+" + damage + "의 데미지를 줍니다.";
    }
    public override void CardActive()
    {
        Entity target = TileUtils.AutoTarget(player.currentTile, range);
        if (target != null)
        {
            DamageToTarget(target, damage);
            EffectDelegate.instance.MadeBullet(BulletType.Stone, PlayerControl.instance.PlayerObject.transform.position, target.transform);
        }
    }

    private List<Tile> targetTiles;
    public override void CardEffectPreview()
    {
        targetTiles = TileUtils.SquareRange(player.currentTile, range);
        for (int i = 0; i < targetTiles.Count; i++)
        {
            targetTiles[i].mySprite.color = Color.red;
        }
    }
    public override void CancelPreview()
    {
        for (int i = 0; i < targetTiles.Count; i++)
        {
            targetTiles[i].mySprite.color = Color.white;
        }
    }
}

public class CardData_Arrow : CardData_Attack
{
    public CardData_Arrow(int index, Player pl, Attribute atr) : base(index, pl,atr)
    {
        damage = 10;
        //Set ImageInfo
        range = 5;
        cardExplain = range + "의 범위중 한 적에게+" + damage + "의 데미지를 줍니다.";
    }
    public override void CardActive()
    {
        Entity target = TileUtils.AutoTarget(player.currentTile, range);
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
            targetTiles[i].mySprite.color = Color.red;
        }
    }
    public override void CancelPreview()
    {
        for (int i = 0; i < targetTiles.Count; i++)
        {
            targetTiles[i].mySprite.color = Color.white;
        }
    }
}


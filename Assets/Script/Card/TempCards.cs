using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;

public class CardData_BFSword : CardData_Normal
{
    public CardData_BFSword(int index) : base(index)
	{
        damage = 10;
        range = 1;
        //Set ImageInfo
        cardExplain = range + "의 범위의 모든 적에게+" + damage + "의 데미지를 줍니다.";
    }
    public override void CardActive()
    {

        if (TileUtils.IsHitableAround(PlayerControl.instance.PlayerObject.currentTile, range))
        {
            List<Entity> targets = TileUtils.GetNearEnemies(PlayerControl.instance.PlayerObject.currentTile, range);
            foreach(Entity e in targets)
            {
                e.currentHp -= damage;
				EffectDelegate.instance.MadeEffect (CardEffectType.Slash, e.transform.position);
            }
        }
    }

    private List<Tile> targetTiles;
    public override void CardEffectPreview()
    {
        targetTiles = TileUtils.SquareRange(PlayerControl.instance.PlayerObject.currentTile, range);
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
public class CardData_Tumble : CardData_Magic
{
    int cardNum = 3;

    public CardData_Tumble() { }
    public CardData_Tumble(int index) : base(index)
    {
        cardExplain = "카드 " + cardNum + "장을 드로우 합니다.";
        effectType = CardEffectType.Heal;
    }

    public override void CardActive()
    {
        Routine del = DrawCards;
        CoroutineDelegate.instance.StartRoutine(del);
    }
	IEnumerator DrawCards()
    {
        for(int i=0; i<cardNum;i++)
        {
			PlayerControl.instance.MagicDraw();
            yield return new WaitForSeconds(0.1f);
        }
    }
	public override CardAbilityType GetCardAbilityType (){
		return CardAbilityType.Util;
	}
	public override string GetCardAbilityValue (){
		return cardNum.ToString();
	}
}

public class CardData_Stone : CardData_Normal{
	public CardData_Stone(int index) : base(index)
	{
		damage = 5;
		//Set ImageInfo
		range = 3;
		cardExplain = range + "의 범위중 한 적에게+" + damage + "의 데미지를 줍니다.";
	}
	public override void CardActive()
	{
		Entity target = TileUtils.AutoTarget (PlayerControl.instance.PlayerObject.currentTile, range);
        if(target != null)
        {
            target.currentHp -= damage;
            EffectDelegate.instance.MadeBullet(BulletType.Stone, PlayerControl.instance.PlayerObject.transform.position, target.transform);
        }     
	}

	private List<Tile> targetTiles;
	public override void CardEffectPreview()
	{
		targetTiles = TileUtils.SquareRange(PlayerControl.instance.PlayerObject.currentTile, range);
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
 
public class CardData_Arrow : CardData_Normal{
	public CardData_Arrow(int index) : base(index)
	{
		damage = 10;
		//Set ImageInfo
		range = 5;
		cardExplain = range + "의 범위중 한 적에게+" + damage + "의 데미지를 줍니다.";
	}
	public override void CardActive()
	{
		Entity target = TileUtils.AutoTarget (PlayerControl.instance.PlayerObject.currentTile, range);
        if(target!=null)
        {
            target.currentHp -= damage;
            EffectDelegate.instance.MadeBullet(BulletType.Arrow, PlayerControl.instance.PlayerObject.transform.position, target.transform);
        }     
	}

	private List<Tile> targetTiles;
	public override void CardEffectPreview()
	{
		targetTiles = TileUtils.SquareRange(PlayerControl.instance.PlayerObject.currentTile, range);
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

public class CardData_Portion : CardData_Magic{
	public CardData_Portion(int index) : base(index)
	{
		cardExplain = "모든 hp를 회복합니다.";
		effectType = CardEffectType.Heal;
	}
	public override void CardActive()
	{
        PlayerControl.instance.PlayerObject.currentHp += 999;
		EffectDelegate.instance.MadeEffect(effectType, PlayerControl.instance.PlayerObject);
	}

	public override CardAbilityType GetCardAbilityType (){
		return CardAbilityType.Heal;
	}
	public override string GetCardAbilityValue (){
		return "All";
	}
}
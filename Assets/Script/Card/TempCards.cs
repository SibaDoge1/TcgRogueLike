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

        if (TileUtils.IsHitableAround(GameManager.instance.GetCurrentRoom().GetPlayerTile(), range))
        {
            List<OnTileObject> targets = TileUtils.GetNearEnemies(GameManager.instance.GetCurrentRoom().GetPlayerTile(), range);
            foreach(OnTileObject e in targets)
            {
                e.currentHp -= damage;
				EffectDelegate.instance.MadeEffect (CardEffectType.Slash, e.transform.position);
            }
        }
    }

    private List<Tile> targetTiles;
    public override void CardEffectPreview()
    {
        targetTiles = TileUtils.SquareRange(GameManager.instance.GetCurrentRoom().GetPlayerTile(), range);
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
    public CardData_Tumble() { }
    public CardData_Tumble(int index) : base(index)
    {
        cardExplain = "자신의 hp를" + healAmount + "만큼 회복하고 "+cardNum+"장을 드로우 합니다.";
        effectType = CardEffectType.Heal;
    }
    int cardNum=3;
    int healAmount = 4;

    public override void CardActive()
    {
		GameManager.instance.GetCurrentRoom().GetPlayerTile().OnTileObj.currentHp += healAmount;
        Routine del = DrawCards;
        CoroutineDelegate.instance.StartRoutine(del);
        EffectDelegate.instance.MadeEffect(effectType, PlayerControl.instance.PlayerObject);
    }
	IEnumerator DrawCards(){
        for(int i=0; i<cardNum;i++)
        {
            PlayerControl.instance.DrawCard();
            yield return new WaitForSeconds(0.1f);
        }
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
		OnTileObject target = TileUtils.AutoTarget (GameManager.instance.GetCurrentRoom ().GetPlayerTile (), range);
		target.currentHp -= damage;
		EffectDelegate.instance.MadeBullet(BulletType.Arrow, PlayerControl.instance.PlayerObject.transform.position, target.transform);
	}

	private List<Tile> targetTiles;
	public override void CardEffectPreview()
	{
		targetTiles = TileUtils.SquareRange(GameManager.instance.GetCurrentRoom().GetPlayerTile(), range);
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
		OnTileObject target = TileUtils.AutoTarget (GameManager.instance.GetCurrentRoom ().GetPlayerTile (), range);
		target.currentHp -= damage;
		EffectDelegate.instance.MadeBullet(BulletType.Arrow, PlayerControl.instance.PlayerObject.transform.position, target.transform);
	}

	private List<Tile> targetTiles;
	public override void CardEffectPreview()
	{
		targetTiles = TileUtils.SquareRange(GameManager.instance.GetCurrentRoom().GetPlayerTile(), range);
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
		GameManager.instance.GetCurrentRoom ().GetPlayerTile ().OnTileObj.currentHp += 999;
		EffectDelegate.instance.MadeEffect(effectType, PlayerControl.instance.PlayerObject);
	}


}
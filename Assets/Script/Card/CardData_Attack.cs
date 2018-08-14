using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;


public abstract class CardData_Attack : CardData {


    protected Figure figure;

	protected int range;
	protected int damage;

    protected List<GameObject> ranges = new List<GameObject>();

	public override CardAbilityType GetCardAbilityType (){
		return CardAbilityType.Attack;
	}
	public  string GetCardAttackValue ()
    {
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

public class Card_CroAtt : CardData_Attack {
	public Card_CroAtt() 
	{
        index = 0;
		damage = 5;
		range = 1;
        cardAtr = (Attribute)Random.Range(0, 3);
        figure = Figure.CROSS;
        SetData();
	}
	public override void CardActive()
    {
        base.CardActive();

        Enemy enemy = null;
		if (TileUtils.IsEnemyInRange (player.currentTile, range, figure)) {
            enemy = TileUtils.AutoTarget (player.currentTile, range, figure);

            DamageToTarget(enemy, damage);
            EffectDelegate.instance.MadeEffect (effectType, enemy);
		}
	}

    protected List<Tile> targetTiles;
	public override void CardEffectPreview (){
		targetTiles = TileUtils.Range (player.currentTile, range, figure);
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


public class Card_XAtt : CardData_Attack
{
    public Card_XAtt()
    {
        index = 1;
        damage = 5;
        range = 1;
        cardAtr = (Attribute)Random.Range(0, 3);
        figure = Figure.X;
        SetData();
    }
    public override void CardActive()
    {
        base.CardActive();

        Enemy enemy = null;
        if (TileUtils.IsEnemyInRange(player.currentTile, range, figure))
        {
            enemy = TileUtils.AutoTarget(player.currentTile, range, figure);

            DamageToTarget(enemy, damage);
            EffectDelegate.instance.MadeEffect(effectType, enemy);
        }
    }

    protected List<Tile> targetTiles;
    public override void CardEffectPreview()
    {
        targetTiles = TileUtils.Range(player.currentTile, range, figure);
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

public class Card_SquAtt : CardData_Attack
{
    public Card_SquAtt() 
    {
        cardAtr = (Attribute)Random.Range(0, 3);
        index = 2;
        damage = 5;
        range = 1;
        figure = Figure.SQUARE;
        SetData();
    }
    public override void CardActive()
    {
        base.CardActive();

        if (TileUtils.IsEnemyInRange(player.currentTile, range, figure))
        {
            Enemy enemy = TileUtils.AutoTarget(player.currentTile, range, figure);
            DamageToTarget(enemy,damage);

            /*
             List<Enemy> targets = TileUtils.GetNearEnemies(player.currentTile, range);
             foreach (Enemy e in targets)
             {
                 DamageToTarget(e,damage);
                 EffectDelegate.instance.MadeEffect(CardEffectType.Slash, e.transform.position);
             }
             */
        }
    }

    private List<Tile> targetTiles;
    public override void CardEffectPreview()
    {
        targetTiles = TileUtils.Range(player.currentTile, range, figure);
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
public class Card_SquAttAll : CardData_Attack
{
    public Card_SquAttAll()
    {
        rating = Rating.R1;

        cardAtr = Attribute.TEJAS;
        index = 3;
        damage = 5;
        range = 1;
        figure = Figure.SQUARE;
        SetData();
    }
    public override void CardActive()
    {
        base.CardActive();

        if (TileUtils.IsEnemyInRange(player.currentTile, range, figure))
        {

            List<Enemy> targets = TileUtils.GetEnemies(player.currentTile, range, figure);
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
        targetTiles = TileUtils.Range(player.currentTile, range, figure);
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
public class Card_Mid3Att : CardData_Attack
{
    int num = 3;
    public Card_Mid3Att()
    {
        rating = Rating.R1;

        cardAtr = Attribute.VAYU;
        index = 4;
        damage = 5;
        SetData();
    }
    public override void CardActive()
    {
        base.CardActive();

        List<Tile> tiles = TileUtils.EmptySquareRange(player.currentTile, 2);
        if(TileUtils.IsEnemyInRange(tiles))
        {
           List<Enemy> targets = TileUtils.GetEnemies(tiles, num);
            for(int i=0; i<targets.Count;i++)
            {
                EffectDelegate.instance.MadeEffect(CardEffectType.Slash, targets[i].currentTile);
                DamageToTarget(targets[i], damage);
            }
        }
        
    }

    private List<Tile> targetTiles;
    public override void CardEffectPreview()
    {
        targetTiles = TileUtils.EmptySquareRange(player.currentTile, 2);
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
public class Card_PierceAtt : CardData_Attack
{
    int num = 3;
    public Card_PierceAtt()
    {
        rating = Rating.R1;
        cardAtr = Attribute.APAS;
        index = 5;
        damage = 5;
        figure = Figure.CROSS;
        range = 2;
        SetData();
    }
    public override void CardActive()
    {
        base.CardActive();

        if (TileUtils.IsEnemyInRange(player.currentTile,range,figure))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(player.currentTile, range, figure);
            for(int i=0;i<enemies.Count;i++)
            {
                EffectDelegate.instance.MadeEffect(CardEffectType.Slash, enemies[i].currentTile);
                DamageToTarget(enemies[i], damage);
            }
        }


    }

    private List<Tile> targetTiles;
    public override void CardEffectPreview()
    {
        targetTiles = TileUtils.Range(player.currentTile, 2, figure);
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
public class Card_StrSquAllAtt : CardData_Attack
{
    public Card_StrSquAllAtt()
    {
        rating = Rating.R1;
        cardAtr = Attribute.PRITHVI;
        index = 6;
        damage = 10;
        figure = Figure.SQUARE;
        range = 1;
        SetData();
    }
    public override void CardActive()
    {
        base.CardActive();
        if (TileUtils.IsEnemyInRange(player.currentTile, range, figure))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(player.currentTile, range, figure);
            for (int i = 0; i < enemies.Count; i++)
            {
                EffectDelegate.instance.MadeEffect(CardEffectType.Slash, enemies[i].currentTile);
                DamageToTarget(enemies[i], damage);
            }
        }
    }

    private List<Tile> targetTiles;
    public override void CardEffectPreview()
    {
        targetTiles = TileUtils.Range(player.currentTile, range, figure);
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




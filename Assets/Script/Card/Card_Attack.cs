using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;


public abstract class Card_Attack : Card
{


    protected Figure figure;

	protected int range;
	protected int damage;

    protected List<GameObject> ranges = new List<GameObject>();

	public  string GetCardAttackValue ()
    {
		return damage.ToString();
	}
    protected void InitCard(Rating _rating, Attribute _atr, int _index, int _damage)
    {
        rating = _rating;
        cardAtr = _atr;
        index = _index;
        damage = _damage;
        SetData();
    }
    protected void InitCard(Rating _rating, Attribute _atr, Figure _figure, int _index, int _damage, int _range)
    {
        rating = _rating;
        cardAtr = _atr;
        figure = _figure;
        index = _index;
        damage = _damage;
        range = _range;
        SetData();
    }
    /// <summary>
    /// 속성 계산 판정기
    /// </summary>
    private int AtrCompare(Attribute card, Attribute enemy)
    {
        if (card == enemy)
        {
            cardSound = CardSoundType.CriticalHit;
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
            MakeEffect(target.transform.position);
            MakeSound(target.transform.position);
            //int atr = AtrCompare(cardAtr, target.Atr);
            target.GetDamage(dam * player.Atk, player);
        }
    }

    public override void CancelPreview()
    {
        EffectDelegate.instance.DestroyEffect(ranges);
        ranges.Clear();
    }
}

public class Card_CroAtt : Card_Attack {
	public Card_CroAtt() 
	{
        InitCard(Rating.R0, Attribute.APAS, Figure.CROSS, 0, 5, 1);
	}
    protected override void CardActive()
    {
        if (TileUtils.IsEnemyInRange(player.currentTile, range, figure))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(player.currentTile, range, figure);
            for (int i = 0; i < enemies.Count; i++)
            {
                DamageToTarget(enemies[i], damage);
            }
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

}


public class Card_XAtt : Card_Attack
{
    public Card_XAtt()
    {
        InitCard(Rating.R0, Attribute.PRITHVI , Figure.X,1,5,1);
    }
    protected override void CardActive()
    {
        if (TileUtils.IsEnemyInRange(player.currentTile, range, figure))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(player.currentTile, range, figure);
            for (int i = 0; i < enemies.Count; i++)
            {
                DamageToTarget(enemies[i], damage);
            }
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

}

public class Card_SquAtt : Card_Attack
{
    public Card_SquAtt() 
    {
        InitCard(Rating.R0, Attribute.TEJAS, Figure.SQUARE,2,5,1);
    }
    protected override void CardActive()
    {
        if (TileUtils.IsEnemyInRange(player.currentTile, range, figure))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(player.currentTile, range, figure);
            for (int i = 0; i < enemies.Count; i++)
            {
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

}

public class Card_SquAttAll : Card_Attack
{
    public Card_SquAttAll()
    {
        InitCard(Rating.R1, Attribute.VAYU, Figure.SQUARE, 3, 5, 1);
    }
    protected override void CardActive()
    {
        if (TileUtils.IsEnemyInRange(player.currentTile, range, figure))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(player.currentTile, range, figure);
            for (int i = 0; i < enemies.Count; i++)
            {
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

}

public class Card_Mid3Att : Card_Attack
{
    int num = 3;
    public Card_Mid3Att()
    {
        InitCard(Rating.R1, Attribute.VAYU, 4, 5);
    }
    protected override void CardActive()
    {
        List<Tile> tiles = TileUtils.EmptySquareRange(player.currentTile, 2);
        for(int i=0; i<tiles.Count;i++)
        {
            if(tiles[i].OnTileObj != null && tiles[i].OnTileObj is Enemy)
            {
                DamageToTarget(tiles[i].OnTileObj as Enemy,damage);
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

}
public class Card_PierceAtt : Card_Attack
{
    int num = 3;
    public Card_PierceAtt()
    {
        InitCard(Rating.R1, Attribute.APAS, Figure.CROSS, 5, 5, 2);
    }
    protected override void CardActive()
    {

        if (TileUtils.IsEnemyInRange(player.currentTile, range, figure))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(player.currentTile, range, figure);
            for (int i = 0; i < enemies.Count; i++)
            {
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
}
public class Card_StrSquAllAtt : Card_Attack
{
    public Card_StrSquAllAtt()
    {
        InitCard(Rating.R1,Attribute.PRITHVI,Figure.SQUARE,6,10,1);
    }
    protected override void CardActive()
    {
        if (TileUtils.IsEnemyInRange(player.currentTile, range, figure))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(player.currentTile, range, figure);
            for (int i = 0; i < enemies.Count; i++)
            {
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

}






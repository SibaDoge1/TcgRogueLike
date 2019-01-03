using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;


public abstract class Card_Attack : Card
{


    /// <summary>
    /// 속성 계산 판정기
    /// </summary>
    private int AtrCompare(byte cardAttribute, byte enemyAttribute)
    {
        if (cardAttribute == enemyAttribute)
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
}

public class Card_CroAtt : Card_Attack
{
    /// <summary>
    /// 카드 클래스 정의시 해야할것 1
    /// </summary>
    protected override void SetIndex()
    {
        index = 1;
    }
    /// <summary>
    /// 카드 클래스 정의시 해야할것 4(선택)
    /// </summary>
    protected override void SetRangeData()
    {
        range = 1;
        figure = Figure.CROSS;
    }

    /// <summary>
    /// 카드 클래스 정의시 해야할것 2
    /// </summary>
    protected override void CardActive()
    {
        if (TileUtils.IsEnemyInRange(player.currentTile, range, figure))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(player.currentTile, range, figure);
            for (int i = 0; i < enemies.Count; i++)
            {
                DamageToTarget(enemies[i], val1);
            }
        }
    }

    /// <summary>
    /// 카드 클래스 정의시 해야할것 3(선택)
    /// </summary>
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
    protected override void SetIndex()
    {
        index = 2;
    }
    protected override void SetRangeData()
    {
        range = 1;
        figure = Figure.X;
    }
    protected override void CardActive()
    {
        if (TileUtils.IsEnemyInRange(player.currentTile, range, figure))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(player.currentTile, range, figure);
            for (int i = 0; i < enemies.Count; i++)
            {
                DamageToTarget(enemies[i], val1);
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
    protected override void SetIndex()
    {
        index = 3;
    }
    protected override void SetRangeData()
    {
        range = 1;
        figure = Figure.SQUARE;
    }
    protected override void CardActive()
    {
        if (TileUtils.IsEnemyInRange(player.currentTile, range, figure))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(player.currentTile, range, figure);
            for (int i = 0; i < enemies.Count; i++)
            {
                DamageToTarget(enemies[i], val1);
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
    protected override void SetIndex()
    {
        index = 4;
    }
    protected override void SetRangeData()
    {
        range = 1;
        figure = Figure.SQUARE;
    }
    protected override void CardActive()
    {
        if (TileUtils.IsEnemyInRange(player.currentTile, range, figure))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(player.currentTile, range, figure);
            for (int i = 0; i < enemies.Count; i++)
            {
                DamageToTarget(enemies[i], val1);
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
    protected override void SetIndex()
    {
        index = 5;
    }
    protected override void CardActive()
    {
        List<Tile> tiles = TileUtils.EmptySquareRange(player.currentTile, 2);
        for(int i=0; i<tiles.Count;i++)
        {
            if(tiles[i].OnTileObj != null && tiles[i].OnTileObj is Enemy)
            {
                DamageToTarget(tiles[i].OnTileObj as Enemy,val1);
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
    protected override void SetIndex()
    {
        index = 6;
    }
    protected override void SetRangeData()
    {
        range = 3;
        figure = Figure.CROSS;
    }
    protected override void CardActive()
    {

        if (TileUtils.IsEnemyInRange(player.currentTile, range, figure))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(player.currentTile, range, figure);
            for (int i = 0; i < enemies.Count; i++)
            {
                DamageToTarget(enemies[i], val1);
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
    protected override void SetIndex()
    {
        index = 7;
    }
    protected override void SetRangeData()
    {
        range = 1;
        figure = Figure.CROSS;
    }
    protected override void CardActive()
    {
        if (TileUtils.IsEnemyInRange(player.currentTile, range, figure))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(player.currentTile, range, figure);
            for (int i = 0; i < enemies.Count; i++)
            {
                DamageToTarget(enemies[i], val1);
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






using Arch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Card_Special : Card
{
    public Card_Special(CardData cardData)
    {
        index = cardData.index;
        name = cardData.name;
        cost = cardData.cost;
        cardType = CardType.S;
        val1 = cardData.val1;
        val2 = cardData.val2;
        val3 = cardData.val3;
        info = cardData._info;
        spritePath = cardData.spritePath;
    }
    public override void OnCardReturned()
    {
        base.OnCardReturned();
        player.GetDamage(1);
    }
    public override void CardReturnCallBack(Card data)
    {
        base.CardReturnCallBack(data);
        cost -= 1;
    }
}

public class Card_Reload : Card_Special{
    public Card_Reload(CardData cd) : base(cd)
    {

    }

    public override void CardReturnCallBack(Card data)
    {
    }
    protected override void CardActive()
    {
        PlayerControl.player.GetDamage(1);
		PlayerControl.instance.ReLoadDeck ();
	}
}
public class Card_Teleport : Card_Special
{

    public Card_Teleport(CardData cd) : base(cd)
    {
        isDirectionCard = true;
    }

    protected override void CardActive(Direction dir)
    {
        Vector2Int d = Vector2Int.zero;
        switch(dir)
        {
            case Direction.NORTH:
                d = Vector2Int.up;
                break;
            case Direction.EAST:
                d = Vector2Int.right;
                break;
            case Direction.SOUTH:
                d = Vector2Int.down;
                break;
            case Direction.WEST:
                d = Vector2Int.left;
                break;
        }

        player.MoveTo(player.pos + d * 3);
        
    }

    private List<Tile> targetTiles;
    public override void CardEffectPreview()
    {
        targetTiles = new List<Tile> {
            player.currentRoom.GetTile(player.pos + new Vector2Int(1, 0) * 3),
             player.currentRoom.GetTile(player.pos + new Vector2Int(-1, 0) * 3),
              player.currentRoom.GetTile(player.pos + new Vector2Int(0, 1) * 3),
               player.currentRoom.GetTile(player.pos + new Vector2Int(0, -1) * 3)
        };
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
public class Card_Heal : Card_Special
{
    public Card_Heal(CardData cd) : base(cd)
    {

    }
    protected override void CardActive()
    {
        player.GetHeal(1);
    }
}

//2마리 이상 적을 타격 시 체력 2 회복
public class Card_RedGrasp : Card_Special
{

    public Card_RedGrasp(CardData cd) : base(cd)
    {

    }

    public override void CardEffectPreview()
    {
        List<Tile> targetTiles = TileUtils.Range(player.currentTile, 1, Figure.SQUARE);
        for (int i = 0; i < targetTiles.Count; i++)
        {
            ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.CARD, targetTiles[i]));
            if (ranges[i] != null)
            {
                ranges[i].transform.parent = player.transform;
            }
        }
    }

    protected override void CardActive()
    {
        if(TileUtils.IsEnemyInRange(player.currentTile,1,Figure.SQUARE))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(player.currentTile, 1, Figure.SQUARE);
            if(enemies.Count>val2)
            {
                player.GetHeal(val3);
            }
            for (int i = 0; i < enemies.Count; i++)              
                {
                    DamageToTarget(enemies[i], val1);
                }       
        }
    }
}


//2마리 이상 적을 타격 시 AKS 5 회복
public class Card_BlueGrasp : Card_Special
{
    public Card_BlueGrasp(CardData cd) :base(cd)
    {

    }

    public override void CardEffectPreview()
    {
        List<Tile> targetTiles = TileUtils.Range(player.currentTile, 1, Figure.SQUARE);
        for (int i = 0; i < targetTiles.Count; i++)
        {
            ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.CARD, targetTiles[i]));
            if (ranges[i] != null)
            {
                ranges[i].transform.parent = player.transform;
            }
        }
    }

    protected override void CardActive()
    {
        if (TileUtils.IsEnemyInRange(player.currentTile, 1, Figure.SQUARE))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(player.currentTile, 1, Figure.SQUARE);
            if (enemies.Count > val2)
            {
                PlayerData.AkashaGage += val3;
            }
            for (int i = 0; i < enemies.Count; i++)
            {
                DamageToTarget(enemies[i], val1);
            }
        }
    }
}

//기존 패 버리고 같은 장수 다시 드로우
public class Card_TimeFrog : Card_Special
{
    public Card_TimeFrog(CardData cd) : base(cd)
    {

    }

    public override void CardEffectPreview()
    {
        List<Tile> targetTiles = TileUtils.Range(player.currentTile, 1, Figure.SQUARE);
        for (int i = 0; i < targetTiles.Count; i++)
        {
            ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.CARD, targetTiles[i]));
            if (ranges[i] != null)
            {
                ranges[i].transform.parent = player.transform;
            }
        }
    }

    protected override void CardActive()
    {
        if (TileUtils.IsEnemyInRange(player.currentTile, 1, Figure.SQUARE))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(player.currentTile, 1, Figure.SQUARE);
            for (int i = 0; i < enemies.Count; i++)
            {
                DamageToTarget(enemies[i], val1);
            }
        }
        PlayerData.AkashaGage += val3;
        int hand = PlayerControl.instance.hand.CurrentHandCount;
        for(int i=0; i<hand;i++)
        {
            PlayerControl.instance.hand.ReturnCard();
        }
        for (int i = 0; i<hand; i++)
        {
            PlayerControl.instance.NaturalDraw();
        }
    }
}

//1턴 피해면역
public class Card_CrimsonCrow : Card_Special
{
    public Card_CrimsonCrow(CardData cd) : base(cd)
    {

    }

    public override void CardEffectPreview()
    {
        List<Tile> targetTiles = TileUtils.Range(player.currentTile, 2, Figure.CROSS);
        for (int i = 0; i < targetTiles.Count; i++)
        {
            ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.CARD, targetTiles[i]));
            if (ranges[i] != null)
            {
                ranges[i].transform.parent = player.transform;
            }
        }
    }

    protected override void CardActive()
    {
        if (TileUtils.IsEnemyInRange(player.currentTile, 2, Figure.CROSS))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(player.currentTile, 2, Figure.CROSS);
            for (int i = 0; i < enemies.Count; i++)
            {
                DamageToTarget(enemies[i], val1);
            }
        }
        PlayerData.AkashaGage += val3;
        PlayerControl.status.UpdateBuff(BUFF.IMMUNE,val2);
    }
}

//덱에 남은 카드가 5장 이하일 시 패널티 삭제
public class Card_Bishop : Card_Special
{
    public Card_Bishop(CardData cd) : base(cd)
    {

    }

    public override void CardEffectPreview()
    {
        List<Tile> targetTiles = TileUtils.Range(player.currentTile, 2, Figure.Diagonal);
        for (int i = 0; i < targetTiles.Count; i++)
        {
            ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.CARD, targetTiles[i]));
            if (ranges[i] != null)
            {
                ranges[i].transform.parent = player.transform;
            }
        }
    }

    protected override void CardActive()
    {
        if (TileUtils.IsEnemyInRange(player.currentTile, 2, Figure.Diagonal))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(player.currentTile, 2, Figure.Diagonal);
            for (int i = 0; i < enemies.Count; i++)
            {
                DamageToTarget(enemies[i], val1);
            }
        }

        PlayerControl.status.UpdateBuff(BUFF.AKASHA, val3);
        if (PlayerControl.instance.deck.DeckCount<=val2)
        {
            PlayerControl.status.EraseDeBuff();
        }
    }
}

public class Card_WolfBite : Card_Special
{
    public Card_WolfBite(CardData cd) : base(cd)
    {

    }
    private List<Tile> GetRange()
    {
        List<Tile> targetTiles = new List<Tile>();
        int x = (int)player.pos.x; int y = (int)player.pos.y;

        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 1, y + 2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x, y + 2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 1, y + 2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 2, y + 2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 2, y + 2)));

        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 1, y - 2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x, y - 2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 1, y - 2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 2, y - 2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 2, y - 2)));

        for (int i = targetTiles.Count - 1; i >= 0; i--)
        {
            if (targetTiles[i] == null)
            {
                targetTiles.RemoveAt(i);
            }
        }
        return targetTiles;
    }
    public override void CardEffectPreview()
    {
        List<Tile> targetTiles = GetRange();

        for (int i = targetTiles.Count - 1; i >= 0; i--)
        {
            if (targetTiles[i] == null)
            {
                targetTiles.RemoveAt(i);
            }
        }

        for (int i = 0; i < targetTiles.Count; i++)
        {
            ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.CARD, targetTiles[i]));
            if (ranges[i] != null)
            {
                ranges[i].transform.parent = player.transform;
            }
        }
    }

    protected override void CardActive()
    {
        if (TileUtils.IsEnemyInRange(GetRange()))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(GetRange());
            for (int i = 0; i < enemies.Count; i++)
            {
                DamageToTarget(enemies[i], val1);
            }
        }
        player.GetDamage(val3);
    }
}

public class Card_BearClaw : Card_Special
{
    public Card_BearClaw(CardData cd) : base(cd)
    {

    }

    private List<Tile> GetRange()
    {
        List<Tile> targetTiles = new List<Tile>();
        int x = (int)player.pos.x; int y = (int)player.pos.y;

        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 2, y + 1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 2, y)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 2, y - 1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 2, y + 2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 2, y - 2)));

        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 2, y + 1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 2, y)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 2, y - 1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 2, y + 2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 2, y - 2)));


        for (int i = targetTiles.Count - 1; i >= 0; i--)
        {
            if (targetTiles[i] == null)
            {
                targetTiles.RemoveAt(i);
            }
        }
        return targetTiles;
    }
    public override void CardEffectPreview()
    {
        List<Tile> targetTiles = GetRange();

        for (int i = targetTiles.Count - 1; i >= 0; i--)
        {
            if (targetTiles[i] == null)
            {
                targetTiles.RemoveAt(i);
            }
        }

        for (int i = 0; i < targetTiles.Count; i++)
        {
            ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.CARD, targetTiles[i]));
            if (ranges[i] != null)
            {
                ranges[i].transform.parent = player.transform;
            }
        }
    }

    protected override void CardActive()
    {
        if (TileUtils.IsEnemyInRange(GetRange()))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(GetRange());
            for (int i = 0; i < enemies.Count; i++)
            {
                DamageToTarget(enemies[i], val1);
            }
        }
        player.GetDamage(val3);
    }
}

public class Card_Justice : Card_Special
{
    public Card_Justice(CardData cd) : base(cd)
    {

    }

    public override void CardEffectPreview()
    {
        List<Tile> targetTiles = TileUtils.Range(player.currentTile, 2, Figure.EMPTYSQUARE);
        for (int i = 0; i < targetTiles.Count; i++)
        {
            ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.CARD, targetTiles[i]));
            if (ranges[i] != null)
            {
                ranges[i].transform.parent = player.transform;
            }
        }
    }

    protected override void CardActive()
    {
        if (TileUtils.IsEnemyInRange(player.currentTile, 2, Figure.EMPTYSQUARE))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(player.currentTile, 2, Figure.EMPTYSQUARE);
            for (int i = 0; i < enemies.Count; i++)
            {
                DamageToTarget(enemies[i], val1);
            }
        }
        player.GetHeal(val3);
    }
}

public class Card_WindCat : Card_Special
{
    public Card_WindCat(CardData cd) : base(cd)
    {

    }

    public override void CardEffectPreview()
    {
        List<Tile> targetTiles = GetRange();
        for (int i = 0; i < targetTiles.Count; i++)
        {
            ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.CARD, targetTiles[i]));
            if (ranges[i] != null)
            {
                ranges[i].transform.parent = player.transform;
            }
        }
    }

    protected override void CardActive()
    {
        List<Tile> targetTiles = GetRange();

        if (TileUtils.IsEnemyInRange(targetTiles))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(targetTiles);
            for (int i = 0; i < enemies.Count; i++)
            {
                DamageToTarget(enemies[i], val1);
            }
        }
        MoveToRandom();
        PlayerControl.status.UpdateBuff(BUFF.MOVE,val3);
    }

    private void MoveToRandom()
    {
        List<Tile> UnExplore = new List<Tile>(player.currentRoom.GetTileToList());

        while(UnExplore.Count>1)
        {
            int rand = UnityEngine.Random.Range(0, UnExplore.Count);

            if (UnExplore[rand].IsStandAble(player))
            {
                player.MoveTo(UnExplore[rand].pos);
            }
            else
            {
                UnExplore.RemoveAt(rand);
            }
        }
        Debug.Log("랜덤이동 실패");
    }

    private List<Tile> GetRange()
    {
        List<Tile> targetTiles = new List<Tile>();
        int x = (int)player.pos.x; int y = (int)player.pos.y;

        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x +1, y +2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x+1, y+1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x, y+1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x-1, y+1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x-2, y+1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x-1, y)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x+1, y)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x-1, y-1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x, y-1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x+1, y-1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x+2, y-1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x-1, y-2)));

        for (int i = targetTiles.Count - 1; i >= 0; i--)
        {
            if (targetTiles[i] == null)
            {
                targetTiles.RemoveAt(i);
            }
        }
        return targetTiles;
    }
}

public class Card_HalfMask : Card_Special
{
    public Card_HalfMask(CardData cd) : base(cd)
    {

    }

    public override void CardEffectPreview()
    {
        List<Tile> targetTiles = GetRange();
        for (int i = 0; i < targetTiles.Count; i++)
        {
            ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.CARD, targetTiles[i]));
            if (ranges[i] != null)
            {
                ranges[i].transform.parent = player.transform;
            }
        }
    }

    protected override void CardActive()
    {
        List<Tile> targetTiles = GetRange();

        if (TileUtils.IsEnemyInRange(targetTiles))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(targetTiles);
            for (int i = 0; i < enemies.Count; i++)
            {
                DamageToTarget(enemies[i], val1);
            }
        }else
        {
            player.GetHeal(val3);
        }

    }

    private List<Tile> GetRange()
    {
        List<Tile> targetTiles = new List<Tile>();
        int x = (int)player.pos.x; int y = (int)player.pos.y;

        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x -2, y- 2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x , y - 2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 2, y - 2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 1, y - 1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 1, y - 1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 2, y)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 2, y)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 1, y + 1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 1, y + 1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 2, y + 2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x, y + 2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 2, y + 2)));

        for (int i = targetTiles.Count - 1; i >= 0; i--)
        {
            if (targetTiles[i] == null)
            {
                targetTiles.RemoveAt(i);
            }
        }
        return targetTiles;
    }
}
public class Card_BlackThunder : Card_Special
{

    public Card_BlackThunder(CardData cd) : base(cd)
    {

    }

    public override void CardEffectPreview()
    {
        List<Tile> targetTiles = GetRange();
        for (int i = 0; i < targetTiles.Count; i++)
        {
            ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.CARD, targetTiles[i]));
            if (ranges[i] != null)
            {
                ranges[i].transform.parent = player.transform;
            }
        }
    }

    protected override void CardActive()
    {
        List<Tile> targetTiles = GetRange();

        if (TileUtils.IsEnemyInRange(targetTiles))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(targetTiles);
            for (int i = 0; i < enemies.Count; i++)
            {
                DamageToTarget(enemies[i], val1);
            }
        }

    }

    private List<Tile> GetRange()
    {
        List<Tile> targetTiles = new List<Tile>();
        int x = (int)player.pos.x; int y = (int)player.pos.y;

        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 2, y - 2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 2, y)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 2, y + 2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 1, y - 1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x , y - 1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x +1, y-1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 2, y)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 1, y)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x +1, y)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x +2, y)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 1, y +1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x , y + 1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 1, y + 1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 2, y + 2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x , y + 2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 2, y + 2)));

        for (int i = targetTiles.Count - 1; i >= 0; i--)
        {
            if (targetTiles[i] == null)
            {
                targetTiles.RemoveAt(i);
            }
        }
        return targetTiles;
    }
    public override void CardReturnCallBack(Card data)
    {
        base.CardReturnCallBack(data);
        UpgradeThis();
    }
}
public class Card_PoisonSnail : Card_Special
{
    public Card_PoisonSnail(CardData cd) : base(cd)
    {

    }

    public override void CardEffectPreview()
    {
        List<Tile> targetTiles = TileUtils.Range(player.currentTile, 2, Figure.SQUARE);
        for (int i = 0; i < targetTiles.Count; i++)
        {
            ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.CARD, targetTiles[i]));
            if (ranges[i] != null)
            {
                ranges[i].transform.parent = player.transform;
            }
        }
    }

    protected override void CardActive()
    {
        if (TileUtils.IsEnemyInRange(player.currentTile, 2, Figure.SQUARE))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(player.currentTile, 2, Figure.SQUARE);
            for (int i = 0; i < enemies.Count; i++)
            {
                DamageToTarget(enemies[i], val1);
            }
        }
        
        player.SetHp(val2);
        PlayerControl.status.UpdateBuff(BUFF.CARD,val3);
        PlayerControl.status.UpdateBuff(BUFF.AKASHA,val3);
    }
}
public class Card_Shield : Card_Special
{
    public Card_Shield(CardData cd) : base(cd)
    {
        isDirectionCard = true;
    }

    public override void CardEffectPreview()
    {
        if(PlayerControl.instance.SelectedDirCard == this)
        {
            List<Tile> targetTiles = TileUtils.CrossRange(player.currentTile,1);
            for (int i = 0; i < targetTiles.Count; i++)
            {
                ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.CARD, targetTiles[i]));
                if (ranges[i] != null)
                {
                    ranges[i].transform.parent = player.transform;
                }
            }
        }
        else
        {
            List<Tile> targetTiles = GetRange();
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

    protected override void CardActive()
    {
        List<Tile> targetTiles = GetRange();

        if (TileUtils.IsEnemyInRange(targetTiles))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(targetTiles);
            for (int i = 0; i < enemies.Count; i++)
            {
                DamageToTarget(enemies[i], val1);
            }
        }

    }
    protected override void CardActive(Direction dir)//CardType : T
    {
        Vector2Int d = Vector2Int.zero;
        switch (dir)
        {
            case Direction.NORTH:
                d = Vector2Int.up;
                break;
            case Direction.EAST:
                d = Vector2Int.right;
                break;
            case Direction.SOUTH:
                d = Vector2Int.down;
                break;
            case Direction.WEST:
                d = Vector2Int.left;
                break;
        }

        player.MoveTo(player.pos + d * val3);
    }

    private List<Tile> GetRange()
    {
        List<Tile> targetTiles = new List<Tile>();
        int x = (int)player.pos.x; int y = (int)player.pos.y;

        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x +1, y)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x +2, y)));

        for (int i = targetTiles.Count - 1; i >= 0; i--)
        {
            if (targetTiles[i] == null)
            {
                targetTiles.RemoveAt(i);
            }
        }
        return targetTiles;
    }
}

public class Card_Rush : Card_Special
{
    public Card_Rush(CardData cd) : base(cd)
    {
        isDirectionCard = true;
    }

    public override void CardEffectPreview()
    {
        if (PlayerControl.instance.SelectedDirCard == this)
        {
            List<Tile> targetTiles = TileUtils.CrossRange(player.currentTile, 1);
            for (int i = 0; i < targetTiles.Count; i++)
            {
                ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.CARD, targetTiles[i]));
                if (ranges[i] != null)
                {
                    ranges[i].transform.parent = player.transform;
                }
            }
        }
        else
        {
            List<Tile> targetTiles = GetRange();
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

    protected override void CardActive()
    {
        List<Tile> targetTiles = GetRange();

        if (TileUtils.IsEnemyInRange(targetTiles))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(targetTiles);
            for (int i = 0; i < enemies.Count; i++)
            {
                DamageToTarget(enemies[i], val1);
            }
        }

    }
    protected override void CardActive(Direction dir)//CardType : T
    {
        Vector2Int d = Vector2Int.zero;
        switch (dir)
        {
            case Direction.NORTH:
                d = Vector2Int.up;
                break;
            case Direction.EAST:
                d = Vector2Int.right;
                break;
            case Direction.SOUTH:
                d = Vector2Int.down;
                break;
            case Direction.WEST:
                d = Vector2Int.left;
                break;
        }

        player.MoveTo(player.pos + d * val3);
    }

    private List<Tile> GetRange()
    {
        List<Tile> targetTiles = new List<Tile>();
        int x = (int)player.pos.x; int y = (int)player.pos.y;

        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 1, y)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 2, y)));

        for (int i = targetTiles.Count - 1; i >= 0; i--)
        {
            if (targetTiles[i] == null)
            {
                targetTiles.RemoveAt(i);
            }
        }
        return targetTiles;
    }
}
public class Card_Mist : Card_Special
{


}
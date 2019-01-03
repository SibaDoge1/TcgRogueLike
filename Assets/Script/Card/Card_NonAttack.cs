using Arch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card_NonAttack : Card {

	public Card_NonAttack(){}
}

public class Card_Reload : Card_NonAttack{
    protected override void SetIndex()
    {
        index = 0;
    }
 
    protected override void CardActive()
    {
		PlayerControl.instance.ReLoadDeck ();
	}
}
public class Card_Teleport : Card_NonAttack
{
    protected override void SetIndex()
    {
        index = 9;
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

        player.MoveTo(player.pos + d*3);
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
public class Card_Heal : Card_NonAttack
{
    protected override void SetIndex()
    {
        index = 10;
    }
    protected override void CardActive()
    {
        player.GetHeal(1);
    }
}
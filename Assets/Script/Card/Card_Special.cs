using Arch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        info = DataInfoToCardInfo(cardData._info);
        spritePath = cardData.spritePath;
    }

    private string DataInfoToCardInfo(string data)
    {
        string[] s = string.Copy(data).Split('<', '>');
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

public class Card_Reload : Card_Special{
    public Card_Reload(CardData cd) : base(cd)
    {

    } 
 
    protected override void CardActive()
    {
        PlayerControl.Player.GetDamage(1);
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
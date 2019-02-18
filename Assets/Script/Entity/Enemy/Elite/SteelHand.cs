using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteelHand : Enemy {

    bool isMoved = false;
    protected override void Think()
    {
        if (isMoved)
        {
            currentActionList = attackList;
            isMoved = false;
        }
        else
        {
            currentActionList = moveList;
            isMoved = true;
        }
    }
    List<Action> moveList;
    List<Action> attackList;
    protected override void SetActionLists()
    {
        DelayList = null;
        moveList = new List<Action>() { new Action(MoveToward) , new Action(MoveToward) };
        attackList = new List<Action>() { new Action(RangeOnAction),new Action(AttackThenRangeOffAction) };
    }

    IEnumerator MoveToward()
    {
        MoveTo(PathFinding.GenerateCrossPath(this, PlayerControl.player.currentTile)[0].pos);
        yield return null;
    }

    IEnumerator RangeOnAction()
    {
        List<Arch.Tile> tiles = TileUtils.SquareRange(currentTile, 1);
        for (int i = 0; i < tiles.Count; i++)
        {
            rangeList.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.ENEMY, tiles[i]));
        }
        enemyUI.ActionImageOn();
        yield return null;
    }

    IEnumerator AttackThenRangeOffAction()
    {
        List<Arch.Tile> tiles = TileUtils.SquareRange(currentTile, 1);

        if (TileUtils.AI_Find(tiles))
        {
            PlayerControl.player.GetDamage(atk);
        }

        ClearRangeList();
        enemyUI.ActionImageOff();

        yield return StartCoroutine(AnimationRoutine(0));
    }

}

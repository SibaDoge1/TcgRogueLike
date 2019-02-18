using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class Spider : Enemy
    {


    #region AI
    protected override void Think()
    {
        if (TileUtils.AI_CircleFind(currentTile, 1))
        {
            currentActionList = attackList;
        }
        else
        {
            currentActionList = moveList;
        }
    }
    List<Action> moveList;
    List<Action> attackList;
    protected override void SetActionLists()
    {
        DelayList = new List<Action>() { new Action(DelayAction) };
        moveList = new List<Action>() { new Action(MoveToWard) };
        attackList = new List<Action>() { new Action(Attack) };
    }

    IEnumerator MoveToWard()
    {
        MoveTo(PathFinding.GenerateCrossPath(this, PlayerControl.player.currentTile)[0].pos);
        yield return null;
    }


    IEnumerator DelayAction()
    {
        yield return null;
    }


    IEnumerator Attack()
    {
        PlayerControl.player.GetDamage(atk);
        yield return null;
    }
    #endregion



}



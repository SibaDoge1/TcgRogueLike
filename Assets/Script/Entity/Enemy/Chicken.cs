﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : Enemy {

    protected override void Think()
    {
        if (TileUtils.AI_DiagonalFind(currentTile, 1))
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
        DelayList = new List<Action>() { new Action(DelayAction) , new Action(DelayAction1)};
        moveList = new List<Action>() { new Action(MoveToward) };
        attackList = new List<Action>() { new Action(Attack) };
    }

    IEnumerator MoveToward()
    {
        enemyUI.ActionImageOff();
        MoveTo(PathFinding.GenerateDiagonalPath(this, PlayerControl.player.currentTile)[0].pos);
        yield return null;
    }


    IEnumerator DelayAction()
    {
        yield return null;
    }

    IEnumerator DelayAction1()
    {
        enemyUI.ActionImageOn();
        yield return null;
    }


    IEnumerator Attack()
    {
        enemyUI.ActionImageOff();

        List<Arch.Tile> tiles = TileUtils.DiagonalRange(currentTile, 1);
        for (int i = 0; i < tiles.Count; i++)
        {
            ObjectPoolManager.instance.PoolEffect(EnemyEffect.HITBLUE, tiles[i]);
        }
        PlayerControl.player.GetDamage(atk);
        yield return StartCoroutine(AnimationRoutine(0));
    }

}

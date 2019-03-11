using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManekingHand : Enemy {



    #region AI
    protected override void Think()
    {
        if(TileUtils.AI_CircleFind(currentTile,1))
        {
            currentActionList = attackList;
        }else
        {
            currentActionList = moveList;
        }
    }
    List<Action> moveList;
    List<Action> attackList;
    protected override void SetActionLists()
    {
        DelayList = new List<Action>() {new Action(DelayAction) };
        moveList = new List<Action>() { new Action(MoveToward) };
        attackList = new List<Action>() { new Action(Attack) };
    }

         IEnumerator MoveToward()
        {
        enemyUI.ActionImageOff();
        MoveTo(PathFinding.GenerateCrossPath(this, PlayerControl.player.currentTile)[0].pos);
            yield return null;
        }
    

          IEnumerator DelayAction()
        {
            enemyUI.ActionImageOn();
            yield return null;
        }
    
 
          IEnumerator Attack()
        {
             enemyUI.ActionImageOff();
        List<Arch.Tile> tiles = TileUtils.CircleRange(currentTile, 1);
        for(int i=0; i<tiles.Count;i++)
        {
            ObjectPoolManager.instance.PoolEffect(EnemyEffect.HITBLUE, tiles[i]);
        }
            PlayerControl.player.GetDamage(atk);
            yield return StartCoroutine(AnimationRoutine(0));
        }
    
    #endregion

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bozz : Enemy {

 
    #region AI

    protected override void Think()
    {
        currentActionList = attackList;
    }

    List<Action> attackList;
    protected override void SetActionLists()
    {
        DelayList = new List<Action>()
        { new Action(DelayAction) ,new Action(DelayAction) };
        attackList = new List<Action>()
        { new Action(RangeOnAction),new  Action(AttackThenRangeOffAction)};
    }


         IEnumerator RangeOnAction()
        {
            List<Arch.Tile> tiles = TileUtils.EmptySquareRange(currentTile, 2);
            for (int i=0; i<tiles.Count; i++)
            {
                rangeList.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.ENEMY, tiles[i]));
            }
            enemyUI.ActionImageOn();
            yield return null;
        }
    

          IEnumerator AttackThenRangeOffAction()
        {
            List<Arch.Tile> tiles = TileUtils.EmptySquareRange(currentTile, 2);
            

            if (TileUtils.AI_Find(tiles))
            {
                PlayerControl.player.GetDamage(atk);
            }

            ClearRangeList();
            enemyUI.ActionImageOff();

             yield return StartCoroutine(AnimationRoutine(0));
        }
    

          IEnumerator DelayAction()
        {
            yield return null;
        }
    
    #endregion

}

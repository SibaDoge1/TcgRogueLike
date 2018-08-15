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
            PlayAnimation("Ready");
            List<Arch.Tile> tiles = TileUtils.SquareRange(currentTile, 1);
            for (int i=0; i<tiles.Count; i++)
            {
                rangeList.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.ENEMY, tiles[i]));
            }
            yield return null;
        }
    

          IEnumerator AttackThenRangeOffAction()
        {
            PlayAnimation("Attack"); 
            if (TileUtils.AI_SquareFind(currentTile,1))
            {
                PlayerControl.Player.GetDamage(atk);
                EffectDelegate.instance.MadeEffect(CardEffectType.Hit, PlayerControl.Player);
            }
            ClearRangeList();
            yield return null;
        }
    

          IEnumerator DelayAction()
        {
            PlayAnimation("Idle");
            yield return null;
        }
    
    #endregion

}

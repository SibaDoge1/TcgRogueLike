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

            enemyUI.ActionImageOn();
            yield return null;
        }
    

          IEnumerator AttackThenRangeOffAction()
        {
            List<Arch.Tile> tiles = TileUtils.EmptySquareRange(currentTile, 2);
            SoundDelegate.instance.PlayEffectSound(EffectSound.SFX2, transform.position);
            enemyUI.ActionImageOff();


        for (int i=0; i<tiles.Count;i++)
            {
                ArchLoader.instance.MadeEffect(EnemyEffect.POW, tiles[i]);
            }

            if (TileUtils.AI_Find(tiles))
            {
                PlayerControl.player.GetDamage(atk);
            }


             yield return StartCoroutine(AnimationRoutine(0));
        }
    

          IEnumerator DelayAction()
        {
            yield return null;
        }
    
    #endregion

}

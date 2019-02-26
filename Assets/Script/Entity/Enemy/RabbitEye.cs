using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitEye : Enemy {


    #region AI
    private sbyte attackCooltime = 0;
    protected override void Think()
    {
        if (TileUtils.AI_SquareFind(currentTile, 2) && attackCooltime<=0)
        {
            currentActionList = attackList;
        }
        else
        {
            currentActionList = moveList;
        }
    }
    List<Action> attackList;
    List<Action> moveList;
    protected override void SetActionLists()
    {
        DelayList = null;
        attackList = new List<Action>()
        { new Action(RangeOnAction),new Action(AttackThenRangeOffAction)};
        moveList = new List<Action>()
        { new Action(SimpleMove)};
    }

         Arch.Tile aimedTile;
         IEnumerator RangeOnAction()
        {
            enemyUI.ActionImageOn();
            aimedTile = PlayerControl.player.currentTile;
            rangeList.Add(ArchLoader.instance.MadeEffect(RangeEffectType.ENEMY, PlayerControl.player.currentTile));
            yield return null;
        }
  


          IEnumerator AttackThenRangeOffAction()
        {
            ArchLoader.instance.MadeEffect(EnemyEffect.FORCE, aimedTile);
            if (aimedTile.OnTileObj != null && aimedTile.OnTileObj is Player)
            {
            PlayerControl.player.GetDamage(atk);
            }
            SoundDelegate.instance.PlayEffectSound(EffectSound.SFX1, transform.position);

                enemyUI.ActionImageOff();
                ClearRangeList();
                attackCooltime = 2;
            yield return StartCoroutine(AnimationRoutine(0));
        }

    byte moveCount = 0;
    IEnumerator SimpleMove()
    {
        switch(moveCount)
        {
            case 0:
            case 1:
                MoveTo(pos + new Vector2Int(1, 0));
                break;
            case 2:
            case 3:
                MoveTo(pos + new Vector2Int(0, -1));
                break;
            case 4:
            case 5:
                MoveTo(pos + new Vector2Int(-1, 0));
                break;
            case 6:
            case 7:
                MoveTo(pos + new Vector2Int(0, 1));
                break;
        }
        moveCount++;
        if(moveCount == 8)
        {
            moveCount = 0;
        }

        attackCooltime--;
        yield return null;
    }
    #endregion
}

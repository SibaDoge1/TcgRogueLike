using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManekingHand : Enemy {

    protected override void Start()
    {
        base.Start();
        fullHp =SettingHp; currentHp =SettingHp;
    }

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
            MoveTo(PathFinding.GeneratePath(this, PlayerControl.Player)[0].pos);
            PlayAnimation("Idle");
            yield return null;
        }
    

          IEnumerator DelayAction()
        {
            PlayAnimation("Idle");
            yield return null;
        }
    
 
          IEnumerator Attack()
        {
            PlayerControl.Player.GetDamage(atk);
            PlayAnimation("Attack");
            EffectDelegate.instance.MadeEffect(CardEffectType.Hit, PlayerControl.Player.currentTile);
            yield return null;
        }
    
    #endregion

}

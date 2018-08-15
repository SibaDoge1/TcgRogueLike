using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitEye : Enemy {


    #region AI
    protected override void Think()
    {
        if (TileUtils.AI_CircleFind(currentTile, 1))
        {
            currentActionList = moveList;
        }
        else
        {
            currentActionList = attackList;
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
        { new Action(SimpleRunAway)};
    }

    Arch.Tile aimedTile;
         IEnumerator RangeOnAction()
        {
            PlayAnimation("Ready");
            aimedTile = PlayerControl.Player.currentTile;
            rangeList.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.ENEMY, PlayerControl.Player.currentTile));
            yield return null;
        }
  


          IEnumerator AttackThenRangeOffAction()
        {
            PlayAnimation("Attack"); 
            if (aimedTile.OnTileObj != null && aimedTile.OnTileObj is Player)
            {
            PlayerControl.Player.GetDamage(atk);
            EffectDelegate.instance.MadeEffect(CardEffectType.Hit, PlayerControl.Player);

        }
        ClearRangeList();
            yield return null;
        }



    Vector2Int dir;
          IEnumerator SimpleRunAway()
        {
            if (dir == Vector2Int.zero)
            {
                List<Arch.Tile> nearTiles = TileUtils.CircleRange(currentTile, 1);
                for (int i = 0; i < nearTiles.Count; i++)
                {
                    if (nearTiles[i].OnTileObj == null)
                    {
                        dir = TileUtils.GetDir(currentTile, nearTiles[i]);
                        break;
                    }
                }
            }
            MoveTo(currentTile.pos + dir);
            PlayAnimation("Idle");
            yield return null;
        }
    
    #endregion
}

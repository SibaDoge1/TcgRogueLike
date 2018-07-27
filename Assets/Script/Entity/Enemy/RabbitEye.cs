using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitEye : Enemy {

    protected override void Start()
    {
        base.Start();
        _fullHp = SettingHp; _currentHp = SettingHp;
    }

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
            PlayAnimation("Attack"); EffectDelegate.instance.MadeEffect(CardEffectType.Hit, rangeList[0].transform);
            if (aimedTile.OnTileObj != null && aimedTile.OnTileObj is Player)
            {
            aimedTile.OnTileObj.GetDamage(atk);
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
    protected override void OnDieCallback()
    {
        if (UnityEngine.Random.Range(0, 8) == 0)
        {
            PlayerControl.instance.AddCard(new CardData_Stone(5,PlayerControl.Player, (Attribute)UnityEngine.Random.Range(1, 4)));
        }
        else if (UnityEngine.Random.Range(0, 12) == 0)
        {
            PlayerControl.instance.AddCard(new CardData_Bandage(2, PlayerControl.Player));
        }

        base.OnDieCallback();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bozz : Enemy {

    protected override void Start()
    {
        base.Start();
        _fullHp = SettingHp; _currentHp = SettingHp;
        
    }
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
            PlayAnimation("Attack"); EffectDelegate.instance.MadeEffect(CardEffectType.Hit, rangeList[0].transform);
            if (TileUtils.AI_SquareFind(currentTile,1))
            {
                PlayerControl.Player.GetDamage(atk);
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
    protected override void OnDieCallback()
    {
        //TODO : DROP CARD TEMP
        if (UnityEngine.Random.Range(0, 10) == 0)
        {
            PlayerControl.instance.AddCard(new Card_Stone(5, PlayerControl.Player,(Attribute)UnityEngine.Random.Range(1,4)));
        }
        else if (UnityEngine.Random.Range(0, 5) == 0)
        {
            PlayerControl.instance.AddCard(new CardData_Bandage(2, PlayerControl.Player));
        }
        base.OnDieCallback();
    }
}

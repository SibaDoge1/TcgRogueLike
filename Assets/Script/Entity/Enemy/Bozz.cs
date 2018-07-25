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

    protected override void Think()
    {
        currentActionList = attackList;
    }

    List<Action> attackList;
    protected override void SetActionLists()
    {
        DelayList = new List<Action>()
        { new SimpleDelayAction(this) ,new SimpleDelayAction(this) };
        attackList = new List<Action>()
        { new RangeOnAction(this),new  AttackThenRangeOffAction(this)};
    }

    #region 원거리 공격 AI
    class RangeOnAction : Action
    {
        Bozz b;
        public RangeOnAction(Bozz e) : base(e) { b = e; }
        public override void Do()
        {
            b.rangeList.Add(EffectDelegate.instance.MadeRange(RangeType.ENEMY, PlayerControl.instance.PlayerObject.currentTile));
        }
    }
    class AttackThenRangeOffAction : Action
    {
        Bozz b;
        public AttackThenRangeOffAction(Bozz e) : base(e) { b = e; }
        public override void Do()
        {
            b.PlayAttackMotion(); EffectDelegate.instance.MadeEffect(CardEffectType.Hit, b.rangeList[0]);
            if (b.rangeList[0].OnTileObj != null && b.rangeList[0].OnTileObj is Player)
            {
                b.rangeList[0].OnTileObj.GetDamage(b.atk);
            }
            b.ClearRangeList();
        }
    }
    #endregion
    protected override void OnDieCallback()
    {
        //TODO : DROP CARD TEMP
        if (UnityEngine.Random.Range(0, 10) == 0)
        {
            PlayerControl.instance.AddCard(new CardData_Stone(5, PlayerControl.instance.PlayerObject,(Attribute)UnityEngine.Random.Range(1,4)));
        }
        else if (UnityEngine.Random.Range(0, 5) == 0)
        {
            PlayerControl.instance.AddCard(new CardData_Bandage(2, PlayerControl.instance.PlayerObject));
        }
        base.OnDieCallback();
    }
}

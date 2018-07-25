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
        { new RangeOnAction(this),new AttackThenRangeOffAction(this)};
        moveList = new List<Action>()
        { new SimpleRunAway(this)};
    }
    #region 원거리 공격 AI
    class RangeOnAction : Action
    {
        RabbitEye b;
        public RangeOnAction(RabbitEye e) : base(e) { b = e; }
        public override void Do()
        {
            b.rangeList.Add(EffectDelegate.instance.MadeRange(RangeType.ENEMY, PlayerControl.instance.PlayerObject.currentTile));
        }
    }
    class AttackThenRangeOffAction : Action
    {
        RabbitEye b;
        public AttackThenRangeOffAction(RabbitEye e) : base(e) { b = e; }
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
        if (UnityEngine.Random.Range(0, 8) == 0)
        {
            PlayerControl.instance.AddCard(new CardData_Stone(5,PlayerControl.instance.PlayerObject, (Attribute)UnityEngine.Random.Range(1, 4)));
        }
        else if (UnityEngine.Random.Range(0, 12) == 0)
        {
            PlayerControl.instance.AddCard(new CardData_Bandage(2, PlayerControl.instance.PlayerObject));
        }

        base.OnDieCallback();
    }
}

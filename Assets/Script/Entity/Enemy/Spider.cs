using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class Spider : Enemy
    {
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
            currentActionList = attackList;
        }
        else
        {
            currentActionList = moveList;
        }
    }
    List<Action> moveList;
    List<Action> attackList;
    protected override void SetActionLists()
    {
        DelayList = new List<Action>() { new Action(DelayAction) };
        moveList = new List<Action>() { new Action(MoveToWard) };
        attackList = new List<Action>() { new Action(Attack) };
    }

    IEnumerator MoveToWard()
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

    protected override void OnDieCallback()
    {
        //TODO : DROP CARD TEMP
        if (UnityEngine.Random.Range(0, 8) == 0)
        {
            PlayerControl.instance.AddCard(new CardData_Stone(5, PlayerControl.Player, (Attribute)UnityEngine.Random.Range(1, 4)));
        }
        else if (UnityEngine.Random.Range(0, 12) == 0)
        {
            PlayerControl.instance.AddCard(new CardData_Bandage(2, PlayerControl.Player));
        }

        base.OnDieCallback();
    }

}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowGoblin : Enemy {

    protected override void Start()
    {
        base.Start();
        fullHp = HP; currentHp = HP;
        
        range = 1;
    }
    protected int range;

    public override bool DoAct()
    {
        if (!base.DoAct())
        {
            OnEndTurn();
            return false;
        }

        if (Room.CalcRange(currentTile.pos, PlayerControl.instance.PlayerObject.currentTile.pos) <= range)
        {
            PlayerControl.instance.PlayerObject.GetDamage(atk);
			PlayAttackMotion ();
            EffectDelegate.instance.MadeEffect(CardEffectType.Hit, PlayerControl.instance.PlayerObject.currentTile);
            OnEndTurn();
        }
        else
        {
            if (!MoveTo(PathFinding.instance.GeneratePath(this, PlayerControl.instance.PlayerObject.currentTile)[0].pos))
                OnEndTurn();
        }
        return true;
    }
    public override void SetRoom(Room room, Vector2Int _pos)
    {
        base.SetRoom(room, _pos);
        gameObject.name = "Goblin" + _pos;
    }

    protected override void OnDieCallback()
    {
        //TODO : DROP CARD TEMP
        if (UnityEngine.Random.Range(0, 10) == 0)
        {
            PlayerControl.instance.AddCard(new CardData_Stone(5, PlayerControl.instance.PlayerObject));
        }
        else if (UnityEngine.Random.Range(0, 5) == 0)
        {
            PlayerControl.instance.AddCard(new CardData_Bandage(2, PlayerControl.instance.PlayerObject));
        }
        base.OnDieCallback();
    }
}

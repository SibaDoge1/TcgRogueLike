using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGoblin : Enemy {

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


        if (TileUtils.CalcRange(currentTile.pos, PlayerControl.instance.PlayerObject.currentTile.pos) <= range)
        {
            PlayerControl.instance.PlayerObject.GetDamage(atk,this);
			PlayAttackMotion ();
            OnEndTurn();
        }
        else
        {
            if (!MoveTo(PathFinding.instance.GeneratePath(this, PlayerControl.instance.PlayerObject.currentTile)[0].pos))
            {
                OnEndTurn();
            }
        }
        return true;
    }
    public override void SetRoom(Room room, Vector2Int _pos)
    {
        base.SetRoom(room, _pos);
        gameObject.name = "BossGoblin" + _pos;
    }
    protected override void OnDieCallback()
    {
        if (UnityEngine.Random.Range(0, 8) == 0)
        {
            PlayerControl.instance.AddCard(new CardData_Stone(5,PlayerControl.instance.PlayerObject, (Attribute)Random.Range(1, 4)));
        }
        else if (UnityEngine.Random.Range(0, 12) == 0)
        {
            PlayerControl.instance.AddCard(new CardData_Bandage(2, PlayerControl.instance.PlayerObject));
        }

        base.OnDieCallback();
    }
}

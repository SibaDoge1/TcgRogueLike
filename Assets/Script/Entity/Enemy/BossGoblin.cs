using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGoblin : Enemy {

    protected override void Start()
    {
        base.Start();
        fullHp = 30; currentHp = 30;
        damage = 5;
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
            PlayerControl.instance.PlayerObject.currentHp -= damage;
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
        UIManager.instance.GameWin();
        if (UnityEngine.Random.Range(0, 8) == 0)
        {
            PlayerControl.instance.AddCard(new CardData_Stone(5));
        }
        else if (UnityEngine.Random.Range(0, 12) == 0)
        {
            PlayerControl.instance.AddCard(new CardData_Bandage(2));
        }

        base.OnDieCallback();
    }
}

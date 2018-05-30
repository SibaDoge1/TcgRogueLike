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
    public override void DoAct()
    {
        base.DoAct();
        if(turn%3!=0)
        {
            OnEndTurn();
            return;
        }

        if (Room.CalcRange(currentTile.pos, currentRoom.GetPlayerTile().pos) <= range)
        {
            currentRoom.GetPlayerTile().OnTileObj.currentHp -= damage;
			PlayAttackMotion ();
            OnEndTurn();
        }
        else
        {
            MoveTo(PathFinding.instance.GeneratePath(this, currentRoom.GetPlayerTile())[0].pos);
        }
    }
    public override void SetRoom(Room room, Vector2Int _pos)
    {
        base.SetRoom(room, _pos);
        gameObject.name = "BossGoblin" + _pos;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGoblin : Enemy {

    protected override void Start()
    {
        base.Start();
        fullHp = 25; currentHp = 25;
        damage = 3;
        range = 1;
    }
    protected int range;

    int turn = 0;
    public override void DoAct()
    {
        turn++;
        if(turn%2==0)
        {
            OnEndTurn();
            return;
        }

        if (Room.CalcRange(currentTile.pos, currentRoom.GetPlayerTile().pos) <= range)
        {
            Debug.Log("Attack");
            currentRoom.GetPlayerTile().OnTileObj.currentHp -= damage;
            OnEndTurn();
            return;
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

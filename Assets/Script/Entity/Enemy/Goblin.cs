using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Enemy {

    protected void Start()
    {
        fullHp = 5; currentHp = 5;
    }
    public void DoAct()
    {
		if (Room.CalcRange (currentTile.pos, currentRoom.GetPlayerTile ().pos) <= 1) {
			//TODO ATTACK PLAYER
		} else {
			MoveTo (PathFinding.instance.GeneratePath (this, currentRoom.GetPlayerTile()) [0].pos);
		}
    }
    public override void SetRoom(Room room, Vector2Int _pos)
    {
        base.SetRoom(room, _pos);
        gameObject.name = "Goblin" + _pos;
    }
}

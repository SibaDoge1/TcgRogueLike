using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Enemy {
    protected override void Start()
    {
        base.Start();
        fullHp = 5; currentHp = 5;
    }
    public override void DoAct()
    {
        Debug.Log("ACT");
        if(TileUtils.AI_SquareFind(currentTile,1))
        {
           GameManager.player.currentHp -= 1;
        }
        else
       MoveTo( PathFinding.instance.GeneratePath(this, Player.instance.currentTile)[0].pos) ;
    }
    public override void SetRoom(Room room, Vector2Int _pos)
    {
        base.SetRoom(room, _pos);
        gameObject.name = "Goblin" + _pos;
    }
}

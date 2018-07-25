using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManekingHand : Enemy {

    protected override void Start()
    {
        base.Start();
        _fullHp =SettingHp; _currentHp =SettingHp;
    }

    protected override void Think()
    {
        if(TileUtils.AI_CircleFind(currentTile,1))
        {
            currentActionList = attackList;
        }else
        {
            currentActionList = moveList;
        }
    }
    List<Action> moveList;
    List<Action> attackList;
    protected override void SetActionLists()
    {
        DelayList = new List<Action>() {new SimpleDelayAction(this) };
        moveList = new List<Action>() { new SimpleMoveToWard(this) };
        attackList = new List<Action>() { new SimpleAttack(this) };
    }


    protected override void OnDieCallback (){
		//TODO : DROP CARD TEMP
		if (UnityEngine.Random.Range (0, 8) == 0) {
			PlayerControl.instance.AddCard (new CardData_Stone (5,PlayerControl.instance.PlayerObject,(Attribute)UnityEngine.Random.Range(1, 4)));
		} else if (UnityEngine.Random.Range (0, 12) == 0)
        {
			PlayerControl.instance.AddCard (new CardData_Bandage (2, PlayerControl.instance.PlayerObject));
		}

		base.OnDieCallback ();
	}
}

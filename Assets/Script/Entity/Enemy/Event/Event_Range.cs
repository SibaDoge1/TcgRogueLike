using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_Range : Enemy
{
    protected override void SetActionLists()
    {
        act = new List<Action>() {new Action(Delay),new Action(Destroy)};
    }

    List<Action> act;
    protected override void Think()
    {
        currentActionList = act;
    }

    IEnumerator Delay()
    {
        yield return null;
    }
    IEnumerator Destroy()
    {
        currentRoom.RoomName = "fail";
        DestroyThis();
        yield return null;
    }
}

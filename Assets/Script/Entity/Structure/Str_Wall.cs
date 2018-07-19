using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Str_Wall : Structure
{
    public override void SetRoom(Room room, Vector2Int _pos)
    {
        base.SetRoom(room, _pos);
        isHitable = false;
        name = "Wall" + _pos;
    }
    protected override void HpEffect(int value)
    {
        if (value <= 0)
        {
            EffectDelegate.instance.MadeEffect(CardEffectType.Blood, transform.position);
            OnDieCallback();
        }
    }
}

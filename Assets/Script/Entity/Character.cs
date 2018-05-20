using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : OnTileObject {

    public override void MoveTo(Vector2Int _pos)
    {
        int xOffset = pos.x - _pos.x;
        if (xOffset >=1)
        {
            transform.localScale = new Vector3(1,1,1);
        }
        else if (xOffset <= -1)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        base.MoveTo(_pos);
    }

}

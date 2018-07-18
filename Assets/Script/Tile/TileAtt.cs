using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;

/// <summary>
/// 타일 속성
/// </summary>
public abstract class TileAtt :MonoBehaviour
{
	protected Tile thisTile;
    public virtual Tile ThisTile
    {
        get
        {
            return thisTile;
        }
        set
        {
            thisTile = value;
            transform.parent = thisTile.transform;
            transform.position = thisTile.transform.position;
        }
    }
    public abstract void SomethingUpOnThis(Entity ot);
}


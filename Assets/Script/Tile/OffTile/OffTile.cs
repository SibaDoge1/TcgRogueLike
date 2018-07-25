using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;


public abstract class OffTile : MonoBehaviour
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

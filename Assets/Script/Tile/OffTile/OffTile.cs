using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;


public abstract class OffTile : MonoBehaviour
{
    protected Tile currentTile;
    public virtual Tile CurrentTile
    {
        get
        {
            return currentTile;
        }
        set
        {
            currentTile = value;
            transform.parent = currentTile.transform;
            transform.position = currentTile.transform.position;
        }
    }
    public abstract void SomethingUpOnThis(Entity ot);
}

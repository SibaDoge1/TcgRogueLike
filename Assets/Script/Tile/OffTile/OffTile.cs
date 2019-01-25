using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;


public abstract class OffTile : MonoBehaviour
{
    public int offTileNum;
    public bool isEvent;

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
    public virtual void DestroyThis()
    {
        currentTile.offTile = null;
        Destroy(gameObject);
    }
    public virtual bool IsStandAble(Entity et)
    {
        return true;
    }
    public virtual void Init(short num)
    {
        offTileNum = num;
    }
}

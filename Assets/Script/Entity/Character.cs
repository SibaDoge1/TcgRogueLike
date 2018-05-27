using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : OnTileObject {

    protected OnTileUI ontileUI;
    protected override void Awake()
    {
        base.Awake();
        ontileUI = transform.Find("Canvas").GetComponent<OnTileUI>();
    }
    public override bool MoveTo(Vector2Int _pos)
    {
        int xOffset = pos.x - _pos.x;
        SetLocalScale(xOffset);
		return  base.MoveTo(_pos);
    }
    protected void SetLocalScale(int x)
    {
        if(x>0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            ontileUI.SetLocalScale(x);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
            ontileUI.SetLocalScale(x);
        }
    }
    public override int fullHp
    {
        get
        {
            return base.fullHp;
        }

        set
        {
            ontileUI.HpUpdate(value, currentHp);
            base.fullHp = value;
        }
    }
    public override int currentHp
    {
        get
        {
            return base.currentHp;
        }

        set
        {
            ontileUI.HpUpdate(fullHp, value);
            base.currentHp = value;
        }
    }
}

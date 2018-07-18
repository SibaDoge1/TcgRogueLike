using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : Entity {

    protected CharacterUI characterUI;
    protected override void Awake()
    {
        base.Awake();
        characterUI = transform.Find("Canvas").GetComponent<CharacterUI>();
    }
    public override bool MoveTo(Vector2Int _pos)
    {
        int xOffset = pos.y - _pos.y;
        SetLocalScale(xOffset);
		return  base.MoveTo(_pos);
    }
    protected void SetLocalScale(int x)
    {
        if(x>0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            characterUI.SetLocalScale(x);
        }
        else if (x<0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            characterUI.SetLocalScale(x);
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
            base.currentHp = value;
            if (currentHp < fullHp)
            {
                characterUI.HpOn(fullHp, currentHp);
            }
        }
    }
}

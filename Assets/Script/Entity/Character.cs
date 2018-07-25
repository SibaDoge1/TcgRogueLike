using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : Entity {
    [Header("Setting")]
    [SerializeField]
    protected int SettingHp;
    public int atk;
    public int def;

    protected CharacterUI characterUI;
    protected override void Awake()
    {
        base.Awake();
        characterUI = transform.Find("Canvas").GetComponent<CharacterUI>();
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
            characterUI.SetLocalScale(x);
        }
        else if (x<0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            characterUI.SetLocalScale(x);
        }
    }



    public override bool GetDamage(int damage, Entity atker = null)
    {
        int rest = damage - def;
        if (rest > 0)
        {
            base.GetDamage(rest);
            return true;
        }
        else
        {
            return false;
        }
    }
    public override bool GetDamage(float damage, Entity atker = null)
    {
        return GetDamage((int)damage,atker);
    }
    protected override void HpEffect(int value)
    {
        base.HpEffect(value);
        if (value < fullHp)
        {
            characterUI.HpOn(fullHp, value);
        }
    }
}

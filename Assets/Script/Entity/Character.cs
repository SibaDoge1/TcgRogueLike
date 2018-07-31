using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : Entity {
    [Header("Setting")]
    [SerializeField]
    protected int SettingHp;
    [SerializeField]
    protected int SettingAtk;
    [SerializeField]
    protected int SettingDef;

    protected override void Awake()
    {
        base.Awake();
    }
    public override bool MoveTo(Vector2Int _pos)
    {
        int xOffset = pos.x - _pos.x;
        SetLocalScale(xOffset);
		return  base.MoveTo(_pos);
    }
    protected virtual void SetLocalScale(int x)
    {
        if(x>0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (x<0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }



    public virtual bool GetDamage(int damage, Entity atker = null)
    {
        
            int rest = damage - def;
            if (rest > 0)
            {
                CurrentHp -= rest;
                return true;
            }
        
        return false;

    }
    public virtual bool GetDamage(float damage, Entity atker = null)
    {
        return GetDamage((int)damage,atker);
    }
    public virtual bool GetHeal(int heal)
    {
        CurrentHp += heal;
        return true;
    }
    public virtual bool GetHeal(float heal)
    {
        CurrentHp += (int)heal;
        return true;
    }
    protected int fullHp = 1;
    protected int currentHp = 1;
    public virtual int FullHp
    {
        get
        {
            return fullHp;
        }
        set
        {
            fullHp = value;
        }
    }
    public virtual int CurrentHp
    {
        get
        {
            return currentHp;
        }
        set
        {
            DamageEffect(value - CurrentHp);

            if (value<=0)
            {
                OnDieCallback();
            }
            if (value < FullHp)
                currentHp = value;
            else
                currentHp = FullHp;
        }
    }

    protected int atk;
    protected int def;
    public int Atk
    {
        get { return atk; }
        set { atk = value; }
    }
    public int Def
    {
        get { return def; }
        set { def = value; }
    }
    protected virtual void DamageEffect(int value)
    {
        EffectDelegate.instance.MadeEffect(value, transform.position);
    }
}

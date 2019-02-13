using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : Entity {

    public Sprite[] actionSprites;
    Sprite normalSprites;
    Coroutine anim;

    protected override void Awake()
    {
        base.Awake();
        normalSprites = spriteRender.sprite;
    }
    protected override void SetSpriteRender()
    {
        spriteRender = GetComponentInChildren<SpriteRenderer>();
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
            CurrentHp -= damage;
            return true;
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
    protected virtual int CurrentHp
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
    public int Atk
    {
        get { return atk; }
        set { atk = value; }
    }


    protected virtual void DamageEffect(int value)
    {
        EffectDelegate.instance.MadeEffect(value, transform.position);
    }

    protected virtual IEnumerator AnimationRoutine(int num, float animationTime = 0.5f)
    {
        spriteRender.sprite = actionSprites[num];
        yield return new WaitForSeconds(animationTime);
        spriteRender.sprite = normalSprites;
    }
}

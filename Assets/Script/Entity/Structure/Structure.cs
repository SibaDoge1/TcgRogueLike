using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Structure : Entity
{
    protected override void OnDieCallback()
    {
        //TODO : EFFECT
        base.OnDieCallback();
    }
    public override void Init(short _entityNum)
    {
        base.Init(_entityNum);
    }
    public void SetSprite(Sprite _sprite)
    {
        if(spriteRender == null)
        {
            spriteRender = GetComponent<SpriteRenderer>();
        }
        spriteRender.sprite = _sprite;
    }
}

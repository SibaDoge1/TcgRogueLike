using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Structure : Entity
{
    public override void Init(short _entityNum)
    {
        base.Init(_entityNum);
        if (entityNum < 105)
            SpriteRender.sortingLayerName = "Entity(Wall)";
    }
    public void SetSprite(Sprite _sprite)
    {
        SpriteRender.sprite = _sprite;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeLayer : MonoBehaviour {

    public void SetColor(Color color)
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.color = color;
    }
}

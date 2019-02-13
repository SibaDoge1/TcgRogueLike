using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 카드 오브젝트
/// </summary>
public abstract class CardObject : MonoBehaviour
{

    protected CardRender render;
    protected Card data;
    protected virtual void Awake()
    {
        render = transform.Find("render").GetComponent<CardRender>();
    }




    public virtual void SetCardData(Card _data){
		data = _data;
        render.SetRender(_data);
    }

	public virtual void SetParent(Transform tr)
    {
		transform.SetParent(tr);
	}

}

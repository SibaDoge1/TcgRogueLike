using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    EffectObject effectObject;
    Queue<EffectObject> pool = new Queue<EffectObject>();

    public void SetEffect(EffectObject param,int num = 10)
    {
        effectObject = param;
        MakeEffects(num);
    }

    private void MakeEffects(int num)
    {
        for(int i=0; i<num;i++)
        {
            DeActiveObject(Instantiate(effectObject.gameObject, transform).GetComponent<EffectObject>());
        }
    }

    public void DeActiveObject(EffectObject newObject)
    {
        pool.Enqueue(newObject);
        newObject.ConnectToObjectPool(this);
        newObject.transform.parent = transform;
        newObject.gameObject.SetActive(false);
    }

    public EffectObject ActiveObject(Vector3 pos)
    {
        if (pool.Count <= 0)
        {
            MakeEffects(5);
        }

        EffectObject activeObject =  pool.Dequeue();
        activeObject.gameObject.SetActive(true);
        activeObject.CountEffect();
        activeObject.transform.position = pos;
        return activeObject;
    }

    public EffectObject ActiveObject()
    {
        if(pool.Count<=0)
        {
            MakeEffects(5);
        }

        EffectObject activeObject = pool.Dequeue();
        activeObject.gameObject.SetActive(true);
        activeObject.CountEffect();
        return activeObject;
    }

    public void DestroyPool()
    {
        while(pool.Count == 0)
        {
            Destroy(pool.Dequeue());
        }
        Destroy(gameObject);
    }
}

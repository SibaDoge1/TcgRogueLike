using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectObject : MonoBehaviour {

    public bool isCount=true;
    public float destroyTime = 1;
    protected ObjectPool objectPool;

    public void ConnectToObjectPool(ObjectPool pool)
    {
        objectPool = pool;
    }
    public void CountEffect()
    {
        if(isCount)
        {
            StartCoroutine(CountEffectRoutine());
        }
    }
    protected virtual IEnumerator CountEffectRoutine()
    {
        yield return new WaitForSeconds(destroyTime);
        if(objectPool == null)
        {
            Destroy(this.gameObject);
        }else
        {
            OffObject();
        }
    }


    public virtual void OffObject()
    {
        objectPool.DeActiveObject(this);
    }
}

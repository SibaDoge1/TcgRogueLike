using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public static CameraFollow instance;
    private void Awake()
    {
        instance = this;
    }


    public IEnumerator RoomTrace(Room curr)
    {
        float _time = 0f;
        Vector3 Target = curr.transform.position + new Vector3(0, 0, -10);
        while(_time <2f)
        {
            _time += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position,Target,_time/2);
            yield return null;
        }
    }
}

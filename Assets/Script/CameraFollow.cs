using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public int speed;
    public static CameraFollow instance;
    private void Awake()
    {
        instance = this;
    }

    public void PlayerTrace(Player player)
    {
        StopAllCoroutines();
        StartCoroutine(RoomTraceRoutine(player));
    }

     IEnumerator RoomTraceRoutine(Player player)
    {
        
        Vector3 Target;
        while(true)
        {
            Target = player.transform.position + new Vector3(0, -1, -10);
            transform.position = Vector3.Lerp(transform.position,Target,speed*Time.deltaTime);
            yield return null;
        }
    }
}

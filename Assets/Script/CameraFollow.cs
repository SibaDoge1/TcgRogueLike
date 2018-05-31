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
            Target = new Vector3(player.transform.position.x, player.transform.position.y, -10);
            transform.position = Vector3.Lerp(transform.position,Target,speed*Time.deltaTime);
            yield return null;
        }
    }

	IEnumerator CameraImpactRoutine(){
		yield return null;
	}
}

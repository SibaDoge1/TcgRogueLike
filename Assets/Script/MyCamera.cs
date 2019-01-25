using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCamera : MonoBehaviour {

    public float shake;
    public int speed;
    public static MyCamera instance;
    Camera cam;
    private void Awake()
    {
        instance = this;
        cam = GetComponentInChildren<Camera>();
       // cam.aspect = 16f / 9f;
    }

    public void PlayerTrace(Player player)
    {
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

    public void ShakeCamera()
    {
        if(cameraShake != null)
        {
            StopCoroutine(ShakeCameraRoutine());
        }
        cameraShake = StartCoroutine(ShakeCameraRoutine());
    }
    Coroutine cameraShake;
	IEnumerator ShakeCameraRoutine()
    {
        for(float time =0; time <0.2f; time+=Time.deltaTime)
        {
            cam.transform.localPosition = Vector3.zero;
            cam.transform.localPosition += new Vector3(Random.Range(-shake,shake), Random.Range(-shake,shake));
            yield return null;
        }
        cam.transform.localPosition = Vector3.zero;
    }
}

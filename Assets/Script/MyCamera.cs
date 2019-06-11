using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MyCamera : MonoBehaviour
{

    public float shake;
    public float speed;
    public static MyCamera instance;
    Camera cam ;
    private Player _player;
    public Vector2 CamPos { get { return cam.transform.position; } }
    private void Awake()
    {
        instance = this;
        cam = GetComponentInChildren<Camera>();
        //cam = transform.GetComponent<Camera>();
        // cam.aspect = 16f / 9f;
    }

    public void StartPlayerTrace(Player player)
    {
        _player = player;
        roomTrace = StartCoroutine(RoomTraceRoutine(_player));
    }

    public void StopTrace()
    {
        if (roomTrace != null)
        {
            StopCoroutine(roomTrace);
            roomTrace = null;
        }
    }
    public void ReStartTrace()
    {
        if(roomTrace == null)
            roomTrace = StartCoroutine(RoomTraceRoutine(_player));
    }

    public void MoveCam(Vector2 position)
    {
        Vector3 target= new Vector3(position.x, position.y, transform.position.z);
        transform.position = target;
    }

    Coroutine roomTrace;
    IEnumerator RoomTraceRoutine(Player player)
    {        
        Vector3 Target;
        while(true)
        {
            Target = new Vector3(player.transform.position.x, player.transform.position.y, -10);
            //transform.position = Vector3.Lerp(transform.position,Target,speed*Time.deltaTime);
            transform.position = Target;
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

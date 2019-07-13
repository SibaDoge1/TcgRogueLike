using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CamDragArea : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    Vector2 touchOriginPos;
    Vector2 camOriginPos;
    // Start is called before the first frame update
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 originWorldPos = Camera.main.ScreenToWorldPoint(touchOriginPos);
        Vector2 curWorldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        Vector2 diff = curWorldPos - originWorldPos;
        MyCamera.instance.MoveCam(camOriginPos + diff);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //touchOriginPos = Camera.main.ScreenToWorldPoint(eventData.position);
        touchOriginPos = eventData.position;
        camOriginPos = MyCamera.instance.CamPos;
        MyCamera.instance.StopTrace();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //MyCamera.instance.ReStartTrace();
    }
}

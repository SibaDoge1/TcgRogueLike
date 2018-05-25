using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class SelectableTile : MonoBehaviour , IPointerDownHandler
{
    private void Start()
    {
        thisTile = GetComponent<TheTile>();
    }
    TheTile thisTile;
    public void OnPointerDown(PointerEventData ped)
    {
        Debug.Log("Clicked");
        if(ped.button == PointerEventData.InputButton.Right)
        {
            PlayerControll.instance.PlayerMove(thisTile);
        }
    }

}

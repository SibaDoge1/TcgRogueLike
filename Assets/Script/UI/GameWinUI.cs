using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Game Win UI
/// </summary>
public class GameWinUI : MonoBehaviour {

    RectTransform rect;
    Vector3 offPos = new Vector3(0, 2000, 0);
    // Use this for initialization
    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
    public void On()
    {
        rect.anchoredPosition = Vector3.zero;
    }
    public void Off()
    {
        rect.anchoredPosition = offPos;
    }

}

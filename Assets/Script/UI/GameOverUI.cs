using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameOverUI : MonoBehaviour {

    RectTransform rect;
    Text text;
    Vector3 offPos = new Vector3(0, 2000, 0);
    // Use this for initialization
    void Awake () {
        rect = GetComponent<RectTransform>();
        text = transform.Find("Text").GetComponent<Text>();
	}
	public void SetText(string s)
    {
        text.text = s;
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

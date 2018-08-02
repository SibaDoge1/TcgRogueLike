using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardInfoPanel : MonoBehaviour {

    private Text text;
    private void Awake()
    {
        text = transform.Find("Text").GetComponent<Text>();
    }

    public void SetText(string name,string info)
    {
        text.text = "카드 이름 :"+name+"\n" + info;
    }
    public void SetUnknown()
    {
        text.text = "카드 이름 : 미지카드" + "\n" + "알수 없음";
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpUI : MonoBehaviour {
    Transform[] currentHps;
    Text hpText;
    // Use this for initialization
    void Awake ()
    {      
        currentHps = transform.Find("Current").GetComponentsInChildren<Transform>();
        hpText = transform.Find("text").GetComponent<Text>();
    }
    public void HpUpdate(int currentHp_)
    {
        if (currentHp_ > 10)
        {
            currentHp_ = 10;
            Debug.Log("최대 범위 보다 넘는 수치만큼 값이 들어왔습니다.");
        }

        for (int i=1; i<currentHps.Length;i++)
        {
            if(i<=currentHp_)
            {
                currentHps[i].gameObject.SetActive(true);
            }
            else
            {
                currentHps[i].gameObject.SetActive(false);
            }
        }
        hpText.text = currentHp_ + "/" + 10;
    }

}

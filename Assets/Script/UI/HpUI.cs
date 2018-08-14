using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpUI : MonoBehaviour {
    Transform[] fullHps,currentHps;
    Text hpText;
    // Use this for initialization
    void Awake ()
    {      
        fullHps = transform.Find("Full").GetComponentsInChildren<Transform>();
        currentHps = transform.Find("Current").GetComponentsInChildren<Transform>();
        hpText = transform.Find("text").GetComponent<Text>();
    }
    public void HpUpdate(int currentHp_, int fullHp_)
    {
        for(int i=0; i< fullHps.Length;i++)
        {
            if(i<fullHp_)
            {
                fullHps[i].gameObject.SetActive(true);
            }else
            {
                fullHps[i].gameObject.SetActive(false);
            }
        }
        for(int i=0; i<currentHps.Length;i++)
        {
            if(i<currentHp_)
            {
                currentHps[i].gameObject.SetActive(true);
            }
            else
            {
                currentHps[i].gameObject.SetActive(false);
            }
        }
        hpText.text = currentHp_ + "/" + fullHp_;
    }

}

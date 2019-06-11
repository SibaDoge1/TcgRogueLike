using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AkashaUI : MonoBehaviour {
    Transform[] currents;
    // Use this for initialization
    void Awake () {
        currents = transform.Find("Akasha").GetComponentsInChildren<Transform>();
    }
	public void AkashaUpdate(int current, int maxAks)
    {
        if(current> maxAks)
        {
            current = maxAks;
            Debug.Log("최대 범위 보다 넘는 수치만큼 값이 들어왔습니다.");
        }
        for(int i =1; i<currents.Length;i++)
        {
            if(i<=current)
            {
                currents[i].gameObject.SetActive(true);
            }else
            {
                currents[i].gameObject.SetActive(false);
            }
        }
    }

	
}

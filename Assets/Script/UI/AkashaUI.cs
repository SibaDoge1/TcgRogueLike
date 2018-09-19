using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AkashaUI : MonoBehaviour {
    Text akashaText;
    Transform[] currents;
    // Use this for initialization
    void Awake () {
        currents = transform.Find("Akasha").GetComponentsInChildren<Transform>();
        akashaText = transform.Find("text").GetComponent<Text>();
    }
	public void AkashaUpdate(int current,int full)
    {
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
        akashaText.text = current/ (float)full *100 +"%";
    }

	
}

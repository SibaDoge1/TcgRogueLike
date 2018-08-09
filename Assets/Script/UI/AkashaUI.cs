using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AkashaUI : MonoBehaviour {
    Image fullAkasha,currentAkasha;
    Text akashaText, akashaCount;
    // Use this for initialization
    void Awake () {
        fullAkasha = transform.Find("Akasha").GetComponent<Image>();
        currentAkasha = fullAkasha.transform.Find("current").GetComponent<Image>();
        akashaText = transform.Find("text").GetComponent<Text>();
        akashaCount = transform.Find("akashaCount").GetComponent<Text>();
    }
	public void AkashaUpdate(int current,int full)
    {
        currentAkasha.fillAmount = current / (float)full;
        akashaText.text = current/ (float)full *100 +"%";
    }
    public void AkashaCountUpdate(int count)
    {
        akashaCount.text = "X" + count;
    }
	
}

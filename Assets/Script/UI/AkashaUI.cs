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
        akashaText = fullAkasha.transform.Find("text").GetComponent<Text>();
        akashaCount = fullAkasha.transform.Find("akashaCount").GetComponent<Text>();
    }
	public void AkashaUpdate(int current,int full)
    {
        currentAkasha.fillAmount = current / (float)full;
        akashaText.text = current + "/" + full;
    }
    public void AkashaCountUpdate(int count)
    {
        akashaCount.text = "X" + count;
    }
	
}

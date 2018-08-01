using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpUI : MonoBehaviour {
    Image fullHp, currentHp;
    Text hpText;
    // Use this for initialization
    void Awake () {
        fullHp = transform.Find("Hp").GetComponent<Image>();
        currentHp = fullHp.transform.Find("current").GetComponent<Image>();
        hpText = fullHp.transform.Find("text").GetComponent<Text>();
    }
    public void HpUpdate(int currentHp_, int fullHp_)
    {
        currentHp.fillAmount = currentHp_ / (float)fullHp_;
        hpText.text = currentHp_ + "/" + fullHp_;
    }

}

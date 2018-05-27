using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    private void Awake()
    {
        instance = this;
        fullHp = transform.Find("Hp").GetComponent<Image>();
        currentHp = fullHp.transform.Find("current").GetComponent<Image>();
        hpText = fullHp.transform.Find("hpText").GetComponent<Text>();
    }
    Image fullHp, currentHp;
    Text hpText;


	public void HpUpdate(int currentHp_, int fullHp_)
    {
        
		currentHp.fillAmount = currentHp_ / (float) fullHp_;
		hpText.text = currentHp_ + "/" + fullHp_;
    }
}

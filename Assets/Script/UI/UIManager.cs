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


    public void HpUpdate()
    {
		//TODO UI
        //float hp = (float)Player.instance.currentHp / Player.instance.fullHp;
        //currentHp.rectTransform.sizeDelta = new Vector2(hp*400, 50);
        //hpText.text = Player.instance.currentHp+"/"+Player.instance.fullHp;
    }
}

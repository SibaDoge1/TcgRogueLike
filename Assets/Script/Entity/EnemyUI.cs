using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyUI : MonoBehaviour {

    Image fullHpUI;
    Image currentHpUI;
    //Image attImage;
    Image actionImage;
    Coroutine hpRoutine;

    Color originColor;

	void Awake () {
        fullHpUI = transform.Find("HpUI").GetComponent<Image>();
        currentHpUI = fullHpUI.transform.Find("current").GetComponent<Image>();

        originColor = currentHpUI.color;

        //attImage = transform.Find("Atr").GetComponent<Image>();
        actionImage = transform.Find("action").GetComponent<Image>();
        //attImage.color = Color.clear;
        fullHpUI.color = Color.clear;
        currentHpUI.color = Color.clear;
        actionImage.sprite = ArchLoader.instance.GetWarningImage();
        actionImage.rectTransform.localScale = new Vector3(2, 2, 2);
        actionImage.enabled = false;
    }
    public void HpOn(int fullHp,int currentHp)
    {
        fullHpUI.color = Color.white;
        currentHpUI.color = originColor;
        currentHpUI.fillAmount = (float)currentHp / fullHp;
    }

    public void SetLocalScale(int x)
    {
        if(x>0)
        {
            transform.localScale = new Vector3(0.02f, 0.02f,1);
        }
        else
        {
            transform.localScale = new Vector3(-0.02f, 0.02f,1);
        }
    }


   /* Sprite attSprite;
    public void SetAtt(Attribute at)
    {
        switch(at)
        {
            case Attribute.APAS:
                attSprite = Resources.Load<Sprite>("Attribute/apas1");
                break;
            case Attribute.PRITHVI:
                attSprite = Resources.Load<Sprite>("Attribute/prithivi1");
                break;
            case Attribute.TEJAS:
                attSprite = Resources.Load<Sprite>("Attribute/tejas1");
                break;
            case Attribute.VAYU:
                attSprite = Resources.Load<Sprite>("Attribute/vayu1");
                break;
        }

        attImage.color = Color.white;
        attImage.sprite = attSprite;
        attImage.enabled = true;
        //attImage.sprite = Resources.Load<Sprite>("Attribute/dontknow");
    }*/

    public void ActionImageOn()
    {
        actionImage.enabled = true;
    }
    public void ActionImageOff()
    {
        actionImage.enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyUI : MonoBehaviour {

    Image fullHpUI;
    Image currentHpUI;
    Image attImage;
    Image actionImage;
    // Use this for initialization
    Coroutine hpRoutine;

    Color originColor;
    Color targetColor;

	void Awake () {
        fullHpUI = transform.Find("HpUI").GetComponent<Image>();
        currentHpUI = fullHpUI.transform.Find("current").GetComponent<Image>();

        originColor = currentHpUI.color;
        targetColor = new Color(originColor.r, originColor.g, originColor.b, 0);

        attImage = transform.Find("Atr").GetComponent<Image>();
        actionImage = transform.Find("action").GetComponent<Image>();
        attImage.color = Color.clear;
        fullHpUI.color = Color.clear;
        currentHpUI.color = Color.clear;

        actionImage.enabled = false;
    }
    public void HpOn(int fullHp,int currentHp)
    {
        fullHpUI.color = Color.white;
        currentHpUI.color = originColor;
        currentHpUI.fillAmount = (float)currentHp / fullHp;
        HpUpdate(fullHp, currentHp);
    }
	public void HpUpdate(int fullHp,int currentHp)
    {
        if(hpRoutine!=null)
       StopCoroutine(hpRoutine);

       hpRoutine = StartCoroutine(HpUpdateRoutine(fullHp, currentHp));
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
    IEnumerator HpUpdateRoutine(int fullHp, int currentHp)
    {
        fullHpUI.color = Color.white;
        currentHpUI.color = originColor;
        currentHpUI.fillAmount = (float)currentHp / fullHp;

        yield return new WaitForSeconds(0.5f);

        float _time = 0f;
        while(_time<1)
        {
            _time += Time.deltaTime;
            
            fullHpUI.color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), _time);
            currentHpUI.color = Color.Lerp(originColor, targetColor, _time);
            yield return null;          
        }
    }

    Sprite attSprite;
    bool isDiscovered;
    public void SetAtt(Attribute at)
    {
        switch(at)
        {
            case Attribute.AK:
                attSprite = Resources.Load<Sprite>("Attribute/akasha1");
                break;
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
    }
    public void AttIconFlash()
    {
        if(!isDiscovered)
        StartCoroutine(AttFlashRoutine());
    }
    IEnumerator AttFlashRoutine()
    {
        attImage.color = Color.white;
        
        yield return new WaitForSeconds(0.5f);

        float _time = 0f;
        while (_time < 1)
        {
            _time += Time.deltaTime;

            attImage.color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), _time);
            yield return null;
        }
    }
    public void AttIconOn()
    {
        isDiscovered = true;
        attImage.color = Color.white;
        attImage.sprite = attSprite;
    }
    /// <summary>
    ///temp
    /// </summary>
    public void ActionIconOn(bool bo)
    {
        if(bo)
        {
            actionImage.enabled = true;
        }
        else
        {
            actionImage.enabled = false;
        }
    }
}

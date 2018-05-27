using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OnTileUI : MonoBehaviour {

    Image fullHpUI;
    Image currentHpUI;
    // Use this for initialization
    Coroutine hpRoutine;

    Color originColor;
    Color targetColor;

	void Awake () {
        fullHpUI = transform.Find("Canvas").Find("HpUI").GetComponent<Image>();
        currentHpUI = fullHpUI.transform.Find("current").GetComponent<Image>();

        originColor = currentHpUI.color;
        targetColor = new Color(originColor.r, originColor.g, originColor.b, 0);

        fullHpUI.color = Color.clear;
        currentHpUI.color = Color.clear;
    }
	public void HpUpdate(int fullHp,int currentHp)
    {
        if(hpRoutine!=null)
       StopCoroutine(hpRoutine);

       hpRoutine = StartCoroutine(HpUpdateRoutine(fullHp, currentHp));
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
}

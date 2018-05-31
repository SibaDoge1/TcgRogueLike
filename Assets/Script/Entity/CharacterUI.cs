using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharacterUI : MonoBehaviour {

    Image fullHpUI;
    Image currentHpUI;
    // Use this for initialization
    Coroutine hpRoutine;

    Text turnText;
    Color originColor;
    Color targetColor;

	void Awake () {
        fullHpUI = transform.Find("HpUI").GetComponent<Image>();
        currentHpUI = fullHpUI.transform.Find("current").GetComponent<Image>();

        originColor = currentHpUI.color;
        targetColor = new Color(originColor.r, originColor.g, originColor.b, 0);

        fullHpUI.color = Color.clear;
        currentHpUI.color = Color.clear;

        turnText = transform.Find("turn").Find("turnText").GetComponent<Text>();
        turnText.color = Color.black;
    }
	public void HpUpdate(int fullHp, int currentHp)
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
    public void HpUpdateRoutine(int fullHp, int currentHp)
    {
        if (hpRoutine != null)
            StopCoroutine(hpRoutine);
        hpRoutine = StartCoroutine(IEnumHpUpdate(fullHp, currentHp));
    }
    IEnumerator IEnumHpUpdate(int fullHp, int currentHp)
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
    public void SetTurnText(int turn)
    {
        if(turn<=0)
        {
            turnText.text = "A";
        }
        else
        {
            turnText.text = turn.ToString();
        }
    }
}

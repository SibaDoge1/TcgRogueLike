using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticeUI : MonoBehaviour{

    private static Text text;
    private static Image image;


    public void BeforeRoutine()
    {
        if(text == null)
            text = transform.Find("Text").GetComponent<Text>();
        if (image == null)
            image = transform.Find("Image").GetComponent<Image>();
    }

    public void Notice(string str, float time)
    {
        gameObject.SetActive(true);
        StopAllCoroutines();
        BeforeRoutine();
        text.text = str;
        StartCoroutine(FadeOutInRoutine(0.3f, time));
    }

    private IEnumerator FadeOutInRoutine(float time, float waitTime, voidFunc middleFunc = null, voidFunc endFunc = null)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);
        //fade.gameObject.SetActive(true);
        Color DefaultCol = image.color;
        Color col = image.color;
        Color DefaultTextCol = text.color;
        Color textCol = text.color;
        float curtime = 0f;
        while (col.a < 1)
        {
            col = image.color;
            textCol = text.color;
            curtime += Time.deltaTime;
            col.a = Mathf.Lerp(DefaultCol.a, 1, curtime / time);
            textCol.a = Mathf.Lerp(DefaultTextCol.a, 1, curtime / time);
            image.color = col;
            text.color = textCol;
            yield return null;
        }
        if (middleFunc != null)
        {
            middleFunc();
        }
        yield return new WaitForSeconds(waitTime);
        DefaultCol = image.color;
        col = image.color;
        DefaultTextCol = text.color;
        textCol = text.color;
        curtime = 0f;
        while (col.a > 0)
        {
            col = image.color;
            textCol = text.color;
            curtime += Time.deltaTime;
            col.a = Mathf.Lerp(DefaultCol.a, 0, curtime / time);
            textCol.a = Mathf.Lerp(DefaultTextCol.a, 0, curtime / time);
            image.color = col;
            text.color = textCol;
            yield return null;
        }
        if (endFunc != null)
        {
            endFunc();
        }
        gameObject.SetActive(false);
    }
}

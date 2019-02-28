using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    private Image fade;
    // Start is called before the first frame update

    public void BeforeRoutine()
    {
        if (fade == null)
            fade = transform.GetComponent<Image>();
    }

    public IEnumerator FadeOutRoutine(float time, voidFunc func)
    {
        BeforeRoutine();
        //fade.gameObject.SetActive(true);
        fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, 0f);
        Color DefaultCol = fade.color;
        Color col = fade.color;
        float curtime = 0f;
        while (col.a < 1)
        {
            col = fade.color;
            curtime += Time.deltaTime;
            col.a = Mathf.Lerp(DefaultCol.a, 1, curtime / time);
            fade.color = col;
            yield return null;
        }
        if (func != null)
        {
            func();
        }
        //fade.gameObject.SetActive(false);
    }

    public IEnumerator FadeInRoutine(float time, voidFunc func)
    {
        BeforeRoutine();
        fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, 1f);
        //fade.gameObject.SetActive(true);
        Color DefaultCol = fade.color;
        Color col = fade.color;
        float curtime = 0f;
        while (col.a > 0)
        {
            col = fade.color;
            curtime += Time.deltaTime;
            col.a = Mathf.Lerp(DefaultCol.a, 0, curtime / time);
            fade.color = col;
            yield return null;
        }
        if (func != null)
        {
            func();
        }
        //fade.gameObject.SetActive(false);
    }

    public IEnumerator FadeOutInRoutine(float time, float waitTime, voidFunc middleFunc, voidFunc endFunc)
    {
        BeforeRoutine();
        fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, 0f);
        //fade.gameObject.SetActive(true);
        Color DefaultCol = fade.color;
        Color col = fade.color;
        float curtime = 0f;
        while (col.a < 1)
        {
            col = fade.color;
            curtime += Time.deltaTime;
            col.a = Mathf.Lerp(DefaultCol.a, 1, curtime / time);
            fade.color = col;
            yield return null;
        }
        if (middleFunc != null)
        {
            middleFunc();
        }
        yield return new WaitForSeconds(waitTime);
        DefaultCol = fade.color;
        col = fade.color;
        curtime = 0f;
        while (col.a > 0)
        {
            col = fade.color;
            curtime += Time.deltaTime;
            col.a = Mathf.Lerp(DefaultCol.a, 0, curtime / time);
            fade.color = col;
            yield return null;
        }
        if (endFunc != null)
        {
            endFunc();
        }
        //fade.gameObject.SetActive(false);
    }

    public void SetImage(Image image)
    {
        fade = image;
    }
}


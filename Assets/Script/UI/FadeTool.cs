using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public delegate void voidFunc();

public class FadeTool : MonoBehaviour
{
    private static FadeTool instance;
    public static FadeTool Instance
    {
        get { return instance; }
    }

    private Image fade;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) instance = this;
        else if(instance != this)
        {
            Debug.LogWarning("Singleton Error! : " + this.name);
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        fade = GameObject.Find("Canvas").transform.Find("Fade").GetComponent<Image>();
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        fade = GameObject.Find("Canvas").transform.Find("Fade").GetComponent<Image>();
    }
    public void FadeOut(float time, voidFunc func)
    {
        StartCoroutine(FadeOutRoutine(time, func));
    }

    public void FadeIn(float time, voidFunc func)
    {
        StartCoroutine(FadeInRoutine(time, func));
    }
    public void FadeInOut(float time, voidFunc func)
    {
        StartCoroutine(FadeInOutRoutine(time,0f, func, null));
    }
    public void FadeInOut(float time, float waitTime, voidFunc func)
    {
        StartCoroutine(FadeInOutRoutine(time, waitTime, func, null));
    }
    public void FadeInOut(float time, float waitTime, voidFunc func, voidFunc endFunc)
    {
        StartCoroutine(FadeInOutRoutine(time, waitTime, func, endFunc));
    }

    IEnumerator FadeOutRoutine(float time, voidFunc func)
    {
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
        if (func != null)
        {
            func();
        }
        //fade.gameObject.SetActive(false);
    }

    IEnumerator FadeInRoutine(float time, voidFunc func)
    {
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

    IEnumerator FadeInOutRoutine(float time, float waitTime, voidFunc middleFunc, voidFunc endFunc)
    {
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

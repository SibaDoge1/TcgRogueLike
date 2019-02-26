using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toast : MonoBehaviour
{
    public static Toast Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            UnityEngine.Debug.LogError("SingleTone Error : " + this.name);
            Destroy(this);
        }
        panel = transform.GetComponent<Image>();
        txt = transform.Find("Text").GetComponent<Text>();
        orginalColor = txt.color;
        orginalColorP = panel.color;
        txt.color = Color.clear;
        panel.color = Color.clear;
    }
    void Start()
    {
        showToast("Hello", 2);
    }

    private Text txt;
    private Image panel;
    private Color orginalColor;
    private Color orginalColorP;

    void showToast(string text, int duration)
    {
        panel = transform.GetComponent<Image>();
        txt = transform.Find("Text").GetComponent<Text>();
        StartCoroutine(showToastCOR(text, duration));
    }

    private IEnumerator showToastCOR(string text, int duration)
    {
        txt.color = Color.clear;
        panel.color = Color.clear;
        txt.text = text;
        txt.enabled = true;
        panel.enabled = true;

        //Fade in
        yield return fadeInAndOut(true, 0.5f);

        //Wait for the duration
        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            yield return null;
        }

        //Fade out
        yield return fadeInAndOut(false, 0.5f);

        txt.enabled = false;
        panel.enabled = false;
        txt.color = Color.clear;
        panel.color = Color.clear;
    }

    IEnumerator fadeInAndOut(bool fadeIn, float duration)
    {
        //Set Values depending on if fadeIn or fadeOut
        float a, b;
        if (fadeIn)
        {
            a = 0f;
            b = 1f;
        }
        else
        {
            a = 1f;
            b = 0f;
        }
        
        float counter = 0f;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(a, b, counter / duration);

            txt.color = new Color(orginalColor.r, orginalColor.g, orginalColor.b, alpha * orginalColor.a);
            panel.color = new Color(orginalColorP.r, orginalColorP.g, orginalColorP.b, alpha * orginalColorP.a);
            yield return null;
        }
    }
}

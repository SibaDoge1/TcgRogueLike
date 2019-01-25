using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextUI : MonoBehaviour
{

    public float speed = 10;
    public float EndWait = 2f;
    Text text;
    Color clearWhite = new Color(1, 1, 1, 0);
    Animator anime;
    AudioSource sound;
    string[] strings;
    int counter;
    EventTileCallBack cb = null;

    private void Awake()
    {
        sound = GetComponent<AudioSource>();
        anime = GetComponent<Animator>();
        text = GetComponent<Text>();
    }
    
    public void StartText(string[] s,EventTileCallBack callback)
    {
        Debug.Log("START");
        strings = s;
        cb = callback;
        stringRoutine = StartCoroutine(ShowString(s[0]));
        GameManager.instance.IsInputOk = false;
    }
    public void GoNext()
    {
        if(strings == null)
        {
            return; 
        }
        text.enabled = false;
        counter++;
        sound.Stop();
        StopCoroutine(stringRoutine);

        if (counter>=strings.Length)
        {
            if(cb!=null)
            {
                cb();
            }
            ResetString();
        }else
        {
            stringRoutine = StartCoroutine(ShowString(strings[counter]));
        }
    }
    private void ResetString()
    {
        strings = null;
        counter = 0;
        GameManager.instance.IsInputOk = true;
    }

    Coroutine stringRoutine;
    IEnumerator ShowString(string s)
    {
        text.text = "";
        text.enabled = true;
        sound.Play();

        float oneWordTime = 0;
        int counter = 0;
        while(counter < s.Length)
        {
            yield return null;
            oneWordTime += Time.deltaTime;

            if(oneWordTime>=1/speed)
            {
                counter++;
                oneWordTime = 0;
                text.text = s.Substring(0, counter);
            }
        }
        sound.Stop();
    }
    

}

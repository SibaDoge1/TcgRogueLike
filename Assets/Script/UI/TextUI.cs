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
    GameObject sound;
    string[] strings;
    int counter;
    CallBack cb = null;

    private void Awake()
    {
        anime = GetComponent<Animator>();
        text = GetComponent<Text>();
    }
    
    public void StartText(string[] s,CallBack callback)
    {
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

        counter++;
        if(counter>=strings.Length)
        {
            if(cb!=null)
            {
                cb();
            }
            ResetString();
        }else
        {
            StopCoroutine(stringRoutine);
            stringRoutine = StartCoroutine(ShowString(strings[counter]));
        }
    }
    private void ResetString()
    {
        StopCoroutine(stringRoutine);
        text.text = "";
        strings = null;
        counter = 0;
        GameManager.instance.IsInputOk = true;
    }

    Coroutine stringRoutine;
    IEnumerator ShowString(string s)
    {
        //anime.Play("default", -1, 0f);
        for (int i=0; i<=s.Length;i++)
        {
            text.text = s.Substring(0, i);
            yield return new WaitForSeconds(1/speed);
        }
        Destroy(sound);
    }
    

}

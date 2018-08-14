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
    private void Awake()
    {
        anime = GetComponent<Animator>();
        text = GetComponent<Text>();
    }
    
    public void SetString(string s,CallBack cb = null)
    {
        if(stringRoutine !=null)
        {
            StopCoroutine(stringRoutine);
        }
       stringRoutine =  StartCoroutine(ShowString(s,cb));
    }
    public void ResetString()
    {
        text.text = "";
    }
    Coroutine stringRoutine;
    IEnumerator ShowString(string s, CallBack cb = null)
    {       
        for (int i=0; i<s.Length;i++)
        {
            text.text = s.Substring(0, i);
            yield return new WaitForSeconds(1/speed);
        }
        yield return new WaitForSeconds(1f);
        //DO ANIMATION
        anime.Play("ColorLerp",-1,0f);
        if(cb!=null)
        {
            cb();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public delegate void voidFunc();

public static class FadeTool
{
    private static Fade fade;

    public static void Reset()
    {
        if (!IsExist())
        {
            return;
        }
        fade.Reset();
    }

    public static void FadeOut(float time, voidFunc func = null)
    {
        if (!IsExist())
        {
            return;
        }
        fade.StartCoroutine(fade.FadeOutRoutine(time, func));
    }

    public static void FadeIn(float time, voidFunc func = null)
    {
        if (!IsExist())
        {
            return;
        }
        fade.StartCoroutine(fade.FadeInRoutine(time, func));
    }
    public static void FadeOutIn(float time, float waitTime = 0, voidFunc func = null, voidFunc endFunc = null)
    {
        if (!IsExist())
        {
            return;
        }
        fade.StartCoroutine(fade.FadeOutInRoutine(time, waitTime, func, endFunc));
    }
    public static bool IsExist()
    {
        if (fade == null)
        {
            try
            {
                fade = GameObject.Find("Tools_UI").transform.Find("Fade").GetComponent<Fade>();
            }
            catch(NullReferenceException e)
            {
                Debug.LogWarning("Fade object dosen't exist");
                fade = null;
            }
            return fade != null;
        }
        else
            return true;
    }
}

using System;
using UnityEngine;

public static class NoticeTool
{
    private static NoticeUI noticeUI;

    public static void Notice(string str, float time)
    {
        if (!IsExist())
        {
            return;
        }
        noticeUI.Notice(str, time);
    }
    
    public static bool IsExist()
    {
        if (noticeUI == null)
        {
            try
            {
                noticeUI = GameObject.Find("Tools_UI").transform.Find("NoticeUI").GetComponent<NoticeUI>();
            }
            catch (NullReferenceException e)
            {
                Debug.LogWarning("noticeUI object dosen't exist");
                noticeUI = null;
            }
            return noticeUI != null;
        }
        else
            return true;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LoadingManager {
    private static LoadingPanel panel;

    public static bool IsExist()
    {
        if (panel == null)
        {
            try
            {
                panel = GameObject.Find("Tools_UI").transform.Find("LoadingPanel").GetComponent<LoadingPanel>();
            }
            catch(NullReferenceException e)
            {
                Debug.Log("Loading object dosen't exist");
                panel = null;
            }
            return panel != null;
        }
        else
            return true;
    }

    public static void LoadScene(string scene)
    {
        if (!IsExist())
        {
            MainMenu.isBtnEnable = true;
            SceneManager.LoadScene(scene);
        }
        FadeTool.FadeOutIn(0.5f, 0, () => { panel.LoadAsync(scene); }, null);
        //panel.LoadAsync(scene);
    }

    public static void OnLoadComplete()
    {
        if (!IsExist())
        {
            return;
        }
        panel.gameObject.SetActive(false);
    }
}

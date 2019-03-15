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
            panel = GameObject.Find("Tools_UI").transform.Find("LoadingPanel").GetComponent<LoadingPanel>();
            return panel != null;
        }
        else
            return true;
    }

    public static void LoadScene(string scene)
    {
        if (!IsExist())
        {
            SceneManager.LoadScene(scene);
        }
        panel.LoadAsync(scene);
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

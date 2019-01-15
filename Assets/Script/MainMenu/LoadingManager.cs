using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour {
    
    void Awake()
    {
       // DontDestroyOnLoad(this.gameObject);
    }

    public void LoadScene()
    {
        gameObject.SetActive(true);
        StartCoroutine("LoadSceneRoutine");
    }

    public void OnLoadComplete()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator LoadSceneRoutine()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync("Levels/Floor0");
        op.allowSceneActivation = false;
        ArchLoader.instance.StartCache();

        while (!op.isDone)
        {
            yield return new WaitForSeconds(0.1f);
            if (op.progress >= 0.9f)
            {
                op.allowSceneActivation = true;
            }
        }
    }
}

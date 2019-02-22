using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour {
    public static LoadingManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(this.gameObject);
            //DontDestroyOnLoad(transform.parent);
        }
        else
        {
            UnityEngine.Debug.LogError("SingleTone Error : ArchLoader");
            Destroy(this);
        }
       LoadScene();
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
        ArchLoader.instance.StartCache();
        AsyncOperation op = SceneManager.LoadSceneAsync("Levels/Floor0");
        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            yield return new WaitForSeconds(0.1f);
            if (op.progress >= 0.9f)
            {
                //gameObject.SetActive(false);
                op.allowSceneActivation = true;
            }
        }
    }
}

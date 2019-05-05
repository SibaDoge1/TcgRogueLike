using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingPanel : MonoBehaviour {

    public void LoadAsync(string scene)
    {
        gameObject.SetActive(true);
        StartCoroutine(LoadSceneRoutine(scene));
    }

    public void OnLoadComplete()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator LoadSceneRoutine(string scene)
    {
        yield return null;
        ArchLoader.instance.StartCache();
        AsyncOperation op = SceneManager.LoadSceneAsync(scene);
        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            yield return new WaitForSeconds(0.1f);
            if (op.progress >= 0.9f)
            {
                //gameObject.SetActive(false);
                MainMenu.isBtnEnable = true;
                op.allowSceneActivation = true;
                yield return new WaitForSeconds(0.5f);
                OnLoadComplete();
            }
        }
    }
}

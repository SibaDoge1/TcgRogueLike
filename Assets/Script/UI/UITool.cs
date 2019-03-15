using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITool : MonoBehaviour {

    public static UITool instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            UnityEngine.Debug.LogWarning("SingleTone Error : " + this.name);
            Destroy(gameObject);
        }
    }
}

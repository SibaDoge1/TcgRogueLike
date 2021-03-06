﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour {

    public static Tool instance;

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

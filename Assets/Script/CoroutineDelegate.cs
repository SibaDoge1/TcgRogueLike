using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate IEnumerator Routine();
public class CoroutineDelegate : MonoBehaviour {
    public static CoroutineDelegate instance;
	void Awake()
    {
        instance = this;
    }
    public void StartRoutine(Routine routine)
    {
        StartCoroutine(routine());   
    }

}

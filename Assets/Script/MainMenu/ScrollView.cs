using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollView : MonoBehaviour {
    private Animator handle;
    [SerializeField]
    private const float Threshold = 1.0f;
    private Coroutine coroutine;

    void Awake()
    {
        handle = transform.Find("Scrollbar Vertical").Find("Sliding Area").Find("Handle").GetComponent<Animator>();
    }

	public void OnChanged()
    {
        if(coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine("ClickTimer");
        handle.SetBool("isUsing", true);
    }

    IEnumerator ClickTimer()
    {
        yield return new WaitForSeconds(Threshold);
        handle.SetBool("isUsing", false);
    }
}

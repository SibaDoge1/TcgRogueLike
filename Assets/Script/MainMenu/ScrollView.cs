using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollView : MonoBehaviour {
    private Animator handle;
    [SerializeField]
    private const float Threshold = 1.5f;

    void Awake()
    {
        handle = transform.Find("Scrollbar Vertical").Find("Sliding Area").Find("Handle").GetComponent<Animator>();
    }

	public void OnChanged()
    {
        Debug.Log("down!");
        StopCoroutine("ClickTimer");
        StartCoroutine("ClickTimer");
        handle.SetBool("isUsing", true);
    }

    IEnumerator ClickTimer()
    {
        yield return new WaitForSeconds(Threshold);
        handle.SetBool("isUsing", false);
    }
}

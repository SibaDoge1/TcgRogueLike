using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        if (FadeTool.Instance != null)
            FadeTool.Instance.SetImage(transform.GetComponent<Image>());
    }
}

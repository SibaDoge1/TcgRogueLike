using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// GameOver UI
/// </summary>
/// 
public class GameOverUI : MonoBehaviour {

    public void On()
    {
        transform.localPosition = new Vector3(0, 0, 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{

	public void EndTurnButton()
    {
        PlayerControl.instance.EndTurnButton();
    }
    public void StationFieldButton()
    {
        PlayerControl.instance.EndTurnButton();
    }
}

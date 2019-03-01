using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIAnimation {Buff,Attain,Report}
public class UIAnim : MonoBehaviour {

    Animator Buff;
    Animator AttainCard;
    Animator Report;

    private void Awake()
    {
        Buff = transform.Find("Buff").GetComponent<Animator>();
        AttainCard = transform.Find("AttainCard").GetComponent<Animator>();
        Report = transform.Find("Report").GetComponent<Animator>();
    }

    public void ShowAnim(UIAnimation ani)
    {
        switch(ani)
        {
            case UIAnimation.Buff:
                Buff.Play("PopUp", -1, 0);
                break;
            case UIAnimation.Attain:
                AttainCard.Play("PopUp", -1, 0);
                break;
            case UIAnimation.Report:
                Report.Play("PopUp", -1, 0);
                break;
        }
    }

}

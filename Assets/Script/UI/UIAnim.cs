using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UIAnimation {Buff,Attain,Report}
public class UIAnim : MonoBehaviour {

    Animator Buff;
    Animator AttainCard;
    Animator Report;
    Image buffImage;

    
    private void Awake()
    {
        Buff = transform.Find("Buff").GetComponent<Animator>();
        buffImage = Buff.GetComponent<Image>();

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
    public void ShowAnim(UIAnimation ani,BUFF buff)
    {
        buffImage.sprite = ArchLoader.instance.GetBuffImage(buff);
        Buff.Play("PopUp", -1, 0);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnim : MonoBehaviour {

    Animator Buff;
    Animator AttainCard;
    Animator Entry;
    Image buffImage;
    Image entryImage;
    Image attainImage;

    private void Awake()
    {
        Buff = transform.Find("Buff").GetComponent<Animator>();
        buffImage = Buff.GetComponent<Image>();

        Entry = transform.Find("EntryPopUp").GetComponent<Animator>();
        entryImage = Entry.GetComponent<Image>();

        AttainCard = transform.Find("AttainCard").GetComponent<Animator>();
        attainImage = AttainCard.GetComponent<Image>();
    }

    /// <summary>
    /// 이벤트룸 팝업
    /// </summary>
    /// <param name="success"></param>
    public void ShowAnim(bool success)
    {
        if(success)
        {
            attainImage.sprite = ArchLoader.instance.GetPopUpImage("success");
        }else
        {
            attainImage.sprite = ArchLoader.instance.GetPopUpImage("fail");
        }

        AttainCard.Play("PopUp", -1, 0);
    }
    public void ShowAnim(BUFF buff)
    {
        buffImage.sprite = ArchLoader.instance.GetPopUpImage(buff.ToString());
        Buff.Play("PopUp", -1, 0);
    }
    public void ShowAnim(string boss)
    {
        entryImage.sprite = ArchLoader.instance.GetPopUpImage(boss);
        Entry.Play("PopUp", -1, 0);
    }

}

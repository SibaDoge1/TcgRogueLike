using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum TextColorType{Red, Green}
public class EffectText : EffectObject
{
	private Text myText;

    Animator anim;
    protected Animator Anim
    {
        get
        {
            if(anim != null)
            {
                return anim;
            }
            else
            {
                return anim = GetComponent<Animator>();
            }
        }
    }

	public void Init(string text, TextColorType colorType_){
		colorType = colorType_;
        Transform child = transform.Find("Canvas").Find("Text");
        myText = child.GetComponent<Text> ();
		myText.text = text;
		ChangeColor ();
	}

	public TextColorType colorType;
	void ChangeColor(){
		switch(colorType){
		case TextColorType.Red:
			myText.color = new Color (1, 0, 0, 1);
			break;
		case TextColorType.Green:
			myText.color = new Color (0, 1, 0, 1);
			break;
		}
	}
}

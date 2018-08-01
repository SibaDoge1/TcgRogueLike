using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardRender : MonoBehaviour {

    private Image img_Ability;
    private Image img_Attribute;
    private Image img_Frame;
    private Image img_Graphic;

    public Image Img_Ability
    { get { return img_Ability; } set { img_Ability = value; } }
    public Image Img_Attribute
    { get { return img_Attribute; }set { img_Attribute = value; } }
    public Image Img_Frame
    { get { return img_Frame; } set { img_Frame = value; } }
    public Image Img_Graphic
    { get { return img_Graphic; }set { img_Graphic = value; } }

    // Use this for initialization
    void Awake () {
        img_Ability = transform.Find("Ability").GetComponent<Image>();
        img_Attribute = transform.Find("Attribute").GetComponent<Image>();
        img_Frame = transform.Find("Frame").GetComponent<Image>();
        img_Graphic = transform.Find("Graphic").GetComponent<Image>();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardRender : MonoBehaviour {

    private Image img_Attribute;
    private Image img_Frame;
    private Image img_Graphic;
    private Text name;

    public Text Name
    { get { return name; } }
    public Image Img_Attribute
    { get { return img_Attribute; }}
    public Image Img_Frame
    { get { return img_Frame; } }
    public Image Img_Graphic
    { get { return img_Graphic; }}

    // Use this for initialization
    void Awake () {
        name = transform.Find("name").GetComponent<Text>();
        img_Attribute = transform.Find("Attribute").GetComponent<Image>();
        img_Frame = transform.Find("Frame").GetComponent<Image>();
        img_Graphic = transform.Find("Graphic").GetComponent<Image>();
    }
}

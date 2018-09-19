using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardRender : MonoBehaviour {

    private Image img_Frame;
    private Image img_Graphic;
    private Image[] ranks;
    private Text thisName;
    static Color gray = new Color(133, 133, 133);
    public Text Name
    { get { return thisName; } }
    public Image Img_Frame
    { get { return img_Frame; } }
    public Image Img_Graphic
    { get { return img_Graphic; }}
    public Attribute attribute;

    // Use this for initialization
    void Awake ()
    {
        thisName = transform.Find("name").GetComponent<Text>();
        img_Frame = transform.Find("Frame").GetComponent<Image>();
        img_Graphic = transform.Find("Graphic").GetComponent<Image>();
        ranks = transform.Find("Rank").GetComponentsInChildren<Image>();
    }
    public void SetGraphic(Sprite sprite)
    {
        Img_Graphic.sprite = sprite;
    }
    public void SetAttribute(Attribute _attribute)
    {
        attribute = _attribute;
            switch (attribute)
            {
                case Attribute.APAS:
                    Img_Frame.sprite = Resources.Load<Sprite>("Card/card_basic_a");
                    break;
                case Attribute.PRITHVI:
                    Img_Frame.sprite = Resources.Load<Sprite>("Card/card_basic_p");
                    break;
                case Attribute.TEJAS:
                    Img_Frame.sprite = Resources.Load<Sprite>("Card/card_basic_t");
                    break;
                case Attribute.VAYU:
                    Img_Frame.sprite = Resources.Load<Sprite>("Card/card_basic_v");
                    break;
            }
    }
    
    public void SetRank(int rank)
    {
        for(int i=0; i<(5-rank);i++)
        {
            ranks[i].gameObject.SetActive(false);
        }
    }
    public void SetEnable(bool enable)
    {
        if(enable)
        {
            img_Frame.raycastTarget = true;
            img_Frame.color = Color.white;
        }else
        {
            img_Frame.raycastTarget = false;
            img_Frame.color = Color.black;
        }
    }
}

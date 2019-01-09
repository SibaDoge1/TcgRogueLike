using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 카드 렌더링 역활하는 클래스
/// </summary>
public class CardRender : MonoBehaviour {

    private Image img_Frame;
    private Image img_Graphic;
    private Image img_Enable;
    private Image img_Upgrade;
    private Image[] ranks;
    private Text thisName;
    static Color gray = new Color(133, 133, 133);
    public Text Name
    { get { return thisName; } }
    public Image Img_Frame
    { get { return img_Frame; } }
    public Image Img_Graphic
    { get { return img_Graphic; }}
    public CardType attribute;

    // Use this for initialization
    void Awake ()
    {
        thisName = transform.Find("name").GetComponent<Text>();
        img_Frame = transform.Find("Frame").GetComponent<Image>();
        img_Graphic = transform.Find("Graphic").GetComponent<Image>();
        ranks = transform.Find("Rank").GetComponentsInChildren<Image>();
        img_Enable = transform.Find("enable").GetComponent<Image>();
        img_Upgrade = transform.Find("upgrade").GetComponent<Image>();
    }
    public void SetGraphic(Sprite sprite)
    {
        Img_Graphic.sprite = sprite;
    }
    public void SetAttribute(CardType _attribute)
    {
        attribute = _attribute;
            switch (attribute)
            {
                case CardType.A:
                     Img_Frame.sprite = ArchLoader.instance.GetCardFrame("a");
                    break;
                case CardType.P:
                    Img_Frame.sprite = ArchLoader.instance.GetCardFrame("p");
                break;
                case CardType.T:
                    Img_Frame.sprite = ArchLoader.instance.GetCardFrame("t");
                break;
                case CardType.V:
                    Img_Frame.sprite = ArchLoader.instance.GetCardFrame("v");
                break;
            case CardType.NONE:
                    Img_Frame.sprite = ArchLoader.instance.GetCardFrame("a");//추후에 교체
                break;
            case CardType.AKASHA:
                    Img_Frame.sprite = ArchLoader.instance.GetCardFrame("a");//추후에 교체
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
    public void SetEnable(bool b)
    {
        img_Enable.enabled = !b;
    }
    public void SetUpgrade(bool b)
    {
        img_Upgrade.enabled = b;
    }
}

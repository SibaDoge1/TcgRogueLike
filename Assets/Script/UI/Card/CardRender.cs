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
    /// <summary>
    /// 카드데이터로 랜더링
    /// </summary>
    public void SetRender(Card data)
    {
        thisName.text = data.Name;
        SetRank(data.Cost);
        SetAttribute(data.Type);
        img_Graphic.sprite = ArchLoader.instance.GetCardSprite(data.SpritePath);
        img_Upgrade.enabled = data.IsUpgraded;
    }
    public void SetRender()
    {
        thisName.text = "???";
        SetRank(0);
        SetAttribute(CardType.N);
        img_Graphic.sprite = ArchLoader.instance.GetCardSprite("error");
        img_Upgrade.enabled = false;
    }

    private void SetAttribute(CardType attribute)
    {
            switch (attribute)
            {
                case CardType.A:
                    img_Frame.sprite = ArchLoader.instance.GetCardFrame("a");
                    break;
                case CardType.P:
                    img_Frame.sprite = ArchLoader.instance.GetCardFrame("p");
                break;
                case CardType.T:
                    img_Frame.sprite = ArchLoader.instance.GetCardFrame("t");
                break;
                case CardType.V:
                    img_Frame.sprite = ArchLoader.instance.GetCardFrame("v");
                break;
            case CardType.N:
                    img_Frame.sprite = ArchLoader.instance.GetCardFrame("n");
                break;
            case CardType.S:
                    img_Frame.sprite = ArchLoader.instance.GetCardFrame("s");
                break;
            }
    }
    
    private void SetRank(int rank)
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

}

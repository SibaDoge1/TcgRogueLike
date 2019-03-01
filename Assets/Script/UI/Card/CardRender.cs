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
    private Image[] ranks;
    private Text thisName;

    // Use this for initialization
    void Awake ()
    {
        thisName = transform.Find("name").GetComponent<Text>();
        img_Frame = transform.Find("Frame").GetComponent<Image>();
        img_Graphic = transform.Find("Graphic").GetComponent<Image>();
        ranks = transform.Find("Rank").GetComponentsInChildren<Image>();
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
    }
    public void UpdateRender(Card data)
    {
        SetRank(data.Cost);
    }

    public void SetRender()
    {
        thisName.text = "???";
        SetRank(0);
        SetAttribute(CardType.A);
        img_Graphic.sprite = ArchLoader.instance.GetCardSprite("error");
    }

    private void SetAttribute(CardType attribute)
    {
            switch (attribute)
            {
                case CardType.A:
                    img_Frame.sprite = ArchLoader.instance.GetCardFrame("n");
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
            case CardType.S:
                    img_Frame.sprite = ArchLoader.instance.GetCardFrame("a");
                break;
            }
    }
    
    private void SetRank(int rank)
    {
        for(int i=0; i<5 ; i++)
        {
            if(i<=rank-1)
            {
                ranks[i].gameObject.SetActive(true);
            }
            else
            {
                ranks[i].gameObject.SetActive(false);
            }
        }
    }
}

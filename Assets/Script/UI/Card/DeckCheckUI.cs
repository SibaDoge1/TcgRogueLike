using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DeckCheckUI : MonoBehaviour
{

    private RectTransform rect;
    private Vector3 offPos = new Vector3(3000, 3000, 0);
    RectTransform viewPort;
    RectTransform hold;
    private List<CheckCardObject> checkCards = new List<CheckCardObject>();
    private List<CheckCardObject> viewCards = new List<CheckCardObject>();
    private Text Count_All, Count_Unknown, Count_Special, Count_Normal;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        viewPort = transform.Find("DeckPanel").Find("Cards").Find("viewport").GetComponent<RectTransform>();
        hold = transform.Find("DeckPanel").Find("Cards").Find("hold").GetComponent<RectTransform>();

        Count_All = transform.Find("DeckPanel").Find("tap").Find("All").Find("Count").GetComponent<Text>();
        Count_Unknown = transform.Find("DeckPanel").Find("tap").Find("Unknown").Find("Count").GetComponent<Text>();
        Count_Special = transform.Find("DeckPanel").Find("tap").Find("Special").Find("Count").GetComponent<Text>();
        Count_Normal = transform.Find("DeckPanel").Find("tap").Find("Normal").Find("Count").GetComponent<Text>();

    }

    /// <summary>
    /// 켜기
    /// </summary>
    public void On()
    {
        GameManager.instance.IsInputOk = false;
        MakeCards();
        SetDeckCheckMode("ALL");
        rect.anchoredPosition = Vector3.zero;
        UIManager.instance.CardInfoPanel_Off();
    }

    /// <summary>
    /// 끄기
    /// </summary>
    public void Off()
    {
        DeleteCards();
        UIManager.instance.CardInfoPanel_Off();
        rect.anchoredPosition = offPos;
        GameManager.instance.IsInputOk = true;
    }

    /// <summary>
    /// 탭 모드 설정 , 카드 오브젝트 리스트에서 설정한 모드에 맞는 카트들만 보여줌
    /// </summary>
    public void SetDeckCheckMode(string mode)
    {
        viewCards.Clear();
        switch (mode)
        {
            case "ALL":
                for(int i=0; i<checkCards.Count;i++)
                {
                    checkCards[i].gameObject.SetActive(true);
                    checkCards[i].SetParent(viewPort);
                    viewCards.Add(checkCards[i]);
                }
                break;
            case "NORMAL":
                for(int i=0; i<checkCards.Count;i++)
                {
                    if(checkCards[i].IsAttain)
                    {
                        checkCards[i].gameObject.SetActive(false);
                        checkCards[i].SetParent(hold);
                    }
                    else
                    {

                        if (checkCards[i].GetCardData.Type == CardType.S)
                        {
                            checkCards[i].gameObject.SetActive(false);
                            checkCards[i].SetParent(hold);
                        }
                        else
                        {
                            checkCards[i].gameObject.SetActive(true);
                            checkCards[i].SetParent(viewPort);
                            viewCards.Add(checkCards[i]);
                        }

                    }

                }
                break;
            case "SPECIAL":

                for (int i = 0; i < checkCards.Count; i++)
                {
                    if (checkCards[i].IsAttain)
                    {
                        checkCards[i].gameObject.SetActive(false);
                        checkCards[i].SetParent(hold);
                    }
                    else
                    {

                        if (checkCards[i].GetCardData.Type == CardType.S)
                        {
                            checkCards[i].gameObject.SetActive(true);
                            checkCards[i].SetParent(viewPort);
                            viewCards.Add(checkCards[i]);
                        }
                        else
                        {
                            checkCards[i].gameObject.SetActive(false);
                            checkCards[i].SetParent(hold);
                        }

                    }
                }

                break;
            case "UNKNOWN":

                for (int i = 0; i < checkCards.Count; i++)
                {
                    if (checkCards[i].IsAttain)
                    {
                        checkCards[i].gameObject.SetActive(true);
                        checkCards[i].SetParent(viewPort);
                        viewCards.Add(checkCards[i]);
                    }
                    else
                    {
                        checkCards[i].gameObject.SetActive(false);
                        checkCards[i].SetParent(hold);
                    }
                }

                break;
        }
        SortCards();

    }

    /// <summary>
    /// 카드 만들기
    /// </summary>
    private void MakeCards()
    {
        int all, unknown, special=0, normal=0;

        List<Card> deck = PlayerData.Deck;
        for (int i=0; i<deck.Count; i++)//덱에있는 카드들 만들기
        {
            CheckCardObject card = PlayerData.Deck[i].InstantiateCheckCard();
            checkCards.Add(card);
            card.SetRenderKnown();

            if(card.GetCardData.Type == CardType.S)
            {
                special++;
            }else
            {
                normal++;
            }
        }

        List<Card> attain = PlayerData.AttainCards;
        for(int i=0; i<attain.Count;i++)
        {
            CheckCardObject card = PlayerData.AttainCards[i].InstantiateCheckCard();
            checkCards.Add(card);
            card.SetRenderUnknown();
        }

        all = deck.Count + attain.Count;
        unknown = attain.Count;

        Count_All.text = all + "보유";
        Count_Unknown.text = unknown + "보유";
        Count_Special.text = special + "보유";
        Count_Normal.text = normal + "보유";
    }

    /// <summary>
    /// 카드 지우기
    /// </summary>
    private void DeleteCards()
    {
        for(int i=checkCards.Count-1; i>=0 ;i--)
        {
            Destroy(checkCards[i].gameObject);
        }
        checkCards.Clear();
    }

    private void SortCards()
    {
        viewCards.Sort(delegate (CheckCardObject A, CheckCardObject B)
        {
            int aIndex;
            int bIndex;

            if(A.IsAttain)
            {
                aIndex = int.MaxValue;
            }else
            {
                aIndex = (int)A.GetCardData.Type * 1000 + A.GetCardData.Index * 100 + (int)A.GetCardData.CardFigure;
            }

            if(B.IsAttain)
            {
                bIndex = int.MaxValue;
            }else
            {
                bIndex = (int)B.GetCardData.Type * 1000 + B.GetCardData.Index * 100 + (int)B.GetCardData.CardFigure;
            }
            
            if (aIndex > bIndex)
                return 1;
            else if (aIndex < bIndex)
                return -1;
            return 0;
        });

        for (int i = 0; i < viewCards.Count; i++)
        {
            viewCards[i].Locate(i);
        }

    }

}

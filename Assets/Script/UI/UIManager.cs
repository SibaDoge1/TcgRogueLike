using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public delegate void EventTileCallBack();

/// <summary>
/// UI MANAGER 
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Text remainText;
    private void Awake()
    {
        instance = this;
        mapUI = transform.Find("MapUI").GetComponent<MapUI>();
        hpUI = transform.Find("StatusUI").Find("HpUI").GetComponent<HpUI>();
        akashaUI = transform.Find("StatusUI").Find("AkashaUI").GetComponent<AkashaUI>();
        gameOverUI = transform.Find("GameOverUI").GetComponent<GameOverUI>();
        gameWinUI = transform.Find("GameWinUI").GetComponent<GameWinUI>();

        cardInfoPanel = transform.Find("CardInfoPanel").GetComponent<CardInfoPanel>();
        deck = transform.Find("Deck").GetComponent<Deck>();
        deckEdit = transform.Find("Deck").Find("DeckEdit").GetComponent<DeckEditUI>();
        hand = transform.Find("HandCards").Find("HandOffSet").Find("Hand").GetComponent<Hand>();
        textUI = transform.Find("TextUI").GetComponent<TextUI>();
        error = transform.Find("ErrorPopUp").GetComponent<ErrorPopUpUI>();
    }

    TextUI textUI;
    AkashaUI akashaUI;
    HpUI hpUI;
    GameOverUI gameOverUI;
    DeckEditUI deckEdit;
    MapUI mapUI;
    Deck deck;
    Hand hand;
    ErrorPopUpUI error;
    GameWinUI gameWinUI;
    CardInfoPanel cardInfoPanel;

    #region Status
    public void HpUpdate(int currentHp_)
    {
        hpUI.HpUpdate(currentHp_);
    }
    public void AkashaUpdate(int current)
    {
        akashaUI.AkashaUpdate(current);
    }

    #endregion

    #region DeckEdit
    public void DeckEditUIOn()
    {
        if (!GameManager.instance.IsInputOk)
            return;

        deckEdit.On();
    }
    public void DeckEditUIOff()
    {
        deckEdit.Off();
        GameManager.instance.IsInputOk = true;
    }

    #endregion

    #region Map
    /// <summary>
    /// 맵 이미지에 텍스쳐 설정, 크기 설정
    /// </summary>
    public void SetMapTexture(Texture2D texture, Vector2Int size)
    {
        mapUI.SetMapTexture(texture, size);
    }
    /// <summary>
    /// 미니맵 움직이기 target 포지션으로 코루틴써서 옮김
    /// </summary>
    public void MoveMiniMap(Vector3 origin, Vector3 target)
    {
        mapUI.MoveMiniMap(origin, target);
    }


    public void OpenFullMap()
    {
        mapUI.OpenFullMap();
    }
    public void CloseFullMap()
    {
        mapUI.CloseFullMap();
    }
    #endregion

    public void ErrorPopUpOn(Debuffs de)
    {
        error.gameObject.SetActive(true);
    }
    public void ErrorPopUpOff()
    {
        error.gameObject.SetActive(false);
    }
    public Deck GetDeck()
    {
        return deck;
    }
    public Hand GetHand()
    {
        return hand;
    }
    public void GameOverUIOn()
    {
        gameOverUI.On();
    }
    public void GameWinUIOn()
    {
        gameWinUI.On();
    }

    public void ShowTextUI(string[] s, EventTileCallBack cb)
    {
        textUI.StartText(s, cb);
    }
    public void TextUIGoNext()
    {
        textUI.GoNext();
    }
    public void SetCardNumber(int count)
    {
        remainText.text = "" + count;
    }
    /// <summary>
    /// 해당 카드데이터 InfoPanel열기
    /// </summary>
    public void CardInfoPanel_On(Card data)
    {
        cardInfoPanel.gameObject.SetActive(true);
        cardInfoPanel.SetCard(data);
    }
    /// <summary>
    /// Unkwnon으로 열기
    /// </summary>
    public void CardInfoPanel_On()
    {
        cardInfoPanel.gameObject.SetActive(true);
        cardInfoPanel.SetUnknown();
    }
    public void CardInfoPanel_Off()
    {
        cardInfoPanel.gameObject.SetActive(false);
    }



}

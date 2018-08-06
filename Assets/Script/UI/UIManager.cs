using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// UI MANAGER 
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public static Transform canvas;
    private void Awake()
    {
        instance = this;
        canvas = transform;
        mapUI = transform.Find("MapUI").GetComponent<MapUI>();
        hpUI = transform.Find("HpUI").GetComponent<HpUI>();
        akashaUI = transform.Find("AkashaUI").GetComponent<AkashaUI>();
        deckUI = transform.Find("Deck").GetComponent<DeckEditUI>();
        gameOverUI = transform.Find("GameOverUI").GetComponent<GameOverUI>();
    }
    AkashaUI akashaUI;
    HpUI hpUI;
    GameOverUI gameOverUI;
    DeckEditUI deckUI;
    MapUI mapUI;

  

	public void HpUpdate(int currentHp_, int fullHp_)
    {
        hpUI.HpUpdate(currentHp_, fullHp_);
    }
    public void AkashaUpdate(int current, int full)
    {
        akashaUI.AkashaUpdate(current, full);
    }
    public void AkashaCountUpdate(int count)
    {
        akashaUI.AkashaCountUpdate(count);
    }

    public void GameOver()
    {
        gameOverUI.On();
        gameOverUI.SetText("You Died");
    }
    public void GameWin()
    {
        gameOverUI.On();
        gameOverUI.SetText("이겼닭! 오늘 저녁은 치킨이다!");
    }
    public void DeckEditUIOn(bool b = false)
    {
        deckUI.On(b);
        GameManager.instance.IsInputOk = false;
    }
    public void DeckEditUIOff()
    {
        deckUI.Off();
        GameManager.instance.IsInputOk = true;
    }
    /// <summary>
    /// 맵 이미지에 텍스쳐 설정, 크기 설정
    /// </summary>
    public void SetMapTexture(Texture2D texture,Vector2Int size)
    {
        mapUI.SetMapTexture(texture, size);
    }
    /// <summary>
    /// 미니맵 움직이기 target 포지션으로 코루틴써서 옮김
    /// </summary>
    public void MoveMiniMap(Vector3 origin,Vector3 target)
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
}

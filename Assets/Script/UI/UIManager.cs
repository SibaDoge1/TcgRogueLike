using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// UI MANAGER , TODO : 각 UI들 따로 클래스 만들어서 깔끔하게 구현
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    private void Awake()
    {
        instance = this;
        GameOverPanel = transform.Find("GameOver").GetComponent<Image>();
        message = GameOverPanel.transform.Find("Text").GetComponent<Text>();
        fullHp = transform.Find("HpUI").Find("Hp").GetComponent<Image>();
        currentHp = fullHp.transform.Find("current").GetComponent<Image>();
        hpText = fullHp.transform.Find("text").GetComponent<Text>();
        MapUI = transform.Find("MapUI");
        fullMapPanel = MapUI.Find("FullMapPanel").gameObject;
        miniMap = MapUI.Find("MiniMapPanel").Find("MiniMap").GetComponent<RawImage>();
        fullMap = fullMapPanel.transform.Find("FullMap").GetComponent<RawImage>();

        fullAkasha = transform.Find("AkashaUI").Find("Akasha").GetComponent<Image>();
        currentAkasha = fullAkasha.transform.Find("current").GetComponent<Image>();
        akashaText = fullAkasha.transform.Find("text").GetComponent<Text>();
        akashaCount = fullAkasha.transform.Find("akashaCount").GetComponent<Text>();

        GameOverPanel.gameObject.SetActive(false);
    }
    Image fullHp, currentHp, fullAkasha,currentAkasha;
    Text hpText,message,akashaText,akashaCount;
    Image GameOverPanel;

    Transform MapUI;
    GameObject fullMapPanel;
    RawImage miniMap, fullMap;

	public void HpUpdate(int currentHp_, int fullHp_)
    {     
		currentHp.fillAmount = currentHp_ / (float) fullHp_;
		hpText.text = currentHp_ + "/" + fullHp_;
    }
    public void AkashaUpdate(int current,int full)
    {
        currentAkasha.fillAmount = current / (float)full;
        akashaText.text = current + "/" + full;
    }
    public void AkashaCountUpdate(int count)
    {
        akashaCount.text = "X" + count;
    }
    public void GameOver()
    {
        GameOverPanel.gameObject.SetActive(true);
        message.text = "You Died";
    }
    public void GameWin()
    {
        GameOverPanel.gameObject.SetActive(true);
        message.text = "이겼닭! 오늘 저녁은 치킨이다!";
    }

    /// <summary>
    /// 맵 이미지에 텍스쳐 설정, 크기 설정
    /// </summary>
    public void SetMapTexture(Texture2D texture,Vector2Int size)
    {
        miniMap.texture = texture;
        miniMap.rectTransform.sizeDelta = size * 8;
        fullMap.texture = texture;
        float ratio = Mathf.Min(1200f / size.x, 720f / size.y);
        fullMap.rectTransform.sizeDelta = new Vector2(size.x * ratio, size.y * ratio);
    }
    /// <summary>
    /// 미니맵 움직이기 target 포지션으로 코루틴써서 옮김
    /// </summary>
    public void MoveMiniMap(Vector3 origin,Vector3 target)
    {
        StartCoroutine(MoveMiniMapRoutine(origin,target));
    }
    IEnumerator MoveMiniMapRoutine(Vector3 origin, Vector3 target)
    {
            
        float _time = 0;
        while (_time < 1f)
        {
            miniMap.transform.localPosition = Vector3.Lerp(-origin, -target, _time);
            _time += Time.deltaTime;
            yield return null;
        }
    }

    public void OpenFullMap()
    {
        fullMapPanel.SetActive(true);
    }
    public void CloseFullMap()
    {
        fullMapPanel.SetActive(false);
    }
}

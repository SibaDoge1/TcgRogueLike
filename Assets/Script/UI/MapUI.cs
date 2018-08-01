using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapUI : MonoBehaviour {

    Vector3 offPos = new Vector3(0, 2000, 0);
    RectTransform fullMapPanel;
    RawImage miniMap, fullMap;
    // Use this for initialization
    void Awake () {
        fullMapPanel = transform.Find("FullMapPanel").GetComponent<RectTransform>();
        miniMap = transform.Find("MiniMapPanel").Find("MiniMap").GetComponent<RawImage>();
        fullMap = fullMapPanel.transform.Find("FullMap").GetComponent<RawImage>();
    }
    /// <summary>
    /// 맵 이미지에 텍스쳐 설정, 크기 설정
    /// </summary>
    public void SetMapTexture(Texture2D texture, Vector2Int size)
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
    public void MoveMiniMap(Vector3 origin, Vector3 target)
    {
        StartCoroutine(MoveMiniMapRoutine(origin, target));
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
        fullMapPanel.anchoredPosition = Vector2.zero;
    }
    public void CloseFullMap()
    {
        fullMapPanel.anchoredPosition = offPos;
    }

}

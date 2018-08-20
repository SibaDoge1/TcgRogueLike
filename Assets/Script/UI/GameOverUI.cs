using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameOverUI : MonoBehaviour {

    RectTransform rect;
    Vector3 offPos = new Vector3(0, 2000, 0);
    Sprite[] sprites;
    Image image;
    Animator animator;
    // Use this for initialization
    void Awake () {
        image = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        animator = GetComponent<Animator>();
	}
    public void On()
    {
        StartCoroutine(Waiter());
    }
    public void Off()
    {
        rect.anchoredPosition = offPos;
    }
    private void GameOverCallback()
    {
        GameManager.instance.ReGame();
    }
    IEnumerator Waiter()
    {
        yield return new WaitForSeconds(1f);
        rect.anchoredPosition = Vector3.zero;
        animator.Play("GameOver");
    }
}

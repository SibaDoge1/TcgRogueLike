using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {
    public List<Sprite> pages;
    private Image image;
    private int currentPage;

	// Use this for initialization
	void Awake () {
        image = transform.Find("Image").GetComponent<Image>();
        currentPage = 0;

    }

    public void Next()
    {
        MainMenu.ButtonDown();
        if (currentPage < pages.Count-1)
            image.sprite = pages[++currentPage];
        else
            Off();
    }

    public void Back()
    {
        MainMenu.ButtonDown();
        if (currentPage > 0)
            image.sprite = pages[--currentPage];
    }
    public void Skip()
    {
        MainMenu.ButtonDown();
        Off();
    }

    public void On()
    {
        gameObject.SetActive(true);
    }
    public void Off()
    {
        currentPage = 0;
        gameObject.SetActive(false);
    }
}

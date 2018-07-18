using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    private void Awake()
    {
        instance = this;
        tempPanel = transform.Find("GameOver").GetComponent<Image>();
        message = tempPanel.transform.Find("Text").GetComponent<Text>();
        fullHp = transform.Find("StatusUI").Find("Hp").GetComponent<Image>();
        currentHp = fullHp.transform.Find("current").GetComponent<Image>();
        hpText = fullHp.transform.Find("hpText").GetComponent<Text>();

        tempPanel.gameObject.SetActive(false);
    }
    Image fullHp, currentHp;
    Text hpText,message;
    Image tempPanel;

	public void HpUpdate(int currentHp_, int fullHp_)
    {     
		currentHp.fillAmount = currentHp_ / (float) fullHp_;
		hpText.text = currentHp_ + "/" + fullHp_;
    }
    public void GameOver()
    {
        tempPanel.gameObject.SetActive(true);
        message.text = "You Died";
    }
    public void GameWin()
    {
        tempPanel.gameObject.SetActive(true);
        message.text = "이겼닭! 오늘 저녁은 치킨이다!";
    }
   
}

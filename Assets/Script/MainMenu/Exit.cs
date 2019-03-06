using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour {

    public void on()
    {
        gameObject.SetActive(true);
    }

    public void off()
    {
        MainMenu.ButtonDown();
        gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        MainMenu.ButtonDown();
        SaveManager.SaveAll();
        Application.Quit();
    }
}

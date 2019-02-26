using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooglePlayUI : MonoBehaviour {

	// Use this for initialization
	public void OnLogIn () {
        GooglePlayManager.LogIn();
    }
    public void OnLogOut()
    {
        GooglePlayManager.LogOut();
    }

    public void OnLeaderboard()
    {
        GooglePlayManager.ShowLeaderboardUI();
    }
    public void OnAchive()
    {
        GooglePlayManager.ShowAchievementUI();
    }
    public void OnSave()
    {
        GooglePlayManager.SaveToCloud();
    }
    public void OnLoad()
    {
        GooglePlayManager.LoadFromCloud();
    }
    public void OnInit()
    {
        GooglePlayManager.Init();
    }
    public void OnSelect()
    {
        GooglePlayManager.ShowSelectUI();
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooglePlayUI : MonoBehaviour {

	// Use this for initialization
	public void OnLogIn () {
        GooglePlayManager.LogIn(null, null);
    }
    public void OnLogOut()
    {
        GooglePlayManager.LogOut();
    }

    public void OnLeaderboard()
    {
        GooglePlayManager.ShowLeaderboardUI();
    }
    public void OnLeaderboardScore()
    {
        GooglePlayManager.ReportScore(GPGSIds.leaderboard_achivementscore, 5);
    }
    public void OnAchiveClear()
    {
        GooglePlayManager.UnlockAchievement(GPGSIds.achievement_end,100);
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

    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}

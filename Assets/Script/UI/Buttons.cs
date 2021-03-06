﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// UI Button들 Function 여기다가 구현
/// </summary>
public class Buttons : MonoBehaviour
{
    public void EndTurnButton()
    {
        SoundDelegate.instance.PlayMono(MonoSound.BUTTONNORMAL);
        PlayerControl.instance.EndTurnButton();
    }
    public void UpButton()
    {
        SoundDelegate.instance.PlayMono(MonoSound.BUTTONNORMAL);
        if (GameManager.instance.CurrentTurn == Turn.PLAYER && GameManager.instance.IsInputOk)
        {
            if (PlayerControl.instance.IsDirCardSelected)
            {
                PlayerControl.instance.DoDirCard(Direction.NORTH);
            }else
            {
                PlayerControl.instance.MoveToDirection(Direction.NORTH);
            }
        }
    }
    public void LeftButton()
    {
        SoundDelegate.instance.PlayMono(MonoSound.BUTTONNORMAL);
        if (GameManager.instance.CurrentTurn == Turn.PLAYER && GameManager.instance.IsInputOk)
        {
            if (PlayerControl.instance.IsDirCardSelected)
            {
                PlayerControl.instance.DoDirCard(Direction.WEST);
            }
            else
            {
                PlayerControl.instance.MoveToDirection(Direction.WEST);
            }
        }
    }
    public void RightButton()
    {
        SoundDelegate.instance.PlayMono(MonoSound.BUTTONNORMAL);
        if (GameManager.instance.CurrentTurn == Turn.PLAYER && GameManager.instance.IsInputOk)
        {
            if (PlayerControl.instance.IsDirCardSelected)
            {
                PlayerControl.instance.DoDirCard(Direction.EAST);
            }
            else
            {
                PlayerControl.instance.MoveToDirection(Direction.EAST);
            }
        }
    }
    public void DownButton()
    {
        SoundDelegate.instance.PlayMono(MonoSound.BUTTONNORMAL);
        if (GameManager.instance.CurrentTurn == Turn.PLAYER && GameManager.instance.IsInputOk)
        {
            if (PlayerControl.instance.IsDirCardSelected)
            {
                PlayerControl.instance.DoDirCard(Direction.SOUTH);
            }
            else
            {
                PlayerControl.instance.MoveToDirection(Direction.SOUTH);
            }
        }
    }
    public void TextPanelOff()
    {
        UIManager.instance.TextUIGoNext();
    }
}

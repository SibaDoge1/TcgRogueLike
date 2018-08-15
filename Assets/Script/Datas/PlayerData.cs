﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  플레이어 데이터는 이곳에 저장한다 (Hp,Atk,Def제외) .
/// </summary>
public static class PlayerData
{
    static List<CardData> deck = new List<CardData>();
    public static List<CardData> Deck
    {
        get { return deck; }
        set { deck = value; }
    }
    static List<CardData> attainCards = new List<CardData>();
    public static List<CardData> AttainCards
    {
        get { return attainCards; }
        set { attainCards = value; }
    }

    static int akashaGage;
    public static int AkashaGage
    {
        get { return akashaGage; }
        set
        {
            if(value>=10)
            {
                akashaGage = value - 10;
                AkashaCount++;            
            }else if(value<0)
            {
                akashaGage = 0;
            }
            else
            {
                akashaGage = value;
            }
            UIManager.instance.AkashaUpdate(AkashaGage,10);
        }
    }
    static int akashaCount;
    public static int AkashaCount
    {
        get { return akashaCount; }
        set
        {           
            akashaCount = value;
            UIManager.instance.AkashaCountUpdate(AkashaCount);
        }
    }
    public static void Clear()
    {
        deck.Clear();
        akashaGage = 0;
        akashaCount = 0;
    }
}
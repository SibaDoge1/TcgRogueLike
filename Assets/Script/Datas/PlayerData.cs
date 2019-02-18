using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 데이터는 이곳에 저장한다 (Hp,Atk,Def는 Player Class에서).
/// </summary>
public static class PlayerData
{
    static List<Card> deck = new List<Card>();
    public static List<Card> Deck
    {
        get { return deck; }
        set{deck = value;}
    }
    static List<Card> attainCards = new List<Card>();
    public static List<Card> AttainCards
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
                akashaGage = 10;
            }else if(value<0)
            {
                akashaGage = 0;
            }
            else
            {
                if(PlayerControl.playerBuff.IsAkashaAble)
                {
                    akashaGage = value;
                }
                else
                {
                    if(value<=akashaGage)
                    {
                        akashaGage = value;
                    }
                }
            }
            UIManager.instance.AkashaUpdate(AkashaGage);
        }
    }

    public static void AttackedTarget()
    {
        if(!isAttacked)
        {
            AkashaGage++;
            isAttacked = true;
        }
    }

    private static bool isAttacked = false;
    public static void PlayerTurnStart()
    {
        isAttacked = false;
    }

    /// <summary>
    // 플레이어 데이터 초기화
    /// </summary>
    public static void Clear()
    {
        deck.Clear();
        attainCards.Clear();
        akashaGage = 0;
    }

}

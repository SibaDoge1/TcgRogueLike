using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Datas that will be used for save & load
/// </summary>
public class SaveData {

    //Seed
    int seed;

    //Map
    int floor;
    List<int> clearRooms;
    int currentRoom;
    Vector2Int playerPos;

    //PlayerData
    public static List<CardData> deck;
    public static List<CardData> hand;
    public static int hp;
    public static int atk;
    public static int def;


    /// <summary>
    /// Room 입장시 실행
    /// </summary>
    public void Save()
    {
       
    }
}
 
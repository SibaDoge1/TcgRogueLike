using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 카드 데이터,몬스터 데이터,정보 데이터,카드풀 데이터
/// </summary>
public static class Database
{
  
     static Dictionary<int, CardData> cardDatas;
     static Dictionary<int, CardPoolData> cardPoolDatas;
     static Dictionary<int, MonsterData> monsterDatas;

    public static void ReadDatas()
    {
        cardDatas = CsvParser.ReadCardData("Data/CardData/CardData");
        cardPoolDatas = CsvParser.ReadCardPoolData("Data/CardPoolData/CardPoolData");
        monsterDatas = CsvParser.ReadMonsterData("Data/MonsterData/MonsterData");
    }
    public static CardData GetCardData(int i)
    {
        return cardDatas[i];
    }
    public static CardPoolData GetCardPool(int i)
    { 
        return cardPoolDatas[i];
    }
    public static MonsterData GetMonsterData(int i)
    {
        return monsterDatas[i];
    }
    
}
public class CardData
{
    public readonly byte num;
    public readonly string name;
    public readonly byte cost;
    public readonly byte attribute;
    //0->Apas , 1-> Prithivi, 2-> Tejas, 3->Vayu
    public readonly int val1;
    public readonly int val2;
    public readonly int val3;

    public readonly string _info;
    public readonly string spritePath;
    public readonly string className;

    public CardData(string[] data)
    {
        num = byte.Parse(data[0]);
        name = data[1];
        cost = byte.Parse(data[2]);
        attribute = byte.Parse(data[3]);

        val1 = int.Parse(data[4]);
        val2 = int.Parse(data[5]);
        val3 = int.Parse(data[6]);

        _info = data[7];
        spritePath = data[8];

        className = data[9].Substring(0,data[9].Length-1);
    }
}
public class CardPoolData
{
    public readonly byte num;
    public readonly int value;
    public readonly List<int> cardPool = new List<int>();

    public CardPoolData(string[] data)
    {
        num = byte.Parse(data[0]);
        value = int.Parse(data[1]);
        string[] d = data[2].Split('/');
        for(int i=0; i<d.Length;i++)
        {
            cardPool.Add(int.Parse(d[i]));
        }
    }

    /// <summary>
    /// 카드풀 중에서 랜덤으로 카드를 뽑아서 리턴
    /// </summary>
    public Card GetRandomCard()
    {
        return Card.GetCardByNum(cardPool[Random.Range(0, cardPool.Count)]);
    }
}

public class MonsterData
{
    public readonly short num;
    public readonly int hp;
    public readonly int atk;
    public readonly int def;
    public readonly short rank;
    public readonly bool vision;
    public readonly byte visionDistance;

    public MonsterData(string[] data)
    {
        num = short.Parse(data[0]);
        hp = int.Parse(data[1]);
        atk = int.Parse(data[2]);
        def = int.Parse(data[3]);
        rank = short.Parse(data[4]);
        vision = bool.Parse(data[5]);
        visionDistance = byte.Parse(data[6]);
    }
}

public class InfoData
{
    public readonly byte num;
    public readonly string title;
    public readonly string info;
    public readonly string spritePath;

    public InfoData(string[] data)
    {
        num = byte.Parse(data[0]);
        title = data[1];
        info = data[2];
        spritePath = data[3];
    }
}


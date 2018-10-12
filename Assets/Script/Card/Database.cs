using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 카드 데이터,몬스터 데이터,정보 데이터,카드풀 데이터
/// </summary>
public static class Database
{
    public const string cardResourcePath = "Card/Graphic/";
	public const string cardObjectPath = "Card/CardBase";
    public const string editCardObjectPath = "Card/EditCard";
  
     static Dictionary<int, CardData> cardDatas;
     static Dictionary<int, CardPoolData> cardPoolDatas;
     static Dictionary<int, MonsterData> monsterDatas;

    public static void ReadDatas()
    {
        cardDatas = CsvParser.ReadCardData("Data/CardData/CardData");
        cardPoolDatas = CsvParser.ReadCardPoolData("Data/CardPoolData/CardPoolData");
        //monsterDatas = CsvParser.ReadMonsterData();
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

    public readonly int val1;
    public readonly int val2;
    public readonly int val3;

    public readonly string info;
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

        info = data[7];
        spritePath = data[8];
        className = data[9];
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
}

public class MonsterData
{
    public readonly byte num;
    public readonly int hp;
    public readonly int atk;
    public readonly int def;
    public readonly byte rank;
    public readonly bool vision;
    public readonly byte visionDistance;
    public readonly string spritePath;

    public MonsterData(string[] data)
    {
        num = byte.Parse(data[0]);
        hp = int.Parse(data[1]);
        atk = int.Parse(data[2]);
        def = int.Parse(data[3]);
        rank = byte.Parse(data[4]);
        vision = bool.Parse(data[5]);
        visionDistance = byte.Parse(data[6]);
        spritePath = data[7];
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
/*
ToDo : 타일 , 벽 블럭 -> prefab 여러개 만들어서 쓰지말고
어차피 sprite만 다르니까 생성할때 스프라이트 path만 번호로 지정하도록 변경
OffTile -> Inspector에서 SpritePath 지정 아니면 그냥 Prefab에 박자
*/ 


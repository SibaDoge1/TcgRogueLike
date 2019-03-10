using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 카드 데이터,몬스터 데이터,정보 데이터,카드풀 데이터
/// </summary>
public static class Database
{

     public static Dictionary<int, CardData> cardDatas { get; private set; }
     public static Dictionary<string, CardPoolData> cardPoolDatas { get; private set; }
     public static Dictionary<int, MonsterData> monsterDatas { get; private set; }
     public static Dictionary<int, DiaryData> diaryDatas{ get; private set; }
     public static Dictionary<int, AchiveData> achiveDatas { get; private set; }
     private static bool isParsed;

    public static void ReadDatas()
    {
        if (isParsed) return;
        cardDatas = CsvParser.ReadCardData("Data/CardData/CardData");
        cardPoolDatas = CsvParser.ReadCardPoolData("Data/CardPoolData/CardPoolData");
        monsterDatas = CsvParser.ReadMonsterData("Data/MonsterData/MonsterData");
        diaryDatas = CsvParser.ReadDiaryData("Data/DiaryData");
        achiveDatas = CsvParser.ReadAchiveData("Data/AchiveData");
        isParsed = true;
    }
    public static CardData GetCardData(int i)
    {
            return cardDatas[i];
    }
    public static bool CheckCardDataKey(int i)
    {
        return cardDatas.ContainsKey(i);
    }
    public static CardPoolData GetCardPool(string name)
    { 
        if(cardPoolDatas.ContainsKey(name))
        {
            return cardPoolDatas[name];
        }
        else
        {
            Debug.Log("The Room " + name + " isn't exists");
            return cardPoolDatas["default"];
        }
    }
    public static MonsterData GetMonsterData(int i)
    {
        return monsterDatas[i];
    }
    /* 머지했더니 이거 컴파일오류 남, 수정필요
    public static CardPoolData GetCardPoolByValue(int value)
    {
        int offset = int.MaxValue;
        int index = 0;
        for (int i=0; i<cardPoolDatas.Count;i++)
        {
            int temp = Mathf.Abs(value - cardPoolDatas[i].value);
            if(temp<offset)
            {
                offset = temp;
                index = i;
            }
        }
        return cardPoolDatas[index];
    }
    */
    public static AchiveData GetAchiveDataByDiary(int num)
    {
        foreach (KeyValuePair<int, AchiveData> pair in Database.achiveDatas)
        {
            if (int.Parse(achiveDatas[pair.Key].reward) == num)
            {
                return achiveDatas[pair.Key];
            }
        }
        return null;
    }


}
public class CardData
{
    public readonly byte index;
    public readonly string name;
    public readonly byte cost;
    public readonly byte attribute;
    public readonly int val1;
    public readonly int val2;
    public readonly int val3;

    public readonly string _info;
    public readonly string spritePath;
    public readonly string className;
    public readonly string range;
    public readonly CardEffect effect;
    public readonly SoundEffect sound;

    public CardData(string[] data)
    {
        index = byte.Parse(data[0]);
        name = data[1];
        cost = byte.Parse(data[2]);
        attribute = byte.Parse(data[3]);

        val1 = int.Parse(data[4]);
        val2 = int.Parse(data[5]);
        val3 = int.Parse(data[6]);

        _info = data[7];
        spritePath = data[8];
        className = data[9];
        effect = (CardEffect)System.Enum.Parse(typeof(CardEffect), data[10]);
        sound = (SoundEffect)System.Enum.Parse(typeof(SoundEffect), data[11]);
        range = data[12].Replace("\r", "");
    }
}
public class CardPoolData
{
    public readonly string name;
    public readonly List<int> cardPool = new List<int>();

    public CardPoolData(string[] data)
    {
        name = data[0];
        string[] d = data[1].Split('/');
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
    public readonly bool elite;

    public MonsterData(string[] data)
    {
        num = short.Parse(data[0]);
        hp = int.Parse(data[1]);
        atk = int.Parse(data[2]);
        def = int.Parse(data[3]);
        rank = short.Parse(data[4]);
        vision = bool.Parse(data[5]);
        visionDistance = byte.Parse(data[6]);
        elite = bool.Parse(data[7]);
    }
}

public class DiaryData
{
    public readonly byte num;
    public readonly Category category;
    public readonly string title;
    public readonly string info;
    public readonly string spritePath;

    public DiaryData(string[] data)
    {
        num = byte.Parse(data[0]);
        switch (data[1])
        {
            case "이레귤러": category = Category.irregulars; break;
            case "R.A.칩": category = Category.raChips; break;
            case "기록": category = Category.records; break;
            case "인물": category = Category.humans; break;
            case "etc": category = Category.etc; break;
            default: Debug.Log("다이어리 카테고리 형식이 맞지 않습니다!" + data[0] + data[1]); break;
        }
        title = data[2];
        info = data[3].Replace("$", "\n");
        //Debug.Log(data[3]);
        spritePath = data[4].Replace("\r","");
    }
}

public class AchiveData
{
    public readonly byte num;
    public readonly string info;
    public readonly string type;
    public readonly string condition;
    public readonly string reward;
    public readonly string cardReward;

    public AchiveData(string[] data)
    {
        num = byte.Parse(data[0]);
        info = data[1];
        type = data[2];
        condition = data[3];
        reward = data[4];
        cardReward = data[5];
    }
}

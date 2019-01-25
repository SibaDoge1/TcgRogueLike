using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

/// <summary>
/// CSV 데이터 읽어오는 클래스 입니다.
/// </summary>


public static class CsvParser 
{
    private static string[] ReadString(string path)
    {
        TextAsset csvTextAsset = Resources.Load(path) as TextAsset;
        return csvTextAsset.text.Split('\n');
    }
    private static string[] ReadString(UnityEngine.Object ob)
    {
        TextAsset csvTextAsset = ob as TextAsset;
        return csvTextAsset.text.Split('\n');
    }


    #region RoomData
    /// <summary>
    /// 방 한개 데이터 가져오기
    /// </summary>
    public static string[,] ReadRoom(int floor,RoomType rt,string name)
    {
        string[] line = ReadString("Data/RoomData/Floor"+floor+"/"+rt.ToString()+"/"+name);
        int row, collum;

        row = Int32.Parse(line[0].Split(',')[0]); collum = Int32.Parse(line[0].Split(',')[1]);
        string[,] roomData = new string[row, collum];

        for (int loop = 0; loop < collum; loop++)
        {

            string[] split = line[loop+1].Split(',');

            for (int loop2 = 0; loop2 < row; loop2++)
            {
                roomData[loop2, loop] = split[loop2];
            }
        }        
        return roomData;
    }

    /// <summary>
    /// 해당타입의 방 데이터를 모두 받아옴
    /// </summary>
    public static List<string[,]> ReadRoom (int floor, RoomType rt)
    {
        UnityEngine.Object[] Data = Resources.LoadAll("Data/RoomData/Floor" + floor + "/"+rt.ToString());
        List<string[,]> dataList = new List<string[,]>();
        for (int i = 0; i < Data.Length; i++)
        {
            string[] line = ReadString(Data[i]);
            int row, collum;

            row = Int32.Parse(line[0].Split(',')[0]); collum = Int32.Parse(line[0].Split(',')[1]);
            string[,] roomData = new string[row, collum];

            for (int loop = 0; loop < collum; loop++)
            {
                string[] split = line[loop + 1].Split(',');

                for (int loop2 = 0; loop2 < row; loop2++)
                {
                    roomData[loop2, loop] = split[loop2];
                }
            }
            dataList.Add(roomData);
        }
               
        return dataList;
    }
    #endregion

    #region CardData
    /// <summary>
    /// 카드 데이터 가져오기
    /// </summary>
    public static Dictionary<int,CardData> ReadCardData(string path)
    {
        string[] dataString = ReadString(path);
        Dictionary<int, CardData> cardDatas = new Dictionary<int, CardData>();
        int num = 1;
        string[] split;


        while (num < dataString.Length )
        {
            split = dataString[num].Split(',');
            if(split[0].Length == 0)
            {
                break;
            }
            cardDatas.Add(int.Parse(split[0]), new CardData(split));
            num++;
        }
        return cardDatas;
    }
    /// <summary>
    /// 카드풀 데이터
    /// </summary>
    public static Dictionary<int, CardPoolData> ReadCardPoolData(string path)
    {
        string[] dataString = ReadString(path);
        Dictionary<int, CardPoolData> datas = new Dictionary<int, CardPoolData>();
        int num = 1;
        string[] split;


        while (num < dataString.Length)
        {
            split = dataString[num].Split(',');
            if (split[0].Length == 0)
            {
                break;
            }
            datas.Add(int.Parse(split[0]), new CardPoolData(split));
            num++;
        }
        return datas;
    }
    #endregion

    #region MonsterData
    /// <summary>
    /// 몬스터 데이터
    /// </summary>
    public static Dictionary<int, MonsterData> ReadMonsterData(string path)
    {
        string[] dataString = ReadString(path);
        Dictionary<int, MonsterData>datas = new Dictionary<int, MonsterData>();
        int num = 1;
        string[] split;

        while (num < dataString.Length)
        {
            split = dataString[num].Split(',');
            if (split[0].Length == 0)
            {
                break;
            }
            datas.Add(int.Parse(split[0]), new MonsterData(split));
            num++;
        }
        return datas;
    }
    #endregion
    #region DiaryData
    public static Dictionary<int, DiaryData> ReadDiaryData(string path)
    {
        string[] dataString = ReadString(path);
        Dictionary<int, DiaryData> datas = new Dictionary<int, DiaryData>();
        int num = 1;
        string[] split;

        while (num < dataString.Length)
        {
            split = dataString[num].Split(',');
            if (split[0].Length == 0)
            {
                break;
            }
            datas.Add(int.Parse(split[0]), new DiaryData(split));
            num++;
        }
        return datas;
    }
    #endregion
    #region AchiveData
    public static Dictionary<int, AchiveData> ReadAchiveData(string path)
    {
        string[] dataString = ReadString(path);
        Dictionary<int, AchiveData> datas = new Dictionary<int, AchiveData>();
        int num = 1;
        string[] split;

        while (num < dataString.Length)
        {
            split = dataString[num].Split(',');
            if (split[0].Length == 0)
            {
                break;
            }
            datas.Add(int.Parse(split[0]), new AchiveData(split));
            num++;
        }
        return datas;
    }
    #endregion
}


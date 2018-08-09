using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

/// <summary>
/// 맵 데이터 읽어오는 클래스 입니다.
/// </summary>


public static class CsvParser 
{  
    private static string[] ReadString(string path)
    {
        TextAsset csvTextAsset = Resources.Load(path) as TextAsset;
        return csvTextAsset.text.Split('\n');
    }

    /// <summary>
    /// 방 한개 데이터 가져오기
    /// </summary>
    public static string[,] ReadRoom(int floor,RoomType rt,string name)
    {
        string[] line = ReadString("RoomData/Floor"+floor+"/"+rt.ToString()+"/"+name);
        int row, collum;

        row = Int32.Parse(line[0].Split(',')[0]); collum = Int32.Parse(line[0].Split(',')[1]);
        string[,] roomData = new string[row, collum];

        for (int loop = 0; loop < row; loop++)
        {

            string[] split = line[loop+1].Split(',');


            for (int loop2 = 0; loop2 < collum; loop2++)
            {
                roomData[loop, loop2] = split[loop2];
            }
        }

        
      
        return roomData;
    }

    public static string [] ReadString(UnityEngine.Object ob)
    {
        TextAsset csvTextAsset = ob as TextAsset;
        return csvTextAsset.text.Split('\n');
    }
    public static List<string[,]> ReadRoom (int floor, RoomType rt)
    {
        UnityEngine.Object[] Data = Resources.LoadAll("RoomData/Floor" + floor + "/"+rt.ToString());
        List<string[,]> dataList = new List<string[,]>();
        for (int i = 0; i < Data.Length; i++)
        {
            string[] line = ReadString(Data[i]);
            int row, collum;

            row = Int32.Parse(line[0].Split(',')[0]); collum = Int32.Parse(line[0].Split(',')[1]);
            string[,] roomData = new string[row, collum];

            for (int loop = 0; loop < row; loop++)
            {
                string[] split = line[loop + 1].Split(',');


                for (int loop2 = 0; loop2 < collum; loop2++)
                {
                    roomData[loop, loop2] = split[loop2];
                }
            }
            dataList.Add(roomData);
        }
               
        return dataList;
    }
}


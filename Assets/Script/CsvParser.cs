using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// CSV읽어오는 클래스입니다
/// 출처 : 최원재 선배님 
/// </summary>


public class CsvParser : MonoBehaviour
{
   

    private static string[] GetLines(string fileName)
    {
        TextAsset csvTextAsset = (TextAsset)Resources.Load(fileName) as TextAsset;
        return csvTextAsset.text.Split('\n');
    }

    public static IEnumerator ReadMap(string fileName)
       {

        string[] line = GetLines(fileName);
        int row, collum;

        row = line.Length-1; collum = line[0].Split(',').Length;
        yield return row; yield return collum;
        List<string> data = new List<string>(); 

           for (int loop = 0; loop <=row-1; loop++)
           {
             if(line[loop].Length<=1)
               {
                continue;
               }

               string[] split = line[loop].Split(',');
               if(split[0].Length<=1)
                {
                continue;
                }

               for (int loop2 = 0; loop2 <= collum -1; loop2++)
               {
                data.Add(split[loop2]);
               }
           }

          yield return data;
       }
       

}


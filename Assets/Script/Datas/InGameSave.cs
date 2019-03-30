using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

[System.Serializable]
public class InGameSaveData
{
    public bool isSaved = false;
    public string version;

    public int floor;
    public int hp;
    public List<CardSaveData> deckCards;
    public List<int> attainCards;
    public EndingConditions ending;
    public int seed;
    public DateTimeOffset savedTime;
}

/// <summary>
/// 범위도 똫같이 로드하기위해 필요함
/// </summary>
[System.Serializable]
public class CardSaveData
{
    public int index;
    public CardType type;
    public Figure figure;
    public CardSaveData(int _index,CardType _type, Figure _figure)
    {
        index = _index;
        type = _type;
        figure = _figure;
    }
}
[System.Serializable]
public class EndingConditions
{
    private bool isEdit = false;
    public bool IsEdited
    {
        get { return isEdit; }
    }
    public void CardExchanged()
    {
        isEdit = true;
    }
    private bool xynus = false;
    public bool Xynus
    {
        get { return xynus; }
    }
    public void KillXynus()
    {
        xynus = true;
    }
    private bool pablus = false;
    public bool Pablus
    {
        get { return pablus; }
    }
    public void KillPablus()
    {
        pablus = true;
    }
}
public static class InGameSaveManager
{
    static string fileName = "Ingame.dat";
    static string dataPath = Application.persistentDataPath;
    static string version = "0312";
    static InGameSaveData saveData;
    static InGameSaveData SaveData
    {
        get
        {
            if (saveData == null)
            {
                saveData = Read(dataPath + "/" + fileName);
            }

            return saveData;
        }
    }

    #region tool
    public static int Floor
    {
        get
        {
            return SaveData.floor;
        }
    }
    public static int Hp
    {
        get
        {
            return SaveData.hp;
        }
    }
    public static List<CardSaveData> DeckCards
    {
        get { return SaveData.deckCards; }
    }
    public static List<int> AttainCards
    {
        get { return SaveData.attainCards; }
    }
    public static EndingConditions Ending
    {
        get { return SaveData.ending ; }
    }
    public static int Seed
    {
        get { return SaveData.seed; }
    }

    private static InGameSaveData Read(string path)
    {
        InGameSaveData data;
        using (FileStream SaveFile = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
        {
            BinaryFormatter binFormatter = new BinaryFormatter();
            try
            {
                data = binFormatter.Deserialize(SaveFile) as InGameSaveData;
                Debug.Log("인게임 세이브 로드 성공");
            }
            catch
            {
                Debug.Log("인게임 로드 실패 , 새로운 데이터를 만듭니다.");
                data = new InGameSaveData();
                binFormatter.Serialize(SaveFile, data);
            }
            finally
            {
                SaveFile.Close();
            }
        }
        return data;
    }

    private static void Save(string path)
    {
        using (FileStream clear = new FileStream(path + "/" + fileName, FileMode.Create))
        {
            clear.Close();
        }

        saveData.savedTime = DateTime.Now;
        using (FileStream WriteFile = new FileStream(path + "/" + fileName, FileMode.Open, FileAccess.Write))
        {
            BinaryFormatter binFormatter = new BinaryFormatter();
            binFormatter.Serialize(WriteFile, SaveData);

            WriteFile.Close();

            Debug.Log("세이브 완료");
        }
    }
    #endregion



    public static void WriteAndSave(int _floor, int _hp, List<CardSaveData> _deckCards,
        List<int> _attainCards, int _seed, EndingConditions _ending)
    {
        SaveData.floor = _floor;
        SaveData.hp = _hp;
        SaveData.deckCards = _deckCards;
        SaveData.attainCards = _attainCards;
        SaveData.ending = _ending;
        SaveData.isSaved = true;
        SaveData.seed = _seed;
        SaveData.version = version;
        Save(dataPath);
    }

    public static void ClearSaveData()
    {
        saveData = new InGameSaveData();
        Save(dataPath);
    }

    public static bool CheckSaveData()
    {
        if (!SaveData.isSaved)
        {
            return false;
        }
        if (SaveData.version != version)
        {
            return false;
        }
        if (SaveData.floor < 1 || SaveData.floor > 5)
        {
            return false;
        }
        if (SaveData.deckCards == null || SaveData.deckCards.Count != 15)
        {
            return false;
        }
        if (SaveData.attainCards == null)
        {
            return false;
        }

        return true;

    }

    #region ForCloud //클라우드 세이브...추후를 위해 남겨둠
    /*
    public static void Load()
    {
        saveData = Read(dataPath + "/" + fileName);
        GooglePlayManager.LoadFromCloud(fileName, OnCloudLoadCompleted);
    }

    private static void Save(string path)
    {
        using (FileStream clear = new FileStream(path, FileMode.Create))
        {
            clear.Close();
        }

        SaveData.savedTime = DateTime.Now;
        using (MemoryStream ms = new MemoryStream(1024 * 16))
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, SaveData);
            byte[] binaryData = ms.ToArray();
            SaveManager.BinaryLocalSave(binaryData, fileName, path);

            GooglePlayManager.SaveToCloud("InGameData", binaryData);
        }
    }

    public static void OnCloudLoadCompleted(byte[] byteArr)
    {
        InGameSaveData data;
        using (MemoryStream ms = new MemoryStream(1024 * 16))
        {
            foreach (byte b in byteArr)
                ms.WriteByte(b);

            ms.Position = 0;

            BinaryFormatter bf = new BinaryFormatter();
            data = bf.Deserialize(ms) as InGameSaveData;
            Debug.Log("인게임 클라우드 세이브 로드 성공");
            if (SaveData != null)
            {
                if (DateTimeOffset.Compare(data.savedTime, SaveData.savedTime) < 0)
                {
                    Debug.Log("로컬이 클라우드보다 최신입니다. 로컬세이브를 적용합니다. ");
                    return;
                }
            }
            saveData = data;
        }
    }
    */
    #endregion
}


using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class SaveData
{
    public SaveData()
    {
        //default value
        cardUnlockData = new Dictionary<int, bool>();
        diaryUnlockData = new Dictionary<int, bool[]>();
        monsterKillData = new Dictionary<int, int>();
        stageArriveData = new List<int>();
        achiveUnlockData = new Dictionary<int, bool>();
        isGetEnding = false;
        gameOverNum = 0;
        isSet = false;
        savedTime = DateTimeOffset.Parse("2000/01/01 00:00:00");
    }
    public float bgmValue;
    public float fxValue;
    public float UIValue;
    public Dictionary<int, bool> cardUnlockData;
    public Dictionary<int, bool> achiveUnlockData;
    public Dictionary<int, bool[]> diaryUnlockData; // [0] 일지 해금여부, [1] 새로운 일지인지 여부
    public Dictionary<int, int> monsterKillData;
    public List<int> stageArriveData;
    public bool isGetEnding;
    public uint gameOverNum;
    public bool isSet;
    public DateTimeOffset savedTime;
}

/// <summary>
/// Datas that will be used for save & load
/// </summary>
public static class SaveManager
{
    private static SaveData saveData;
    public static int numOfStages = 5;
    public static string Ext = ".dat";
    public static string FileName = "save";
    public static string Path = Application.persistentDataPath;
    private static byte[] primaryData;

    #region FirstSetUp
    public static void FirstSetUp()
    {
        if (saveData == null)
            saveData = new SaveData();
        if (saveData.isSet)
        {
            InitCardUnlockDatas();
            InitAchiveUnlockDatas();
            InitDiaryUnlockDatas();
            InitMonsterKillDatas();
            InitStageArriveDatas();
            saveData.savedTime = DateTime.Now;
            return;
        }
        else
        {
            SetBgmValue(1f);
            SetFxValue(1f);
            SetUIValue(1f);
            InitCardUnlockDatas();
            InitAchiveUnlockDatas();
            InitDiaryUnlockDatas();
            InitMonsterKillDatas();
            InitStageArriveDatas();
            saveData.savedTime = DateTime.Now;
            saveData.isSet = true;
        }
        
        Debug.Log(JsonConvert.SerializeObject(saveData));
    }
    #endregion

    #region defaultSetter
    private static void SetCardUnlockData(int i, bool isUnlock)
    {
        if (!saveData.cardUnlockData.ContainsKey(i))
        {
            Debug.LogWarning("키가 딕셔너리에 존재하지 않습니다.");
            //saveData.cardUnlockData.Add(i, isUnlock);
            return;
        }
        saveData.cardUnlockData[i] = isUnlock;
    }
    private static void SetAchiveUnlockData(int i, bool isUnlock)
    {
        if (!saveData.achiveUnlockData.ContainsKey(i))
        {
            Debug.LogWarning("키가 딕셔너리에 존재하지 않습니다.");
            //saveData.achiveUnlockData.Add(i, isUnlock);
            return;
        }
        saveData.achiveUnlockData[i] = isUnlock;
    }
    private static void SetDiaryUnlockData(int i, bool isUnlock)
    {
        if (!saveData.diaryUnlockData.ContainsKey(i))
        {
            Debug.LogWarning("키가 딕셔너리에 존재하지 않습니다.");
            //saveData.diaryUnlockData.Add(i, new bool[] { isUnlock, false });
            return;
        }
        saveData.diaryUnlockData[i][0] = isUnlock;
        if (isUnlock == true) saveData.diaryUnlockData[i][1] = true;
        else saveData.diaryUnlockData[i][1] = false;
    }
    private static void SetMonsterKillData(int i, int value)
    {
        if (!saveData.monsterKillData.ContainsKey(i))
        {
            Debug.LogWarning("키가 딕셔너리에 존재하지 않습니다.");
            //saveData.monsterKillData.Add(i, value);
            return;
        }
        saveData.monsterKillData[i] = value;
    }
    private static void SetStageArriveData(int i, int value)
    {
        if (i > saveData.stageArriveData.Count)
        {
            Debug.LogWarning("키가 딕셔너리에 존재하지 않습니다.");
            //saveData.stageArriveData.Add(0);
            //return;
        }
        saveData.stageArriveData[i] = value;

    }

    private static void InitCardUnlockDatas()
    {
        foreach (KeyValuePair<int, CardData> pair in Database.cardDatas)
        {
            if (saveData.cardUnlockData.ContainsKey(pair.Key)) continue;
            saveData.cardUnlockData.Add(pair.Key, false);
        }
    }
    private static void InitAchiveUnlockDatas()
    {
        foreach (KeyValuePair<int, AchiveData> pair in Database.achiveDatas)
        {
            if (saveData.achiveUnlockData.ContainsKey(pair.Key)) continue;
            saveData.achiveUnlockData.Add(pair.Key, false);
        }
    }
    private static void InitDiaryUnlockDatas()
    {
        foreach (KeyValuePair<int, DiaryData> pair in Database.diaryDatas)
        {
            if (saveData.diaryUnlockData.ContainsKey(pair.Key)) continue;
            saveData.diaryUnlockData.Add(pair.Key, new bool[] { false, false });
        }
    }
    private static void InitMonsterKillDatas()
    {
        foreach (KeyValuePair<int, MonsterData> pair in Database.monsterDatas)
        {
            if (saveData.monsterKillData.ContainsKey(pair.Key)) continue;
            saveData.monsterKillData.Add(pair.Key, 0);
        }
    }
    private static void InitStageArriveDatas()
    {
        for(int i = saveData.stageArriveData.Count; i < 5; i++)
        {
            saveData.stageArriveData.Add(0);
        }
    }

    #endregion

    #region ForDebug
    public static void SetDiaryAllTrue()
    {
        foreach (KeyValuePair<int, DiaryData> pair in Database.diaryDatas)
        {
            SetDiaryUnlockData(pair.Key, true);
        }
    }
    public static void SetCardAllTrue()
    {
        foreach (KeyValuePair<int, CardData> pair in Database.cardDatas)
        {
            SetCardUnlockData(pair.Key, true);
        }
    }
    #endregion

    #region Getter
    public static bool GetIsSet()
    {
        return saveData.isSet;
    }
    public static float GetBgmValue()
    {
        return saveData.bgmValue;
    }
    public static float GetFxValue()
    {
        return saveData.fxValue;
    }
    public static float GetUIValue()
    {
        return saveData.UIValue;
    }
    public static bool GetCardUnlockData(int i)
    {
        return saveData.cardUnlockData[i];
    }
    public static bool GetAchiveUnlockData(int i)
    {
        return saveData.achiveUnlockData[i];
    }
    public static bool[] GetDiaryUnlockData(int i)
    {
        return saveData.diaryUnlockData[i];
    }
    public static int GetMonsterKillData(int i)
    {
        return saveData.monsterKillData[i];
    }
    public static int GetStageArriveData(int i)
    {
        return saveData.stageArriveData[i];
    }
    public static int CardDataCount { get { return saveData.cardUnlockData.Count; } }
    public static int AchiveDataCount { get { return saveData.achiveUnlockData.Count; } }
    public static int DiaryDataCount { get { return saveData.diaryUnlockData.Count; } }
    public static int MonsterDataCount { get { return saveData.monsterKillData.Count; } }
    public static int StageDataCount { get { return saveData.stageArriveData.Count; } }
    #endregion

    public static void SetBgmValue(float value)
    {
        saveData.bgmValue = value;
        if (SoundDelegate.instance != null)
            SoundDelegate.instance.BGMSound = saveData.bgmValue;
    }
    public static void SetFxValue(float value)
    {   
        saveData.fxValue = value;
        if (SoundDelegate.instance != null)
            SoundDelegate.instance.EffectSound = saveData.fxValue;//효과음 조절
    }
    public static void SetUIValue(float value)
    {   //TODO: UI 투명도 조절
        saveData.UIValue = value;
    }

    /// <summary>
    /// 몬스터를 죽일 시 콜
    /// </summary>
    /// <param name="i"> DB상의 몬스터 번호 </param>
    public static void killMonster(int i)
    {
        saveData.monsterKillData[i]++;
        if (saveData.monsterKillData[i] - 1 > 0) return; //버그시 확인 필

        foreach (KeyValuePair<int, AchiveData> pair in Database.achiveDatas)
        {
            if(pair.Value.type == "kill" && int.Parse(pair.Value.condition) == i)
            {
                GetAchivement(pair.Key);
                return;
            }
        }
        SaveAll();
    }

    /// <summary>
    /// 아래 함수를 제외한 특수한 업적달성시 콜 할 것
    /// </summary>
    /// <param name="i">업적번호</param>
    public static void GetAchivement(int i)
    {
        saveData.achiveUnlockData[i] = true;
        if (Database.achiveDatas[i].reward.Length != 0)
        {
            SetDiaryUnlockData(int.Parse(Database.achiveDatas[i].reward), true);
        }
        if (Database.achiveDatas[i].cardReward.Length != 0)
        {
            SetCardUnlockData(int.Parse(Database.achiveDatas[i].cardReward), true);
        }
        SaveAll();
    }



    /// <summary>
    /// 랜덤하게 카드를 언락하는 함수
    /// </summary>
    public static void GetRandomCard()
    {
        List<int> unobtainedCards = new List<int>();
        foreach (KeyValuePair<int, CardData> pair in Database.cardDatas)
        {
            if (GetCardUnlockData(pair.Key) == false)
                unobtainedCards.Add(pair.Key);
        }
        int randomIdx = UnityEngine.Random.Range(0, unobtainedCards.Count);
        SetCardUnlockData(unobtainedCards[randomIdx], true);

        SaveAll();
    }

    /// <summary>
    /// 스테이지 도달 시 콜
    /// </summary>
    /// <param name="i">도달한 층</param>
    public static void ArriveStage(int i)
    {
        saveData.stageArriveData[i]++;
        foreach (KeyValuePair<int, AchiveData> pair in Database.achiveDatas)
        {
            if (pair.Value.type == "floor" && int.Parse(pair.Value.condition) == i)
            {
                GetAchivement(pair.Key);
                return;
            }
        }
        SaveAll();
    }

    /// <summary>
    /// 엔딩 달성 시 콜
    /// </summary>
    public static void GetEnding()
    {
        saveData.isGetEnding = true;
        foreach (KeyValuePair<int, AchiveData> pair in Database.achiveDatas)
        {
            if (pair.Value.type == "ending")
            {
                GetAchivement(pair.Key);
                return;
            }
        }
        SaveAll();
        SceneManager.LoadScene("EndingScene");
    }

    /// <summary>
    /// 게임오버 시 콜
    /// </summary>
    public static void GetGameOver()
    {
        saveData.gameOverNum++;
        foreach (KeyValuePair<int, AchiveData> pair in Database.achiveDatas)
        {
            if (pair.Value.type == "gameover" && int.Parse(pair.Value.condition) >= saveData.gameOverNum)
            {
                GetAchivement(pair.Key);
            }
        }
        SaveAll();
    }

    public static void ApplySave()
    {
        if (saveData == null) return;
        FirstSetUp();
        SetBgmValue(saveData.bgmValue);
        SetFxValue(saveData.fxValue);
        SetUIValue(saveData.UIValue);
    }

    /// <summary>
    /// 세이브시에 부르는 메소드
    /// </summary>
    public static void SaveAll()
    {
        if (saveData == null) FirstSetUp();

        string json = JsonConvert.SerializeObject(saveData);
        Debug.Log("Saving: " + json);
        primaryData = Encoding.UTF8.GetBytes(json);
        BinarySave(primaryData, FileName, Path);

        GooglePlayManager.SaveToCloud();
    }

    /// <summary>
    /// 로드시에 부르는 메소드
    /// </summary>
    public static void LoadAll()
    {
        SaveManager.BinaryLoad(SaveManager.FileName, SaveManager.Path);
        ApplySave();
        GooglePlayManager.LoadFromCloud();
    }


    #region For Diary
    public static bool CheckNew()
    {
        for (int i = 1; i < saveData.diaryUnlockData.Count; i++)
        {
            if (saveData.diaryUnlockData[i][1] == true) return true;
        }
        return false;
    }

    public static void ChangeNewToOld(int i)
    {
        saveData.diaryUnlockData[i][1] = false;
    }
    #endregion

    #region For Save/Load


    public static byte[] OnCloudSaveStart()
    {
        if (primaryData != null)
            return primaryData;
        else
            return null;
    }

    public static void OnCloudLoadCompleted(byte[] byteArr)
    {
        string json = Encoding.UTF8.GetString(byteArr);
        if (json.Length == 0)
        {
            Debug.Log("0 lenth data from cloud save, quit");
            return;
        }
        Debug.Log("cloud load: " + json);
        SaveData cloud = JsonConvert.DeserializeObject<SaveData>(json);
        if (saveData != null)
        {
            if (DateTimeOffset.Compare(cloud.savedTime, saveData.savedTime) < 0)
            {
                Debug.Log("로컬이 클라우드보다 최신입니다. 로컬세이브를 적용합니다. " + json);
                return;
            }
        }
        saveData = cloud;
        ApplySave();
        return;
    }

    public static bool BinarySave(byte[] json, string filename, string path)
    {
        using (FileStream file = new FileStream(path + "/" + filename + Ext, FileMode.Create))
        {
            using (BufferedStream buf = new BufferedStream(file))
            {
                if (file == null)
                {
                    Debug.LogError("file Create Error!");
                    return false;
                }
                buf.Write(json, 0, json.Length);
                Debug.Log("Local Save Complete" + json);
                return true;
            }
        }
    }

    public static bool BinaryLoad(string filename, string path)
    {
        using (FileStream file = new FileStream(path + "/" + filename + Ext, FileMode.OpenOrCreate))
        {
            using (BufferedStream buf = new BufferedStream(file))
            {
                byte[] primary = new byte[buf.Length];
                buf.Read(primary, 0, primary.Length);
                string json = Encoding.UTF8.GetString(primary);
                if (json.Length == 0)
                {
                    Debug.Log("no local Save");
                    return false;
                }
                saveData = JsonConvert.DeserializeObject<SaveData>(json);
                Debug.Log("Local Save Loaded" + json);
                return true;
            }
        }
    }
    #endregion
}

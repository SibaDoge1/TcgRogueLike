using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Datas that will be used for save & load
/// </summary>
public static class SaveData
{
    public static float bgmValue { get; private set; }
    public static float fxValue { get; private set; }
    public static float UIValue { get; private set; }
    public static List<bool> cardUnlockData { get; private set; }
    public static List<bool> achiveUnlockData { get; private set; }
    public static List<bool[]> diaryUnlockData { get; private set; }  // [0] 일지 해금여부, [1] 새로 해금된 건지 여부
    public static Dictionary<int, int> monsterKillData { get; private set; }
    public static List<bool> stageArriveData { get; private set; } //TODO: 미구현
    public static bool isGetEnding { get; private set; }
    public static uint gameOverNum { get; private set; }
    private static bool isSet;

    #region FirstSetUp
    public static void FirstSetUp()
    {
        if (isSet)
        {
            if (cardUnlockData.Count <= Database.cardDatas.Count) // 데이터가 추가되면 리스트도 변경
                SetCardUnlockDataAll();
            if (diaryUnlockData.Count <= Database.diaryDatas.Count)
                SetDiaryUnlockDataAll();
            if (achiveUnlockData.Count <= Database.achiveDatas.Count)
                SetAchiveUnlockDataAll();
            if (monsterKillData.Count <= Database.monsterDatas.Count)
                SetMonsterKillDataAll();
            return;
        }
        else                                                        //default value
            SetBgmValue(1f);
            SetFxValue(1f);
            SetUIValue(1f);
            cardUnlockData = new List<bool>();
            SetCardUnlockDataAll();
            diaryUnlockData = new List<bool[]>();
            SetDiaryUnlockDataAll();
            monsterKillData = new Dictionary<int, int>();
            SetMonsterKillDataAll();
            stageArriveData = new List<bool>();
            SetStageArriveDataAll();
            isGetEnding = false;
            gameOverNum = 0;
            isSet = true;
    }
    #endregion

    #region defaultSetter

    private static void SetCardUnlockDataAll()
    {
        for (int idx = cardUnlockData.Count; idx <= Database.cardDatas.Count; idx++)
        {
            cardUnlockData.Add(false);
        }
    }
    private static void SetAchiveUnlockDataAll()
    {
        for (int idx = achiveUnlockData.Count; idx <= Database.cardDatas.Count; idx++)
        {
            achiveUnlockData.Add(false);
        }
    }
    private static void SetDiaryUnlockDataAll()
    {
        for (int idx = diaryUnlockData.Count; idx <= Database.diaryDatas.Count; idx++)
        {
            diaryUnlockData.Add(new bool[] { false, false });
        }
    }
    private static void SetMonsterKillDataAll()
    {
        foreach(KeyValuePair<int, MonsterData> pair in Database.monsterDatas)
        {
            if (monsterKillData.ContainsKey(pair.Key)) continue;
            else monsterKillData.Add(pair.Key, 0);
        }
    }
    private static void SetStageArriveDataAll()
    {
       
    }

    #endregion

    #region ForDebug
    public static void SetDiaryAllTrue()
    {
        for (int i = 0; i < diaryUnlockData.Count; i++)
            SetDiaryUnlockData(i);
    }
    public static void SetCardAllTrue()
    {
        for (int i = 0; i < cardUnlockData.Count; i++)
            SetCardUnlockData(i);
    }
    #endregion

    public static void SetBgmValue(float value)
    {
        bgmValue = value;
        SoundDelegate.instance.BGMSound = bgmValue;
    }
    public static void SetFxValue(float value)
    {   //TODO: 효과음 조절
            fxValue = value;
    }
    public static void SetUIValue(float value)
    {   //TODO: UI 투명도 조절
        UIValue = value;
    }

    public static void SetCardUnlockData(int i)
    {
        cardUnlockData[i] = true;
    }

    public static void SetDiaryUnlockData(int i)
    {
        diaryUnlockData[i][0] = true;
        diaryUnlockData[i][1] = true;
    }

    /// <summary>
    /// 몬스터를 죽일 시 콜
    /// </summary>
    /// <param name="i"> DB상의 몬스터 번호 </param>
    public static void killMonster(int i)
    {
        monsterKillData[i]++;
        if (monsterKillData[i] - 1 > 0) return; //버그시 확인 필

        for(int idx = 1; idx <= Database.achiveDatas.Count; idx++)
        {
            if(Database.achiveDatas[idx].type == "kill" && int.Parse(Database.achiveDatas[idx].condition) == i)
            {
                GetAchivement(idx);
            }
        }
    }

    /// <summary>
    /// 아래 함수를 제외한 특수한 업적달성시 콜 할 것
    /// </summary>
    /// <param name="i">업적번호</param>
    public static void GetAchivement(int i)
    {
        achiveUnlockData[i] = true;
        if (Database.achiveDatas[i].reward.Length != 0)
        {
            SetDiaryUnlockData(int.Parse(Database.achiveDatas[i].reward));
        }
        if (Database.achiveDatas[i].cardReward.Length != 0)
        {
            SetCardUnlockData(int.Parse(Database.achiveDatas[i].cardReward));
        }
    }

    /// <summary>
    /// 스테이지 도달 시 콜
    /// </summary>
    /// <param name="i">도달한 층</param>
    public static void ArriveStage(int i)
    {
        stageArriveData[i] = true;
        for (int idx = 1; idx <= Database.achiveDatas.Count; idx++)
        {
            if (Database.achiveDatas[idx].type == "floor" && int.Parse(Database.achiveDatas[idx].info) == i)
            {
                GetAchivement(idx);
            }
        }
    }

    /// <summary>
    /// 엔딩 달성 시 콜
    /// </summary>
    public static void GetEnding()
    {
        isGetEnding = true;
        for (int idx = 1; idx <= Database.achiveDatas.Count; idx++)
        {
            if (Database.achiveDatas[idx].type == "ending")
            {
                GetAchivement(idx);
            }
        }
        SceneManager.LoadScene("EndingScene");
    }

    /// <summary>
    /// 게임오버 시 콜
    /// </summary>
    public static void GetGameOver()
    {
        gameOverNum++;
        for (int idx = 1; idx <= Database.achiveDatas.Count; idx++)
        {
            if (Database.achiveDatas[idx].type == "gameover" && int.Parse(Database.achiveDatas[idx].info) <= gameOverNum)
            {
                GetAchivement(idx);
            }
        }
    }


    public static bool CheckNew()
    {
        for (int i = 1; i < diaryUnlockData.Count; i++)
        {
            if (diaryUnlockData[i][1] == true) return true;
        }
        return false;
    }

    public static void ChangeNewToOld(int i)
    {
        diaryUnlockData[i][1] = false;
    }
}
 
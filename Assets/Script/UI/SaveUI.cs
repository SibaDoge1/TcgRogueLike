using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveUI : MonoBehaviour
{
    Vector3 offPos = new Vector3(0, -5000, 0);
    float originVolume;
    voidFunc onSave;

    public void SaveUIOn(voidFunc OnSave = null)
    {
        onSave = OnSave;
        transform.localPosition = new Vector3(0, 0, 0);
        originVolume = SoundDelegate.instance.BGMSound;
        SoundDelegate.instance.BGMSound = 0;
    }

    public void OnYesButtonDown()
    {
        InGameSaveManager.WriteAndSave
            (GameManager.instance.CurrentMap.Floor,
            PlayerControl.player.GetHp,
            CardsToDataClass(PlayerControl.instance.DeckManager.Deck),
            CardsToNumber(PlayerControl.instance.DeckManager.AttainCards),
             GameManager.instance.BuildSeed,
                GameManager.instance.EndingCondition);

        onSave();
        SaveManager.SaveAll();
        DisableAllChildren();
        LoadingManager.LoadScene("Levels/MainMenu");      
    }

    public void OnNoButtonDown()
    {
        transform.localPosition = offPos;
        SoundDelegate.instance.BGMSound = originVolume;
    }

    private List<int> CardsToNumber(List<Card> datas)
    {
        List<int> numbers = new List<int>();
        for (int i = 0; i < datas.Count; i++)
        {
            numbers.Add(datas[i].Index);
        }
        return numbers;
    }
    private List<CardSaveData> CardsToDataClass(List<Card> datas)
    {
        List<CardSaveData> data = new List<CardSaveData>();
        for (int i = 0; i < datas.Count; i++)
        {
            data.Add(new CardSaveData(datas[i].Index, datas[i].Type, datas[i].CardFigure));
        }
        return data;
    }
    private void DisableAllChildren()
    {
        for(int i=0; i<transform.childCount;i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
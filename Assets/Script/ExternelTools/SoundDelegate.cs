using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BGM
{
    FLOOR1,
    Floor1_BOSS,
    Title
}
public enum CardSoundType
{
    Hit,
    CriticalHit
}
public enum EffectSoundType
{
    GetHit,
    RoomClear,
    RoomMove,
    GameOver,
}
public enum ButtonSoundType
{
    Normal,
    Select,
    Exit,
    Confirm,
}
public class SoundDelegate : MonoBehaviour {
    #region variables
    AudioSource bgm;

    private float bgmSound = 1f;
    public float BGMSound
    {
        get { return bgmSound; }
        set
        {
            bgmSound = value;
            bgm.volume = bgmSound;
        }
    }

    private float effectSound =1f;
    public float EffectSound
    {
        get { return effectSound; }
        set { effectSound = value; }
    }

    public static SoundDelegate instance;
    public AudioClip[] bgmAudioClips;
    public GameObject[] cardAudioObject;
    public GameObject[] effectAudioObject;
    #endregion

    void Awake()
    {
        //Changes: destroy 부분 추가
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            UnityEngine.Debug.LogError("SingleTone Error : SoundDelegate");
            Destroy(this);
        }

        bgm = transform.Find("BGM").GetComponent<AudioSource>();
    }

    public void PlayBGM(BGM b)
    {
        bgm.clip = bgmAudioClips[(int)b];
        bgm.Play();
    }
    public GameObject PlayCardSound(CardSoundType cst,Vector3 position)
    {
        return Instantiate(cardAudioObject[(int)cst],position,Quaternion.identity);
    }
    public GameObject PlayEffectSound(EffectSoundType ef, Vector3 position)
    {
        return Instantiate(effectAudioObject[(int)ef], position, Quaternion.identity);
    }
}

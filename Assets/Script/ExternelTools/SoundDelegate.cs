using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BGM
{
    NONE,
    FIELD1,
    FIELD2,
    FIELD3,
    FIELD4,
    BOSSFIRE,//화산,연기
    BOSSROBOT,//기계
    BOSSSPIDER,//거미신부
    FIELDTITLECUT,
    NORMALENDINGCUT,
    TRUEENDINGCUT
}
public enum MonoSound//1개만 재생되는것들
{
    BUTTONTITLE,
    BUTTONNORMAL,
    BUTTONYES,
    BUTTONNO
}
public enum SoundEffect//재생할때마다 prefab 생성되는것들
{
    SFX1,
    SFX2,
    SFX3,
    SFX4,
    SFX5,
    SFX6,
    SFX7,
    SFX8,
    SFX9,
    SFX10,//여기까지 카드
    MOVE,
    AKSLOW,//TODO
    CONNECT,
    CARD,
    RELOADCARD,
    GAMEOVER,
    ERROR,
    HEAL,
    DAMAGE,
    ATTACK,
    ROOMCLEAR
}
/// <summary>
/// 이펙트 사운드는 ArchLoader쪽으로 이동
/// </summary>
public class SoundDelegate : MonoBehaviour {
    #region variables
    AudioSource bgm;
    BGM current = BGM.NONE;
    AudioSource monosound;

    public float BGMSound
    {
        get { return bgm.volume; }
        set
        {
            bgm.volume = value;
        }
    }

    public float EffectSound
    {
        get { return monosound.volume; }
        set
        {
            monosound.volume = value;
        }
    }

    public static SoundDelegate instance;
    #endregion

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            UnityEngine.Debug.LogWarning("SingleTone Error : " + this.name);
            Destroy(gameObject);
        }
        bgm = transform.Find("BGM").GetComponent<AudioSource>();
        monosound = transform.Find("MONO").GetComponent<AudioSource>();
    }

    public void PlayBGM(BGM b)
    {
        if(current == b)
        {
            return;
        }else
        {
            current = b;
            if(current == BGM.NONE)
            {
                bgm.Stop();
            }else
            {
                bgm.clip = ArchLoader.instance.GetBGM(b);
                bgm.Play();
            }
        }
    }
    public void PlayMono(MonoSound mono)
    {
        monosound.clip = ArchLoader.instance.GetMono(mono);
        monosound.Play();
    }
    public void PlayEffectSound(SoundEffect eType, Vector3 position)
    {
        ObjectPoolManager.instance.PoolSoundEffect().
            Init(ArchLoader.instance.GetSoundEffect(eType),EffectSound,position);
    }

    public void PlayGameOverSound(BGM targetBGM, float time)
    {
        PlayEffectSound(SoundEffect.GAMEOVER, Camera.main.transform.position);
        StartCoroutine(BgmFade(targetBGM,time));
    }

    IEnumerator BgmFade(BGM targetBGM,float time)
    {
        float value = BGMSound;
        float counter = 0;
        while(counter<time)
        {        
            BGMSound = Mathf.Lerp(value, 0, counter / time);
            counter += Time.deltaTime;
            yield return null;
        }
        PlayBGM(targetBGM);
        BGMSound = value;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObject : EffectObject
{
    AudioSource audio;
    AudioSource Audio
    {
        get
        {
            if(audio != null)
            {
                return audio;
            }
            else
            {
                return audio = GetComponent<AudioSource>();
            }
        }
    }
    public void Init(AudioClip audio,float volume,Vector3 pos)
    {        
        Audio.clip = audio;
        Audio.volume = volume;
        Audio.Play();
        transform.position = pos;
    }
	
}

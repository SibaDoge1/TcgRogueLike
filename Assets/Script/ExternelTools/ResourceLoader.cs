using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Filed 유닛들 Load 해주는 클래스 입니다.
/// </summary>
public class ResourceLoader : MonoBehaviour {

    public static ResourceLoader instance;
    private void Awake()
    {
        instance = this;
    }
    public GameObject LoadEntity(int num)
    {
       if(num < 1000)
        {
            return (Resources.Load("Fields/Entity/" + num) as GameObject);
        }
        else if(num <2000)
        {
            return (Resources.Load("Fields/Entity/1000/" + num) as GameObject);
        }
        else if(num <3000)
        {
            return (Resources.Load("Fields/Entity/2000/" + num) as GameObject);
        }
        else if (num<4000)
        {
            return (Resources.Load("Fields/Entity/3000/" + num) as GameObject);
        }
        else 
        {
            return (Resources.Load("Fields/Entity/4000/" + num) as GameObject);
        }
    }

    public GameObject LoadTile(int num)
    {
        return (Resources.Load("Fields/Tile/" + num) as GameObject);
    }
    public GameObject LoadOffTile(int num)
    {
        return (Resources.Load("Fields/OffTile/" + num) as GameObject);
    }
    public GameObject LoadPlayer()
    {
        return Resources.Load("Player") as GameObject;
    }


}

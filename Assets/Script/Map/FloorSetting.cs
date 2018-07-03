using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSetting : MonoBehaviour {

    public static FloorSetting instance;
    // Use this for initialization
    void Awake()
    {
        instance = this;
    }

    public int[] roomNum;//3층까지
    public Vector2Int[] mapSize;

}

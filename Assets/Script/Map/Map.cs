using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 맵 데이터를 가지고있는 클래스 입니다.
/// </summary>
public class Map : MonoBehaviour
{

    public void SetStartRoom(Room startRoom_)
    {
        startRoom = startRoom_;
        //StartRoom.OpenDoors();
    }
    public void Init(int fl,int ba,int ev,bool bo,bool isend)
    {
        floor = fl;
        battleRoomNum = ba;
        eventRoomNum = ev;
        boss = bo;
        end = isend;
    }
    private List<Room> room;
    public List<Room> Rooms
    {
        get { return room; }
        set { room = value; }
    }

    private Room startRoom;
    public Room StartRoom
    {
        get { return startRoom; }
    }
    private Room currentRoom;
    public Room CurrentRoom
    {
        get { return currentRoom; }
        set { currentRoom = value; }
    }

    #region variables
    private int floor;//층 수
    public int Floor
    {
        get { return floor; }
        set { floor = value; }
    }
    private int battleRoomNum;
    public int BattleRoomNum
    {
        get { return battleRoomNum; }
    }
    private int eventRoomNum;
    public int EventRoomNum
    {
        get { return eventRoomNum; }
    }
    private bool boss;
    public bool Boss
    {
        get { return boss; }
    }
    private bool end;
    public bool End
    {
        get { return end; }
    }


    private Vector2Int minBorder;
    public Vector2Int MinBorder
    {
        get { return minBorder; }
        set { minBorder = value; }
    }
    private Vector2Int maxBorder;
    public Vector2Int MaxBorder
    {
        get { return maxBorder; }
        set { maxBorder = value; }
    }
    #endregion

    public void SetRoomOn(Room room)
    {
        room.gameObject.SetActive(true);
    }
    public void SetRoomOff(Room room)
    {
        room.gameObject.SetActive(false);
    }

}

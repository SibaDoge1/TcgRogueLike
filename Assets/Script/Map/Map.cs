using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour{
    public void SetStartRoom(Room startRoom_)
    {
        startRoom = startRoom_;
        currentRoom = startRoom;
    }

	private List<Room> room;
    public List<Room> Room
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

    public int roomNum;//방갯수

    public Vector2Int mapSize;
    public Vector2Int maxRoomPos;//방의위치
    public Vector2Int minRoomPos;

    public Vector2Int maxRoomSize;//방의크기
    public Vector2Int minRoomSize;



}

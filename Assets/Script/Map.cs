using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map {
    public Map(Room startRoom_)
    {
        startRoom = startRoom_;
        currentRoom = startRoom;
    }


	List<Room> room;
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

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
    DEFAULT,
    STARTING,
    BATTLE,
}

public class Room : MonoBehaviour
{
	public Vector2Int Pos;
	public RoomType type;
	public bool doorTop, doorBot, doorLeft, doorRight;

	public void setRoom(Vector2Int _Pos, RoomType _type)
    {
        Pos = _Pos;
		type = _type;

        transform.parent = MapGenerator.instance.transform;
        transform.localPosition = new Vector3(Pos.x*MapGenerator.roomSize.x,Pos.y * MapGenerator.roomSize.y, 0);
        gameObject.name = "Room_" + Pos.x + "_" + Pos.y;
	}

}

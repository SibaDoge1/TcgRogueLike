using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
    DEFAULT,
    BATTLE,
}

public class Room : MonoBehaviour
{
	public Vector2Int Pos;
	public RoomType type;
	public bool doorTop, doorBot, doorLeft, doorRight;
    public Node[,] thisNodes;

	public void setRoom(Vector2Int _Pos, RoomType _type)
    {
        Pos = _Pos;
		type = _type;

        transform.parent = MapGenerator.instance.transform;
        transform.localPosition = new Vector3(Pos.x*MapGenerator.roomSize.x,Pos.y * MapGenerator.roomSize.y, 0);
        gameObject.name = "Room_" + Pos.x + "_" + Pos.y;

        setNodes();
	}
    void setNodes()
    {
        thisNodes = new Node[MapGenerator.roomSize.x, MapGenerator.roomSize.y];
        for (int i = 0; i < MapGenerator.roomSize.x; i++)
        {
            for(int j=0;j< MapGenerator.roomSize.y; j++)
            {
                if(i==0 || j==0 || j==MapGenerator.roomSize.y-1 || i==MapGenerator.roomSize.x-1)
                {
                    thisNodes[i, j] = new WallNode(new Vector2Int(i, j));
                }else
                {
                    thisNodes[i, j] = new GroundNode(new Vector2Int(i, j));
                }
            }
        }
    }

    public void SetDoors()
    {
        if (!doorTop)
        {
            OnTileObject temp = Instantiate(Resources.Load("Structure/00") as GameObject, transform).GetComponent<OnTileObject>();
             temp.transform.localPosition = new Vector3(-8.5f + MapGenerator.roomSize.x / 2, -4.5f + MapGenerator.roomSize.y - 1, 0);
             thisNodes[MapGenerator.roomSize.x/2, MapGenerator.roomSize.y - 1].onTile = temp;

            temp = Instantiate(Resources.Load("Structure/00") as GameObject, transform).GetComponent<OnTileObject>();
            temp.transform.localPosition = new Vector3(-8.5f + MapGenerator.roomSize.x / 2 -1, -4.5f + MapGenerator.roomSize.y - 1, 0);
            thisNodes[MapGenerator.roomSize.x/2-1, MapGenerator.roomSize.y - 1].onTile = temp;
        }
        if (!doorRight)
        {
            OnTileObject temp = Instantiate(Resources.Load("Structure/00") as GameObject, transform).GetComponent<OnTileObject>();
            temp.transform.localPosition = new Vector3(-8.5f + MapGenerator.roomSize.x - 1, -4.5f + MapGenerator.roomSize.y / 2, 0);
            thisNodes[MapGenerator.roomSize.x - 1, MapGenerator.roomSize.y / 2].onTile = temp;

            temp = Instantiate(Resources.Load("Structure/00") as GameObject, transform).GetComponent<OnTileObject>();
            temp.transform.localPosition = new Vector3(-8.5f + MapGenerator.roomSize.x - 1, -4.5f + MapGenerator.roomSize.y / 2 - 1, 0);
            thisNodes[MapGenerator.roomSize.x - 1, MapGenerator.roomSize.y / 2 - 1].onTile = temp;
        }
        if (!doorBot)
        {
            OnTileObject temp = Instantiate(Resources.Load("Structure/00") as GameObject, transform).GetComponent<OnTileObject>();
            temp.transform.localPosition = new Vector3(-8.5f + MapGenerator.roomSize.x / 2, -4.5f +0, 0);
            thisNodes[MapGenerator.roomSize.x / 2, 0].onTile = temp;

            temp = Instantiate(Resources.Load("Structure/00") as GameObject, transform).GetComponent<OnTileObject>();
            temp.transform.localPosition = new Vector3(-8.5f + MapGenerator.roomSize.x / 2, -4.5f + 0, 0);
            thisNodes[MapGenerator.roomSize.x / 2-1, 0].onTile = temp;
        }
        if (!doorLeft)
        {
            OnTileObject temp = Instantiate(Resources.Load("Structure/00") as GameObject, transform).GetComponent<OnTileObject>();
            temp.transform.localPosition = new Vector3(-8.5f + 0, -4.5f + MapGenerator.roomSize.y / 2, 0);
            thisNodes[0, MapGenerator.roomSize.y / 2].onTile = temp;

            temp = Instantiate(Resources.Load("Structure/00") as GameObject, transform).GetComponent<OnTileObject>();
            temp.transform.localPosition = new Vector3(-8.5f + 0, -4.5f + MapGenerator.roomSize.y / 2 - 1, 0);
            thisNodes[0, MapGenerator.roomSize.y / 2 - 1].onTile = temp;
        }
    }
    public void OpenDoors()
    {
        if (doorTop)
        {
            Destroy(thisNodes[MapGenerator.roomSize.x / 2, MapGenerator.roomSize.y - 1].onTile.gameObject);
            Destroy(thisNodes[MapGenerator.roomSize.x / 2 - 1, MapGenerator.roomSize.y - 1].onTile.gameObject);
            
        }
        if (doorRight)
        {
            Destroy(thisNodes[MapGenerator.roomSize.x - 1, MapGenerator.roomSize.y / 2].onTile.gameObject);
            Destroy(thisNodes[MapGenerator.roomSize.x - 1, MapGenerator.roomSize.y / 2 - 1].onTile.gameObject);
        }
        if (doorBot)
        {
            Destroy(thisNodes[MapGenerator.roomSize.x / 2, 0].onTile.gameObject);
            Destroy(thisNodes[MapGenerator.roomSize.x / 2 - 1, 0].onTile.gameObject);
        }
        if (doorLeft)
        {
            Destroy(thisNodes[0, MapGenerator.roomSize.y / 2].onTile.gameObject);
            Destroy(thisNodes[0, MapGenerator.roomSize.y / 2 - 1].onTile.gameObject);
        }
    }

}

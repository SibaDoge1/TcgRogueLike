﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapRenderer : MonoBehaviour {
	public static MinimapRenderer instance;
	void Awake(){
		instance = this;
	}

    private readonly Color emptyColor = Color.clear;
	private readonly Color wallColor = Color.white;
	private readonly Color tileColor = Color.blue;
    private readonly Color playerColor = Color.red;
    private readonly Color enemyColor = Color.yellow;

	int space;


	private Texture2D texture;


    private Vector2Int textureSize;

	private Vector2Int minBorder;
	private Vector2Int maxBorder;
    private Vector2Int playerPos = new Vector2Int(-1,-1);

    public void Init(Map map)
    {
        minBorder = map.minBorder;
        maxBorder = map.maxBorder;
        space = MapGenerator.space;
        playerPos = new Vector2Int(-1, -1);
        textureSize = new Vector2Int(maxBorder.x - minBorder.x, maxBorder.y - minBorder.y);
        texture = new Texture2D(textureSize.x + space * 2, textureSize.y + space * 2);
        texture.filterMode = FilterMode.Point;

        Color resetColor = Color.clear;
        Color[] resetColorArray = texture.GetPixels();

        for(int i=0; i<resetColorArray.Length;i++)
        {
            resetColorArray[i] = resetColor;
        }

        texture.SetPixels(resetColorArray);


        UIManager.instance.SetMapTexture(texture, textureSize);
    }

    public void DrawRoom(Room room)
    {
        Vector2Int pos = WoldPosToMapPos(room.transform.position);
        for(int i=0; i<room.size.y;i++)
        {
            for(int j=0;j<room.size.x;j++)
            {
                Arch.Tile t = room.GetTile(new Vector2Int(i, j));
                if (t == null)
                    texture.SetPixel(pos.x+i,pos.y+j, emptyColor);
                else
                {
                    if(t.OnTileObj is Structure)
                    {
                        texture.SetPixel(pos.x + i, pos.y + j, wallColor);
                    }else
                    {
                        texture.SetPixel(pos.x + i, pos.y + j, tileColor);
                    }
                }
            }
        }
        texture.Apply();
    }

    public void DrawDoors(Room room)
    {
        List<Door> doors = room.doorList;
        foreach(Door d in doors)
        {
            Vector2Int pos = WoldPosToMapPos(d.transform.position);
            texture.SetPixel(pos.x,pos.y, tileColor);
            switch (d.Dir)
            {
                case Direction.NORTH:
                    for(int i=1; i<space;i++)
                    {
                        texture.SetPixel(pos.x, pos.y + i, wallColor);
                    }
                    break;
                case Direction.EAST:
                    for (int i = 1; i < space; i++)
                    {
                        texture.SetPixel(pos.x+i, pos.y, wallColor);
                    }
                    break;
                case Direction.SOUTH:
                    for (int i = 1; i < space; i++)
                    {
                        texture.SetPixel(pos.x, pos.y - i, wallColor);
                    }
                    break;
                case Direction.WEST:
                    for (int i = 1; i < space; i++)
                    {
                        texture.SetPixel(pos.x-i, pos.y, wallColor);
                    }
                    break;
            }
        }
        texture.Apply();

    }

    /// <summary>
    /// 임시
    /// </summary>
    /// <param name="room"></param>
    public void DrawPlayerPos(Vector3 pPos)
    {
        Vector2Int oldPos = playerPos;
        playerPos = WoldPosToMapPos(pPos);

        if (oldPos.x > 0)
        {
            texture.SetPixel(oldPos.x, oldPos.y, tileColor);
        }

        texture.SetPixel(playerPos.x, playerPos.y, playerColor);
        texture.Apply();

        UIManager.instance.MoveMiniMap(MapPosToUIPos(oldPos),MapPosToUIPos(playerPos));       
    }
    //계산때매 일단 여기서 호출..


    Vector2Int WoldPosToMapPos(Vector3 pos)
    {
        return new Vector2Int((int)pos.x,(int)pos.y) - minBorder + new Vector2Int(space,space);
    }
    Vector3 MapPosToUIPos(Vector2Int pos)
    {
        if (pos.x < 0)
        {
            return Vector3.zero;
        }
        return new Vector3(-4*textureSize.x + 8 * pos.x, -4*textureSize.y + 8 * pos.y);
    }

}
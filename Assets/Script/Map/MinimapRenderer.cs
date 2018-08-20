﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 미니맵 텍스쳐 생성하는 클래스
/// </summary>
public  static class MinimapTexture 
{



    private static readonly Color emptyColor = Color.clear;
    private static readonly Color wallColor = Color.white;
	private static readonly Color tileColor = Color.blue;
    private static readonly Color playerColor = Color.green;
    private static readonly Color enemyColor = Color.red;
    private static readonly Color doorColor = new Color(139/255f,69/255f,19/255f);
    static int space;


	private static Texture2D texture;
    private static Vector2Int textureSize;
	private static Vector2Int minBorder;
	private static Vector2Int maxBorder;
    private static Vector2Int playerMapPos;
    private static List<Vector2Int> enemyPoses = new List<Vector2Int>();

    static public  void Init(Map map)
    {
        minBorder = map.MinBorder;
        maxBorder = map.MaxBorder;
        space = map.GetComponent<MapGenerator>().space;
        playerMapPos = new Vector2Int(-1, -1);
        textureSize = maxBorder - minBorder + new Vector2Int(space*2,space*2);
        texture = new Texture2D(textureSize.x,textureSize.y);
        texture.filterMode = FilterMode.Point;

        Color resetColor = Color.clear;
        Color[] resetColorArray = texture.GetPixels();

        for(int i=0; i<resetColorArray.Length;i++)
        {
            resetColorArray[i] = resetColor;
        }

        texture.SetPixels(resetColorArray);

        firstTime = true;
        UIManager.instance.SetMapTexture(texture, textureSize);
    }

    static public void DrawRoom(Room room)
    {
        Vector2Int pos = WorldPosToMapPos(room.transform.position);
        for(int i=0; i<room.size.y;i++)
        {
            for(int j=0;j<room.size.x;j++)
            {
                Arch.Tile t = room.GetTile(new Vector2Int(j, i));
                if (t.objectNum == 0 && t.OnTileObj == null && t.offTile==null)
                    texture.SetPixel(pos.x+j,pos.y+i, emptyColor);
                else
                {
                    if(t.offTile is OffTile_Door && !((OffTile_Door)t.offTile).IsDestroyed)
                    {
                        texture.SetPixel(pos.x + j, pos.y + i,doorColor);
                    }
                    else if(t.OnTileObj is Structure)
                    {
                        texture.SetPixel(pos.x + j, pos.y + i, wallColor);
                    }else
                    {
                        texture.SetPixel(pos.x + j, pos.y + i, tileColor);
                    }
                }
            }
        }
        texture.Apply();
    }
    

    static bool firstTime = true;
    static  public  void DrawPlayerPos(Vector3 roomPos, Vector2Int pPos)
    {
        Vector2Int oldPos = playerMapPos;
        if (firstTime)
        {
            firstTime = false;
        }
        else
        {            
            texture.SetPixel(oldPos.x, oldPos.y, tileColor);
        }

        playerMapPos = RoomPosToMapPos(roomPos, pPos);
        texture.SetPixel(playerMapPos.x, playerMapPos.y, playerColor);
        texture.Apply();

        UIManager.instance.MoveMiniMap(MapPosToUIPos(oldPos),MapPosToUIPos(playerMapPos));      
    }
    static public void DrawEnemies(Vector3 roomPos, List<Vector2Int> eP)
    {
        for(int i=0; i<enemyPoses.Count;i++) //적 위치 초기화
        {
            texture.SetPixel(enemyPoses[i].x, enemyPoses[i].y, tileColor);
        }
        enemyPoses.Clear();

        for (int i=0; i<eP.Count;i++)//적 위치 변환해서 enemyPoses에 저장
        {
            enemyPoses.Add(RoomPosToMapPos(roomPos, eP[i]));
        }

        for(int i=0; i<enemyPoses.Count;i++)//enemyPoses에 점찍기
        {
            texture.SetPixel(enemyPoses[i].x, enemyPoses[i].y, enemyColor);
        }
        texture.Apply();
    }
    static Vector2Int RoomPosToMapPos(Vector3 roomPos,Vector2Int pos)
    {
        return WorldPosToMapPos(roomPos) + pos;
    }

    static Vector3 MapPosToUIPos(Vector2Int pos)
    {
        if (pos.x < 0)
        {
            return Vector3.zero;
        }
        return new Vector3(-4*textureSize.x + 8 * pos.x, -4*textureSize.y + 8 * pos.y);
    }
    
static Vector2Int WorldPosToMapPos(Vector3 pos)
{
    return new Vector2Int((int)pos.x,(int)pos.y) - minBorder + new Vector2Int(space,space);
}
}
/*static public void DrawDoors(Vector3 roomPos,List<Door> doorList)
{
    foreach(Door d in doorList)
    {
        Vector2Int pos = RoomPosToMapPos(roomPos, d.CurrentTile.pos);
        texture.SetPixel(pos.x,pos.y, tileColor);
        switch (d.Dir)
        {
            case Direction.NORTH:
                for(int i=1; i<=space;i++)
                {
                    texture.SetPixel(pos.x, pos.y + i, doorColor);
                }
                break;
            case Direction.EAST:
                for (int i = 1; i <=space; i++)
                {
                    texture.SetPixel(pos.x+i, pos.y, doorColor);
                }
                break;
            case Direction.SOUTH:
                for (int i = 1; i <=space; i++)
                {
                    texture.SetPixel(pos.x, pos.y - i, doorColor);
                }
                break;
            case Direction.WEST:
                for (int i = 1; i <=space; i++)
                {
                    texture.SetPixel(pos.x-i, pos.y, doorColor);
                }
                break;
        }
    }
    texture.Apply();

}*/

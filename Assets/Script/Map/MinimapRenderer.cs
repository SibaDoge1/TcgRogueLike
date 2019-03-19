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
    private static readonly Color wallColor = Color.clear;
    private static readonly Color playerColor = Color.black;
    private static readonly Color enemyColor = Color.red;
    private static readonly Color normalRoomColor = Color.white;
    private static readonly Color startRoomColor = new Color(190 / 255f, 230 / 255f, 245 / 255f);
    private static readonly Color eventRoomColor = new Color(255 / 255f, 230 / 255f, 175 / 255f);
    private static readonly Color bossRoomColor = new Color(255/255f, 200 / 255f, 195 / 255f);

    private static readonly Color doorColor = new Color(160 / 255f, 160 / 255f, 160 / 255f);
    private static readonly Color hallWayColor = new Color(160 / 255f, 160 / 255f, 160 / 255f);

    private static readonly Color floorColor = new Color(235 / 255f, 180 / 255f, 10 / 255f);
    private static readonly Color deckEditColor = new Color(65 / 255f, 100 / 255f, 250 / 255f);
    private static readonly Color saveColor = new Color(180 / 255f, 110 / 255f, 200 / 255f);

    static int space;
    static Color roomColor = startRoomColor;
    static Color oldRoomColor = startRoomColor;
    private static Texture2D texture;
    private static Vector2Int textureSize;
	private static Vector2Int minBorder;
	private static Vector2Int maxBorder;
    private static Vector2Int playerMapPos;
    private static List<Vector2Int> enemyPoses = new List<Vector2Int>();
    static bool isEnterRoom;

    static public  void Init(Map map)
    {
        minBorder = map.MinBorder;
        maxBorder = map.MaxBorder;
        space = map.GetComponent<MapGenerator>().space;
        playerMapPos = new Vector2Int(-1, -1);
        textureSize = maxBorder - minBorder + new Vector2Int(space*2,space*2);
        texture = new Texture2D(textureSize.x,textureSize.y);
        texture.filterMode = FilterMode.Point;
        enemyPoses = new List<Vector2Int>();

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
                if (t.tileNum == 0 && t.OnTileObj == null && t.offTile==null)
                    texture.SetPixel(pos.x+j,pos.y+i, emptyColor);
                else
                {
                    if(t.offTile == null)
                    {
                        if (t.OnTileObj is Structure)
                        {
                            texture.SetPixel(pos.x + j, pos.y + i, wallColor);
                        }
                        else
                        {
                            texture.SetPixel(pos.x + j, pos.y + i, roomColor);
                        }
                    }
                    else
                    {
                        if (t.offTile is OffTile_Door && !((OffTile_Door)t.offTile).IsDestroyed)
                        {
                            texture.SetPixel(pos.x + j, pos.y + i, doorColor);
                        }
                        else if (t.offTile is OffTile_Floor)
                        {
                            texture.SetPixel(pos.x + j, pos.y + i, floorColor);
                        }
                        else if (t.offTile is OffTile_Shop)
                        {
                            texture.SetPixel(pos.x + j, pos.y + i, deckEditColor);
                        }
                        else if (t.offTile is OffTile_Save)
                        {
                            texture.SetPixel(pos.x + j, pos.y + i, saveColor);
                        }else
                        {
                            texture.SetPixel(pos.x + j, pos.y + i, roomColor);
                        }
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
            if(isEnterRoom)
            {
                texture.SetPixel(oldPos.x, oldPos.y, oldRoomColor);
                isEnterRoom = false;
            }else
            {
                texture.SetPixel(oldPos.x, oldPos.y, roomColor);
            }
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
            texture.SetPixel(enemyPoses[i].x, enemyPoses[i].y, roomColor);
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
        return new Vector2Int((int)pos.x, (int)pos.y) - minBorder + new Vector2Int(space, space);
    }
    static public void SetRoomColor(Room room)
    {
        oldRoomColor = roomColor;
        isEnterRoom = true;
        switch (room.roomType)
        {
            case RoomType.BATTLE:
                roomColor = normalRoomColor;
                break;
            case RoomType.BOSS:
                roomColor = bossRoomColor;
                break;
            case RoomType.EVENT:
                roomColor = eventRoomColor;
                break;
            case RoomType.START:
                if (room.RoomName.Contains("end"))
                {
                    roomColor = normalRoomColor;
                }
                else
                {
                    roomColor = startRoomColor;
                }
                break;
        }
    }
    static public void DrawHallWay(Vector3 doorPos,Vector3 connectedPos,Direction d)
    {
        int doorSpace = (int)Vector3.Magnitude(connectedPos - doorPos);
        Vector2Int pos = WorldPosToMapPos(doorPos);
        switch(d)
        {
            case Direction.EAST:
                for(int i=1; i<doorSpace;i++)
                {
                    texture.SetPixel(pos.x+i, pos.y, doorColor);
                }
                break;
            case Direction.NORTH:
                for (int i = 1; i <doorSpace; i++)
                {
                    texture.SetPixel(pos.x, pos.y+i, doorColor);
                }
                break;
            case Direction.SOUTH:
                for (int i = 1; i <doorSpace; i++)
                {
                    texture.SetPixel(pos.x, pos.y-i, doorColor);
                }
                break;
            case Direction.WEST:
                for (int i = 1; i <doorSpace; i++)
                {
                    texture.SetPixel(pos.x - i, pos.y, doorColor);
                }
                break;
        }

    }
}


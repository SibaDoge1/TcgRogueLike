﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapRenderer : MonoBehaviour {
	public static MinimapRenderer instance;
	void Awake(){
		instance = this;
		img_MiniMap = GetComponent<Image> ();
	}

	private readonly Color wallColor = new Color (0f, 0f, 0f, 1f);
	private readonly Color tileColor = new Color (0.4f, 0.4f, 0.4f, 1f);
	private readonly Color playerColor = new Color (0f, 1f, 0f, 1f);
	private const int roomInterval = 1;

	private Image img_MiniMap;
	private Texture2D miniMapTexture;

	private Vector2Int roomArraySize;
	private Vector2Int maxRoomSize;

	private Vector2Int roomPosMax;
	private Vector2Int roomPosMin;

	private Vector2Int playerPixel;

	private bool[,] renderState;
	public void Init(Map map){
		roomPosMax = map.maxRoomPos;
		roomPosMin = map.minRoomPos;
		roomArraySize = roomPosMax - roomPosMin + Vector2Int.one;
		maxRoomSize = map.maxRoomSize;

		renderState = new bool[roomArraySize.x, roomArraySize.y];

		int pixelSizeX = roomArraySize.x * maxRoomSize.x + (roomArraySize.x - 1 * roomInterval);
		int pixelSizeY = roomArraySize.y * maxRoomSize.y + (roomArraySize.y - 1 * roomInterval);
		int textureSize = Mathf.Max (pixelSizeX, pixelSizeY) + 1; // 10칸 여분
		miniMapTexture = new Texture2D (textureSize, textureSize);
		miniMapTexture.filterMode = FilterMode.Point;

		Color[] colors = miniMapTexture.GetPixels ();
		for (int i = 0; i < colors.Length; i++) {
			colors [i] = wallColor;
		}

		miniMapTexture.SetPixels (colors);

		img_MiniMap.sprite = Sprite.Create (miniMapTexture, new Rect (0, 0, textureSize, textureSize), Vector2.zero);

		CalcRoomCenterPixelCoord ();

		RenderRoom (map.StartRoom);
		DoorOpen (map.StartRoom);
		playerPixel = RoomLocalPosToPixelCoord (map.StartRoom, map.StartRoom.GetPlayerTile ().pos);
		miniMapTexture.SetPixel(playerPixel.x, playerPixel.y, playerColor);
		miniMapTexture.Apply ();
	}


	#region Interface
	public void RenderRoom(Room room){
		Vector2Int tileMinPos = RoomLocalPosToPixelCoord (room, Vector2Int.zero);

		for (int x = 0; x < room.size.x; x++) {
			for (int y = 0; y < room.size.y; y++) {
				if (x == 0 || x == room.size.x - 1 || y == 0 || y == room.size.y - 1) {
					miniMapTexture.SetPixel (tileMinPos.x + x, tileMinPos.y + y, wallColor);
				} else {
					miniMapTexture.SetPixel (tileMinPos.x + x, tileMinPos.y + y, tileColor);
				}
			}
		}
		Vector2Int roomIndex = new Vector2Int (room.pos.x - roomPosMin.x, room.pos.y - roomPosMin.y);
		renderState [roomIndex.x, roomIndex.y] = true;


		//Door Link Check
		//Right
		if (roomIndex.x + 1 < roomArraySize.x && renderState [roomIndex.x + 1, roomIndex.y]) {
			int index = roomCenterX [roomIndex.x];
			while (miniMapTexture.GetPixel (index, roomCenterY [roomIndex.y]) != wallColor) {
				index++;
			}
			index++;
			while (miniMapTexture.GetPixel (index, roomCenterY [roomIndex.y]) == wallColor) {
				miniMapTexture.SetPixel (index, roomCenterY [roomIndex.y], tileColor);
				index++;
			}
		}
		//Left
		if (roomIndex.x - 1 >= 0 && renderState [roomIndex.x - 1, roomIndex.y]) {
			int index = roomCenterX [roomIndex.x];
			while (miniMapTexture.GetPixel (index, roomCenterY [roomIndex.y]) != wallColor) {
				index--;
			}
			index--;
			while (miniMapTexture.GetPixel (index, roomCenterY [roomIndex.y]) == wallColor) {
				miniMapTexture.SetPixel (index, roomCenterY [roomIndex.y], tileColor);
				index--;
			}
		}
		//Up
		if (roomIndex.y + 1 < roomArraySize.y && renderState [roomIndex.x, roomIndex.y + 1] ) {
			int index = roomCenterY [roomIndex.y];
			while (miniMapTexture.GetPixel (roomCenterX [roomIndex.x], index) != wallColor) {
				index++;
			}
			index++;
			while (miniMapTexture.GetPixel (roomCenterX [roomIndex.x], index) == wallColor) {
				miniMapTexture.SetPixel (roomCenterX [roomIndex.x], index, tileColor);
				index++;
			}
		}
		//Down
		if (roomIndex.y - 1 >= 0 && renderState [roomIndex.x, roomIndex.y - 1]) {
			int index = roomCenterY [roomIndex.y];
			while (miniMapTexture.GetPixel (roomCenterX [roomIndex.x], index) != wallColor) {
				index--;
			}
			index--;
			while (miniMapTexture.GetPixel (roomCenterX [roomIndex.x], index) == wallColor) {
				miniMapTexture.SetPixel (roomCenterX [roomIndex.x], index, tileColor);
				index--;
			}
		}


		miniMapTexture.Apply ();
	}

	public void DoorOpen(Room room){
		List<Vector2Int> list = room.GetDoorPos ();
		Vector2Int temp;
		for (int i = 0; i < list.Count; i++) {
			temp = RoomLocalPosToPixelCoord (room, list [i]);
			miniMapTexture.SetPixel (temp.x, temp.y, tileColor);
		}
		miniMapTexture.Apply ();
	}

	public void PlayerTileRefresh(Room room){
		if (playerPixel != null) {
			miniMapTexture.SetPixel(playerPixel.x, playerPixel.y, tileColor);
		}
		playerPixel = RoomLocalPosToPixelCoord (room, room.GetPlayerTile ().pos);
		miniMapTexture.SetPixel(playerPixel.x, playerPixel.y, playerColor);
		miniMapTexture.Apply ();
	}
	#endregion

	private int[] roomCenterX;
	private int[] roomCenterY;
	private void CalcRoomCenterPixelCoord(){
		roomCenterX = new int[roomArraySize.x];
		roomCenterY = new int[roomArraySize.y];

		//Calc X
		int centerIndex;
		int centerPos;

		centerIndex = roomArraySize.x / 2;
		if (roomArraySize.x % 2 == 0) {
			centerPos = miniMapTexture.width / 2 + maxRoomSize.x / 2;
		} else {
			centerPos = miniMapTexture.width / 2;
		}
		for (int i = 0; i < roomCenterX.Length; i++) {
			roomCenterX[i] = centerPos + (i - centerIndex) * (maxRoomSize.x - 1 + roomInterval);
		}


		centerIndex = roomArraySize.y / 2;
		if (roomArraySize.y % 2 == 0) {
			centerPos = miniMapTexture.height / 2 + maxRoomSize.y / 2;
		} else {
			centerPos = miniMapTexture.height / 2;
		}
		for (int i = 0; i < roomCenterY.Length; i++) {
			roomCenterY[i] = centerPos + (i - centerIndex) * (maxRoomSize.y - 1 + roomInterval);
		}

	}

	private Vector2Int GetRoomCenter(Vector2Int roomPos){
		return new Vector2Int (roomCenterX [roomPos.x - roomPosMin.x], roomCenterY [roomPos.y - roomPosMin.y]);
	}


	private Vector2Int RoomLocalPosToPixelCoord(Room room, Vector2Int localPos){
		Vector2Int result = new Vector2Int ();

		result.x = roomCenterX [room.pos.x - roomPosMin.x] - room.size.x / 2 + localPos.x;
		result.y = roomCenterY [room.pos.y - roomPosMin.y] - room.size.y / 2 + localPos.y;
		return result;
	}

}
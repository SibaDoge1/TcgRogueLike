using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapRenderer : MonoBehaviour {
	public static MinimapRenderer instance;
	void Awake(){
		instance = this;
		img_MiniMap = GetComponent<Image> ();
	}

	private const int roomInterval = 1;

	private Image img_MiniMap;
	private Texture2D miniMapTexture;

	private Vector2Int roomArraySize;
	private Vector2Int roomPosMax;
	private Vector2Int roomPosMin;
	public void Init(Map map){
		//Get BoundarySize From Map.cs
		int roomSizeMaxX = 18;
		int roomSizeMaxY = 12;
		roomArraySize.x = 5;
		roomArraySize.y = 3;

		int pixelSizeX = roomArraySize.x * roomSizeMaxX + (roomArraySize.x - 1 * roomInterval);
		int pixelSizeY = roomArraySize.y * roomSizeMaxY + (roomArraySize.y - 1 * roomInterval);
		int textureSize = Mathf.Max (pixelSizeX, pixelSizeY);

		miniMapTexture = new Texture2D (textureSize, textureSize);

	}

	public void RenderRoom(Room room){
		
	}

	private Vector2Int GetRoomCenter(Vector2Int roomPos){
		return roomCenter [roomPos.x - roomPosMin.x, roomPos.y - roomPosMin.y];
	}

	private Vector2Int[,] roomCenter;
	private void CalcRoomCenterPixelCoord(){
		roomCenter = new Vector2Int[roomArraySize.x, roomArraySize.y];

		Vector2Int result = new Vector2Int ();
		if (roomArraySize.x % 2 == 0) {
			
		} else {
			
		}

	}

	public Vector2Int RoomLocalPosToPixelCoord(Room room, Vector2Int localPos){
		
	}

	public void PlayerTileRefresh(){
		
	}

}

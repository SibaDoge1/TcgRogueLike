using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapRenderer : MonoBehaviour {
	public static MinimapRenderer instance;
	void Awake(){
		instance = this;
	}

	public void Init(Map map){
		
	}

	public void RefreshRoom(Room room){
		
	}
}

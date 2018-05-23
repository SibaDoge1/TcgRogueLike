using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateDelegate : MonoBehaviour {
	private static InstantiateDelegate instance;
	void Awake(){
		instance = this;
	}

	public static GameObject Instantiate(GameObject prefab){
		return instance.CreateNewGameObject (prefab);
	}




	private GameObject CreateNewGameObject(GameObject prefab){
		return Instantiate (prefab);
	}
}

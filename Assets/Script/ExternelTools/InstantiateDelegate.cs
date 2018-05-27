using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateDelegate : MonoBehaviour {
	/*private static InstantiateDelegate instance;
	void Awake(){
		instance = this;
	}*/

	public static GameObject ProxyInstantiate(GameObject prefab){
		//return instance.CreateNewGameObject (prefab);
		return Instantiate(prefab);
	}
	public static GameObject ProxyInstantiate(GameObject prefab, Transform parent){
		//return instance.CreateNewGameObject (prefab, parent);
		return Instantiate(prefab, parent);
	}

	/*
	private GameObject CreateNewGameObject(GameObject prefab){
		return Instantiate (prefab);
	}
	private GameObject CreateNewGameObject(GameObject prefab, Transform parent){
		return Instantiate (prefab, parent);
	}*/
}

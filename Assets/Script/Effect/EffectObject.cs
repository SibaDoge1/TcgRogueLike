using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectObject : MonoBehaviour {
	public float destroyTime = 1;

	protected virtual void Start(){
		Destroy (gameObject, destroyTime);
	}
}

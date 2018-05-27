using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TextColorType{Red, Green}
public class EffectText : EffectObject {
	private TextMesh tMesh;
	public void Init(string text, TextColorType colorType_){
		colorType = colorType_;
		Transform child = transform.GetChild (0);
		tMesh = child.GetComponent<TextMesh> ();
		tMesh.text = text;
		child.GetComponent<MeshRenderer> ().sortingLayerName = "Effect";

		StartCoroutine (FadeOutRoutine ());
	}

	public TextColorType colorType;
	IEnumerator FadeOutRoutine(){
		float timer = 1;
		Color meshColor = Color.clear;
		switch(colorType){
		case TextColorType.Red:
			meshColor = new Color (1, 0, 0, 1);
			break;
		case TextColorType.Green:
			meshColor = new Color (0, 1, 0, 1);
			break;
		}
		while (true) {
			timer -= Time.deltaTime * 2;
			if (timer < 0) {
				break;
			}
			meshColor.a = timer;
			tMesh.color = meshColor;
			yield return null;
		}
		tMesh.color = Color.clear;
	}
}

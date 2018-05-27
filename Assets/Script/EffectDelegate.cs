using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;

public enum CardEffectType{Slash, Blood, Heal, Hit}
public class EffectDelegate : MonoBehaviour {
	public static EffectDelegate instance;
	void Awake(){
		instance = this;
	}


	[NamedArrayAttribute (new string[]{
		"Slash","Blood", "Heal", "Hit"
	})]
	public GameObject[] effectPrefabs;
	public GameObject textEffectPrefab;


	public void MadeEffect(CardEffectType eType, Transform parent){
		Instantiate (effectPrefabs [(int)eType], parent);
	}
	public void MadeEffect(CardEffectType eType, OnTileObject parent){
		Instantiate (effectPrefabs [(int)eType], parent.transform);
	}
	public void MadeEffect(CardEffectType eType, Vector3 worldPosition){
		Instantiate (effectPrefabs [(int)eType], worldPosition, Quaternion.identity);
	}
	public void MadeEffect(CardEffectType eType, Tile targetTile){
		Instantiate (effectPrefabs [(int)eType], targetTile.transform.position, Quaternion.identity);
	}
		

	public void MadeEffect(int damage, Transform parent){
		Instantiate (textEffectPrefab, parent).GetComponent<EffectText> ()
			.Init (damage.ToString (), damage >= 0 ? TextColorType.Green : TextColorType.Red);
	}
	public void MadeEffect(int damage, OnTileObject parent){
		Instantiate (
			textEffectPrefab, parent.transform).GetComponent<EffectText> ()
			.Init (damage.ToString (), damage >= 0 ? TextColorType.Green : TextColorType.Red);
	}
	public void MadeEffect(int damage, Vector3 worldPosition){
		Instantiate (
			textEffectPrefab, worldPosition, Quaternion.identity).GetComponent<EffectText> ()
			.Init (damage.ToString (), damage >= 0 ? TextColorType.Green : TextColorType.Red);
	}
	public void MadeEffect(int damage, Tile targetTile){
		Instantiate (
			textEffectPrefab, targetTile.transform.position, Quaternion.identity).GetComponent<EffectText> ()
			.Init (damage.ToString (), damage >= 0 ? TextColorType.Green : TextColorType.Red);
	}
}

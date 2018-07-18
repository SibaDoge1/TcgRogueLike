using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;

public enum CardEffectType{Slash, Blood, Heal, Hit}
public enum BulletType{Stone, Arrow}
public class EffectDelegate : MonoBehaviour {
	public static EffectDelegate instance;
	void Awake(){
		instance = this;
	}


	[NamedArrayAttribute (new string[]{
		"Slash", "Blood", "Heal", "Hit"
	})]
	public GameObject[] effectPrefabs;
	[NamedArrayAttribute (new string[]{
		"Stone", "Arrow"
	})]
	public GameObject[] bulletPrefabs;
	public GameObject textEffectPrefab;


	public void MadeEffect(CardEffectType eType, Transform parent){
		Instantiate (effectPrefabs [(int)eType], parent);
	}
	public void MadeEffect(CardEffectType eType, Entity parent){
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
	public void MadeEffect(int damage, Entity parent){
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

	public void MadeBullet(BulletType bType, Vector3 startPos, Transform target){
		(Instantiate (bulletPrefabs [(int)bType], startPos, Quaternion.identity)as GameObject).GetComponent<Bullet> ().Shoot (target);
	}
}

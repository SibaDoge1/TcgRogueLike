using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;

public enum CardEffectType{Slash, Blood, Heal, Hit}
public enum BulletType{Stone, Arrow}
public enum RangeType {CARD,ENEMY }
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
    public GameObject[] rangeLayer;

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
        if (damage == 0)
            return; 

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

    Dictionary<Tile, GameObject> tileToRange = new Dictionary<Tile, GameObject>();
    public Tile MadeRange(RangeType range, Tile targetTile)
    {
        if (targetTile == null || targetTile.OnTileObj is Structure)
            return null;
        else if(tileToRange.ContainsKey(targetTile))
        {
            DestroyImmediate(tileToRange[targetTile]);
            tileToRange.Remove(targetTile);
        }
        
     tileToRange.Add(targetTile, Instantiate(rangeLayer[(int)range],
     targetTile.transform.position, Quaternion.identity));

        return targetTile;        
    }
    public void DeleteRange(Tile targetTile)
    {
        if (targetTile == null || !tileToRange.ContainsKey(targetTile))
            return;
        else
        {
           DestroyImmediate(tileToRange[targetTile]);
           tileToRange.Remove(targetTile);
        }
    }
}

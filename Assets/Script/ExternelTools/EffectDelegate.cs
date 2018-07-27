using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;

public enum CardEffectType{Slash, Blood, Heal, Hit}
public enum BulletType{Stone, Arrow}
public enum StatusEffectType {Spider}
public enum RangeEffectType {CARD,ENEMY}
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
    public GameObject[] rangeLayer;
    public GameObject[] statusEffectPrefab;
    public GameObject textEffectPrefab;

    public GameObject MadeEffect(CardEffectType eType, Transform parent){
        return Instantiate(effectPrefabs [(int)eType], parent);
	}
	public GameObject MadeEffect(CardEffectType eType, Entity parent){
        return Instantiate(effectPrefabs [(int)eType], parent.transform);
	}
	public GameObject MadeEffect(CardEffectType eType, Vector3 worldPosition){
        return Instantiate(effectPrefabs [(int)eType], worldPosition, Quaternion.identity);
	}
	public GameObject MadeEffect(CardEffectType eType, Tile targetTile){
        return Instantiate(effectPrefabs [(int)eType], targetTile.transform.position, Quaternion.identity);
	}
	public GameObject MadeEffect(StatusEffectType eType, Entity parent)
    {
        return Instantiate(statusEffectPrefab[(int)eType], parent.transform);
    }
    public GameObject MadeEffect(StatusEffectType eType, Transform parent)
    {
        MadeEffect(StatusEffectType.Spider, PlayerControl.Player);
        return Instantiate(statusEffectPrefab[(int)eType], parent);
    }
    public void DestroyEffect(GameObject go)
    {
        DestroyImmediate(go);
    }
	public GameObject MadeEffect(int damage, Transform parent){
        GameObject go = Instantiate(textEffectPrefab, parent);
        go.GetComponent<EffectText>().Init(damage.ToString(), damage >= 0 ? TextColorType.Green : TextColorType.Red);
        return go;
    }
	public GameObject MadeEffect(int damage, Entity parent){
        GameObject go =  Instantiate(textEffectPrefab, parent.transform);
        go.GetComponent<EffectText>().Init(damage.ToString(), damage >= 0 ? TextColorType.Green : TextColorType.Red);
        return go;
    }
	public GameObject MadeEffect(int damage, Vector3 worldPosition){
        if (damage == 0)
            return null;

        GameObject go= Instantiate(
            textEffectPrefab, worldPosition, Quaternion.identity);
        go.GetComponent<EffectText>()
            .Init(damage.ToString(), damage >= 0 ? TextColorType.Green : TextColorType.Red);
        return go;
    }
	public GameObject MadeEffect(int damage, Tile targetTile){
        GameObject go = Instantiate(
            textEffectPrefab, targetTile.transform.position, Quaternion.identity);
        go.GetComponent<EffectText>()
            .Init(damage.ToString(), damage >= 0 ? TextColorType.Green : TextColorType.Red);
        return go;
    }

	public GameObject MadeBullet(BulletType bType, Vector3 startPos, Transform target){
        GameObject go = (Instantiate(bulletPrefabs[(int)bType], startPos, Quaternion.identity) as GameObject);
        go.GetComponent<Bullet>().Shoot(target);
        return go;
    }

    public GameObject MadeEffect(RangeEffectType range, Tile targetTile)
    {
        if (targetTile == null || targetTile.OnTileObj is Structure)
            return null;
        GameObject go = Instantiate(rangeLayer[(int)range],
     targetTile.transform.position, Quaternion.identity);

        return go;        
    }
    public void DestroyEffect(List<GameObject> go)
    {
        if(go != null)
        {
            for (int i = 0; i < go.Count; i++)
            {
                DestroyImmediate(go[i]);
            }
        }
    }
}

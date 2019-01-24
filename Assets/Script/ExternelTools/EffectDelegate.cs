using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;

public enum CardEffectType{Shield, Blood, Heal, Hit}
public enum RangeEffectType {CARD,ENEMY,DIR}
public enum UIEffect {CARD,REPORT }
public class EffectDelegate : MonoBehaviour {
	public static EffectDelegate instance;
	void Awake()
    {
		instance = this;
	}



	public GameObject[] effectPrefabs;
    public GameObject[] rangeLayer;
    public GameObject[] UIEffect;
    public GameObject textEffectPrefab;

    public GameObject MadeEffect(UIEffect eType,Transform parent)
    {
        return Instantiate(UIEffect[(int)eType], parent);
    }
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

    public GameObject MadeEffect(RangeEffectType range, Tile targetTile)
    {
        if (targetTile == null || targetTile.OnTileObj is Structure || targetTile.tileNum ==0)
            return null;
        GameObject go = Instantiate(rangeLayer[(int)range],
     targetTile.transform.position, Quaternion.identity);

        return go;        
    }

    /// <summary>
    /// Range Effect Delete할때 사용
    /// </summary>
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

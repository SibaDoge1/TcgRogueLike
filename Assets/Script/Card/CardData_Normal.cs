using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;


public class CardData_Normal : CardData {
	public CardData_Normal(){}
	public CardData_Normal(int index) : base(index){
		//TODO : TEMP

		range = 1;
		target = 1;
		damage = 1;
	}

	protected int range;
	protected int target;
	protected int damage;

	public override void CardActive (){
		int validTarget = target;
		//Check target in range
		for (int i = 0; i < validTarget; i++) {
			
		}
	}

	public override CardAbilityType GetCardAbilityType (){
		return CardAbilityType.Attack;
	}
	public override string GetCardAbilityValue (){
		return damage.ToString();
	}
}

public class CardData_Sword : CardData_Normal {
	public CardData_Sword(int index) : base(index)
	{
		damage = 5;
		range = 1;
		//Set ImageInfo
		cardExplain = range + "의 범위중 한 적에게+" + damage + "의 데미지를 줍니다.";
	}
	public override void CardActive (){
		Entity target = null;
		if (TileUtils.IsHitableAround (GameManager.instance.GetCurrentRoom ().GetPlayerTile (), range)) {
			target = TileUtils.AutoTarget (GameManager.instance.GetCurrentRoom ().GetPlayerTile (), range);
			target.currentHp -= damage;
			EffectDelegate.instance.MadeEffect (effectType, target);
		}
	}

	private List<Tile> targetTiles;
	public override void CardEffectPreview (){
		targetTiles = TileUtils.SquareRange (GameManager.instance.GetCurrentRoom ().GetPlayerTile (), range);
		for(int i = 0; i < targetTiles.Count; i++){
			targetTiles [i].mySprite.color = Color.red;
		}
	}
	public override void CancelPreview (){
		for(int i = 0; i < targetTiles.Count; i++){
			targetTiles [i].mySprite.color = Color.white;
		}
	}
}
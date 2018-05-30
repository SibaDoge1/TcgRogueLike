using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData_Magic : CardData {
	public CardData_Magic(){}
	public CardData_Magic(int index) : base(index){

	}


	public override void CardActive (){

	}
}

public class CardData_Reload : CardData_Magic{
	public CardData_Reload(){}
	public CardData_Reload(int index) : base(index){
		cardExplain = "체력을 1잃고 내 덱을 처음 상태로 복구합니다.";
	}

	public override void CardActive(){
        PlayerControl.instance.PlayerObject.currentHp -= 1; 
		PlayerControl.instance.ReLoadDeck ();
	}

	public override CardAbilityType GetCardAbilityType (){
		return CardAbilityType.Util;
	}
	public override string GetCardAbilityValue (){
		return "RE";
	}
}

public class CardData_Bandage : CardData_Magic{
	public CardData_Bandage(){}
	public CardData_Bandage(int index) : base(index){
		cardExplain = "자신의 hp를" + healAmount + "만큼 회복합니다.";
		effectType = CardEffectType.Heal;
	}

	int healAmount = 3;

	public override void CardActive (){
        GameManager.instance.GetCurrentRoom().GetPlayerTile().OnTileObj.currentHp += healAmount;
		EffectDelegate.instance.MadeEffect (effectType, PlayerControl.instance.PlayerObject);
    }

	public override CardAbilityType GetCardAbilityType (){
		return CardAbilityType.Heal;
	}
	public override string GetCardAbilityValue (){
		return healAmount.ToString();
	}
}
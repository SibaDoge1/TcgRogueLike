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

public class CardData_Bandage : CardData_Magic{
	public CardData_Bandage(){}
	public CardData_Bandage(int index) : base(index){
		cardExplain = "자신의 hp를" + healAmount + "만큼 회복합니다.";
	}

	int healAmount = 3;

	public override void CardActive (){
		//TODO : PLAYER HEAL
	}
}

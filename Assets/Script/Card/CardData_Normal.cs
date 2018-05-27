using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
}

public class CardData_Sword : CardData_Normal {
	public CardData_Sword(int index) : base(index)
	{
		damage = 5;
		range = 1;
		//Set ImageInfo
		cardExplain = range + "의 범위에 적에게+" + damage + "의 데미지를 줍니다.";
	}
	public override void CardActive (){
		int validTarget = target;
		//Check target in range
		for (int i = 0; i < validTarget; i++) {

		}
	}
}
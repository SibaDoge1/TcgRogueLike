using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDatabase {
	public static CardData GetCardData(int cardIndex){

		return new CardData_Normal (cardIndex);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDatabase {
	public static CardData GetCardData(int cardIndex){

		return new CardData_Normal (cardIndex);
	}

	public const string cardResourcePath = "Card/";
	public const string cardObjectPath = "Card/CardBase";
	public static readonly string[] cardSpritePaths = {
		"card_sword",
		"card_heal"
	};
	public static readonly string[] cardNames = {
		"짱짱쎈 칼",
		"짱짱 회복"
	};
	public static readonly string[] cardExplains = {
		"아 칼 아시는구나.\n이 칼은 말이죠\n존.나.쎕.니.다.",
		"피통이 Char올라"
	};
}

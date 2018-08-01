using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CardDatabase {

	public const string cardResourcePath = "Card/";
	public const string cardObjectPath = "Card/CardBase";
    public const string editCardObjectPath = "Card/EditCard";
	public static readonly string[] cardSpritePaths = {
		"card_reload",
		"card_sword",
		"card_heal",
		"card_bfsword",
		"card_tumble",

		"card_stone",
		"card_portion",
		"card_arrow"
	};
	public static readonly string[] cardNames = {
		"Re:제로",
		"짱짱쎈 칼",
		"짱짱 회복",
		"B.F.대검",
		"구르기",

		"짱돌",
		"신-비한 물럌",
		"활이다!"
	};
}

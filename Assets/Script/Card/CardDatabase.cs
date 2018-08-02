using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CardDatabase {

    public const string reloadSpritePath = "reload";
    public const string reloadNamePath = "리로드";
    public const string reloadInfoPath = "리로드하고 1의 데미지를 받습니다.";


    public const string cardResourcePath = "Card/Graphic/";
	public const string cardObjectPath = "Card/CardBase";
    public const string editCardObjectPath = "Card/EditCard";
	public static readonly string[] cardSpritePaths = 
    {
        "cross",
        "x",
        "square",
        "squareAll",
        "mid3Att",
        "pierce",
        "strSquareAll"
	};
	public static readonly string[] cardNames = {
        "십자공격",
        "X자 공격",
        "광범위 공격",
        "광범위 총공격",
        "중범위 3공격",
        "관통 공격",
        "강한 광역공격"
	};
    public static readonly string[] cardInfos =
    {
        "범위 내에서 5데미지만큼 오토타겟 합니다.",
        "범위 내에서 5데미지만큼 오토타겟 합니다.",
        "범위 내에서 5데미지만큼 오토타겟 합니다.",
        "범위 내에서 5데미지만큼 전부 공격 합니다.",
        "범위 내에서 5데미지만큼 3마리 공격 합니다.",
        "범위 내에서 5데미지만큼 전부 공격합니다.",
        "범위 내에서 10데미지만큼 전부 공격합니다."
    };
}

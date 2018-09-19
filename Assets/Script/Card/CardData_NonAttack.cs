using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData_NonAttack : CardData {
	public CardData_NonAttack(){}


    public override CardAbilityType GetCardAbilityType()
    {
        return CardAbilityType.NonAttack;
    }
}

public class CardData_Reload : CardData_NonAttack{
	public CardData_Reload()
    {
        SetData();
	}

    protected override void SetData()
    {
        cardName = CardDatabase.reloadNamePath;
        spritePath = CardDatabase.reloadSpritePath;
        cardExplain = CardDatabase.reloadInfoPath;
    }
    protected override void CardActive()
    {
		PlayerControl.instance.ReLoadDeck ();
	}
}

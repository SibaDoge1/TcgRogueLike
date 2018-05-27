using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour {

	private Deck deck;
	private HandCard hand;

	public void DrawCard(){
		hand.DrawHand (deck.Draw ());
	}

	public void ReLoadDeck(){
		deck.Load ();
	}

	/// <summary>
	/// Call from GameManager
	/// </summary>
	public void OnStartPlayerTurn(){
		hand.CheckAvailable ();
	}

	/// <summary>
	/// Call from GameManager(Move) or CardObject(Card)
	/// </summary>
	public void EndTurn(){
		//TODO : GameManager End Callback
	}

	public void BattleEncount(){
		
	}


	public void EndBattle(){



	}

	private void ShowHand(){
		
	}

	private void HideHand(){
		
	}

	#region Private

	#endregion


}

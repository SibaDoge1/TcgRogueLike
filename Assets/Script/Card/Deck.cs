using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour {

	public Text txt_RemainCard;
	public List<CardData> deck;

	public CardObject Draw(){
		if (deck.Count <= 0) {
			return null;
		}
		CardData c = deck [deck.Count - 1];
		deck.RemoveAt(deck.Count - 1);
		RefreshText ();
		return c.Instantiate ();
	}

	public void Load(){
		deck = new List<CardData> (PlayerData.deck);
		Shuffle ();
		deck.Add (deck [0]);
		deck [0] = new CardData_Reload (0);
		RefreshText ();
	}

	#region Private

	private void Shuffle(){
		CardData temp;
		int randIndex;
		for (int i = 0; i < deck.Count - 1; i++) {
			randIndex = Random.Range (0, deck.Count);
			temp = deck [i];
			deck [i] = deck [randIndex];
			deck [randIndex] = temp;
		}
	}

	private void RefreshText(){
		txt_RemainCard.text = "X " + deck.Count; 
	}
	#endregion
}

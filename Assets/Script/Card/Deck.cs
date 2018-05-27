using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour {

	public List<CardData> deck;

	public CardObject Draw(){
		CardData c = deck [deck.Count - 1];
		deck.RemoveAt(deck.Count - 1);
		return c.Instantiate ();
	}

	public void Load(){
		deck = new List<CardData> (PlayerData.deck);
		Shuffle ();
	}

	#region Private

	private void Shuffle(){
		CardData temp;
		int randIndex;
		for (int i = 0; i < deck.Count; i++) {
			randIndex = Random.Range (0, deck.Count);
			temp = deck [i];
			deck [i] = deck [randIndex];
			deck [randIndex] = temp;
		}
	}

	#endregion
}

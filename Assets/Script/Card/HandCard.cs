using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCard : MonoBehaviour {
	private List<CardObject> hand;

	//Add from Deck
	public void DrawHand(CardObject cardObject){
		cardObject.transform.parent = transform;
		hand.Add (cardObject);
		SetCardPosition ();
	}

	//Add from Others (different animation)
	public void AddHand(CardObject cardObject){
		
	}

	public void CheckAvailable(){
		for (int i = 0; i < hand.Count; i++) {
			if (hand [i].IsAvailable ()) {
				//TODO : Available Effect
			} else {
				//TODO : Disable Effect
			}
		}
	}


	#region Private
	private void SetCardPosition(){
		for (int i = 0; i < hand.Count; i++) {
			hand [i].SetLocation (hand.Count, i);
		}
	}
	#endregion
}

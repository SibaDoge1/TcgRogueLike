using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCard : MonoBehaviour {
	public int CurrentHandCount{
		get{ return hand.Count; }
	}
	public Transform drawStartPosition;
	public Transform cardFoldPosition;
	private List<CardObject> hand = new List<CardObject> ();
	private bool isHided = false;

	//Add from Deck
	public void DrawHand(CardObject cardObject){
		cardObject.SetParent (this);
		cardObject.transform.position = drawStartPosition.position;
		cardObject.transform.localScale = Vector3.one;

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

	private Coroutine handRoutine;
	private IEnumerator HandRoutine(bool isHided){
		float timer = 0;
		Vector3 targetScale = Vector3.one;
		Vector3 targetPosition = Vector3.zero;
		targetPosition.z = 10;
		if (isHided) {
			targetScale = Vector3.one * 0.3f;
			targetPosition = new Vector3 (5f, -2.5f, 10f);
		}
		while (true) {
			timer += Time.deltaTime;
			if (timer > 1) {
				transform.localScale = targetScale;
				transform.localPosition = targetPosition;
				break;
			}
			transform.localScale = Vector3.Lerp (transform.localScale, targetScale, timer);
			transform.localPosition = Vector3.Lerp (transform.localPosition, targetPosition, timer);
			yield return null;
		}
	}

	public void ToggleHand(){
		if(isHided){
			ShowAll ();
		}else{
			HideAll ();
		}
	}

	public void RemoveCard(CardObject co){
		hand.Remove (co);
		SetCardPosition ();
	}

	#region Private
	private void SetCardPosition(){
		for (int i = 0; i < hand.Count; i++) {
			hand [i].SetLocation (hand.Count, i, isHided);
		}
	}

	private void HideAll(){
		isHided = true;
		SetCardPosition ();
		if (handRoutine != null) {
			StopCoroutine (handRoutine);
		}
		handRoutine = StartCoroutine (HandRoutine (isHided));
	}
	private void ShowAll(){
		isHided = false;
		SetCardPosition ();
		if (handRoutine != null) {
			StopCoroutine (handRoutine);
		}
		handRoutine = StartCoroutine (HandRoutine (isHided));
	}
	#endregion
}

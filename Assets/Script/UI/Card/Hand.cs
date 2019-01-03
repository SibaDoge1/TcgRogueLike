﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Hand
/// </summary>
public class Hand : MonoBehaviour {
	private Vector3 originLocalPosition;
	void Awake()
    {
		originLocalPosition = transform.localPosition;
        drawStartPosition = transform.Find("CardDrawPosition");
        cardinfo = transform.Find("CardInfoPanel").GetComponent<CardInfoPanel>();
	}


	public int CurrentHandCount{
		get{ return hand.Count; }
	}

    public GameObject joyStick;
	private Transform drawStartPosition;
    private CardInfoPanel cardinfo;
    private bool isHided;
	private List<CardObject> hand = new List<CardObject> ();

    public void CardInfoOn(Card c)
    {
        cardinfo.gameObject.SetActive(true);
        cardinfo.SetText(c.CardData.name,c.GetCardInfoString());
        cardinfo.SetRender(c.CardData.spritePath);
        cardinfo.SetAttribute(c.CardData.attribute);
    }
    public void CardInfoOff()
    {
        cardinfo.gameObject.SetActive(false);
    }
    public void SetJoyStick(bool b)
    {
        joyStick.gameObject.SetActive(b);
    }
	//Add from Deck
	public void DrawHand(CardObject cardObject){
		if(cardObject == null){
			return;
		}

		cardObject.SetParent (this);
		cardObject.transform.position = drawStartPosition.position;
		cardObject.transform.localScale = Vector3.one;

		hand.Add (cardObject);
		SetCardPosition ();
	}

	//Add from Others (different animation)
	public void AddHand(CardObject cardObject){
		if(cardObject == null){
			return;
		}
		cardObject.SetParent (this);
		cardObject.transform.localPosition = Vector3.zero;
		cardObject.transform.localScale = Vector3.one;

		hand.Add (cardObject);
		StartCoroutine (AddHandRoutine (cardObject));
	}

	private IEnumerator AddHandRoutine(CardObject co){
		co.transform.localScale = Vector3.zero;
		float timer = 0;
		while (true) {
			timer += Time.deltaTime;
			if (timer > 0.5f) {
				break;
			}
			co.transform.localScale = Vector3.Lerp (co.transform.localScale, Vector3.one, 0.1f);	
			yield return null;
		}
		SetCardPosition ();
	}


	private Coroutine handRoutine;
	private IEnumerator HandRoutine(bool isHided){
		float timer = 0;
		Vector3 targetScale = Vector3.one;
		Vector3 targetPosition = new Vector3(0,0,0);
		targetPosition.z = 10;
		if (isHided) {
			targetScale = Vector3.one * 0.3f;
			targetPosition = new Vector3(0, -300, 0);
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

	public void ToggleHand()
    {
		if(isHided)
        {
            ShowAll();
		}else{
            HideAll();
        }
    }
    public void EnableCards(bool enable)
    {
        if(enable)
        {
            for(int i=0; i<hand.Count;i++)
            {
                hand[i].EnableInteraction(true);
            }
        }
        else
        {
            for (int i = 0; i < hand.Count; i++)
            {
                hand[i].EnableInteraction(false);
            }
        }
    }

	public void RemoveCard(CardObject co){
		hand.Remove (co);
		SetCardPosition ();
	}
    public void RemoveAll()
    {
        for(int i = hand.Count-1; i>=0;i--)
        {
            hand[i].Remove();
        }
    }

	public void ChooseOne(){
		if (moveRoutine != null) {
			StopCoroutine (moveRoutine);
		}
		moveRoutine = StartCoroutine (HandMoveRoutine (originLocalPosition - new Vector3 (0, 1, 0)));
	}

	public void ChooseRollback(){
		if (moveRoutine != null) {
			StopCoroutine (moveRoutine);
		}
		moveRoutine = StartCoroutine (HandMoveRoutine (originLocalPosition));
	}
	private Coroutine moveRoutine;
	IEnumerator HandMoveRoutine(Vector3 targetLocalPosition){
		float timer = 0;
		while (true) {
			timer += Time.deltaTime;
			if (timer > 1) {
				transform.localPosition = targetLocalPosition;
				break;
			}
			transform.localPosition = Vector3.Lerp (transform.localPosition, targetLocalPosition, 0.1f);
			yield return null;
		}	
	}
	#region Private
	private void SetCardPosition(){
		for (int i = 0; i < hand.Count; i++) {
			hand [i].SetLocation (hand.Count, i, isHided);
		}
	}

	private void HideAll()
    {
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
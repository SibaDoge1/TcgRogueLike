using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Hand
/// </summary>
public class HandManager : MonoBehaviour {
	private Vector3 originLocalPosition;
    private Vector3 hideLocalPosition = new Vector3(5000, 0);
	void Awake()
    {
		originLocalPosition = transform.localPosition;
        drawStartPosition = transform.Find("CardDrawPosition");
        MakeCards(5);
	}

    private void MakeCards(int num)
    {
        for(int i=0; i< num; i++)
        {
            HandCardObject cardObject = ArchLoader.instance.GetCardObject();
            deactiveList.Add(cardObject);
            cardObject.gameObject.SetActive(false);
            cardObject.SetParent(transform);
            cardObject.SetHand(this);
        }
    }
    private HandCardObject ActiveCard()
    {
        HandCardObject active = deactiveList[0];
        activeList.Add(active);
        deactiveList.Remove(active);
        active.gameObject.SetActive(true);
        return active;
    }
    public void DeActiveCard(HandCardObject handCard)
    {
        if(!deactiveList.Contains(handCard))
        {
            deactiveList.Add(handCard);
            handCard.gameObject.SetActive(false);
            SetCardPosition();
        }
    }


    public int CurrentHandCount{
		get{ return activeList.Count; }
	}

    public GameObject joyStick;
	private Transform drawStartPosition;
    private bool isHided;
	private List<HandCardObject> activeList = new List<HandCardObject> ();
    private List<HandCardObject> deactiveList = new List<HandCardObject>();

    public List<Card> GetHandCardList()
    {
        List<Card> cards = new List<Card>();

        for(int i=0; i<activeList.Count;i++)
        {
            cards.Add(activeList[i].Data);
        }

        return cards;
    }
    public bool isOnHand(Card data)
    {
        for(int i=0; i<activeList.Count; i++)
        {
            if(data == activeList[i].Data)
            {
                return true;
            }
        }
        return false;
    }
    public void SetJoyStick(bool b)
    {
        joyStick.gameObject.SetActive(b);
    }

	//Add from Deck
	public void DrawHand(Card data){
		if(data == null){
			return;
		}
        HandCardObject newCard = ActiveCard();
        newCard.SetCardData(data);
        newCard.transform.position = drawStartPosition.position;

		SetCardPosition ();
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





    /// <summary>
    /// ActiveList에서 지우기만 , DeActivate는 따로해줘야함
    /// 일케안하면 오류생김
    /// </summary> 
    public void RemoveFromActive(HandCardObject co)
    {
        activeList.Remove(co);
        SetCardPosition();
    }
    

    public void DumpCard()
    {
            activeList[0].DumpCard();
    }


    public void DumpAll()
    {
        for(int i = activeList.Count-1; i>=0;i--)
        {
            activeList[i].DumpCard();
        }
    }

    public void ReturnCard()
    {
        if(PlayerControl.player.currentRoom.IsEnemyAlive())
        {
            activeList[0].ReturnCard();
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
		for (int i = 0; i < activeList.Count; i++) {
			activeList [i].SetLocation (activeList.Count, i, isHided);
		}
	}

	public void On()
    {
        transform.localPosition = originLocalPosition;
	}
	public void Off()
    {
        transform.localPosition = hideLocalPosition;
	}
	#endregion
}

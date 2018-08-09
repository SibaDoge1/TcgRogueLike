using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public delegate void LocateCallback();
public class CardObject : MonoBehaviour, IDragHandler,IPointerDownHandler,IPointerUpHandler{

    private Hand hand;
    private CardRender render;
    private Image rayCaster;
    private CardData data;

    protected void Awake()
    {
        render = transform.Find("render").GetComponent<CardRender>();
        rayCaster = render.transform.Find("Frame").GetComponent<Image>();
    }



	public void SetCardRender(CardData data_){
		data = data_;
        render.Img_Graphic.sprite = Resources.Load<Sprite>(CardDatabase.cardResourcePath + data_.SpritePath);
        render.Name.text = data.CardName;
		CardAbilityType a = data.GetCardAbilityType ();
		switch (a) {
		case CardAbilityType.Attack:
                {//속성 불러오기
                    render.Img_Attribute.enabled = true;
                    Attribute at = data.CardAtr;
                    switch (at)
                    {
                        case Attribute.AK:
                            render.Img_Attribute.sprite = Resources.Load<Sprite>("Attribute/akasha1");
                            break;
                        case Attribute.APAS:
                            render.Img_Attribute.sprite = Resources.Load<Sprite>("Attribute/apas1");
                            break;
                        case Attribute.PRITHVI:
                            render.Img_Attribute.sprite = Resources.Load<Sprite>("Attribute/prithivi1");
                            break;
                        case Attribute.TEJAS:
                            render.Img_Attribute.sprite = Resources.Load<Sprite>("Attribute/tejas1");
                            break;
                        case Attribute.VAYU:
                            render.Img_Attribute.sprite = Resources.Load<Sprite>("Attribute/vayu1");
                            break;
                    }
                }
                break;
            case CardAbilityType.Util:
                break;
		}
	}

	public void SetParent(Hand hand_)
    {
		hand = hand_;
		transform.SetParent(hand.transform);
	}

	public bool IsAvailable()
    {
		return data.IsAvailable ();
	}


	#region UserInput
	private const float handYOffset = -0.8f;
	private Vector3 originPos;
	private const int DragThreshold = 40;
	private const int ActiveThreshold = 200;
    public void OnPointerDown(PointerEventData ped){
		hand.ChooseOne ();
		data.CardEffectPreview ();

        hand.CardInfoOn(data);

        if (locateRoutine != null) {
			StopCoroutine (locateRoutine);
		}
		base.transform.rotation = Quaternion.identity;
	}

	public void OnPointerUp(PointerEventData ped){
		hand.ChooseRollback ();
		data.CancelPreview ();
        hand.CardInfoOff();

        if (((Vector2)base.transform.localPosition - (Vector2)originPos).magnitude > ActiveThreshold && GameManager.instance.CurrentTurn == Turn.PLAYER && GameManager.instance.GetCurrentRoom().IsEnemyAlive()) {
            hand.RemoveCard (this);
            ActiveSelf();
            Destroy(gameObject);
		} else {
            transform.localScale = Vector3.one;
            //transform.localPosition = originPos;
            locateRoutine = StartCoroutine(LocateRoutine(originPos,  null));
		}
	}

    public void OnDrag(PointerEventData ped){
		Vector2 touchPos = ped.position;
		base.transform.position = touchPos;



        if (((Vector2)base.transform.localPosition - (Vector2)originPos).magnitude > DragThreshold) {
            transform.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
		} else
        {
            transform.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
	#endregion

	public void SetLocation(int total, int target, bool isHided){
		if (locateRoutine != null) {
			StopCoroutine (locateRoutine);
		}
		originPos = GetPosition (total, target);
		//originRot = Quaternion.Euler (new Vector3 (0, 0, GetRotation (total, target)));

		if (isHided) {
			locateRoutine = StartCoroutine (LocateRoutine (new Vector3 (0, handYOffset, -target * 0.5f), null));
			DisableInteraction ();
		} else {
			locateRoutine = StartCoroutine (LocateRoutine (originPos,  EnableInteraction));
		}
	}
    public void Remove()
    {
        hand.RemoveCard(this);
        Destroy(gameObject);
    }

	private void EnableInteraction(){
        rayCaster.raycastTarget = true;
    }
    private void DisableInteraction(){
        rayCaster.raycastTarget = false;
    }

    #region Private
    private void ActiveSelf(){
		if (GameManager.instance.CurrentTurn == Turn.PLAYER) {
			data.CardActive ();
			if (data.IsConsumeTurn ()) {
				PlayerControl.instance.EndPlayerTurn();
			}
		}
	}

	private Coroutine locateRoutine;
	private IEnumerator LocateRoutine(Vector3 targetPosition, LocateCallback callBack){

		float timer = 0;
		while (true) {
			timer += Time.deltaTime;
			if (timer > 1) {
				base.transform.localPosition = targetPosition;
				//base.transform.rotation = targetRotation;
				transform.localScale = Vector3.one;
				break;
			}
			base.transform.localPosition = Vector3.Lerp (base.transform.localPosition, targetPosition, 0.1f);
			//base.transform.rotation = Quaternion.Slerp (base.transform.rotation, targetRotation, 0.1f);
			transform.localScale = Vector3.Lerp (transform.localScale, Vector3.one, 0.1f);
			yield return null;
		}

		if (callBack != null) {
			callBack ();
		}
	}

	/*private const float maxRotation = 30f;
	private static float GetRotation(int total, int target){
		if (total < 4) {
			return 0;
		} else {
			return 0.5f * maxRotation - (maxRotation / (total - 1)) * target;
		}
	}*/
		
	private static Vector3 GetPosition(int total, int target)
    {
		if (total == 1) {
			return new Vector3 (0, handYOffset * 120, 0);
		}
		float totalInterval = total*0.5f;
		Vector3 result = new Vector3(-totalInterval + (totalInterval * 2 / (total - 1)) * target, handYOffset);
		//result.x = -totalInterval + (totalInterval * 2 / (total - 1)) * target;
        //result.y = Mathf.Sqrt(100 - result.x * result.x) + handYOffset-10;
        return result * 120;
	}
	#endregion
}

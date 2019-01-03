using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
/// <summary>
/// 핸드에 있는 카드 오브젝트
/// </summary>
public class CardObject : MonoBehaviour, IDragHandler,IPointerDownHandler,IPointerUpHandler{

    private Hand hand;
    private CardRender render;
    private Card data;
    protected void Awake()
    {
        render = transform.Find("render").GetComponent<CardRender>();
    }

    private void Update()
    {
        EnableInteraction(data.IsAvailable());
    }


    public void SetCardRender(Card data_){
		data = data_;
        render.Name.text = data.CardData.name;
        render.SetRank(data.CardData.cost);
        render.SetAttribute(data.CardData.attribute);
        render.SetGraphic(ArchLoader.instance.GetCardSprite(data.CardData.spritePath));
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
    public void OnPointerDown(PointerEventData ped)
    {
        if(!GameManager.instance.IsInputOk || PlayerControl.instance.IsDirCardSelected)
        {
            return;
        }

		hand.ChooseOne ();
		data.CardEffectPreview ();
        hand.SetJoyStick(false);
        hand.CardInfoOn(data);

        if (locateRoutine != null) {
			StopCoroutine (locateRoutine);
		}
		base.transform.rotation = Quaternion.identity;
	}

	public void OnPointerUp(PointerEventData ped){
        if (!GameManager.instance.IsInputOk || PlayerControl.instance.IsDirCardSelected)
        {
            return;
        }

        hand.ChooseRollback ();
        hand.CardInfoOff();
        hand.SetJoyStick(true);    
        data.CancelPreview();

        if (((Vector2)base.transform.localPosition - (Vector2)originPos).magnitude > ActiveThreshold && GameManager.instance.CurrentTurn == Turn.PLAYER && GameManager.instance.CurrentRoom().IsEnemyAlive() && IsAvailable())
        {
            hand.RemoveCard (this);
            ActiveSelf();
            Destroy(gameObject);
		} else {
            transform.localScale = Vector3.one;
            //transform.localPosition = originPos;
            locateRoutine = StartCoroutine(LocateRoutine(originPos));
		}
	}

    public void OnDrag(PointerEventData ped){
        if (!GameManager.instance.IsInputOk || PlayerControl.instance.IsDirCardSelected)
        {
            return;
        }

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
			locateRoutine = StartCoroutine (LocateRoutine (new Vector3 (0, handYOffset, -target * 0.5f)));

		} else {
			locateRoutine = StartCoroutine (LocateRoutine (originPos));
		}
	}
    public void Remove()
    {
        hand.RemoveCard(this);
        Destroy(gameObject);
    }

    public void EnableInteraction(bool enable)
    {
        if(enable)
        {
            render.SetEnable(true);
        }
        else
        {
            render.SetEnable(false);
        }
    }

    #region Private
    private void ActiveSelf()
    {
        if(data.IsDirectionCard)
        {
            PlayerControl.instance.SelectedDirCard = data;
        }else
        {
            data.DoCard();
            if (data.IsConsumeTurn())
            {
                GameManager.instance.OnEndPlayerTurn();
            }
        }
	}

	private Coroutine locateRoutine;
	private IEnumerator LocateRoutine(Vector3 targetPosition)
    {

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
	}


		
	private static Vector3 GetPosition(int total, int target)
    {
		if (total == 1) {
			return new Vector3 (0, handYOffset * 120, 0);
		}
		float totalInterval = total*0.5f + (total * total)/(28);
		Vector3 result = new Vector3(-totalInterval + (totalInterval * 2 / (total - 1)) * target, handYOffset);
		//result.x = -totalInterval + (totalInterval * 2 / (total - 1)) * target;
        //result.y = Mathf.Sqrt(100 - result.x * result.x) + handYOffset-10;
        return result * 120;
	}
	#endregion
}

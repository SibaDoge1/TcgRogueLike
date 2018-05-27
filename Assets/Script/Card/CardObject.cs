using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardObject : MonoBehaviour {
	private void Start(){
		spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer> ();
	}
	private SpriteRenderer spriteRenderer;
	private CardData data;
	public void Init(CardData data_, Sprite sprite_){
		data = data_;
		//spriteRenderer.sprite = sprite_;
	}

	public void Active(){
		data.CardActive ();
	}

	public bool IsAvailable(){
		return data.IsAvailable ();
	}


	#region UserInput
	private Vector2 originPos;
	private Quaternion originRot;
	private const int DragThreshold = 3;
	void OnMouseDown(){
		if (locateRoutine != null) {
			StopCoroutine (locateRoutine);
		}

		transform.rotation = Quaternion.identity;
	}

	void OnMouseUp(){
		Vector2 touchPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);

		if ((touchPos - originPos).magnitude > DragThreshold) {
			//TODO : Active Card
			Debug.Log("Active!");
		} else {
			locateRoutine = StartCoroutine (LocateRoutine (originPos, originRot));
		}
	}

	void OnMouseDrag(){
		Vector2 touchPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);

		transform.position = touchPos;
		if ((touchPos - originPos).magnitude > DragThreshold) {
			spriteRenderer.transform.localScale = new Vector3 (20, 30, 1);
			//show Small size
		} else {
			spriteRenderer.transform.localScale = new Vector3 (10, 15, 1);
			//Show Big size
		}
	}
	#endregion

	public void SetLocation(int total, int target){
		if (locateRoutine != null) {
			StopCoroutine (locateRoutine);
		}
		originPos = GetPosition (total, target);
		originRot = Quaternion.Euler (new Vector3 (0, 0, GetRotation (total, target)));
		locateRoutine = StartCoroutine (LocateRoutine (originPos, originRot));
	}



	#region Private
	private Coroutine locateRoutine;
	private IEnumerator LocateRoutine(Vector3 targetPosition, Quaternion targetRotation){

		float timer = 0;
		while (true) {
			timer += Time.deltaTime;
			if (timer > 1) {
				transform.position = targetPosition;
				transform.rotation = targetRotation;
				break;
			}
			transform.position = Vector3.Lerp (transform.position, targetPosition, 0.1f);
			transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, 0.1f);
			yield return null;
		}
	}

	private const float maxRotation = 30f;
	private static float GetRotation(int total, int target){
		if (total < 4) {
			return 0;
		} else {
			return 0.5f * maxRotation - (maxRotation / (total - 1)) * target;
		}
	}
		
	private static Vector3 GetPosition(int total, int target){
		if (total == 1) {
			return new Vector3 (0, 0, 0);
		}
		float totalInterval = 0.3f + total * 0.4f;
		Vector3 result = new Vector3 ();
		result.x = -totalInterval + (totalInterval * 2 / (total - 1)) * target;
		result.y = Mathf.Sqrt(25 - result.x * result.x);
		result.z = -target;
		return result;
	}
	#endregion
}

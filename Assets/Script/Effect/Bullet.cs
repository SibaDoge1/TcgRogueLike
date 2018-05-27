using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public virtual void Shoot(OnTileObject target){
		StartCoroutine (ShootingRoutine (target));
	}


	private IEnumerator ShootingRoutine(OnTileObject target){
		float timer = 0;
		Vector3 originPos = transform.position;
		while (true) {
			timer += Time.deltaTime * 5f;
			if (timer > 1) {
				break;
			}
			transform.position = Vector3.Lerp (originPos, target.transform.position, timer);
			yield return null;
		}
		EffectDelegate.instance.MadeEffect (CardEffectType.Hit, target);
	}
}

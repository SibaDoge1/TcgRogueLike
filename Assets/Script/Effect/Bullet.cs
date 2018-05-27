using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	private Vector3 recentTargetPos;
	public virtual void Shoot(Transform target){
		recentTargetPos = target.position;
		StartCoroutine (ShootingRoutine (target));
	}


	private IEnumerator ShootingRoutine(Transform target){
		float timer = 0;
		Vector3 originPos = transform.position;
		while (true) {
			timer += Time.deltaTime * 5f;
			if (timer > 1) {
				break;
			}
			transform.position = Vector3.Lerp (originPos, target.position, timer);
			yield return null;
		}
		if (target == null) {
			EffectDelegate.instance.MadeEffect (CardEffectType.Hit, recentTargetPos);
		} else {
			EffectDelegate.instance.MadeEffect (CardEffectType.Hit, target);
		}
		Destroy (gameObject);
	}
}

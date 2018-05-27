using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Arrow : Bullet {

	public override void Shoot (Transform target){
		Vector2 delta = target.position - transform.position;
		transform.Rotate (0, 0, Vector2.Angle (delta, new Vector2 (0, 1)));
		base.Shoot (target);
	}
}

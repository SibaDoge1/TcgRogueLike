using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Particle : EffectObject {
	public float particleStopTime = 0.5f;
	private ParticleSystem particle;
	protected override void Start (){
		particle = GetComponent<ParticleSystem> ();
		base.Start ();
	}

	IEnumerator ParticleStopRoutine(){
		yield return new WaitForSeconds (particleStopTime);
		particle.Stop ();
	}
}

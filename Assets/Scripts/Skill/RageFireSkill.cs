using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageFireSkill : Skill {

	[Header("Option")]
	public int damage;
	public int fireCount;
	public float fireDelay;

	public override void Init() { 
		
	}

	public override IEnumerator TriggerSkill() {
		yield return StartCoroutine(Fire());
	}

	private IEnumerator Fire() {
		WaitForSeconds waitSec = new WaitForSeconds(fireDelay);
		for (int count = 0; count < fireCount; count++) {
			WeaponManager.TryFire();
			yield return waitSec;
		}
	}
}

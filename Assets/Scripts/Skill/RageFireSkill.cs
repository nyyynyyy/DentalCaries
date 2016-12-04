using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName="Skills/RageFireSkill")]
public class RageFireSkill : Skill {

	[Header("Option")]
	public int damage;
	public int fireCount;
	public float fireDelay;

	public override void Init() { 
		
	}

	public override void TriggerSkill() {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviourC {

	public float cooldown;

	public abstract void Init();
	public abstract IEnumerator TriggerSkill();
}

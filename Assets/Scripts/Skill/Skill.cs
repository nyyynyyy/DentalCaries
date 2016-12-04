using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : ScriptableObject {

	public string skillName;
	public float cooldown;

	public abstract void Init();
	public abstract void TriggerSkill();
}

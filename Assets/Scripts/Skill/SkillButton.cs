using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SkillButton : MonoBehaviour {
	public Image darkMask;
	public Text cooldownTextDisplay;

	[SerializeField] private Skill skill;
	private Button _button;
	private float _cooldownTimeLeft;


	void Start() {
		_button = GetComponent<Button>();

		skill.Init();
		SkillReady();
	}

	private void SkillReady() {
		cooldownTextDisplay.enabled = false;
		darkMask.enabled = false;
		_button.interactable = true;
	}

	public void OnButtonDown() {
		_button.interactable = false;
		StartCoroutine(TriggerButton());
	}

	private IEnumerator TriggerButton() { 
		yield return skill.TriggerSkill();

		_cooldownTimeLeft = skill.cooldown;
		darkMask.enabled = true;
		cooldownTextDisplay.enabled = true;

		yield return Cooldown();
	}

	private IEnumerator Cooldown() {
		while (_cooldownTimeLeft > 0) { 
			Cooldown(Time.deltaTime);
			yield return null;
		}

		SkillReady();
	}

	private void Cooldown(float time) {
		_cooldownTimeLeft -= time;

		cooldownTextDisplay.text = (Mathf.Floor(_cooldownTimeLeft) + 1).ToString();
		darkMask.fillAmount = (_cooldownTimeLeft / skill.cooldown);
	}
}
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

	void Update() {
		if (_cooldownTimeLeft <= 0) {
			SkillReady();
		} else {
			Cooldown(Time.deltaTime);
		}
	}

	private void SkillReady() {
		cooldownTextDisplay.enabled = false;
		darkMask.enabled = false;
		_button.interactable = true;
	}

	private void Cooldown(float time) {
		_cooldownTimeLeft -= time;

		cooldownTextDisplay.text = (Mathf.Floor(_cooldownTimeLeft) + 1).ToString();
		darkMask.fillAmount = (_cooldownTimeLeft / skill.cooldown);
	}

	public void TriggerButton() {
		_cooldownTimeLeft = skill.cooldown;

		_button.interactable = false;
		darkMask.enabled = true;
		cooldownTextDisplay.enabled = true;

		skill.TriggerSkill();
	}
}
using System.Collections;
using UnityEngine;

[System.Serializable]
public class LandMine : MonoBehaviour {
	private int _damage;
	private float _delay;

	private int _duration;

	private bool _attackPosible = true;
	private int _upgradeUnit = 0;

	public void Init(LandMineData data) {
		_damage = data.damage;
		_delay = data.delay;
		_duration = data.duration;
	}

	public void Repair(LandMineData data) {
		if (!CanRepair(data)) {
			return;
		}

		_duration = data.duration;
	}

	public bool CanRepair(LandMineData data) {
		return data.duration > _duration;
	}

	private bool IsDeath() {
		return _duration <= 0;
	}

	void OnTriggerStay(Collider other) {
		EnemyCheck(other);
	}

	private void EnemyCheck(Collider other) {
		if (other.tag == "Enemy") {
			Enemy enemy = other.GetComponent<Enemy>();
			HitEnemy(enemy);
		}
	}

	private void HitEnemy(Enemy enemy) {
		if (!IsDeath() && _attackPosible) {
			enemy.Damage(_damage);
			StartCoroutine(AttackDelay());

			if (--_duration <= 0) {
				Death();
			}
		}
	}

	private void Death() {
		_duration = 0;

		StartCoroutine(AlphaColor(0.2f, 0.8f));
	}

	private IEnumerator AttackDelay() {
		_attackPosible = false;
		yield return new WaitForSeconds(_delay);
		_attackPosible = true;
	}

	private IEnumerator AlphaColor(float alpha, float time) {
		Material material = GetComponent<MeshRenderer>().material;
		Color color = material.color;

		int loopSize = (int) (30f * time);
		WaitForSeconds loopDelay = new WaitForSeconds(time / loopSize);

		float addValue = (alpha - color.a) / (float)loopSize;
		Debug.Log(addValue);
		alpha = color.a;

		for (int i = 0; i < loopSize; i++) {
			alpha += addValue;
			Debug.Log(alpha);
			material.color = new Color(color.r, color.g, color.b, alpha);
			Debug.Log(material.color);
			yield return loopDelay;
		}
	}
}
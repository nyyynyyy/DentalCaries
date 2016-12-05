using System.Collections;
using UnityEngine;

[System.Serializable]
public class LandMine : MonoBehaviour {
	private int _damage;
	private float _delay;

	private bool _attackPosible = true;
	private int _upgradeUnit = 0;

	public void Init(LandMineData data) {
		_damage = data.damage;
		_delay = data.delay;
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
		if (_attackPosible) {
			enemy.Damage(_damage);
			StartCoroutine(AttackDelay());
		}
	}

	IEnumerator AttackDelay() {
		_attackPosible = false;
		yield return new WaitForSeconds(_delay);
		_attackPosible = true;
	}
}
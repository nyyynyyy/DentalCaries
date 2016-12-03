using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMine : MonoBehaviour {

	[Header("Attack")]
	public int damage;
	public float delay;

	private bool _attackPosible = true;

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
			enemy.Damage(damage);
			StartCoroutine(AttackDelay());
		}
	}

	IEnumerator AttackDelay() {
		_attackPosible = false;
		yield return new WaitForSeconds(delay);
		_attackPosible = true;
	}
}

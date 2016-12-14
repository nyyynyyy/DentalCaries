using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMineCollider : MonoBehaviour {

	private LandMine _landMine;

	private bool _attackPosible = true;

	void Start() {
		_landMine = GetComponent<LandMine>();
	}

	void OnTriggerStay(Collider other)
	{
		CheckEnemy(other);
	}

	private void CheckEnemy(Collider other)
	{
		if (other.tag == "Enemy")
		{
			Enemy enemy = other.GetComponent<Enemy>();
			OnTriggerEnemy(enemy);
		}
	}

	private void OnTriggerEnemy(Enemy enemy)
	{
		if (!_landMine.IsDeath() && _attackPosible)
		{
			_landMine.Attack(enemy);
			StartCoroutine(AttackDelay());
		}
	}

	private IEnumerator AttackDelay()
	{
		_attackPosible = false;
		yield return new WaitForSeconds(_landMine.attackInfo.delay);
		_attackPosible = true;
	}
}

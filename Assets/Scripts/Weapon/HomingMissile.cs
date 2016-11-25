using UnityEngine;
using System.Collections;

public class HomingMissile : Weapon {

	public float moveSpeed;
	public float rotationSpeed;
	public float homingRadius;

	private Transform _target;

	public override void Fire() {
		StartCoroutine(Move());
		StartCoroutine(FindEnemy());
	}

	public override void HitEnemy(Enemy enemy) {
		enemy.Damage(1);
		Delete();
	}

	public override void Delete() {
		StopAllCoroutines();
		gameObject.SetActive(false);
	}

	IEnumerator Move() {
		while (true) {
			yield return null;

			transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
			if (!_target) {
				continue;
			}

			Vector3 distance = (_target.transform.position - transform.position);
			if (distance.magnitude - 1 > homingRadius) {
				_target = null;
				continue;
			}

			Quaternion lerfRotation = Quaternion.LookRotation(distance);
			lerfRotation = Quaternion.Slerp(transform.rotation,
											lerfRotation,
											rotationSpeed * Time.deltaTime);

			transform.rotation = lerfRotation;
		}
	}

	IEnumerator FindEnemy() {
		yield return new WaitForSeconds(0.1F);

		Collider[] colliders = Physics.OverlapSphere(transform.position, homingRadius);
		for (int i = 0; i < colliders.Length; i++) {
			if (colliders[i].tag == "Enemy") {
				_target = colliders[i].transform;
				StopCoroutine(FindEnemy());
				yield break;
			}
		}

		yield return FindEnemy();
	}
}

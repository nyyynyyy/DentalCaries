using UnityEngine;
using System.Collections;

public class DefaultBullet : Weapon {

	[Header("Option")]
	public float speed;

	public override void Delete() {
		StopCoroutine(Move());
		gameObject.SetActive(false);
	}

	public override void Fire() {
		StartCoroutine(Move());
	}

	public override void HitEnemy(Enemy enemy) {
		enemy.Damage(1);
		Delete();
	}

	IEnumerator Move() {
		while (true) { 
			transform.Translate(Vector3.forward * Time.deltaTime * speed);
			yield return null;
		}
	}
}

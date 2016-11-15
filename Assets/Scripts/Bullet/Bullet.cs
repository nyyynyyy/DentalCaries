using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	private static BulletManager bulletManager;

	public float speed = 7F;

	private bool _isUsed = false;

	public static void SetBulletManager(BulletManager bulletManager)
	{
		Bullet.bulletManager = bulletManager;
	}

	void OnTriggerEnter(Collider other)
	{
		EnemyCheck(other);
		Disable();
	}

	private void EnemyCheck(Collider other) {
		if (other.tag == "Enemy") {
			Enemy enemy = other.GetComponent<Enemy>();
			enemy.Damage();
		}
	}

	private void Disable() {
		_isUsed = false;
		//_currentTime = 0;
		StopCoroutine(TimeRemove());
		bulletManager.SetDisableBullet(this);
	}

	public void Fire() {
		_isUsed = true;
		StartCoroutine(FireUpdating());
		StartCoroutine(TimeRemove());
	}

	private IEnumerator FireUpdating() {
		while (_isUsed) {
			//_currentTime += Time.deltaTime;
			//if(_currentTime >= 
			transform.position = transform.position + transform.forward * Time.deltaTime * speed;
			yield return null;
		}
	}

	private IEnumerator TimeRemove() {
		yield return new WaitForSeconds(3F);
		Disable();
	}
}

using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	private static BulletManager bulletManager;

	public float speed = 7F;

	private bool isUsed = false;

	public static void SetBulletManager(BulletManager bulletManager)
	{
		Bullet.bulletManager = bulletManager;
	}

	void OnTriggerEnter(Collider other)
	{
		Disable();
        Debug.Log("ASDASD");
	}

	private void Disable() {
		isUsed = false;
		bulletManager.SetDisableBullet(this);
	}

	public void Fire() {
		isUsed = true;
		StartCoroutine(FireUpdating());
	}

	private IEnumerator FireUpdating() {
		while (isUsed) {
			transform.position = transform.position + transform.forward * Time.deltaTime * speed;
			yield return null;
		}
	}
}

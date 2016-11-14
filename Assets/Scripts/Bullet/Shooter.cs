using UnityEngine;
using System.Collections;

public class Shooter : MonoBehaviour {
	public Transform shootLocation;
	public float shootDelay = 0.5F;

	private WaitForSeconds waitForShootDelay;

	void Start() {
		waitForShootDelay = new WaitForSeconds(shootDelay);
		StartCoroutine(Shoot());
	}

	private IEnumerator Shoot() {
		while (true) {
			if (Input.GetMouseButton(0)) {
				BulletManager.Shoot(shootLocation);
				yield return waitForShootDelay;
			} 
			yield return null;
		}
	}
}

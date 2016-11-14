using UnityEngine;
using System.Collections;

public class Shooter : MonoBehaviour {
	public Camera mainCamera;
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
				Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

				BulletManager.Shoot(shootLocation.transform.position, ray.origin);
				yield return waitForShootDelay;
			} 
			yield return null;
		}
	}
}

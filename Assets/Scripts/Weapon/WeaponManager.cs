using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponManager : MonoBehaviour {
	private static WeaponManager instance;

	[Header("Weapon Model")]
	public WeaponModelMove weaponModel;

	[Header("Weapon")]
	public Weapon mainWeapon;
	public int maxAmount;
	public float deleteDelay;

	[Header("Fire")]
	public float fireDelay;
	public Transform fireLocation;

	private WaitForSeconds _waitForFireDelay;
	private WaitForSeconds _weaponDeleteDelay;
	private Queue<Weapon> _readyWeapons;

	void Start () {
		if (instance) {
			throw new System.Exception("Already loaded class instance 'WeaponManager.cs'");
		}

		instance = this;

		_waitForFireDelay = new WaitForSeconds(fireDelay);
		_weaponDeleteDelay = new WaitForSeconds(deleteDelay);
		_readyWeapons = new Queue<Weapon>();

		SetWeapons();

		StartCoroutine(MouseClick());
	}

	private void SetWeapons() {
		Transform storage = new GameObject("Weapons").transform;

		for (int i = 0; i < maxAmount; i ++) {
			GameObject weapon = Instantiate(mainWeapon.gameObject);

			weapon.transform.SetParent(storage);
			weapon.SetActive(false);

			_readyWeapons.Enqueue(weapon.GetComponent<Weapon>());
		}
	}

	private bool Fire() {
		if (_readyWeapons.Count <= 0 || GameManager.instance.pause) {
			return false;
		}

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit rayHit;
		Vector3 targetPos;

		if (Physics.Raycast(ray, out rayHit, 30f) && rayHit.transform.tag == "Enemy") {
			targetPos = rayHit.transform.position;
			if (rayHit.collider is BoxCollider) {
				targetPos += ((BoxCollider)rayHit.collider).center;
			}
		}
		else {
			targetPos = weaponModel.transform.position + ray.GetPoint(15);
		}

		weaponModel.SetAngleTarget(targetPos);


		//Vector3 targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));

		Weapon fireWeapon = _readyWeapons.Dequeue();

		fireWeapon.transform.position = fireLocation.position;
		fireWeapon.transform.LookAt(targetPos);
		fireWeapon.gameObject.SetActive(true);
		fireWeapon.Fire();

		StartCoroutine(DeleteWeapon(fireWeapon));

		return true;
	}

	IEnumerator MouseClick() {
		// 무한 반복
		while (true) {
			// 마우스를 누르면 무기 발사
			if (Input.GetMouseButtonDown(0) && !ViewManager.instance.isTps && Fire()) {
				yield return _waitForFireDelay;
			} else { 
				yield return null;
			}
		}
	}

	IEnumerator DeleteWeapon(Weapon weapon) {
		yield return _weaponDeleteDelay;

		weapon.Delete();
		weapon.gameObject.SetActive(false);

		_readyWeapons.Enqueue(weapon);
	}
}

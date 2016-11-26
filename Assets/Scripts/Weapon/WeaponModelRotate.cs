using UnityEngine;
using System.Collections;

public class WeaponModelRotate : MonoBehaviour {

	[Header("Aim Position")]
	public Vector3 posBase;
	public Vector3 posAim;

	private Vector3 _target;
	private float _lastSetTargetTime = 0;

	void FixedUpdate () {
		if (Time.time - _lastSetTargetTime >= 0.85F) {
			transform.localPosition = Vector3.Slerp(transform.localPosition, posBase, Time.deltaTime * 10);
			transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(-9.944f,0,0), Time.deltaTime * 10);
			return;
		}

		if (_target != null) {
			transform.localPosition = Vector3.Slerp(transform.localPosition, posAim, Time.deltaTime * 10);
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_target), Time.deltaTime * 10);
		}
	}

	public void SetAngleTarget(Vector3 target) {
		_lastSetTargetTime = Time.time;
		_target = target;
	}
}

using System.Collections;
using UnityEngine;

public class WeaponModelMove : MonoBehaviour {
	[Header("Aim Position")]
	public Vector3 posBase;
	public Vector3 posAim;
	public Vector3 directionBase;

	[Header("Move Axis Rotate")]
	public float rotateValue_base;
	public float rotateValue_target;

	private Quaternion _target;
	private Quaternion _directionBase;

	private ViewManager.Arrow _rotateViewArrow;

	private float _lastSetTargetTime = -10;
	private float _lastRotateViewTime = -10;

	void Start() {
		_directionBase = Quaternion.Euler(directionBase);
	}

	void FixedUpdate() {
		Move();
	}

	private enum RotationType { 
		Base,
		Target
	}

	private void Move() { 
		if (GetTimeDistance(_lastSetTargetTime) >= 0.85F) {
			transform.localPosition = Vector3.Slerp(transform.localPosition, posBase, Time.deltaTime * 10);
			transform.localRotation = Quaternion.Slerp(transform.localRotation, GetRotateQuaternion(_directionBase, RotationType.Base), Time.deltaTime * 5);

			return;
		}

		transform.localPosition = Vector3.Slerp(transform.localPosition, posAim, Time.deltaTime * 10);
		transform.localRotation = Quaternion.Slerp(transform.localRotation, GetRotateQuaternion(_target, RotationType.Target), Time.deltaTime * 10);

		Quaternion loc = transform.localRotation;
		//loc.y = Mathf.Clamp(loc.y, -0.5f, 0.5f);
		loc.z = 0;
		transform.localRotation = loc;
	}

	private Quaternion GetRotateQuaternion(Quaternion origin, RotationType rotationType) {
		if (GetTimeDistance(_lastRotateViewTime) >= 0.05F) {
			return origin;
		}

		float rotateValue = rotationType == RotationType.Base ? rotateValue_base : rotateValue_target;
		if (_rotateViewArrow == ViewManager.Arrow.Left)
			rotateValue *= -1;

		Vector3 eulerAngles = origin.eulerAngles;

		if (rotationType == RotationType.Base) {
			eulerAngles.z -= rotateValue;
		} else {
			eulerAngles.y -= rotateValue;
		}

		return Quaternion.Euler(eulerAngles);
	}

	private float GetTimeDistance(float capturedTime) {
		return Time.time - capturedTime;
	}

	public void SetAngleTarget(Vector3 target) {
		_lastSetTargetTime = Time.time;

		_target = Quaternion.Euler(Quaternion.LookRotation(target).eulerAngles - transform.root.localEulerAngles);
	}

	public void RotateView(ViewManager.Arrow arrow) {
		_lastRotateViewTime = Time.time;
		_rotateViewArrow = arrow;
	}
}

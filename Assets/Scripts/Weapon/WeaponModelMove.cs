using System.Collections;
using UnityEngine;

public class WeaponModelMove : MonoBehaviour {
	[Header("Aim Position")]
	public Vector3 posBase;
	public Vector3 posAim;
	public Vector3 directionBase;

	[Header("Move Z-Axis Rotate")]
	public float rotateViewValue;

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

	private void Move() { 
		if (GetTimeDistance(_lastSetTargetTime) >= 0.85F) {
			transform.localPosition = Vector3.Slerp(transform.localPosition, posBase, Time.deltaTime * 10);
			transform.localRotation = Quaternion.Slerp(transform.localRotation, GetRotateQuaternion(_directionBase), Time.deltaTime * 5);

			return;
		}

		transform.localPosition = Vector3.Slerp(transform.localPosition, posAim, Time.deltaTime * 10);
		transform.localRotation = Quaternion.Slerp(transform.localRotation, GetRotateQuaternion(_target), Time.deltaTime * 10);

		Quaternion loc = transform.localRotation;
		//loc.y = Mathf.Clamp(loc.y, -0.5f, 0.5f);
		loc.z = 0;
		transform.localRotation = loc;
	}

	private Quaternion GetRotateQuaternion(Quaternion origin) {
		if (GetTimeDistance(_lastRotateViewTime) >= 0.1F) {
			return origin;
		}

		Vector3 eulerAngles = origin.eulerAngles;
		eulerAngles.z -= _rotateViewArrow == ViewManager.Arrow.Right ? rotateViewValue : -rotateViewValue;

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

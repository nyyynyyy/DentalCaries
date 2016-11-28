using System.Collections;
using UnityEngine;

public class WeaponModelMove : MonoBehaviour {

	[Header("Fire Animation")]
	public Animation anim;
	public AnimationClip fireClip;

	[Header("Aim Position")]
	public Vector3 posBase;
	public Vector3 posAim;
	public Vector3 directionBase;

	private Vector3 _target;
	private Quaternion _directionBase;
	private float _lastSetTargetTime = -10;

	void Start() { 
		_directionBase = Quaternion.Euler(directionBase);
	}

	void FixedUpdate () {
		
		float time = Time.time - _lastSetTargetTime;
		if (time >= 0.85F) {
			transform.localPosition = Vector3.Slerp(transform.localPosition, posBase, Time.deltaTime * 10);
			transform.localRotation = Quaternion.Slerp(transform.localRotation, _directionBase, Time.deltaTime * 10);

			return;
		}

		transform.localPosition = Vector3.Slerp(transform.localPosition, posAim, Time.deltaTime * 10);
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_target), Time.deltaTime * 10);

		Quaternion loc = transform.localRotation;
		loc.y = Mathf.Clamp(loc.y, -0.43f, 0.43f);
		loc.z = 0;
		transform.localRotation = loc;
	}

	public void SetAngleTarget(Vector3 target) {
		_lastSetTargetTime = Time.time;
		_target = target;
	}
}

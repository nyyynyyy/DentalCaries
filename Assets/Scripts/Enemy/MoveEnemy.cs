using UnityEngine;
using System.Collections;

public class MoveEnemy : MonoBehaviour {

    private Enemy _enemy;
    private Transform _target;

    void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }

	// Use this for initialization
	void Start () {
        //SetMyAnlge();
    }

    // Update is called once per frame
    void FixedUpdate () {
        Move();
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Heart")
        {
            //Debug.Log("HIT HEART");
            StartCoroutine(_enemy.HitHeart());
        }
    }

    public void SetTarget(Transform target)
    {
		_target = target;
    }

    private void Move()
    {
		if (!CanMove ()) {
			return;
		}

        Vector3 movePosition;
		float angle;

		movePosition = transform.position + transform.forward * -_enemy._moveSpeed * Time.deltaTime;
		angle = Quaternion.LookRotation(transform.position - _target.position).eulerAngles.y;

		transform.position = movePosition;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
    }

	private bool CanMove() {
		RaycastHit hitInfo;
		return !(Physics.Raycast (transform.position + Vector3.up * 0.5f, -transform.forward * 3F, out hitInfo, 1.5F) && hitInfo.transform.tag == "Enemy");
	}
}
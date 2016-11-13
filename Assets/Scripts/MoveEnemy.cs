using UnityEngine;
using System.Collections;

public class MoveEnemy : MonoBehaviour {

    private Enemy _enemy;
    private float angle;

    void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }

	// Use this for initialization
	void Start () {
        SetMyAnlge();
    }

    // Update is called once per frame
    void FixedUpdate () {
        Move();
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Heart")
        {
            _enemy.HitHeart();
        }
    }

    private void SetMyAnlge()
    {
        float x = transform.position.x;
        float z = transform.position.z;
        angle = Mathf.Atan(z / x) * Mathf.Rad2Deg + (x > 0 ? 0 : 180f);
    }

    private void Move()
    {
        Vector3 movePosition;
        transform.rotation = Quaternion.Euler(0, angle, 0);
        movePosition = transform.position + transform.right * -_enemy.speed * Time.deltaTime;
        transform.position = movePosition;
    }
}

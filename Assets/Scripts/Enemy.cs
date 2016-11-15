using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    [Header("State")]
    public float _speed = 3f;
    public float _health = 3f;

    private MoveEnemy _move;

    void Awake()
    {
        _move = GetComponent<MoveEnemy>();
    }

    public void Spawn(Vector3 spawnPoint, float speed, float health)
    {
        transform.position = spawnPoint;
        _move.SetMyAnlge();
        _health = health;
        _speed = speed;
        gameObject.SetActive(true);
    }

    public void HitHeart()
    {
        _speed = 0;
        Debug.Log("HIT");
    }

	public void Damage() {
		_health--;
		if (_health <= 0) {
			Death();
		}
	}

	public void Death() {
		// health init code here.
		gameObject.SetActive(false);
	}
}

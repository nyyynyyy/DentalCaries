using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    [Header("State")]
    public float _moveSpeed = 3f;
    public float _attackSpeed = 1f;
    public float _attackPower = 2f;
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
        _moveSpeed = speed;
        gameObject.SetActive(true);
    }

    public IEnumerator HitHeart()
    {
        _moveSpeed = 0;
        while(!GameManager.instance.IsGame()){
            GameManager.instance.Damage(_attackPower);
            yield return new WaitForSeconds(_attackSpeed);
            Debug.Log("DAMAGE");
        }
        
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

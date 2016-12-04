using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    [Header("State")]
    public EnemyType _type;
 
    private string _name;
    public float _moveSpeed;
    private float _attackSpeed;
    private float _attackPower;

    private float _maxHp;
    private float _nowHp;

    private int _gold;

    private MoveEnemy _move;
	private Rigidbody _rigidbody;
	private RigidbodyConstraints _rigidbodyBaseConstraints;

    void Awake()
    {
        _move = GetComponent<MoveEnemy>();
		_rigidbody = GetComponent<Rigidbody>();
		_rigidbodyBaseConstraints = GetComponent<Rigidbody>().constraints;
    }

    public void Init(EnemyType type)
    {
        _type = type;
    }

    public void Spawn(Vector3 spawnPoint, Transform targetPoint, string name, float hp, float moveSpeed, float attackPower, float attackSpeed, int gold)
    {
		transform.position = spawnPoint;
		_move.SetTarget(targetPoint);
        _name = name;
        _maxHp = _nowHp = hp;
        _moveSpeed = moveSpeed;
        _attackPower = attackPower;
        _attackSpeed = attackSpeed;
        _gold = gold;
		_rigidbody.constraints = _rigidbodyBaseConstraints;

        gameObject.SetActive(true);
    }

    public IEnumerator HitHeart() {
		_rigidbody.constraints = RigidbodyConstraints.FreezeAll;
		_moveSpeed = 0;
		while (!GameManager.instance.IsGame()) {
			GameManager.instance.Damage(_attackPower);
			yield return new WaitForSeconds(_attackSpeed);
		//	Debug.Log("DAMAGE");
		}
    }

	public void Damage(int amount) {
		_nowHp = Mathf.Max(_nowHp - amount, 0);

		HealthBar.instance.ViewEnemyBar(_maxHp, _nowHp, amount, _name, transform.name);

		if (_nowHp <= 0) {
			Death();
		}
	}

	public void Death() {
		EnemyManager.instance.CreateDeathParticle(transform);

		StopAllCoroutines();
		RoundManager.instance.DeathUnit();
        GameManager.instance.TakeMoney(_gold);
		gameObject.SetActive(false);
	}

	private IEnumerator DamageAnimation() {
		yield return null;
	}
}

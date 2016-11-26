using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public string name;

    [Header("State")]
    public float _moveSpeed = 3f;
    public float _attackSpeed = 1f;
	public float _attackPower = 2f;

	public float _maxHp = 3F;
    private float _nowHp;

    private MoveEnemy _move;
	private Rigidbody _rigidbody;

	

    void Awake()
    {
        _move = GetComponent<MoveEnemy>();
		_rigidbody = GetComponent<Rigidbody>();
    }

    public void Spawn(Vector3 spawnPoint, Transform targetPoint, float hp, float moveSpeed, float attackPower, float attackSpeed)
    {
		transform.position = spawnPoint;
		_move.SetTarget(targetPoint);
        _maxHp = _nowHp = hp;
        _moveSpeed = moveSpeed;
        _attackPower = attackPower;
        _attackSpeed = attackSpeed;
		_rigidbody.constraints = RigidbodyConstraints.None;

        gameObject.SetActive(true);
    }

    public IEnumerator HitHeart()
    {
        _moveSpeed = 0;
		_rigidbody.constraints = RigidbodyConstraints.FreezeAll;
		_moveSpeed = 0;
		while (!GameManager.instance.IsGame()) {
			GameManager.instance.Damage(_attackPower);
			yield return new WaitForSeconds(_attackSpeed);
			Debug.Log("DAMAGE");
		}
    }

	public void Damage(int amount) {
		_nowHp = Mathf.Max(_nowHp - amount, 0);

		if (name.Equals("") || name == null) name = "테스트용 복셀균";
		HealthBar.instance.ViewUi(_maxHp, _nowHp, name);

		if (_nowHp <= 0) {
			Death();
		}
	}

	public void Death() {
		EnemyManager.instance.CreateDeathParticle(transform);

		StopCoroutine(HitHeart());
		gameObject.SetActive(false);
	}
}

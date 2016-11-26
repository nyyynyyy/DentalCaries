using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public string name;

    [Header("State")]
    public float moveSpeed = 3f;
    public float attackSpeed = 1f;
	public float attackPower = 2f;
	public float maxHp = 3F;

    private MoveEnemy _move;
	private Rigidbody _rigidbody;

	private float _nowHp;

    void Awake()
    {
        _move = GetComponent<MoveEnemy>();
		_rigidbody = GetComponent<Rigidbody>();
    }

    public void Spawn(Vector3 spawnPoint, Transform targetPoint,float speed)
    {
		transform.position = spawnPoint;
		_move.SetTarget(targetPoint);
		_nowHp = maxHp;
        moveSpeed = speed;
		_rigidbody.constraints = RigidbodyConstraints.None;

        gameObject.SetActive(true);
    }

    public IEnumerator HitHeart()
    {
        moveSpeed = 0;
		_rigidbody.constraints = RigidbodyConstraints.FreezeAll;
		moveSpeed = 0;
		while (!GameManager.instance.IsGame()) {
			GameManager.instance.Damage(attackPower);
			yield return new WaitForSeconds(attackSpeed);
			Debug.Log("DAMAGE");
		}
    }

	public void Damage(int amount) {
		_nowHp = Mathf.Max(_nowHp - amount, 0);

		if (name.Equals("") || name == null) name = "테스트용 복셀균";
		HealthBar.instance.ViewUi(maxHp, _nowHp, name);

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

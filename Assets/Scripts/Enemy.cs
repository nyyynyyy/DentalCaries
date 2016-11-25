using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    [Header("State")]
    public float _moveSpeed = 3f;
    public float _attackSpeed = 1f;
    public float _attackPower = 2f;
    public float _health = 3f;

	[Header("Death Particle")]
	public ParticleSystem _deathParticle;
	public float _particleDuation = 2F;

    private MoveEnemy _move;
	private Rigidbody _rigidbody;

    void Awake()
    {
        _move = GetComponent<MoveEnemy>();
		_deathParticle = Instantiate(_deathParticle).GetComponent<ParticleSystem>();
		_deathParticle.transform.SetParent(transform);
		_rigidbody = GetComponent<Rigidbody>();
    }

    public void Spawn(Vector3 spawnPoint, Transform targetPoint,float speed, float health)
    {
		transform.position = spawnPoint;
		_move.SetTarget(targetPoint);
        _health = health;
        _moveSpeed = speed;
		_rigidbody.constraints = RigidbodyConstraints.None;

		StopCoroutine(ComeBackHomeMyParticle());
		_deathParticle.transform.localPosition = Vector3.zero;

        gameObject.SetActive(true);
    }

    public IEnumerator HitHeart()
    {
        _moveSpeed = 0;
        while(!GameManager.instance.IsGame()){
            GameManager.instance.Damage(_attackPower);
			_rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
            yield return new WaitForSeconds(_attackSpeed);
            Debug.Log("DAMAGE");
        }
        
    }

	public void Damage(int amount) {
		_health -= amount;
		if (_health <= 0) {
			Death();
		}
	}

	public void Death() {
		// health init code here.
		_deathParticle.transform.SetParent(null);
		_deathParticle.Play();

		transform.localScale = Vector3.zero;

		StartCoroutine(ComeBackHomeMyParticle());
		//Destroy(_deathParticle.gameObject, _particleDuration);
	}

	IEnumerator ComeBackHomeMyParticle() {
		yield return new WaitForSeconds(_particleDuation);
		_deathParticle.transform.SetParent(transform);
		gameObject.SetActive(false);
		transform.localScale = Vector3.one;
	}
}

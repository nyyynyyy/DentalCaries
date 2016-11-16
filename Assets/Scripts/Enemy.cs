using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    [Header("State")]
    public float _speed = 3f;
    public float _health = 3f;

	[Header("Death Particle")]
	public ParticleSystem _deathParticle;
	public float _particleDuation = 2F;

    private MoveEnemy _move;

    void Awake()
    {
        _move = GetComponent<MoveEnemy>();
		_deathParticle = Instantiate(_deathParticle).GetComponent<ParticleSystem>();
		_deathParticle.transform.SetParent(transform);
    }

    public void Spawn(Vector3 spawnPoint, float speed, float health)
    {
        transform.position = spawnPoint;
        _move.SetMyAnlge();
        _health = health;
        _speed = speed;

		StopCoroutine(ComeBackHomeMyParticle());
		_deathParticle.transform.localPosition = Vector3.zero;

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

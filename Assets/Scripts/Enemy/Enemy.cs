using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public string name;

    [Header("State")]
    public float moveSpeed = 3f;
    public float attackSpeed = 1f;
	public float attackPower = 2f;
	public float maxHp = 3F;

	[Header("Death Particle")]
	public ParticleSystem _deathParticle;
	public float _particleDuation = 2F;

    private MoveEnemy _move;
	private Rigidbody _rigidbody;

	private float _nowHp;

    void Awake()
    {
        _move = GetComponent<MoveEnemy>();
		_deathParticle = Instantiate(_deathParticle).GetComponent<ParticleSystem>();
		_deathParticle.transform.SetParent(transform);
		_rigidbody = GetComponent<Rigidbody>();
    }

    public void Spawn(Vector3 spawnPoint, Transform targetPoint,float speed)
    {
		transform.position = spawnPoint;
		_move.SetTarget(targetPoint);
		_nowHp = maxHp;
        moveSpeed = speed;
		_rigidbody.constraints = RigidbodyConstraints.None;

		StopCoroutine(ComeBackHomeMyParticle());
		_deathParticle.transform.localPosition = Vector3.zero;
		_deathParticle.transform.localScale = Vector3.one;
		_deathParticle.transform.rotation = Quaternion.Euler(0, 0, 0);

        gameObject.SetActive(true);
    }

    public IEnumerator HitHeart()
    {
        moveSpeed = 0;
		_rigidbody.constraints = RigidbodyConstraints.FreezeAll;
		moveSpeed = 0;
        while(!GameManager.instance.IsGame()){
            GameManager.instance.Damage(attackPower);
            yield return new WaitForSeconds(attackSpeed);
            Debug.Log("DAMAGE");
        }
        
    }

	public void Damage(int amount) {
		_nowHp -= amount;
		if (_nowHp <= 0) {
			_nowHp = 0;
			Death();
		}
        if (name.Equals("") || name == null) name = "테스트용 복셀균";
		StartCoroutine(HealthBar.instance.ViewUi(maxHp, _nowHp, name));
	}

	public void Death() {
		// health init code here.
		_deathParticle.transform.SetParent(null);
		_deathParticle.Play();

		transform.localScale = Vector3.zero;
		_rigidbody.velocity = Vector3.zero;

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

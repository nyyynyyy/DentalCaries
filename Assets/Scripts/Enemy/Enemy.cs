using UnityEngine;
using System.Collections;

public enum State {
	Move,
	Attack,
	Idle
}

public enum EnmeyAbility
{
    Normal,
    Fast,
    Slow,
    Strong,
    Super,
}

public class Enemy : MonoBehaviour {

    [Header("State")]
    public EnemyType _type;

    private EnmeyAbility _ability;

    private string _name;
    private float _moveSpeed;
    private float _attackSpeed;
    private float _attackPower;

    private float _maxHp;
    private float _nowHp;

    private int _gold;

    private Vector3 _startPos = Vector3.zero;

    private MoveEnemy _move;
	private Rigidbody _rigidbody;
	private RigidbodyConstraints _rigidbodyBaseConstraints;
	private State _state;

    #region Property
    public bool isDeath
    {
        get
        {
            return _state == State.Idle;
        }
    }

    public float maxHp
    {
        get
        {
            return _maxHp;
        }
    }

    public float nowHp
    {
        get
        {
            return _nowHp;
        }
    }

    public string name
    {
        get
        {
            return _name;
        }
    }

    public float moveSpeed
    {
        get
        {
            return _moveSpeed;
        }
    }

    public float travelDistancePer
    {
        get
        {
            float max = _startPos.x * _startPos.x + _startPos.z * _startPos.z;
            float now = transform.position.x * transform.position.x + transform.position.z * transform.position.z;
            return now / max;
        }
    }

    public State state
    {
        get { return _state; }
    }
    #endregion

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

    public void Spawn(Vector3 spawnPoint, Transform targetPoint, Wave wave)
    {
		transform.position = spawnPoint;
        _startPos = spawnPoint;
		_move.SetTarget(targetPoint);

        _ability = wave.ability;
        _maxHp = _nowHp = wave.hp;
        _moveSpeed = wave.moveSpeed;
        _attackPower = wave.attackPower;
        _attackSpeed = wave.attackSpeed;
        _gold = wave.gold;

		_rigidbody.constraints = _rigidbodyBaseConstraints;

        MixAbility(wave.ability, wave.name);

		_state = State.Move;


        gameObject.SetActive(true);
    }

    private void MixAbility(EnmeyAbility ability, string name)
    {
        switch (ability)
        {
            case EnmeyAbility.Normal:
                _name =  name;
                break;
            case EnmeyAbility.Fast:
                _name = "빠른 " + name;
                _moveSpeed *= 1.5f;
                _gold *= 2;
                break;
            case EnmeyAbility.Slow:
                _name = "느린 " + name;
                _moveSpeed *= 0.5f;
                _attackPower *= 2f;
                break;
            case EnmeyAbility.Strong:
                _name = "튼튼 " + name;
                _maxHp *= 3f;
                _nowHp = _maxHp;
                _gold *= 2;
                break;
            case EnmeyAbility.Super:
                _name = "슈퍼 " + name;
                _maxHp *= 50f;
                _nowHp = _maxHp;
                _gold *= 30;
                transform.localScale = new Vector3(3f, 3f, 3f);
                break;
            default:
                _name = "[버그]피드백부탁";
                break;
        }
    }

    public IEnumerator Attack() {
		_state = State.Attack;
		_rigidbody.constraints = RigidbodyConstraints.FreezeAll;
		_moveSpeed = 0;

		while (!GameManager.instance.IsGame()) {
			GameManager.instance.Damage(_attackPower);
			yield return new WaitForSeconds(_attackSpeed);
		}
    }

	public void Damage(int amount) {
		_nowHp = Mathf.Max(_nowHp - amount, 0);

		EnemyHpBar.instance.BarDecrease(this, amount);

		if (_nowHp <= 0) {
			Death();
		}
	}

	public void Death() {
		_state = State.Idle;

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

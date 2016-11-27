using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {
	public static EnemyManager instance;

    [Header("Point")]
    public Transform waitPoint;
	public Transform heartPoint;

    [Header("Enemy")]
    public Transform enemys;
    public GameObject[] enemyUnits;

	[Header("Death Particle")]
	public ParticleSystem deathParticle;
	public float deathParticleDeleteDelay;
    public int deathParticleMax;

    [Header("SpawnPoint")]
	public Transform playingPlace;

    private List<Enemy> _enemyList = new List<Enemy>();
	private List<ParticleSystem> _deathParticlList = new List<ParticleSystem>();

	// spawnPoint
	private List<Transform> _spawnPoints;

    void Awake()
    {
        if (instance)
        {
            Debug.LogError("Already loaded instance: EnemyManager.cs");
            return;
        }

        instance = this;

        _spawnPoints = new List<Transform>();
    }

	void Start () {
        Debug.Log("ENEMY MANGER IS READY");

        SetParticle();
        SetSpawnPoints();
    }

    #region Particle Method
    private void SetParticle()
    {
        GameObject particleParent = new GameObject("DeathParticles");

        for (int i = 0; i < deathParticleMax; i++)
        {
            GameObject particle = (GameObject)Instantiate(deathParticle.gameObject);

            particle.name = "DeathParticle" + i;

            particle.transform.parent = particleParent.transform;

            _deathParticlList.Add(particle.GetComponent<ParticleSystem>());

            particle.SetActive(false);
        }
    }

    public void CreateDeathParticle(Transform transform) {
		ParticleSystem particle = _deathParticlList.Find(o => !o.gameObject.activeInHierarchy);
		StartCoroutine(ParticleActiveDelay(particle, deathParticleDeleteDelay));

		particle.transform.position = transform.position;
		particle.Play();
	}

	private IEnumerator ParticleActiveDelay(ParticleSystem particle, float delay) { 
		particle.gameObject.SetActive(true);
		yield return new WaitForSeconds(delay);
		particle.Clear();
		particle.gameObject.SetActive(false);
	}
    #endregion

    #region Enemy Method
    public void CreateEnemy(EnemyType type, float hp, float moveSpeed, float attackPower, float attackSpeed)
    {
		Enemy selectedEnemy = _enemyList.Find(o => !o.gameObject.activeInHierarchy && o._type == type);
        if (!selectedEnemy)
        {
            Debug.Log("Enemy is not ready :: Check Enemy List");
        }
		if (selectedEnemy && CanSpawnEnemy())
		{
			selectedEnemy.Spawn(UseSpawnPoint(selectedEnemy).position, heartPoint, hp, moveSpeed, attackPower, attackSpeed);
			//side random move
			System.Random random = new System.Random();
			Vector3 randomPos = selectedEnemy.transform.position + (Vector3.right * random.Next(4));
			selectedEnemy.transform.position = randomPos;
		}
    }

    public void SetEnemy(EnemyType type, int max) { 
        for (int i = 0; i < max; i++)
        {
            GameObject unit = (GameObject)Instantiate(enemyUnits[(int)type], waitPoint.position, Quaternion.identity);
            Enemy enemy = unit.GetComponent<Enemy>();

            unit.name = type.ToString() + ":" + i;
            unit.transform.parent = enemys;

            enemy.Init(type);
            _enemyList.Add(enemy);

            unit.SetActive(false);
        }
    }

    public void ClearEnemy()
    {
        for(int i = 0; i < _enemyList.Count; i++)
        {
            DestroyImmediate(_enemyList[i].gameObject);
        }
        _enemyList.Clear();
    }
    #endregion

    #region INSI
    /**** Spawn Point ****/
    private bool CanSpawnEnemy() {
		return _spawnPoints.Count > 0;
	}

	private Transform UseSpawnPoint(Enemy enemy) {
		if (!CanSpawnEnemy()) {
			return null;
		}

		System.Random random = new System.Random();

		int index = random.Next(_spawnPoints.Count);
		Transform spawnPoint = _spawnPoints[index];
		_spawnPoints.Remove(spawnPoint);

		StartCoroutine(FreeSpawnPoint(spawnPoint));
		return spawnPoint;
	}

	private IEnumerator FreeSpawnPoint(Transform spawnPoint) {
		yield return new WaitForSeconds(3);
		_spawnPoints.Add(spawnPoint);
	}

	private void SetSpawnPoints() { 
		float movePos;
		float fixingPos = 0.45F;
		float y = 0.7F;
		for (int i = 0; i <= 4; i++) {
			movePos = 0.35F - (0.175F * i);

			CreateSpawnPoint(fixingPos, y, movePos);
			CreateSpawnPoint(-fixingPos, y, movePos);

			CreateSpawnPoint(movePos, y, fixingPos);
			CreateSpawnPoint(movePos, y, -fixingPos);
		}
	}

	private void CreateSpawnPoint(float x, float y, float z)
	{
		GameObject testObject = new GameObject();

		testObject.name = "SpawnPoint";
		testObject.transform.SetParent(playingPlace);
		testObject.transform.localPosition = new Vector3(x, y, z);

		_spawnPoints.Add(testObject.transform);
	}
    #endregion
}

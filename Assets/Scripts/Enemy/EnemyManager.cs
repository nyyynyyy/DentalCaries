﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {
	public static EnemyManager instance;

    [Header("Point")]
    public Transform waitPoint;
	public Transform heartPoint;

    [Header("Enemy")]
    public GameObject[] enemyUnit;
    public int maxEnemy = 15;

	[Header("Death Particle")]
	public ParticleSystem deathParticle;
	public float deathParticleDeleteDelay;

	[Header("SpawnPoint")]
	public Transform playingPlace;

    private List<Enemy> _enemyList = new List<Enemy>();
	private List<ParticleSystem> _deathParticlList = new List<ParticleSystem>();

	// spawnPoint
	private List<Transform> _spawnPoints;

	// Use this for initialization
	void Start () {
		if (instance) {
			Debug.LogError("Already loaded instance: EnemyManager.cs");
			return;
		}

		instance = this;
		_spawnPoints = new List<Transform>();

        SetEnemy();
		SetSpawnPoints();
        //StartCoroutine(Round());
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

    public void CreateEnemy(float hp, float moveSpeed, float attackPower, float attackSpeed)
    {
		Enemy selectedEnemy = _enemyList.Find(o => !o.gameObject.activeInHierarchy);
		if (selectedEnemy && CanSpawnEnemy())
		{
			selectedEnemy.Spawn(UseSpawnPoint(selectedEnemy).position, heartPoint, hp, moveSpeed, attackPower, attackSpeed);
			//side random move
			System.Random random = new System.Random();
			Vector3 randomPos = selectedEnemy.transform.position + (Vector3.right * random.Next(4));
			selectedEnemy.transform.position = randomPos;
		}
    }

    private void SetEnemy()
    {
        GameObject tempParent = new GameObject("Enemys");
		GameObject tempParticleParent = new GameObject("DeathParticles");
		tempParticleParent.transform.parent = tempParent.transform;

        for (int i = 0; i < maxEnemy; i++)
        {
            GameObject temp = (GameObject)Instantiate(enemyUnit[0], waitPoint.position, Quaternion.identity);
            GameObject tempParticle = (GameObject)Instantiate(deathParticle.gameObject);

			temp.name = "Enemy" + i;
			tempParticle.name = "DeathParticle" + i;

            temp.transform.parent = tempParent.transform;
			tempParticle.transform.parent = tempParticleParent.transform;

            _enemyList.Add(temp.GetComponent<Enemy>());
			_deathParticlList.Add(tempParticle.GetComponent<ParticleSystem>());

            temp.SetActive(false);
			tempParticle.SetActive(false);
        }
    }

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
	/********************/


	//Junk Source
	//private Vector3 GetSpawnPoint()
	//{
	//	Vector3 result;
	//	int x = 0, y = 0;

	//	switch (Random.Range(0, 4))
	//	{
	//		case 0: // N
	//			x = Random.Range(-24, 24);
	//			y = 14;
	//			break;
	//		case 1: // E
	//			x = 14;
	//			y = Random.Range(-24, 24);
	//			break;
	//		case 2: // S
	//			x = Random.Range(-24, 24);
	//			y = -14;
	//			break;
	//		case 3: // W
	//			x = -14;
	//			y = Random.Range(-24, 24);
	//			break;
	//	}
	//	result = new Vector3(x, 2f, y);
	//	return result;
	//}
}

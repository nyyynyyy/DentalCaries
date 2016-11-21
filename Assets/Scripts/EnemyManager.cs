using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {
	private static EnemyManager instance;

    [Header("Point")]
    public Transform waitPoint;

    [Header("Enemy")]
    public GameObject[] enemyUnit;
    public int maxEnemy = 15;

	[Header("SpawnPoint")]
	public Transform playingPlace;

    private List<Enemy> enemyList = new List<Enemy>();

	// spawnPoint
	private List<Transform> _spawnPoints;

	// Use this for initialization
	void Start () {
		if (instance) {
			Debug.LogError("Already loaded instance: Test.cs");
			return;
		}

		instance = this;
		_spawnPoints = new List<Transform>();

        SetEnemy();
		SetSpawnPoints();
        StartCoroutine(Round());
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    
    private IEnumerator Round()
    {
        CreateEnemy();
        yield return new WaitForSeconds(0.2F);
        StartCoroutine(Round());
    }

    // @Develope..
    public void CreateEnemy()
    {
        Enemy selectedEnemy = enemyList.Find(o => !o.gameObject.active);
        //selectedEnemy.Spawn(GetSpawnPoint(), 2f, 3);
		if (selectedEnemy && CanSpawnEnemy())
			
		{
			selectedEnemy.Spawn(UseSpawnPoint(selectedEnemy).position, 1f, 3);
		}
		else {
			Debug.Log("asdf");
		}
    }

    private void SetEnemy()
    {
        GameObject tempParent = new GameObject();
        tempParent.name = "Enemys";
        for (int i = 0; i < maxEnemy; i++)
        {
            GameObject temp = (GameObject)Instantiate(enemyUnit[0], waitPoint.position, Quaternion.identity);
            temp.name = "Enemy" + i;
            temp.transform.parent = tempParent.transform;
            enemyList.Add(temp.GetComponent<Enemy>());
            temp.SetActive(false);
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

	/** Junk Source **/

	// @Develope..
	//private Vector3 GetSpawnPoint()
	//{
	//    Vector3 result;
	//    int x=0, y=0;

	//    switch(Random.Range(0, 4))
	//    {
	//        case 0: // N
	//            x = Random.Range(-24, 24);
	//            y = 14;
	//            break;
	//        case 1: // E
	//            x = 14;
	//            y = Random.Range(-24, 24);
	//            break;
	//        case 2: // S
	//            x = Random.Range(-24, 24);
	//            y = -14;
	//            break;
	//        case 3: // W
	//            x = -14;
	//            y = Random.Range(-24, 24);
	//            break;         
	//    }
	//    result = new Vector3(x, 2f, y);
	//    return result;
	//}
}
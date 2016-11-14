using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {

    [Header("Point")]
    public Transform waitPoint;

    [Header("Enemy")]
    public GameObject[] enemyUnit;

    public int maxEnemy = 15;

    private List<Enemy> enemyList = new List<Enemy>();

	// Use this for initialization
	void Start () {
        SetEnemy();
        StartCoroutine(Round());
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    
    private IEnumerator Round()
    {
        CreateEnemy();
        yield return new WaitForSeconds(2);
        StartCoroutine(Round());
    }

    public void CreateEnemy()
    {
        Enemy selectedEnemy = enemyList.Find(o => !o.gameObject.active);
        selectedEnemy.Spawn(GetSpawnPoint());
    }

    private Vector3 GetSpawnPoint()
    {
        Vector3 result;
        int x=0, y=0;

        switch(Random.Range(0, 4))
        {
            case 0: // N
                x = Random.Range(-24, 24);
                y = 14;
                break;
            case 1: // E
                x = 14;
                y = Random.Range(-24, 24);
                break;
            case 2: // S
                x = Random.Range(-24, 24);
                y = -14;
                break;
            case 3: // W
                x = -14;
                y = Random.Range(-24, 24);
                break;         
        }
        result = new Vector3(x, 2f, y);
        return result;
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
}

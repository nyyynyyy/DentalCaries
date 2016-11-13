using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {

    [Header("Point")]
    public Transform[] spawnPoint;
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
        yield return new WaitForSeconds(1);
        StartCoroutine(Round());
    }

    public void CreateEnemy()
    {
        Enemy selectedEnemy = enemyList.Find(o => !o.gameObject.active);
        selectedEnemy.Spawn(spawnPoint[2].position);
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

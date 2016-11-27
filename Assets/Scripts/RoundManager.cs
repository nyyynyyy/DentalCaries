using UnityEngine;
using System.Collections;

public class RoundManager : MonoBehaviour {

    public int roundNum;

	void Start () {
        Debug.Log("ROUND MANGER IS READY");
        EnemyManager.instance.SetEnemy(EnemyType.N001, 15);
        //EnemyManager.instance.ClearEnemy();
        StartCoroutine(Round());
    }

    private IEnumerator Round()
    {
        EnemyManager.instance.CreateEnemy(EnemyType.N001, 10, 0.5f, 1f, 1f);
        yield return new WaitForSeconds(1F);
        StartCoroutine(Round());
    }
}

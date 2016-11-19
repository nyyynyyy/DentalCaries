using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Test : MonoBehaviour {
	private static Test instance;

	[Header("Goal Target")]
	public Transform goalTarget;

	[Header("Spawnpoint GameObject")]
	public GameObject spawnPointPrefab;

	private Dictionary<Enemy, GameObject> _usingSpawnPoints;
	private List<GameObject> _spawnPoints;

	private Test() {
		if (instance) {
			Debug.LogError("Already loaded instance: Test.cs");

		}

		instance = this;
		_usingSpawnPoints = new Dictionary<Enemy, GameObject>();
		_spawnPoints = new List<GameObject>();

	}

	// Use this for initialization
	void Start () {
		float movePos;
		float fixingPos = 0.45F;
		float y = 0.7F;
		for (int i = 0; i <= 4; i++) {
			movePos = 0.35F - (0.175F * i);

			CreatePoint(fixingPos, y, movePos);
			CreatePoint(-fixingPos, y, movePos);

			CreatePoint(movePos, y, fixingPos);
			CreatePoint(movePos, y, -fixingPos);
		}
	}

	private void CreatePoint(float x, float y, float z) {
		GameObject testObject = Instantiate(spawnPointPrefab);

		testObject.transform.SetParent(transform);
		testObject.transform.localPosition = new Vector3(x, y, z);
	}

	public static bool SpawnEnemy(Enemy enemy) {
		if(instance._spawnPoints.Count <= 0)
			return false;


		return true;
	}
}

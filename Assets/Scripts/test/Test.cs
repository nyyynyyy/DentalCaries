using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {
	public GameObject testPrefab;

	// Use this for initialization
	void Start () {
		float movePos;
		float fixingPos = 0.45F;
		float y = 0.7F;
		for (int i = 0; i <= 4; i++) {
			movePos = 0.35F - (0.175F * i);

			SpawnObject(fixingPos, y, movePos);
			SpawnObject(-fixingPos, y, movePos);

			SpawnObject(movePos, y, fixingPos);
			SpawnObject(movePos, y, -fixingPos);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void SpawnObject(float x, float y, float z) {
		GameObject testObject = Instantiate(testPrefab);
		testObject.transform.SetParent(transform);
		testObject.transform.localPosition = new Vector3(x, y, z);
	}
}

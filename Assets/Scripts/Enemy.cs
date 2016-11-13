using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public float speed = 3f;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Spawn(Vector3 spawnPoint)
    {
        transform.position = spawnPoint;
        gameObject.SetActive(true);
    }

    public void HitHeart()
    {
        speed = 0;
        Debug.Log("HIT");
    }
}

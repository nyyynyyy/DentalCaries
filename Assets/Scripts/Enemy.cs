using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    [Header("State")]
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _health = 3f;

    public float speed
    {
        get
        {
            return _speed;
        }
        set
        {
            _speed = Mathf.Max(value, 0);
        }
    }

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

	public void Damage() {
		_health--;
		if (_health <= 0) {
			Death();
		}
	}

	public void Death() {
		// health init code here.
		gameObject.SetActive(false);
	}
}

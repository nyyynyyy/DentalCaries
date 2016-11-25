using UnityEngine;
using System.Collections;

public abstract class Weapon : MonoBehaviour {
	
	[Header("Option")]
	public float fireDelay; 

	void OnTriggerEnter(Collider other)
	{
		EnemyCheck(other);
	}

	private void EnemyCheck(Collider other)
	{
		if (other.tag == "Enemy")
		{
			Enemy enemy = other.GetComponent<Enemy>();
			HitEnemy(enemy);
		}
	}

	public abstract void Delete();

	public abstract void Fire();

	public abstract void HitEnemy(Enemy enemy);
}

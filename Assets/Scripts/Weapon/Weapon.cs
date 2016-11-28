using UnityEngine;

public abstract class Weapon : MonoBehaviour {

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

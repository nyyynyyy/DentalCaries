using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BulletManager : MonoBehaviour {
	private static BulletManager instance;

	private BulletManager() {
		if (instance) {
			Debug.LogError("Already existing instance BulletManager.");
			return;
		}
		instance = this;
	}

	public GameObject bulletPrefab;
	public int maxBullet = 20;

    public Camera mainCamera;
    public Transform shootLocation;
    public float shootDelay = 0.5F;

    private Queue<GameObject> _bulletQueue = new Queue<GameObject>();
	private GameObject _bulletsStorage;

    private WaitForSeconds waitForShootDelay;

    void Awake()
	{
		
	}

	void Start() {
        SetBullet();
        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        waitForShootDelay = new WaitForSeconds(shootDelay);
        while (true)
        {
            if (Input.GetMouseButton(0) && !ViewManager.instance.isTps)
            {
                Vector3 targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
                BulletManager.Shoot(shootLocation.transform.position, targetPosition);
                yield return waitForShootDelay;
            }
            yield return null;
        }
    }

    private void SetBullet()
    {
        _bulletsStorage = new GameObject();
        _bulletsStorage.name = "Bullets";
        // 총알 20개를 미리 만들어둠
        for (int i = 0; i < maxBullet; i++)
        {
            Bullet bullet = Instantiate(bulletPrefab).GetComponent<Bullet>();
            bullet.transform.SetParent(_bulletsStorage.transform);
            SetDisableBullet(bullet);
        }
        Bullet.SetBulletManager(this);
    }

	// 총알 게임 오브젝트를 비활성화 후 저장
	public void SetDisableBullet(Bullet bullet) {
		GameObject bulletObject = bullet.gameObject;

		bulletObject.SetActive(false);
		bulletObject.transform.localPosition = Vector3.zero;

		_bulletQueue.Enqueue(bulletObject);
	}

	public static void Shoot(Vector3 shootLocation, Vector3 targetLocation) { 
		if (instance._bulletQueue.Count <= 0) { return; }

		GameObject currentBullet = instance._bulletQueue.Dequeue();
		Bullet bulletScript = currentBullet.GetComponent<Bullet>();

		currentBullet.transform.position = shootLocation;
		currentBullet.transform.LookAt(targetLocation);

		currentBullet.SetActive(true);

		bulletScript.Fire();
	}
}

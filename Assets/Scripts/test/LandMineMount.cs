using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SelectPoint {
	public int x, z;

	public SelectPoint(int x, int y) {
		this.x = x;
		this.z = y;
	}
}

public class LandMineMount : MonoBehaviourC {

	[Header("Map")]
	public Transform map;

	[Header("Blocks Option")]
	public int size;
	public int centerLange;

	[Header("LandMine Prefab")]
	public LandMine landMinePrefab;

	[Header("Selector")]
	public Transform selector;

	private Vector3[,] _locArray;
	private LandMine[,] _landMineArray;

	private Vector3 _selectorLoc;
	private int _fieldLayerMask;
	private float _blockSize;

	private SelectPoint _currentPoint;
	private SelectPoint emptyPoint = new SelectPoint(-1, -1);

	// Use this for initialization
	void Start () {
		if (map.lossyScale.x - map.lossyScale.z > 0f) {
			throw new System.Exception("map x and z size is no same");
		} else if (size % 2 == 0) {
			throw new System.Exception("size is no odd number");
		}

		_locArray = new Vector3[size, size];
		_landMineArray = new LandMine[size, size];
		_selectorLoc = Vector3.zero;
		_fieldLayerMask = 1 << LayerMask.NameToLayer("PlayField");
		_blockSize = map.lossyScale.x / size;

		ResizeSelector();

		Vector3 mapCenter = map.position;
		for (int z = 0; z < size; z++) { 
			for (int x = 0; x < size; x++) {
				Vector3 loc = mapCenter;
				loc.x = (x - size / 2) * _blockSize;
				loc.z = (z - size / 2) * _blockSize;
				loc.y = 1;

				_locArray[x, z] = loc;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (ViewManager.instance.viewMode != ViewType.MINE) {
			selector.gameObject.SetActive(false);
			_currentPoint = emptyPoint;
			return;
		}

		if (!GetPingerDown()) {
			return;
		}

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;

		if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, _fieldLayerMask)) {
			int x = Mathf.FloorToInt((hitInfo.point.x + _blockSize / 2) / _blockSize) + size / 2;
			int z = Mathf.FloorToInt((hitInfo.point.z + _blockSize / 2) / _blockSize) + size / 2;

			// Tiles: [0, 0] ~ [size, size]
			SelectPoint point = new SelectPoint(x, z);

			// 'heart' gameObject ignore place
			if (IsCenterLoc(point)) {
				return;
			}

			// mount landmine, and return
			if (_currentPoint.Equals(point)) {
				MountLandMine(point);
				selector.gameObject.SetActive(false);
				return;
			}

			// selector location and color setting
			_selectorLoc.x = transform.position.x + (x - size / 2) * _blockSize;
			_selectorLoc.z = transform.position.z + (z - size / 2) * _blockSize;

			selector.position = _selectorLoc;
			Color selectorColor;
			if (_landMineArray[x, z] == null && GameManager.instance.money > 50) {
				selectorColor = Color.green;
				_currentPoint = point;
			} else {
				selectorColor = Color.red;
			}

			selector.GetComponent<MeshRenderer>().material.color = selectorColor;
			selector.gameObject.SetActive(true);
		}
	}

	private void MountLandMine(SelectPoint point) {
		if (!GameManager.instance.SpendMoney(50)) {
			return;
		}
		_currentPoint = emptyPoint;

		LandMine landMine = Instantiate(landMinePrefab);
		landMine.transform.position = _locArray[point.x, point.z];
		landMine.transform.localScale = landMine.transform.localScale * _blockSize;

		//RoundManager.instance.
		_landMineArray[point.x, point.z] = landMine;
	}

	private bool IsCenterLoc(SelectPoint point) {
		return IsCenter(point.x, size, centerLange) && IsCenter(point.z, size, centerLange);
	}

	private bool IsCenter(int value, int maxValue, int range) {
		// if size 25 = 12
		int center = maxValue / 2;
		return Mathf.Abs(center - value) <= range;
	}

	private void ResizeSelector() {
		selector.localScale = new Vector3(_blockSize, selector.localScale.y, _blockSize);
	}
}
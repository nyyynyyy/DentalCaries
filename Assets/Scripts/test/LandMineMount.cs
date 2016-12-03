using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMineMount : MonoBehaviour {

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
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;

		if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, _fieldLayerMask)) {
			int x = Mathf.FloorToInt((hitInfo.point.x + _blockSize / 2) / _blockSize) + size / 2;
			int z = Mathf.FloorToInt((hitInfo.point.z + _blockSize / 2) / _blockSize) + size / 2;

			if (IsCenterLoc(x, z)) {
				return;
			}

			_selectorLoc.x = transform.position.x + (x - size / 2) * _blockSize;
			_selectorLoc.z = transform.position.z + (z - size / 2) * _blockSize;

			selector.position = _selectorLoc;
			Color selectorColor;
			if (_landMineArray[x, z] == null) {
				selectorColor = Color.green;
			} else {
				selectorColor = Color.red;
			}
			selectorColor.a = 0.5f;

			selector.GetComponent<MeshRenderer>().material.color = selectorColor;

			if (Input.GetMouseButtonDown(0)) {
				LandMine landMine  = Instantiate(landMinePrefab);
				landMine.transform.position = _locArray[x, z];
				landMine.transform.localScale = landMine.transform.localScale * _blockSize;

				_landMineArray[x, z] = landMine;
			}
		}
	}

	private bool IsCenterLoc(int x, int z) {
		return IsCenter(x, size, centerLange) && IsCenter(z, size, centerLange);
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

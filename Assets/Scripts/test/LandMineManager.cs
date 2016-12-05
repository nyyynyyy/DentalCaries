using UnityEngine;

#region Land Mine struct serializable
[System.Serializable]
public class LandMineData {
	[Header("Base")]
	public string name;
	public int buildCost;
	public LandMine landMine;

	[Header("Attack")]
	public int damage;
	public float delay;

	[Header("Upgrade")]
	public LandMineUpgrade[] upgradeData;

	private int _upgradeUnit = 0;

	public LandMineData(LandMineData data, LandMine landMine) {
		this.name = data.name;
		this.buildCost = data.buildCost;

		this.damage = data.damage;
		this.delay = data.delay;

		this.upgradeData = data.upgradeData;

		this.landMine = landMine;

		this.landMine.Init(this);
	}

	private bool HasNextUpgrade() {
		return _upgradeUnit < upgradeData.Length;
	}

	public void Upgrade() { 
		
	}
}

[System.Serializable]
public struct LandMineUpgrade {
	[Header("Cost")]
	public int cost;

	[Header("Attack")]
	public int damage;
	public float delay;
}
#endregion

public class LandMineManager : MonoBehaviourC {
	private struct SelectPoint {
		public int x, z;

		public SelectPoint(int x, int y) {
			this.x = x;
			this.z = y;
		}
	}

	[Header("Map")]
	public Transform map;

	[Header("Grid Option")]
	public int gridUnit;
	public int centerLange;

	[Header("LandMine Data")]
	public LandMineData[] landMines;

	[Header("Selector")]
	public Transform gridSelector;

	// grid
	private Vector3[,] _gridLocArray;
	private LandMineData[,] _mountLandMineDataArray;

	// land mines parent GameObject
	private Transform _landMineStorage;

	// grid selector loc
	private Vector3 _gridSelectorLoc;

	// map layer except others layer
	private int _fieldLayerMask;

	// a grid size
	private float _gridSize;

	// current selected LandMineData
	private LandMineData _currentLandMine;

	// select point relation variable
	private SelectPoint _selectedPoint;
	private readonly SelectPoint noneSelectPoint = new SelectPoint(-1, -1);

	void Start () {
		if (map.lossyScale.x - map.lossyScale.z > 0f) {
			throw new System.Exception("map x and z size is no same.");
		} else if (gridUnit % 2 == 0) {
			throw new System.Exception("grid unit is no odd number.");
		} else if (landMines.Length == 0 || landMines[0].landMine == null) { 
			throw new System.Exception("landMines field is empty.");
		}

		#region variable initialize
		_landMineStorage = new GameObject("LandMines").transform;
		_gridLocArray = new Vector3[gridUnit, gridUnit];
		_mountLandMineDataArray = new LandMineData[gridUnit, gridUnit];
		_gridSelectorLoc = Vector3.zero;
		_fieldLayerMask = 1 << LayerMask.NameToLayer("PlayField");
		_gridSize = map.lossyScale.x / gridUnit;
		_currentLandMine = landMines[0];

		foreach (LandMineData landMineData in landMines) {
			landMineData.landMine.Init(landMineData);
		}

		ResizeSelector();
		#endregion

		#region grid initialize
		Vector3 mapCenter = map.position;
		//float locY = transform.position.y;
		for (int z = 0; z < gridUnit; z++) { 
			for (int x = 0; x < gridUnit; x++) {
				Vector3 loc = mapCenter;
				loc.x = (x - gridUnit / 2) * _gridSize;
				loc.z = (z - gridUnit / 2) * _gridSize;
				loc.y = -0.375f;

				_gridLocArray[x, z] = loc;
			}
		}
		#endregion
	}
	
	void Update () {
		if (ViewManager.instance.viewMode != ViewType.MINE) {
			gridSelector.gameObject.SetActive(false);
			_selectedPoint = noneSelectPoint;
			return;
		}

		if (!GetPingerDown()) {
			return;
		}

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;

		if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, _fieldLayerMask)) {
			int x = Mathf.FloorToInt((hitInfo.point.x + _gridSize / 2) / _gridSize) + gridUnit / 2;
			int z = Mathf.FloorToInt((hitInfo.point.z + _gridSize / 2) / _gridSize) + gridUnit / 2;

			// Tiles: [0, 0] ~ [size, size]
			SelectPoint point = new SelectPoint(x, z);
			ResetSelector(point);
		}
	}

	private void ResizeSelector() {
		gridSelector.localScale = new Vector3(_gridSize, gridSelector.localScale.y, _gridSize);
	}

	private void ResetSelector(SelectPoint point) { 
		// 'heart' gameObject ignore place
		if (IsCenterLoc(point)) {
			return;
		}

		// selector location and color setting
		_gridSelectorLoc.x = transform.position.x + (point.x - gridUnit / 2) * _gridSize;
		_gridSelectorLoc.z = transform.position.z + (point.z - gridUnit / 2) * _gridSize;
		Color selectorColor;

		_selectedPoint = point;
		if (_mountLandMineDataArray[point.x, point.z] != null) {
			// landMine Select
			selectorColor = Color.blue;
			ViewManager.instance.SelectedMine();
		} else if (GameManager.instance.money >= 50) {
			// grid Select
			selectorColor = Color.green;
			ViewManager.instance.SelectedGrid();
		} else {
			// no money
			_selectedPoint = noneSelectPoint;
			selectorColor = Color.red;
		}

		ViewManager.instance.LeaveUndo();

		gridSelector.position = _gridSelectorLoc;
		gridSelector.GetComponent<MeshRenderer>().material.color = selectorColor;
		gridSelector.gameObject.SetActive(true);
	}

	private LandMineData RemoveMount(SelectPoint point) { 
		LandMineData removeMount = _mountLandMineDataArray[point.x, point.z];
		_mountLandMineDataArray[point.x, point.z] = null;
		Destroy(removeMount.landMine.gameObject);

		return removeMount;
	}

	#region button
	public void ButtonMount() {
		if (_selectedPoint.Equals(noneSelectPoint) || !GameManager.instance.SpendMoney(_currentLandMine.buildCost)) {
			return;
		}

		LandMine landMine = Instantiate(_currentLandMine.landMine);
		LandMineData mountLandMineData = new LandMineData(_currentLandMine, landMine);

		landMine.transform.SetParent(_landMineStorage);
		landMine.transform.position = _gridLocArray[_selectedPoint.x, _selectedPoint.z];
		landMine.transform.localScale = landMine.transform.localScale * _gridSize;

		//RoundManager.instance.
		_mountLandMineDataArray[_selectedPoint.x, _selectedPoint.z] = mountLandMineData;

		gridSelector.gameObject.SetActive(false);

		ViewManager.instance.ChanceUndo();
	}

	public void ButtonUndoMount() {
		if (_selectedPoint.Equals(noneSelectPoint)) {
			return;
		}

		GameManager.instance.TakeMoney(RemoveMount(_selectedPoint).buildCost);

		ResetSelector(_selectedPoint);
	}

	public void ButtonRemove() { 
		if (_selectedPoint.Equals(noneSelectPoint)) {
			return;
		}

		RemoveMount(_selectedPoint);

		ResetSelector(_selectedPoint);
	}

	public void ButtonCancle() {
		_selectedPoint = noneSelectPoint;
		gridSelector.gameObject.SetActive(false);
		ViewManager.instance.LeaveUndo();
	}
	#endregion
		
	#region center check function
	private bool IsCenterLoc(SelectPoint point) {
		return IsCenter(point.x, gridUnit, centerLange) && IsCenter(point.z, gridUnit, centerLange);
	}

	private bool IsCenter(int value, int maxValue, int range) {
		// if size 25 = 12
		int center = maxValue / 2;
		return Mathf.Abs(center - value) <= range;
	}
	#endregion
}
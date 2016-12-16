using UnityEngine;

#region Land Mine class serializable
[System.Serializable]
public struct LandMineData {
	[Header("Base")]
	public string name;
	public int buildCost;
	public int durability;

	[Header("LandMine GameObject Prefab")]
	public LandMine landMine;

	[Header("Attack Info")]
	public int damage;
	public float delay;

	[Header("Upgrade")]
	public LandMineUpgrade[] upgradeData;
}

[System.Serializable]
public class LandMineUpgrade 
{
	[Header("Cost")]
	public int cost;

	[Header("Attack")]
	public int damage;
	public float delay;
}
#endregion

public class LandMineManager : MonoBehaviourC 
{
	private struct SelectPoint 
	{
		public int x, z;

		private static SelectPoint nonePoint = new SelectPoint(-1, -1);

		public SelectPoint(int x, int y) 
		{
			this.x = x;
			this.z = y;
		}

		public static SelectPoint none 
		{ 
			get { return nonePoint; }
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public static bool operator ==(SelectPoint point1, SelectPoint point2) {
			return point1.Equals(point2);
		}

		public static bool operator !=(SelectPoint point1, SelectPoint point2)
		{
			return !point1.Equals(point2);
		}
	}

	#region IUndoExcutor, MountUndo, UpgradeUndo
	private interface IUndoExcutor 
	{
		void Undo(LandMineManager manager);
	}

	private class MountUndo : IUndoExcutor
	{
		private SelectPoint _point;
		private int _cost;

		public MountUndo(SelectPoint point, int cost) 
		{
			_point = point;
			_cost = cost;
		}

		public void Undo(LandMineManager manager)
		{
			manager.RemoveMount(_point);
			GameManager.instance.TakeMoney(_cost);
		}
	}

	private class UpgradeUndo : IUndoExcutor 
	{
		private LandMine _landMine;
		private int _cost;

		public UpgradeUndo(LandMine landMine, int cost) 
		{
			_landMine = landMine;
			_cost = cost;
		}

		public void Undo(LandMineManager manager)
		{
			_landMine.Downgrade();
			GameManager.instance.TakeMoney(_cost);
		}
	}
	#endregion

	#region variable
	[Header("Map")]
	public Transform _map;

	[Header("Grid Option")]
	public int _gridUnit;
	public int _centerLange;

	[Header("LandMine Data")]
	//public LandMineDurabilitySlider _durabilitySliderObject;
	public LandMineData[] _landMines;

	[Header("Selector")]
	public Transform _gridSelector;

	// grid
	private Vector3[,] _gridLocArray;
	private LandMine[,] _mountLandMineArray;

	// land mines parent GameObject
	private Transform _landMineStorage;

	// grid selector loc
	private Vector3 _gridSelectorLoc;

	// map layer except others layer
	private int _fieldLayerMask;

	// a grid size
	private float _gridSize;

	// current selected LandMineData
	private LandMineData _currentLandMineData;
	// undo data
	private IUndoExcutor _undoExcutor;

	// select point relation variable
	private SelectPoint _selectedPoint;

	#endregion

	void Start () 
	{
		if (_map.lossyScale.x - _map.lossyScale.z > 0f) 
		{
			throw new System.Exception("map x and z size is no same.");
		} 
		else if (_gridUnit % 2 == 0) 
		{
			throw new System.Exception("grid unit is no odd number.");
		} 
		else if (_landMines.Length == 0 || _landMines[0].landMine == null) 
		{ 
			throw new System.Exception("landMines field is empty.");
		}

		#region variable initialize
		_landMineStorage = new GameObject("LandMines").transform;
		_gridLocArray = new Vector3[_gridUnit, _gridUnit];
		_mountLandMineArray = new LandMine[_gridUnit, _gridUnit];
		_gridSelectorLoc = Vector3.zero;
		_fieldLayerMask = 1 << LayerMask.NameToLayer("PlayField");
		_gridSize = _map.lossyScale.x / _gridUnit;
		_currentLandMineData = _landMines[0];

		ResizeSelector();
		#endregion

		#region grid initialize
		Vector3 mapCenter = _map.position;
		//float locY = transform.position.y;
		for (int z = 0; z < _gridUnit; z++)
		{
			for (int x = 0; x < _gridUnit; x++) 
			{
				Vector3 loc = mapCenter;
				loc.x = (x - _gridUnit / 2) * _gridSize;
				loc.z = (z - _gridUnit / 2) * _gridSize;
				loc.y = -0.375f;

				_gridLocArray[x, z] = loc;
			}

		}

		ButtonCancle();
		#endregion
	}
	
	void Update () 
	{
		if (ViewManager.instance.viewMode != ViewType.MINE) 
		{
			return;
		}

		if (!GetPinger())
		{
			return;
		}

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;

		if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, _fieldLayerMask))
		{
			_undoExcutor = null;

			int x = Mathf.FloorToInt((hitInfo.point.x + _gridSize / 2) / _gridSize) + _gridUnit / 2;
			int z = Mathf.FloorToInt((hitInfo.point.z + _gridSize / 2) / _gridSize) + _gridUnit / 2;

			// Tiles: [0, 0] ~ [size, size]
			SelectPoint point = new SelectPoint(x, z);
			ResetSelector(point);
		}
	}

	private void ResizeSelector()
	{
		_gridSelector.localScale = new Vector3(_gridSize, _gridSelector.localScale.y, _gridSize);
	}

	private void ResetSelector(SelectPoint point)
	{ 
		// 'heart' gameObject ignore place
		if (IsCenterLoc(point)) {
			return;
		}

		// selector location and color setting
		_gridSelectorLoc.x = transform.position.x + (point.x - _gridUnit / 2) * _gridSize;
		_gridSelectorLoc.z = transform.position.z + (point.z - _gridUnit / 2) * _gridSize;

		_selectedPoint = point;

		Color selectorColor;
		LandMine selectLandMine;
		if ((selectLandMine = Get(point)) != null)
		{
			// landMine Select
			selectorColor = Color.blue;
			UpdateLandMine(selectLandMine);
		}
		else if (GameManager.instance.money >= _currentLandMineData.buildCost) 
		{
			// grid Select
			selectorColor = Color.green;
			ViewManager.instance.SetMineButton(GridState.Grid);
		} 
		else 
		{
			// no money
			_selectedPoint = SelectPoint.none;
			selectorColor = Color.red;
			ViewManager.instance.SetMineButton(GridState.None);
		}

		_gridSelector.position = _gridSelectorLoc;
		selectorColor.a = 0.5f;
		_gridSelector.GetComponent<MeshRenderer>().material.color = selectorColor;
		_gridSelector.gameObject.SetActive(true);
	}

	private LandMine Get(SelectPoint point)
	{
		return _mountLandMineArray[point.x, point.z];
	}

	private void Set(SelectPoint point, LandMine landMine)
	{
		_mountLandMineArray[point.x, point.z] = landMine;
	}

	private void Mount(SelectPoint point) {
		LandMine landMine = Instantiate(_currentLandMineData.landMine);
		landMine.Init(_currentLandMineData);

		landMine.transform.SetParent(_landMineStorage);
		landMine.transform.position = _gridLocArray[point.x, point.z];
		landMine.transform.localScale = landMine.transform.localScale * _gridSize;

		//RoundManager.instance.
		Set(point, landMine);

		_gridSelector.gameObject.SetActive(false);

		UpdateUndoData(new MountUndo(point, _currentLandMineData.buildCost));
		UpdateLandMine(landMine);
	}

	private LandMine RemoveMount(SelectPoint point)
	{ 
		LandMine removeMount = Get(point);
		Set(point, null);
		Destroy(removeMount.gameObject);
		ResetSelector(point);

		return removeMount;
	}

	private void Undo() 
	{
		_undoExcutor.Undo(this);
		_undoExcutor = null;

		ResetSelector(_selectedPoint);
	}

	private void UpdateUndoData(IUndoExcutor excutor) 
	{
		_undoExcutor = excutor;
	}

	private void UpdateLandMine(LandMine landMine) {
		ViewManager.instance.SetMineButton(landMine.upgradeInfo.HasNext(), landMine.CanRepair(), _undoExcutor != null);
		LandMine.AttackInfo attack = landMine.attackInfo;

		ViewManager.instance.SetMineState(
			landMine.upgradeInfo.level,
			attack.damage,
			attack.delay,
			landMine.maxDurability,
			landMine.durability
		);
	}

	#region button
	public void ButtonMount()
	{
		if (_selectedPoint == SelectPoint.none || !GameManager.instance.SpendMoney(_currentLandMineData.buildCost))
		{
			return;
		}

		Mount(_selectedPoint);
	}

	public void ButtonUndo()
	{
		Undo();
		ResetSelector(_selectedPoint);
	}

	public void ButtonRemove()
	{ 
		if (_selectedPoint == SelectPoint.none)
		{
			return;
		}

		RemoveMount(_selectedPoint);
		ResetSelector(_selectedPoint);
	}

	public void ButtonCancle()
	{
		_selectedPoint = SelectPoint.none;
		_gridSelector.gameObject.SetActive(false);
		//ViewManager.instance.LeaveUndo();
	}

	public void ButtonUpgrade()
	{ 
		if (_selectedPoint == SelectPoint.none) 
		{
			return;
		}

		LandMine landMine = Get(_selectedPoint);
		int cost;
		if (!landMine.upgradeInfo.HasNext() || !GameManager.instance.SpendMoney(cost = landMine.upgradeInfo.MoveNext().cost))
		{
			return;
		}

		landMine.Upgrade();
		UpdateUndoData(new UpgradeUndo(landMine, cost));
		ResetSelector(_selectedPoint);
	}

	public void ButtonRepair() 
	{ 
		if (_selectedPoint == SelectPoint.none)
		{
			return;
		}

		LandMine landMine = Get(_selectedPoint);
		int cost = 20;
		if (!landMine.CanRepair() || !GameManager.instance.SpendMoney(cost)) 
		{
			return;
		}

		landMine.Repair();
		UpdateLandMine(landMine);
	}
	#endregion

	#region center check function
	private bool IsCenterLoc(SelectPoint point)
	{
		return IsCenter(point.x, _gridUnit, _centerLange) && IsCenter(point.z, _gridUnit, _centerLange);
	}

	private bool IsCenter(int value, int maxValue, int range)
	{
		// if size 25 = 12
		int center = maxValue / 2;
		return Mathf.Abs(center - value) <= range;
	}
	#endregion
}
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(PlayField))]
public class PlayFieldSelect : MonoBehaviour {
	// following cursor
	[Header("Following Cursor")]
	public Transform cursor;

	// Update is called once per frame
	private Vector3 _currentCursor;
	private float blockSizeX;
	private float blockSizeZ;
	private PlayField _grid;

	private int _fieldLayerMask;

	void Start() {
		Vector3 cursorSize = transform.lossyScale;
		cursorSize.y = cursor.lossyScale.y;
		cursor.localScale = cursorSize;

		blockSizeX = cursor.localScale.x;
		blockSizeZ = cursor.localScale.z;
		_grid = GetComponent<PlayField>();
		_fieldLayerMask = 1 << LayerMask.NameToLayer("PlayField");
	}

	void Update () {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;

		if(Physics.Raycast(ray, out hitInfo, Mathf.Infinity, _fieldLayerMask)) {
			float x = (hitInfo.point.x + (blockSizeX * _grid.xSize / 2)) / blockSizeX;
			float z = (hitInfo.point.z + (blockSizeZ * _grid.zSize / 2)) / blockSizeZ;

			Debug.Log("Tile: " + Mathf.FloorToInt(x) +", "+Mathf.FloorToInt(z));

			_currentCursor.x = transform.position.x + Mathf.FloorToInt(x) * blockSizeX + blockSizeX/2;
			_currentCursor.z = transform.position.z + Mathf.FloorToInt(z) * blockSizeZ + blockSizeZ / 2;

			cursor.position = _currentCursor;
		}
	}
}

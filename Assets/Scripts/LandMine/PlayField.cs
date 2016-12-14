using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class PlayField : MonoBehaviour {

	public Transform rootField;

	public int xSize, zSize;
	public int centerLange;

	private Mesh _mesh;
	private Vector3[] vertices;

	private void Awake() {
		_mesh = GetComponent<MeshFilter>().mesh;
		_mesh = new Mesh();

		Vector3 size = rootField.localScale;
		size.x /= xSize;
		size.z /= zSize;
		size.y = 1;
		transform.localScale = size;

		Vector3 position = rootField.position;
		position.y = transform.position.y;
		position.x -= rootField.localScale.x / 2;
		position.z -= rootField.localScale.z / 2;

		transform.position = position;
			
		Generate();
	}

	private void Generate() {
		vertices = new Vector3[(xSize + 1) * (zSize + 1)];
		for (int i = 0, z = 0; z <= zSize; z++) {
			for (int x = 0; x <= xSize; x++, i++) {
				vertices[i] = new Vector3(x, 0, z);
			}
		}

		int[] triangles = new int[xSize * zSize * 6];
		for (int ti = 0, vi = 0, z = 0; z < zSize; z++, vi++) {
			for (int x = 0; x < xSize; x++, ti += 6, vi++) {
				if(IsCenterLoc(x, z, centerLange))
					continue;
				
				triangles[ti] = vi;
				triangles[ti + 3] = triangles[ti + 2] = vi + 1;
				triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
				triangles[ti + 5] = vi + xSize + 2;
			}
		}


		_mesh.vertices = vertices;
		_mesh.triangles = triangles;

		_mesh.RecalculateNormals();
		GetComponent<MeshCollider>().sharedMesh = _mesh;
	}

	private bool IsCenterLoc(int x, int z, int range) {
		return IsCenter(x, xSize, range) && IsCenter(z, zSize, range);
	}

	private bool IsCenter(int value, int size, int range) {
		// if size 25 = 12
		int center = size / 2;

		if (size % 2 == 1) {
			return Mathf.Abs(center - value) <= range;
		}

		return center + range >= value && value >= center - range - 1;
	}
}

using UnityEngine;
using System.Collections;

public class MeshDeform : MonoBehaviour {
	private Mesh mesh;

	void Start() {
//		gameObject.AddComponent("MeshFilter");
//		gameObject.AddComponent("MeshRenderer");
		mesh = GetComponent<MeshFilter>().mesh;

		mesh.Clear();
		mesh.vertices = new Vector3[] {
			new Vector3(-1, 0, -1),
			new Vector3(-1, 0, 1),
			new Vector3(1, 0, 1),
			new Vector3(1, 0, -1)
		};
		mesh.uv = new Vector2[] {
			new Vector2(-1, -1),
			new Vector2(-1, 1),
			new Vector2(1, 1),
			new Vector2(1, -1)
		};
		mesh.triangles = new int[] {0, 1, 2, 0, 2, 3};
		mesh.normals = new Vector3[] {
			new Vector3(0, 1, 0),
			new Vector3(0, 1, 0),
			new Vector3(0, 1, 0),
			new Vector3(0, 1, 0),
		};	
	}

	void Update() {
//		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//		//			Debug.Log ("mouse down" + ray);
//		RaycastHit hit;
//		if (Physics.Raycast(ray, out hit, 100.0f)) {
//			Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red);
//			Debug.Log("Mouse Down Hit the following triangle: " + hit.triangleIndex);
//			
//		}
	}

}
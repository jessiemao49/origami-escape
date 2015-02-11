using UnityEngine;
using System.Collections;

public class PaperFoldTest : MonoBehaviour {

	private Mesh mesh;
	private bool faceSelected;

	void Start() {
//		gameObject.AddComponent("MeshFilter");
//		gameObject.AddComponent("MeshRenderer");
		mesh = GetComponent<MeshFilter>().mesh;
		faceSelected = false;
	}

	void Update() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 100.0f)) {

			// Draw a ray for debug
			Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red);

			if(Input.GetMouseButtonDown(0)) {
				if (faceSelected) {

				}


				Debug.Log("Mouse Down Hit the following triangle: " + hit.triangleIndex);
				Vector3[] vertices = mesh.vertices;
				int[] triangles = mesh.triangles;
				Vector3 p0 = vertices[triangles[hit.triangleIndex * 3 + 0]];
				Vector3 p1 = vertices[triangles[hit.triangleIndex * 3 + 1]];
				Vector3 p2 = vertices[triangles[hit.triangleIndex * 3 + 2]];
				Transform hitTransform = hit.collider.transform;
				p0 = hitTransform.TransformPoint(p0);
				p1 = hitTransform.TransformPoint(p1);
				p2 = hitTransform.TransformPoint(p2);

				// Outline triangle for debug
				Debug.DrawLine(p0, p1, Color.green, 2, false);
				Debug.DrawLine(p1, p2, Color.green, 2, false);
				Debug.DrawLine(p2, p0, Color.green, 2, false);

				vertices[triangles[hit.triangleIndex * 3 + 0]].y += 2;

				mesh.vertices = vertices;
				mesh.RecalculateBounds();
				mesh.RecalculateNormals();
				mesh.Optimize();
			}
		}
	}
	
}
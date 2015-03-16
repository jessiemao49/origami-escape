using UnityEngine;
using System.Collections;

public class Paper_Back : MonoBehaviour {
	public MeshFilter filter;

	void Start() {
		filter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
	}

	// Take the mesh from the front. Flip all the normals and set as its own mesh.
	public void setMesh(Mesh m) {
		Mesh msh = new Mesh ();
		msh.vertices = m.vertices;
		msh.triangles = m.triangles;
		msh.uv = m.uv;
		msh.normals = m.normals;

		msh.RecalculateBounds();
		filter.mesh = msh;
	}
}

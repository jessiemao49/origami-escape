using UnityEngine;
using System.Collections;

public class Paper : MonoBehaviour {
	public MeshFilter filter;
	public Paper_Back back;
	private System.Collections.Generic.List<PFace> faces;
	private System.Collections.Generic.List<PVertex> verts;
	private int vcount;
	public int maxLayer;
	private float epsilon = 0.0001f;
	private bool front;

	void Start () {
		faces = new System.Collections.Generic.List<PFace> ();
		verts = new System.Collections.Generic.List<PVertex> ();
		filter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
		vcount = 0;
		maxLayer = 0;
		createSquare ();
		front = true;
		triangulateFaces ();
	}

	public void Restart() {
		faces = new System.Collections.Generic.List<PFace> ();
		verts = new System.Collections.Generic.List<PVertex> ();
		vcount = 0;
		maxLayer = 0;
		createSquare ();
		front = true;
		triangulateFaces ();
	}

	void Update () {
		drawMeshOutline ();
	}

	public int getID() { return vcount++; }

	void createSquare() {
		verts = new System.Collections.Generic.List<PVertex> ();
		System.Collections.Generic.List<PEdge> edges = new System.Collections.Generic.List<PEdge> ();
		faces.Clear ();

		verts.Add(new PVertex (new Vector3 (1.0f, 0.0f, 1.0f), vcount++));
		verts.Add(new PVertex (new Vector3 (1.0f, 0.0f, -1.0f), vcount++));
		verts.Add(new PVertex (new Vector3 (-1.0f, 0.0f, -1.0f), vcount++));
		verts.Add(new PVertex (new Vector3 (-1.0f, 0.0f, 1.0f), vcount++));
		verts[0].setUV (new Vector2 (1.0f, 1.0f));
		verts[1].setUV (new Vector2 (1.0f, 0.0f));
		verts[2].setUV (new Vector2 (0.0f, 0.0f));
		verts[3].setUV (new Vector2 (0.0f, 1.0f));

		
		edges.Add (new PEdge (verts [0], verts [1]));
		edges.Add (new PEdge (verts [1], verts [2]));
		edges.Add (new PEdge (verts [2], verts [3]));
		edges.Add (new PEdge (verts [3], verts [0]));
		
		verts [0].addNeighbor (edges [0]);
		verts [0].addNeighbor (edges [3]);
		verts [1].addNeighbor (edges [0]);
		verts [1].addNeighbor (edges [1]);
		verts [2].addNeighbor (edges [1]);
		verts [2].addNeighbor (edges [2]);
		verts [3].addNeighbor (edges [2]);
		verts [3].addNeighbor (edges [3]);
		
		faces.Add (new PFace ());
		faces [0].addVert (verts [0]);
		faces [0].addVert (verts [1]);
		faces [0].addVert (verts [2]);
		faces [0].addVert (verts [3]);	

		faces [0].addEdge (edges [0]);
		faces [0].addEdge (edges [1]);
		faces [0].addEdge (edges [2]);
		faces [0].addEdge (edges [3]);
		faces [0].setLayer (0);
	}

	public void triangulateFaces() {
		Mesh frontMesh = new Mesh();
		System.Collections.Generic.List<int> frontIDs = new System.Collections.Generic.List<int> ();
		System.Collections.Generic.List<Vector3> frontVerts = new System.Collections.Generic.List<Vector3> ();
		System.Collections.Generic.List<Vector3> frontNorms = new System.Collections.Generic.List<Vector3> ();
		System.Collections.Generic.List<Vector2> frontUVs = new System.Collections.Generic.List<Vector2> ();

		Mesh backMesh = new Mesh();
		System.Collections.Generic.List<int> backIDs = new System.Collections.Generic.List<int> ();
		System.Collections.Generic.List<Vector3> backVerts = new System.Collections.Generic.List<Vector3> ();
		System.Collections.Generic.List<Vector3> backNorms = new System.Collections.Generic.List<Vector3> ();
		System.Collections.Generic.List<Vector2> backUVs = new System.Collections.Generic.List<Vector2> ();

		int frontVertCount = 0;
		int backVertCount = 0;
		foreach (PFace f in faces) {
			int vertCount = f.getDisplay() ? frontVertCount : backVertCount;
			System.Collections.Generic.List<int> meshIDs = f.getDisplay() ? frontIDs : backIDs;
			System.Collections.Generic.List<Vector3> meshVerts = f.getDisplay() ? frontVerts : backVerts;
			System.Collections.Generic.List<Vector3> meshNorms = f.getDisplay() ? frontNorms : backNorms;
			System.Collections.Generic.List<Vector2> meshUVs = f.getDisplay() ? frontUVs : backUVs;

			int i = 0;
			Vector2[] vertices2D = new Vector2[f.getNumVerts()];
			// Only triangulate based on x and z 
			foreach (PVertex v in f.getVerts()) {
				vertices2D [i] = new Vector2 (v.getPos()[0], v.getPos()[2]);
				meshUVs.Add (v.getUV ());
				i++;
			}

			// Use the triangulator to get indices for creating triangles
			Triangulator tr = new Triangulator (vertices2D);
			int[] indices = tr.Triangulate ();

			// Push indices for this face on to the lists
			for (i = 0 ; i < indices.Length; i++) {
				meshIDs.Add(indices[i] + vertCount);
			}

			// Push normals and verts, also assigning layers
			foreach(PVertex v in f.getVerts ()){
				Vector3 p = v.getPos ();
				p.y = f.getLayer() * epsilon;
				meshVerts.Add(p);
				if (f.getDisplay()) {
					meshNorms.Add (f.getNormal());
				} else {
					meshNorms.Add (-f.getNormal());
				}
			}
			frontVertCount = frontVerts.Count;
			backVertCount = backVerts.Count;
		}

		// Create the meshes
		frontMesh.vertices = frontVerts.ToArray();
		frontMesh.triangles = frontIDs.ToArray();
		frontMesh.uv = frontUVs.ToArray();
		frontMesh.normals = frontNorms.ToArray ();
		frontMesh.RecalculateBounds();

		backMesh.vertices = backVerts.ToArray();
		backMesh.triangles = backIDs.ToArray();
		backMesh.uv = backUVs.ToArray();
		backMesh.normals = backNorms.ToArray();
		backMesh.RecalculateBounds();

		if (front) {
			back.setMesh (backMesh);
			filter.mesh = frontMesh;
		} else {
			back.setMesh (frontMesh);
			filter.mesh = backMesh;
		}

		// Now join the two meshes together to pass into the mass collider
		Mesh joinedMesh = new Mesh ();
		foreach (Vector3 v in backVerts) {
			frontVerts.Add (v);
		}
		foreach (int i in backIDs) {
			frontIDs.Add (i + frontVertCount);
		}
		foreach (Vector3 uv in backUVs) {
			frontUVs.Add (uv);
		}
		foreach(Vector3 norm in backNorms) {
			frontNorms.Add (norm);
		}
		joinedMesh.vertices = frontVerts.ToArray();
		joinedMesh.triangles = frontIDs.ToArray();
		joinedMesh.uv = frontUVs.ToArray();
		joinedMesh.normals = frontNorms.ToArray ();
		joinedMesh.RecalculateBounds();
		GetComponent<MeshCollider>().sharedMesh = frontMesh;
	}

	public void flipPaper() {
		front = !front;
		foreach (PVertex v in verts) {
			v.flipZ();
		}
		// Reverse all the layers
		foreach (PFace f in faces) {
			f.setLayer(maxLayer - f.getLayer());
		}
		triangulateFaces();
	}

	// Draws the triangles of mesh for debugging purposes
	void drawMeshOutline() {
		int[] triangles = filter.mesh.GetTriangles (0);
		for (int i = 0 ; i < triangles.Length / 3; i++) {
			Vector3 p1 = filter.mesh.vertices[triangles[i*3 + 1]];
			Vector3 p0 = filter.mesh.vertices[triangles[i*3 + 0]];
			Vector3 p2 = filter.mesh.vertices[triangles[i*3 + 2]];

			Debug.DrawLine(p0, p1);
			Debug.DrawLine(p1, p2);
			Debug.DrawLine(p2, p0);
		}
	}

	public System.Collections.Generic.List<PFace> getFaces() { return faces; }
	public System.Collections.Generic.List<PVertex> getVerts() { return verts; }

	public void addFace(PFace f) { faces.Add (f); }
	public void addVert(PVertex v) { verts.Add (v); }

}

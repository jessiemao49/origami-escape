using UnityEngine;
using System.Collections;

public class Paper : MonoBehaviour {
	private MeshFilter filter;
	private System.Collections.Generic.List<PFace> faces;
	private int vcount;
	
	void Start () {
		faces = new System.Collections.Generic.List<PFace> ();
		// Set up game object with mesh;
//		gameObject.AddComponent(typeof(MeshRenderer));
		filter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
		vcount = 0;
		createSquare ();
		triangulateFaces ();
	}

	void Update () {
		drawMeshOutline ();

	}

	public int getID() {
		return vcount++;
	}

	void createSquare() {
		System.Collections.Generic.List<PVertex> verts = new System.Collections.Generic.List<PVertex> ();
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
	}
	
	// For testing purposes	
	void createShape() {
		System.Collections.Generic.List<PVertex> verts = new System.Collections.Generic.List<PVertex> ();
		System.Collections.Generic.List<PEdge> edges = new System.Collections.Generic.List<PEdge> ();
		faces.Clear ();

		verts.Add(new PVertex (new Vector3 (2.0f, 0.0f, 1.0f), vcount++));
		verts.Add(new PVertex (new Vector3 (1.0f, 0.0f, -0.5f), vcount++));
		verts.Add(new PVertex (new Vector3 (-1.0f, 0.0f, -1.0f), vcount++));
		verts.Add(new PVertex (new Vector3 (-1.0f, 0.0f, 1.2f), vcount++));
		verts.Add(new PVertex (new Vector3 (0.4f, 0.0f, 0.9f), vcount++));

		edges.Add (new PEdge (verts [0], verts [1]));
		edges.Add (new PEdge (verts [1], verts [2]));
		edges.Add (new PEdge (verts [2], verts [3]));
		edges.Add (new PEdge (verts [3], verts [4]));
		edges.Add (new PEdge (verts [4], verts [0]));

		verts [0].addNeighbor (edges [0]);
		verts [0].addNeighbor (edges [4]);
		verts [1].addNeighbor (edges [0]);
		verts [1].addNeighbor (edges [2]);
		verts [2].addNeighbor (edges [3]);
		verts [2].addNeighbor (edges [1]);
		verts [3].addNeighbor (edges [2]);
		verts [3].addNeighbor (edges [4]);
		verts [4].addNeighbor (edges [3]);
		verts [4].addNeighbor (edges [0]);

		faces.Add (new PFace ());
		faces [0].addVert (verts [0]);
		faces [0].addVert (verts [1]);
		faces [0].addVert (verts [2]);
		faces [0].addVert (verts [3]);	
		faces [0].addVert (verts [4]);	
	}
	
	void createShape2() {
		System.Collections.Generic.List<PVertex> verts = new System.Collections.Generic.List<PVertex> ();
		System.Collections.Generic.List<PEdge> edges = new System.Collections.Generic.List<PEdge> ();
		faces.Clear ();

		verts.Add(new PVertex (new Vector3 (1.0f, 0.0f, 1.0f), vcount++));
		verts.Add(new PVertex (new Vector3 (1.0f, 0.0f, -1.0f), vcount++));
		verts.Add(new PVertex (new Vector3 (-1.0f, 0.0f, -1.0f), vcount++));
		verts.Add(new PVertex (new Vector3 (-1.0f, 0.0f, 1.0f), vcount++));
		
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
	}

	public void triangulateFaces() {
		Mesh msh = new Mesh();
		System.Collections.Generic.List<int> meshIDs = new System.Collections.Generic.List<int> ();
		System.Collections.Generic.List<Vector3> meshVerts = new System.Collections.Generic.List<Vector3> ();
		System.Collections.Generic.List<Vector2> meshUVs = new System.Collections.Generic.List<Vector2> ();

		int vertCount = 0;
		for (int x = 0; x < faces.Count; x++) {
			PFace f = faces[x];
			int numVerts = f.getNumVerts();

			Vector2[] vertices2D = new Vector2[numVerts];
			// Only triangulate based on x and z 
			int i = 0 ;
			foreach(PVertex v in f.getVerts()) {
				vertices2D [i] = new Vector2 (v.getPos()[0], v.getPos()[2]);

				// Calculate UV based on neighbors 
				if (v.getUV() == new Vector2(-1, -1)) {
					// Find neighbor N1 (u1, v1), neighbor N2 (u1, v1)
					// Find angle alpha between N2 - N1 and P - N1 in world coordinates
					// Find J = world distance between N1, N2
					// Find K = UV Distance between N1, N2
					// K / J * |P(w) - N1(w)| = distance D between N1 and P in UV coordinates
					// rotate N2-N1 by alpha, normalize, times D
					Debug.Log ("P  " + v.getID() + ": " + v.getPos());
					System.Collections.Generic.List<PVertex> n = new System.Collections.Generic.List<PVertex>();
					foreach(PVertex u in v.getNeighborVerts()) {
						if (u.getUV () != new Vector2(-1, -1)) {
							n.Add (u);
						}
						if (n.Count == 2) { break; }
					}

					Vector2 A = new Vector2((n[1].getPos() - n[0].getPos ()).x, (n[1].getPos() - n[0].getPos ()).z);
					Vector2 B = new Vector2((v.getPos () - n[0].getPos ()).x, (v.getPos () - n[0].getPos ()).z);
					Debug.Log ("N1: " + n[0].getUV());
					Debug.Log ("N2: " + n[1].getUV());
					float theta = Mathf.Acos(Vector2.Dot (A, B) / A.magnitude / B.magnitude);
					if (Mathf.Abs(Vector2.Dot(A,B)) < 0.0001) {
						theta = Mathf.PI;
					}

					Debug.Log ("THETA: " + theta);
					Debug.Log ("A: " + A);
					Debug.Log ("A: " + n[1].getPos () + ", " +  n[0].getPos ());
					float J = A.magnitude;
					float K = (n[1].getUV() - n[0].getUV ()).magnitude;
					float D = K / J * (v.getPos() - n[0].getPos()).magnitude;
					Debug.Log ("J: " + J);
					Debug.Log ("K: " + K);
					Debug.Log ("D: " + D);
					Vector2 direction = (Quaternion.Euler(0, theta, 0) * A).normalized;
					Debug.Log ("Direction: " + direction);
					Vector2 UV = n[0].getUV() + (direction * D);
					Debug.Log (">>> UV: " + UV);
					v.setUV (UV);
				}
				meshUVs.Add(v.getUV());
				i++;
			}

			// Use the triangulator to get indices for creating triangles
			Triangulator tr = new Triangulator (vertices2D);
			int[] indices = tr.Triangulate ();

			// Push indices and vertices for this face on to the lists
			for (i = 0 ; i < indices.Length; i++) {
				meshIDs.Add(indices[i] + vertCount);
			}

			foreach(PVertex v in f.getVerts ()){
				meshVerts.Add(v.getPos());
			}
			vertCount = meshVerts.Count;
		}

		// Create the mesh
		msh.vertices = meshVerts.ToArray();
		msh.triangles = meshIDs.ToArray();
		msh.uv = meshUVs.ToArray();
		msh.RecalculateNormals();
		msh.RecalculateBounds();

//		Renderer renderer = GetComponent<MeshRenderer> ().renderer;
//		Material mat = Resources.Load ("textures/paper") as Material;
//		if (mat != null) {
//			renderer.material = mat;
//			Debug.Log ("Material loaded");
//		} else {
//			Debug.LogError("NO MATERIAL");
//		}
		filter.mesh = msh;

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

	public void addFace(PFace f) { faces.Add (f); }

}

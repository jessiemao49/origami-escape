using UnityEngine;
using System.Collections;

public class TargetShape : MonoBehaviour {

	public MeshFilter filter;
	public Material outlineMat;
	private PFace targetShape;
	private System.Collections.Generic.List<PEdge> edges;
	private System.Collections.Generic.List<PVertex> verts;
	private int vcount;
	private float elevation;

	void Start () {
		verts = new System.Collections.Generic.List<PVertex> ();
		edges = new System.Collections.Generic.List<PEdge> ();
		filter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
		vcount = 0;
		elevation = 0.05f;
		CreateShape1 ();
		CreateOutlines ();
		triangulateFaces ();
	}

	void CreateShape1() {
		verts = new System.Collections.Generic.List<PVertex> ();
		edges = new System.Collections.Generic.List<PEdge> ();

		verts.Add(new PVertex (new Vector3 (1.0f, elevation, -1.0f), vcount++));
		verts.Add(new PVertex (new Vector3 (-1.0f, elevation, -1.0f), vcount++));
		verts.Add(new PVertex (new Vector3 (-1.0f, elevation, 1.0f), vcount++));
		
		edges.Add (new PEdge (verts [0], verts [1]));
		edges.Add (new PEdge (verts [1], verts [2]));
		edges.Add (new PEdge (verts [2], verts [0]));

		verts [0].addNeighbor (edges [0]);
		verts [0].addNeighbor (edges [2]);
		verts [1].addNeighbor (edges [0]);
		verts [1].addNeighbor (edges [1]);
		verts [2].addNeighbor (edges [1]);
		verts [2].addNeighbor (edges [2]);

		targetShape = new PFace ();
		targetShape.addVert (verts [0]);
		targetShape.addVert (verts [1]);
		targetShape.addVert (verts [2]);

		targetShape.addEdge (edges [0]);
		targetShape.addEdge (edges [1]);
		targetShape.addEdge (edges [2]);
	}

	void CreateOutlines() {		
		// Create outlines
		foreach (PEdge e in edges) {
			GameObject outline = new GameObject ();
			LineRenderer lr = outline.AddComponent ("LineRenderer") as LineRenderer;
			lr.material = outlineMat;
			lr.SetWidth (0.05f, 0.05f);
			lr.SetPosition (0, e.getP0 ().getPos ());
			lr.SetPosition (1, e.getP1 ().getPos ());
		}
	}

	
	public void triangulateFaces() {
		Mesh msh = new Mesh();
		System.Collections.Generic.List<int> meshIDs = new System.Collections.Generic.List<int> ();
		System.Collections.Generic.List<Vector3> meshVerts = new System.Collections.Generic.List<Vector3> ();
		int vertCount = 0;
		PFace f = targetShape;
		int numVerts = f.getNumVerts();
		
		Vector2[] vertices2D = new Vector2[numVerts];
		// Only triangulate based on x and z 
		int i = 0 ;
		foreach(PVertex v in f.getVerts()) {
			vertices2D [i] = new Vector2 (v.getPos()[0], v.getPos()[2]); 
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

		// Create the mesh
		msh.vertices = meshVerts.ToArray();
		msh.triangles = meshIDs.ToArray();
		msh.RecalculateNormals();
		msh.RecalculateBounds();
		
		filter.mesh = msh;
		GetComponent<MeshCollider>().sharedMesh = msh;
	}

	
}
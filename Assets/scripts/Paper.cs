﻿using UnityEngine;
using System.Collections;

public class Paper : MonoBehaviour {
	private MeshFilter filter;
	private System.Collections.Generic.List<PEdge> edges;
	private System.Collections.Generic.List<PVertex> verts;
	private System.Collections.Generic.List<PFace> faces;
	
	void Start () {
		verts = new System.Collections.Generic.List<PVertex> ();
		edges = new System.Collections.Generic.List<PEdge> ();
		faces = new System.Collections.Generic.List<PFace> ();
		// Set up game object with mesh;
		gameObject.AddComponent(typeof(MeshRenderer));
		filter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;

		createShape2 ();
		triangulateFaces ();
	}

	void Update () {
		drawMeshOutline ();

	}

	void createSquare() {
		verts.Clear ();
		edges.Clear ();
		faces.Clear ();

		verts.Add(new PVertex (new Vector3 (1.0f, 0.0f, 1.0f), 0));
		verts.Add(new PVertex (new Vector3 (1.0f, 0.0f, -1.0f), 1));
		verts.Add(new PVertex (new Vector3 (-1.0f, 0.0f, -1.0f), 2));
		verts.Add(new PVertex (new Vector3 (-1.0f, 0.0f, 1.0f), 3));
		
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
		verts.Clear ();
		edges.Clear ();
		faces.Clear ();
		
		verts.Add(new PVertex (new Vector3 (2.0f, 0.0f, 1.0f), 0));
		verts.Add(new PVertex (new Vector3 (1.0f, 0.0f, -0.5f), 1));
		verts.Add(new PVertex (new Vector3 (-1.0f, 0.0f, -1.0f), 2));
		verts.Add(new PVertex (new Vector3 (-1.0f, 0.0f, 1.2f), 3));
		verts.Add(new PVertex (new Vector3 (0.4f, 0.0f, 0.9f), 4));

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
		verts.Clear ();
		edges.Clear ();
		faces.Clear ();
		
		verts.Add(new PVertex (new Vector3 (1.0f, 0.0f, 1.0f), 0));
		verts.Add(new PVertex (new Vector3 (1.0f, 0.0f, -1.0f), 1));
		verts.Add(new PVertex (new Vector3 (-1.0f, 0.0f, -1.0f), 2));
		verts.Add(new PVertex (new Vector3 (-1.0f, 0.0f, 1.0f), 3));
		
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

		int j = 0;
		foreach(PFace face in faces) {
			Debug.Log ("Face: " + j++);
			Debug.Log ("FACE VERTS:");
			foreach (PVertex v in face.getVerts()) {
				Debug.Log ("vertex " + v.getID () + ": " + v.getPos ());
			}
			Debug.Log ("FACE EDGES:");
			foreach (PEdge e in face.getEdges()) {
				Debug.Log (e.getP0 ().getID () + "--" + e.getP1 ().getID ());
			}
		}

		for (int x = 0; x < faces.Count; x++) {
			PFace f = faces[x];
			int numVerts = f.getVerts ().Count;

			Vector2[] vertices2D = new Vector2[numVerts];
			// Only triangulate based on x and z 
			for (int i = 0; i < numVerts; i++) {
				vertices2D [i] = new Vector2 (f.getVerts()[i].getPos()[0], f.getVerts()[i].getPos()[2]); 
			}

			// Use the triangulator to get indices for creating triangles
			Triangulator tr = new Triangulator (vertices2D);
			int[] indices = tr.Triangulate ();

			// Push indices and vertices for this face on to the lists
			for (int i = 0 ; i < indices.Length; i++) {
				meshIDs.Add(indices[i]);
			}
			for (int i = 0 ; i < numVerts; i++) {
				meshVerts.Add(f.getVerts()[i].getPos());
			}
		}

		// Create the mesh
		msh.vertices = meshVerts.ToArray();
		msh.triangles = meshIDs.ToArray();
		msh.RecalculateNormals();
		msh.RecalculateBounds();
		
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

	public System.Collections.Generic.List<PEdge> getEdges() { return edges; }
	public System.Collections.Generic.List<PVertex> getVerts() { return verts; }
	public System.Collections.Generic.List<PFace> getFaces() { return faces; }

	public void addFace(PFace f) { faces.Add (f); }

}

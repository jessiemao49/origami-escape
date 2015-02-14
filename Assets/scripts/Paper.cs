﻿using UnityEngine;
using System.Collections;

public class Paper : MonoBehaviour {
	private Mesh mesh;
	private System.Collections.Generic.List<PEdge> edges;
	private System.Collections.Generic.List<PVertex> verts;
	private System.Collections.Generic.List<PFace> faces;
	
	void Start () {
		verts = new System.Collections.Generic.List<PVertex> ();
		edges = new System.Collections.Generic.List<PEdge> ();
		faces = new System.Collections.Generic.List<PFace> ();

		createShape ();
		triangulateFaces ();
	}

	void createSquare() {
		verts.Clear ();
		edges.Clear ();
		faces.Clear ();

		verts.Add(new PVertex (new Vector3 (1.0f, 0.0f, 1.0f)));
		verts.Add(new PVertex (new Vector3 (1.0f, 0.0f, -1.0f)));
		verts.Add(new PVertex (new Vector3 (-1.0f, 0.0f, -1.0f)));
		verts.Add(new PVertex (new Vector3 (-1.0f, 0.0f, 1.0f)));
		
		edges.Add (new PEdge (verts [0], verts [1]));
		edges.Add (new PEdge (verts [1], verts [2]));
		edges.Add (new PEdge (verts [2], verts [3]));
		edges.Add (new PEdge (verts [3], verts [0]));
		
		verts [0].addNeighbor (edges [0]);
		verts [0].addNeighbor (edges [3]);
		verts [1].addNeighbor (edges [0]);
		verts [1].addNeighbor (edges [2]);
		verts [2].addNeighbor (edges [3]);
		verts [2].addNeighbor (edges [1]);
		verts [3].addNeighbor (edges [2]);
		verts [3].addNeighbor (edges [0]);
		
		faces.Add (new PFace ());
		faces [0].addVert (verts [0]);
		faces [0].addVert (verts [1]);
		faces [0].addVert (verts [2]);
		faces [0].addVert (verts [3]);	
	}
	
	// For testing purposes	
	void createShape() {
		verts.Clear ();
		edges.Clear ();
		faces.Clear ();
		
		verts.Add(new PVertex (new Vector3 (2.0f, 0.0f, 1.0f)));
		verts.Add(new PVertex (new Vector3 (1.0f, 0.0f, -0.5f)));
		verts.Add(new PVertex (new Vector3 (-1.0f, 0.0f, -1.0f)));
		verts.Add(new PVertex (new Vector3 (-1.0f, 0.0f, 1.2f)));
		verts.Add(new PVertex (new Vector3 (0.4f, 0.0f, 0.9f)));

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

	void triangulateFaces() {
		Mesh msh = new Mesh();
		System.Collections.Generic.List<int> meshIDs = new System.Collections.Generic.List<int> ();
		System.Collections.Generic.List<Vector3> meshVerts = new System.Collections.Generic.List<Vector3> ();

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
		
		// Set up game object with mesh;
		gameObject.AddComponent(typeof(MeshRenderer));
		MeshFilter filter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
		filter.mesh = msh;

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

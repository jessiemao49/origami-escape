using UnityEngine;
using System.Collections;

public class PFace : MonoBehaviour {

	public System.Collections.Generic.List<PVertex> verts;
	public System.Collections.Generic.List<PEdge> edges;

	public PFace() {
		verts = new System.Collections.Generic.List<PVertex> ();
		edges = new System.Collections.Generic.List<PEdge> ();
	}

	public void addVert(PVertex v) { verts.Add (v); }

	public void addVertBetween(PVertex v, PVertex a, PVertex b) {
		int x = verts.IndexOf (a);
		int y = verts.IndexOf (b);
		if (x < y) {
			verts.Insert (x+1, v);
		} else {
			verts.Insert (y+1, v);
		}
	}

	public void addEdge(PEdge e) { edges.Add (e); }
	// For some reason neighbors.remove(e) does some weird shit o ____ o
	public void removeEdge(PEdge e) {
		int count = 0;
		foreach (PEdge edge in edges) {
			if (edge.equals (e)) {
				edges.RemoveAt (count);
				break;
			}
			count++;
		}
	}
	// For some reason neighbors.remove(e) does some weird shit o ____ o
	public void removeVert(PVertex v) {
		int count = 0;
		foreach (PVertex vt in verts) {
			if (v.getID () == vt.getID ()) {
				verts.RemoveAt (count);
				break;
			}
			count++;
		}
	}


	public System.Collections.Generic.List<PVertex> getVerts() {
		return verts;
	}

	public System.Collections.Generic.List<PEdge> getEdges() {
		return edges;
	}


	// Split the face down the two points v0 and v1
	// Edits the structure of the current face to be half, and returns the other half face

	// a -- v0 -- b
	// |     |    |
	// |     |    |
	// |     |    |
	// |     |    |
	// d -- v1 -- c

	public PFace split(PVertex v0, PVertex v1) {
		PVertex curr = v0;
		PVertex prev = v0;
		PEdge newEdge = new PEdge (v0, v1);

		// If they share an edge, can't split face
		foreach (PEdge e in v0.getNeighbors ()) {
			if (e.getOther (v0).getID() == v1.getID ()) { return null; }
		}
		System.Collections.Generic.List<PVertex> newVerts = new System.Collections.Generic.List<PVertex> ();
		System.Collections.Generic.List<PEdge> newEdges = new System.Collections.Generic.List<PEdge> ();
		newVerts.Add (v0);
		while (curr.getID () != v1.getID ()) {
			foreach (PEdge e in curr.getNeighbors ()) {
				PVertex v = e.getOther (curr);
				if (v.getID () != prev.getID ()) {
					prev = curr;
					curr = v;
//					Debug.Log ("Curr: " + curr.getID ());
//					Debug.Log ("Prev: " + prev.getID ());
//					Debug.Log ("V1: " + v1.getID ());
					newVerts.Add (v);
					newEdges.Add (e);
					if (v.getID () != v1.getID ()) {
						removeVert (v);
					}
					removeEdge (e);
					break;
				}
			}
		}
		// v0     v1 -- c -- v0
		edges.Add (newEdge);


		// v0--a--b--v1
		newEdges.Add (newEdge);
		PFace newFace = new PFace ();
		newFace.verts = newVerts;
		newFace.edges = newEdges;

		return newFace;
	}

}

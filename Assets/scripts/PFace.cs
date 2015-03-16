using UnityEngine;
using System.Collections;

public class PFace : MonoBehaviour {
	// Linkedlist of verts and edges maintains order going around the face
	private System.Collections.Generic.LinkedList<PVertex> verts;
	private System.Collections.Generic.LinkedList<PEdge> edges;
	private int layer;
	private Vector3 normal;
	private bool display;

	public PFace() {
		verts = new System.Collections.Generic.LinkedList<PVertex> ();
		edges = new System.Collections.Generic.LinkedList<PEdge> ();
		normal = new Vector3 (0, 1, 0);
		display = true;
	}

	public System.Collections.Generic.LinkedList<PVertex> getVerts() { return verts; }
	public System.Collections.Generic.LinkedList<PEdge> getEdges() { return edges; }
	public int getNumVerts() { return verts.Count; }
	public int getLayer() { return layer; }
	public Vector3 getNormal() { return normal; }
	public void setNormal(Vector3 n) { 
		normal = n;
		// if pointing at you, display = true; else false
		display = Vector3.Dot (n, new Vector3 (0, 1, 0)) > 0;
	}
	public void flipNormal() {
		normal = -normal;
		display = !display;
	}
	public bool getDisplay() { return display; }

	public PEdge[] getEdgesArray() {
		System.Collections.Generic.List<PEdge> ret = new System.Collections.Generic.List<PEdge>();
		foreach (PEdge e in edges) {
			ret.Add (e);
		}
		return ret.ToArray ();
	}

	public void setVerts(System.Collections.Generic.LinkedList<PVertex> v) { verts = v; }	
	public void setEdges(System.Collections.Generic.LinkedList<PEdge> e) { edges = e; }
	public void setLayer(int l) { layer = l; }

	public void addVert(PVertex v) { verts.AddLast (v); }
	public void addEdge(PEdge e) { edges.AddLast (e); }


	public void addVertAfter(PVertex node, PVertex newNode) {
		System.Collections.Generic.LinkedListNode<PVertex> curr = verts.First;
		while (curr != null) {
			if (curr.Value.getID() == node.getID()){
				verts.AddAfter(curr, newNode);
				return;
			}
			curr = curr.Next;
		}
	}
	
	public void addVertBefore(PVertex node, PVertex newNode) {
		System.Collections.Generic.LinkedListNode<PVertex> curr = verts.First;
		while (curr != null) {
			if (curr.Value.getID() == node.getID()){
				verts.AddBefore(curr, newNode);
				return;
			}
			curr = curr.Next;
		}
	}
	public void addEdgeAfter(PEdge node, PEdge newNode) {
		System.Collections.Generic.LinkedListNode<PEdge> curr = edges.First;
		while (curr != null) {
			if (curr.Value.Equals (node)){
				edges.AddAfter(curr, newNode);
				return;
			}
			curr = curr.Next;
		}
	}
	
	public void addEdgeBefore(PEdge node, PEdge newNode) {
		System.Collections.Generic.LinkedListNode<PEdge> curr = edges.First;
		while (curr != null) {
			if (curr.Value.Equals (node)){
				edges.AddBefore(curr, newNode);
				return;
			}
			curr = curr.Next;
		}
	}
	// Because verts.remove doesn't work.
	public void removeEdge(PEdge e) {
		System.Collections.Generic.LinkedListNode<PEdge> curr = edges.First;						
		while (curr != null) {
			PEdge d = curr.Value;
			if (e.Equals (d)) {
				edges.Remove (curr);
				return;
			}
			curr = curr.Next;
		}
	}
	// Because verts.remove doesn't work.
	public void removeEdge(PEdge e, System.Collections.Generic.LinkedList<PEdge> edgelist) {
		System.Collections.Generic.LinkedListNode<PEdge> curr = edgelist.First;						
		while (curr != null) {
			PEdge d = curr.Value;
			if (e.Equals (d)) {
				edges.Remove (curr);
				return;
			}
			curr = curr.Next;
		}

	}

	// Because verts.remove doesn't work.
	public void removeVert(PVertex v) {	
		System.Collections.Generic.LinkedListNode<PVertex> curr = verts.First;						
		while (curr != null) {
			PVertex d = curr.Value;
			if (v.Equals (d)) {
				verts.Remove (curr);
				return;
			}
			curr = curr.Next;
		}
	}
	// Because verts.remove doesn't work.
	public void removeVert(PVertex v, System.Collections.Generic.LinkedList<PVertex> vertlist) {
		System.Collections.Generic.LinkedListNode<PVertex> curr = vertlist.First;						
		while (curr != null) {
			PVertex d = curr.Value;
			if (v.Equals (d)) {
				verts.Remove (curr);
				return;
			}
			curr = curr.Next;
		}	
	}

	// Because verts.find doesn't work.
	System.Collections.Generic.LinkedListNode<PVertex> findVertNode(PVertex v) {
		System.Collections.Generic.LinkedListNode<PVertex> curr = verts.First;
		while (curr!= null) {
			if (curr.Value.Equals (v)) { return curr; }	
			curr = curr.Next;
		}
		return null;
	}
	
	System.Collections.Generic.LinkedListNode<PEdge> findEdgeBetween(PVertex u, PVertex v) {
		System.Collections.Generic.LinkedListNode<PEdge> curr = edges.First;
		while (curr!= null) {
			if (curr.Value.isBetween (u, v)) { 
				return curr; 
			}		
			curr = curr.Next;
		}
		return null;
	}
	// Split the face down the two points v0 and v1
	// Returns the new faces in a list

	// a -- v0 -- b
	// |     |    |
	// |     |    |
	// |     |    |
	// |     |    |
	// d -- v1 -- c

	public System.Collections.Generic.List<PFace> split(PVertex v0, PVertex v1) {

		// If they share an edge, can't split face
		if (v0.isNeighbor(v1)) { return null; }

		PEdge newEdge = new PEdge (v0, v1);
		System.Collections.Generic.LinkedList<PVertex> rightVerts = new System.Collections.Generic.LinkedList<PVertex> ();
		System.Collections.Generic.LinkedList<PEdge> rightEdges = new System.Collections.Generic.LinkedList<PEdge> ();

		System.Collections.Generic.LinkedList<PVertex> leftVerts = new System.Collections.Generic.LinkedList<PVertex> ();
		System.Collections.Generic.LinkedList<PEdge> leftEdges = new System.Collections.Generic.LinkedList<PEdge> ();

		System.Collections.Generic.LinkedListNode<PVertex> currV = findVertNode (v0);
		PVertex next = verts.First.Value;
		if (currV.Next != null) {
			next = currV.Next.Value;
		}
		System.Collections.Generic.LinkedListNode<PEdge> currE = findEdgeBetween (v0, next);

		// Traverse right face first
		while (!currV.Value.Equals(v1)) {
			rightVerts.AddLast (currV.Value);
			rightEdges.AddLast (currE.Value);

			if (currV.Next != null) { currV = currV.Next; }
			else { currV = verts.First; }
			if (currE.Next != null) { currE = currE.Next; }
			else { currE = edges.First; }
		}
		rightVerts.AddLast (currV.Value);
		rightEdges.AddLast (newEdge);
		// Traverse left face
		while (!currV.Value.Equals (v0)) {
			leftVerts.AddLast (currV.Value);
			leftEdges.AddLast (currE.Value);
			
			if (currV.Next != null) { currV = currV.Next; }
			else { currV = verts.First; }
			if (currE.Next != null) { currE = currE.Next; }
			else { currE = edges.First; }
		}
		leftVerts.AddLast (currV.Value);
		leftEdges.AddLast (newEdge);
	
		v0.addNeighbor (newEdge);
		v1.addNeighbor (newEdge);

		PFace newFace1 = new PFace ();
		PFace newFace2 = new PFace ();
		newFace1.setVerts (rightVerts);
		newFace1.setEdges (rightEdges);
		newFace2.setVerts (leftVerts);
		newFace2.setEdges (leftEdges);
		newFace1.setNormal (normal);
		newFace2.setNormal (normal);

		System.Collections.Generic.List<PFace> newFaces = new System.Collections.Generic.List<PFace> ();
		newFaces.Add (newFace1);
		newFaces.Add (newFace2);

		return newFaces;
	}

	// Determines whether v comes before u on the edge e between them, 
	// when you go clockwise around the face
	public bool ccFirst(PVertex v, PVertex u, PEdge e) {
		System.Collections.Generic.LinkedListNode<PEdge> curr = edges.First;
		while (curr.Next != null) {
			if (curr.Value.Equals (e)) {
				if (curr.Next.Value.hasVert(v)) {
					return false;
				} else if (curr.Next.Value.hasVert (u)) {
					return true;
				} else {
					Debug.LogError ("THIS SHOULDN'T HAPPEN");
				}
			}
			curr = curr.Next;
		}
		if (edges.First.Value.hasVert(v)) {
			return false;
		} else if (edges.First.Value.hasVert (u)) {
			return true;
		} else {
			Debug.LogError ("THIS SHOULDN'T HAPPEN");
		}

		return false;

	}

	public override string ToString() {
		string s = "";
		foreach (PVertex v in verts) {
			s += v.getID() + ",";
		}
		return s.Substring (0, s.Length - 1);
	}

}

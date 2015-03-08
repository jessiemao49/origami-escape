using UnityEngine;
using System.Collections;

public class PVertex : MonoBehaviour {

	private Vector3 pos;
	private int id;
	private System.Collections.Generic.List<PEdge> neighbors;
	private Vector2 uv;
	
	public PVertex (Vector3 xyz, int ID) {
		pos = xyz;
		id = ID;
		uv = new Vector2 (-1, -1);
		neighbors = new System.Collections.Generic.List<PEdge>();
	}

	public void setPos(Vector3 xyz) {
		pos = xyz;
	}

	public Vector3 getPos() { return pos; }

	public void addNeighbor(PEdge n) {
		neighbors.Add (n);
	}
	
	// For some reason neighbors.remove(e) does some weird shit o ____ o
	public void removeNeighbor(PEdge e) {
		int count = 0;
		foreach (PEdge edge in neighbors) {
			if (edge.getOther (this).getID () == e.getOther (this).getID ()) {
				neighbors.RemoveAt (count);
				break;
			}
			count++;
		}
	}

	public void removeNeighbor(PVertex v) {
		int count = 0;
		foreach (PEdge e in neighbors) {
			if (e.getOther (this).getID () == v.getID ()) {
				neighbors.RemoveAt (count);
				break;
			}
			count++;
		}
	}

	public bool isNeighbor(PVertex v) {
		foreach (PEdge e in neighbors) {
			if (e.getOther (this).getID() == v.getID ()) { 
				return true;
			}
		}
		return false;
	}

	public System.Collections.Generic.List<PEdge> getNeighbors() {
		return neighbors;
	}

	public System.Collections.Generic.List<PVertex> getNeighborVerts() {
		System.Collections.Generic.List<PVertex> ret = new System.Collections.Generic.List<PVertex> ();
		foreach (PEdge e in neighbors) {
			ret.Add(e.getOther(this));
		}
		return ret;
	}

	public int getID() {
		return id;
	}

	public bool Equals(Object obj)  {
//		if (obj == null || GetType() != obj.GetType()) return false;
		PVertex r = (PVertex)obj;
		// Use Equals to compare instance variables.
		return id == r.getID ();
	}
	
	public override int GetHashCode() {
		return id.ToString().GetHashCode();
	}

	public Vector2 getUV() {
		return uv;
	}

	public void setUV(Vector2 input) {
		uv = input;
	}

}

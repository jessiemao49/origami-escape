using UnityEngine;
using System.Collections;

public class PVertex : MonoBehaviour {

	private Vector3 pos;
	private int id;
	private System.Collections.Generic.List<PEdge> neighbors;
	
	public PVertex (Vector3 xyz, int ID) {
		pos = xyz;
		id = ID;
		neighbors = new System.Collections.Generic.List<PEdge>();
	}


	public void move(Vector3 xyz) {
		pos = xyz;
	}

	public Vector3 getPos() {
		return pos;
	}
	
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

	public System.Collections.Generic.List<PEdge> getNeighbors() {
		return neighbors;
	}

	public int getID() {
		return id;
	}

}

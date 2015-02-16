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

	public void removeNeighbor(PEdge n) {
		neighbors.Remove (n);
	}

	public System.Collections.Generic.List<PEdge> getNeighbors() {
		return neighbors;
	}

	public int getID() {
		return id;
	}

}

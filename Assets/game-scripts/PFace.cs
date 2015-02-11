using UnityEngine;
using System.Collections;

public class PFace : MonoBehaviour {

	private System.Collections.Generic.List<PVertex> verts;
	
	public PFace() {
		verts = new System.Collections.Generic.List<PVertex> ();
	}

	public void addVert(PVertex v) {
		verts.Add (v);
	}



}

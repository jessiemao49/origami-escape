using UnityEngine;
using System.Collections;

public class Fold : MonoBehaviour {
	private Vector3 startMouse;
	private Vector3 endMouse;
	private Ray foldLine;
	private float zDistance = 5.0f;
	private float epsilon = 0.0001f;

	public Paper paper;

	void Start () {
		paper = GetComponent<Paper> ();	
	}
	
	void Update () {
		
		if (Input.GetMouseButtonDown(0)) {
			var mousePos = Input.mousePosition;
			mousePos.z = zDistance;
			startMouse = Camera.main.ScreenToWorldPoint(mousePos);
		}
		if (Input.GetMouseButtonUp(0)) {
			var mousePos = Input.mousePosition;
			mousePos.z = zDistance;
			endMouse = Camera.main.ScreenToWorldPoint(mousePos);
			Debug.DrawLine(startMouse, endMouse, Color.green, 2);

			// Draw fold line
			Ray b = GetBisector(startMouse, endMouse);
			foldLine = b;
			Debug.DrawLine (foldLine.origin - foldLine.direction, foldLine.origin + foldLine.direction, Color.yellow, 2);

			System.Collections.Generic.List<PFace> faces = paper.getFaces();
			System.Collections.Generic.List<PFace> newFaces = new System.Collections.Generic.List<PFace>();
			bool[] toRemove = new bool[faces.Count];

			System.Collections.Generic.List<PVertex> newVertsFold = new System.Collections.Generic.List<PVertex>();
			System.Collections.Generic.List<PFace> flipFaces = new System.Collections.Generic.List<PFace>();
			System.Collections.Generic.HashSet<PVertex> flipVerts = new System.Collections.Generic.HashSet<PVertex>();

			// Find edge that intersects with fold line
			for (int i = 0 ; i < faces.Count; i++) {
				PFace f = faces.ToArray()[i];
				Debug.Log ("Face: " + f.ToString());
				System.Collections.Generic.List<PVertex> newVerts = new System.Collections.Generic.List<PVertex>();
				foreach (PEdge e in f.getEdgesArray()) {
					Debug.Log (e.getP0().getID () +  /*": " + e.getP0 ().getPos() + */ " -- " + e.getP1 ().getID ());// + ": " + e.getP1 ().getPos());
					Vector3 intersection = TestIntersection(b, e.getP0().getPos(), e.getP1().getPos());
					if (Mathf.Abs(intersection.y) < 9.0f) {
						// Add intersection vertex on edge
						// p0 ----- v ------- p1
						//      e       e2

						PVertex p0 = e.getP0 ();
						PVertex p1 = e.getP1 ();
	
						// Check if vert was just created within the fold
						PVertex v = new PVertex (intersection, paper.getID ());

						bool alreadyMade = false;
						foreach(PVertex nv in newVertsFold) {
							if (almostEquals(nv.getPos(), intersection)) {
								v = nv;
								break;
							}
						}

						PEdge e1 = new PEdge(v, e.getP0());
						PEdge e2 = new PEdge(v, e.getP1());

						if (!v.isNeighbor(p0)) {
							v.addNeighbor(e1);
							p0.removeNeighbor (e);
							p0.addNeighbor (e1);
						}

						if (!v.isNeighbor(p1)) {
							v.addNeighbor(e2);
							p1.removeNeighbor(e);
							p1.addNeighbor (e2);
						}

						// Maintain the order of the vertices & edges
						if (f.ccFirst(p0, p1, e)) {
							f.addVertAfter(p0, v);
							f.addEdgeBefore (e, e1);
							f.addEdgeAfter (e, e2);
						} else {
							f.addVertAfter (p1, v);
							f.addEdgeBefore (e, e2);
							f.addEdgeAfter (e, e1);
						}

						f.removeEdge (e);
						newVerts.Add(v);
						if (!alreadyMade) {
							newVertsFold.Add (v);
						}
					}
				}
				// SPLIT face down the newVerts. Don't remove or flip yet until all faces are split.
				if (newVerts.Count > 0) {

					Debug.Log ("Split Face: " + newVerts[0].getID() + ":"
					           + newVerts[0].getPos() + ", " + newVerts[1].getID() + ":"
					           + newVerts[1].getPos ());

					System.Collections.Generic.List<PFace> split = f.split(newVerts[0], newVerts[1]);

					if (split != null) {
						// Find out which face is from the origin side
						foreach(PVertex v in split[0].getVerts ()) {
							if (v.getID() != newVerts[0].getID() && v.getID () != newVerts[1].getID ()) {
								if (isFromOrigin(v)) {
									flipFaces.Add(split[0]);
									foreach( PVertex u in split[0].getVerts()) {
										flipVerts.Add (u);
									}

								} else {
									flipFaces.Add (split[1]);
									foreach( PVertex u in split[1].getVerts()) {
										flipVerts.Add (u);
									}
								}
								break;
							}
						}

						newFaces.Add(split[0]);
						newFaces.Add(split[1]);
						toRemove[i] = true;
					}
				}
			}
			foreach( PVertex v in flipVerts) {
//				flip(v);
			}
			// remove all the marked faces, replace with new faces
			for (int i = faces.Count-1 ; i >= 0; i--) {
				if (toRemove[i]) {
					faces.RemoveAt(i);
				}
			}
			foreach (PFace f in newFaces) {
				faces.Add (f);
			}
			paper.triangulateFaces();

			if (paper.getFaces ().Count > 2) {
				foreach (PFace f in paper.getFaces()) {
					Debug.Log ("FACE");
					foreach(PVertex v in f.getVerts ()) {
						Debug.Log ("- VERT " + v.getID() + ": " + v.getPos());
						/*						foreach(PEdge e in v.getNeighbors()) {
							Debug.Log ("--> " + e.getOther(v).getID());
						}*/
					}
				}
			}
		}


	}

	// Test if the fold ray intersects with the line segment between p0 and p1
	Vector3 TestIntersection(Ray fold, Vector3 p0, Vector3 p1) {
		Vector3 r0 = fold.origin;
		Vector3 s1 = fold.direction;
		Vector3 s2 = p1 - p0;


		float s, t;
		s = (-s1.z * (r0.x - p0.x) + s1.x * (r0.z - p0.z)) / (-s2.x * s1.z + s1.x * s2.z);
		t = (s2.x * (r0.z - p0.z) - s2.z * (r0.x - p0.x)) / (-s2.x * s1.z + s1.x * s2.z);

		if (s >= 0 && s <= 1) {
			return new Vector3(r0.x + (t * s1.x), 0, r0.z + (t * s1.z));
		}

		// Redo test in other direction
		s1 = -fold.direction;

		s = (-s1.z * (r0.x - p0.x) + s1.x * (r0.z - p0.z)) / (-s2.x * s1.z + s1.x * s2.z);
		t = (s2.x * (r0.z - p0.z) - s2.z * (r0.x - p0.x)) / (-s2.x * s1.z + s1.x * s2.z);
		
		if (s >= 0 && s <= 1) {
			return new Vector3(r0.x + (t * s1.x), 0, r0.z + (t * s1.z));
		}

		return new Vector3 (10.0f, 10.0f, 10.0f); // No collision
	}

	Ray GetBisector(Vector3 p0, Vector3 p1) {
		Ray r = new Ray ();
		r.direction = new Vector3 (-(p1.z - p0.z), 0, p1.x - p0.x);
		r.direction.Normalize ();
		r.origin = (p0 + p1) / 2;
		return r;
	}

	bool AlmostZero(Vector3 v) {
		return Mathf.Abs (v.x) < epsilon && Mathf.Abs(v.y) < epsilon && Mathf.Abs (v.z) < epsilon;
	}

	bool almostEquals(Vector3 u, Vector3 v) {
		return Mathf.Abs (u.x - v.x) < epsilon && Mathf.Abs (u.y - v.y) < epsilon && Mathf.Abs (u.z - v.z) < epsilon;
	}

	// Test if PFace f is from the startMouse side of the fold line
	bool isFromOrigin(PVertex v) {
		// A ----- B
		// C   D
		// AC X AB and AD X AB have same sign
		Vector3 A = foldLine.origin;
		Vector3 B = foldLine.origin + foldLine.direction;
		Vector3 C = startMouse;
		Vector3 D = v.getPos();
		return almostEquals (Vector3.Cross (C - A, B - A).normalized, Vector3.Cross (D - A, B - A).normalized);
	}

	// Flip face across fold line
	void flip(PFace f) {
		Vector3 A = foldLine.origin;
		Vector3 B = foldLine.origin + foldLine.direction;
		Vector3 C, P, Q, K;

		foreach (PVertex v in f.getVerts ()) {
			C = v.getPos();
			P = C - A;
			Q = B - A;
			K = Vector3.Dot (P, Q) / Q.sqrMagnitude * Q;
			// Elevate folded verts
			Vector3 pos = A + (2 * K) - P;
//			pos += new Vector3(0.0f, 0.01f, 0.0f);
			v.setPos(pos);
		}
	}

	// Flip vertex across fold line
	void flip(PVertex v) {
		Vector3 A = foldLine.origin;
		Vector3 B = foldLine.origin + foldLine.direction;
		Vector3 C = v.getPos();
		Vector3 P = C - A;
		Vector3 Q = B - A;
		Vector3 K = Vector3.Dot (P, Q) / Q.sqrMagnitude * Q;
		// Elevate folded verts
		Vector3 pos = A + (2 * K) - P;
		//			pos += new Vector3(0.0f, 0.01f, 0.0f);
		v.setPos(pos);
	}

}

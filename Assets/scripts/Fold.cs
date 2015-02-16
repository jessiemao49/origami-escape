using UnityEngine;
using System.Collections;

public class Fold : MonoBehaviour {
	private Vector3 startMouse;
	private Vector3 endMouse;
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
			Debug.DrawLine (b.origin - b.direction, b.origin + b.direction, Color.yellow, 2);


			// SOMETHING IS WRONG HERE

			// EDGE STRUCTURE IS WRONG -- FIX THE WAY YOU REARRANGE VERTICES' NEIGHBORS

			// Find edge that intersects with fold line
			System.Collections.Generic.List<PFace> faces = paper.getFaces();
			foreach (PFace f in faces) {

				System.Collections.Generic.List<PVertex> newVerts = new System.Collections.Generic.List<PVertex>();
				foreach (PEdge e in f.getEdges().ToArray()) {
//					Debug.Log ("LOGGING EDGE: " + e.getP0().getID() + ", " + e.getP1 ().getID ());
					Vector3 intersection = TestIntersection(b, e.getP0().getPos(), e.getP1().getPos ());
					if (Mathf.Abs(intersection.y) < epsilon) {
						// Add intersection vertex on edge

						// p0 ----- v ------- p1
						//      e       e2

						PVertex v = new PVertex(intersection, f.getVerts().Count);
						PEdge e2 = new PEdge(v, e.getP1());
//						Debug.Log("NEW EDGE: " + e2.getP0 ().getID () + ", " + e2.getP1().getID ());

						PVertex p0 = e.getP0 ();
						PVertex p1 = e.getP1 ();

						e.setP1(v);
//						Debug.Log("OLD EDGE: " + e.getP0 ().getID () + ", " + e.getP1().getID ());
						p0.addNeighbor (e);
						v.addNeighbor(e);
						v.addNeighbor(e2);
						p1.removeNeighbor(e);
						p1.addNeighbor (e2);

						f.addVert (v);

						// Add new vert and edge
						paper.getVerts().Add (v);
						paper.getEdges().Add(e2);

						newVerts.Add(v);
					}
				}

				foreach (PVertex v in f.getVerts()) {
					Debug.Log ("Vert " + v.getID() + ": " + v.getPos ());
					foreach(PEdge e in v.getNeighbors()) {
						Debug.Log ("-- " + e.getOther (v).getID ());
					}
				}
//				foreach (PEdge e in f.getEdges ()) {
//					Debug.Log("Edge: " + e.getP0().getID() + ", " + e.getP1 ().getID ());
//				}

				// SPLIT face down the newVerts
				if (newVerts.Count > 0) {
					PFace newFace = f.split(newVerts[0], newVerts[1]);
					paper.triangulateFaces();
				}
			}
		}
	}

	// Test if the fold ray intersects with the line segment between p0 and p1
	Vector3 TestIntersection(Ray fold, Vector3 p0, Vector3 p1) {
		Vector3 r0 = fold.origin;
		Vector3 r1 = fold.origin + fold.direction;
		Vector3 s1 = fold.direction;
		Vector3 s2 = p1 - p0;

		float s, t;
		s = (-s1.z * (r0.x - p0.x) + s1.x * (r0.z - p0.z)) / (-s2.x * s1.z + s1.x * s2.z);
		t = (s2.x * (r0.z - p0.z) - s2.z * (r0.x - p0.x)) / (-s2.x * s1.z + s1.x * s2.z);

		if (s >= 0 && s <= 1) {
			return new Vector3(r0.x + (t * s1.x), 0, r0.z + (t * s1.z));
		}

		// Redo test in other direction
		r1 = fold.origin - fold.direction;
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

}

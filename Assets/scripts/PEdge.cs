using UnityEngine;
using System.Collections;

public class PEdge : MonoBehaviour {

	private PVertex p0;
	private PVertex p1;

	public PEdge (PVertex point0, PVertex point1) {
		p0 = point0;
		p1 = point1;
	}

	public PVertex getP0() { return p0; }
	public PVertex getP1() { return p1; }
	public void setP0(PVertex newp) { p0 = newp; }
	public void setP1(PVertex newp) { p1 = newp; }

	public PVertex getOther(PVertex me) {
		return p0.getID () == me.getID () ? p1 : p0;
	}

	public bool hasVert (PVertex p) {
		return p.Equals (p0) || p.Equals (p1);
	}
	
	public void setOther(ref PVertex me, ref PVertex newYou) {
		if (p0 == me) {
			p1 = newYou;
		} else {
			p0 = newYou;	
		}
	}

	// For some reason overriding this doesn't make list.remove work...
	public bool Equals(Object obj)  {
//		if (obj == null || GetType() != obj.GetType()) return false;
		PEdge other = (PEdge)obj;
		// Use Equals to compare instance variables.
		return (other.getP0 ().getID () == p0.getID () && other.getP1 ().getID () == p1.getID ())
			|| (other.getP1 ().getID () == p0.getID () && other.getP0 ().getID () == p1.getID ());
	}

	public bool isBetween(PVertex v, PVertex u) {
		return (p0.getID () == v.getID () && p1.getID () == u.getID ()) 
			|| (p0.getID () == u.getID () && p1.getID() == v.getID());
	}

	public override int GetHashCode() {
		return p0.GetHashCode() ^ p1.GetHashCode();
	}
	public string toString() {
		return p0.getID () + " -- " + p1.getID ();
	}
}

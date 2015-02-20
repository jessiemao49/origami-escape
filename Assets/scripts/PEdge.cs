using UnityEngine;
using System.Collections;


// NOT SURE HOW REFS WORK

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

	public bool equals(PEdge other) {
		return (other.getP0 ().getID () == p0.getID () && other.getP1 ().getID () == p1.getID ())
			|| (other.getP1 ().getID () == p0.getID () && other.getP0 ().getID () == p1.getID ());
	}

	public PVertex getOther(PVertex me) {
		return p0.getID () == me.getID () ? p1 : p0;
	}
	
	public void setOther(ref PVertex me, ref PVertex newYou) {
		if (p0 == me) {
			p1 = newYou;
		} else {
			p0 = newYou;	
		}
	}

}

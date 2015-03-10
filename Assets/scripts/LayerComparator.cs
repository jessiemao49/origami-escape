using UnityEngine;
using System;
using System.Collections.Generic;

public class LayerComparator : IComparer<PFace> {

	public int Compare(PFace a, PFace b) {
		if (a.getLayer () == b.getLayer ()) {
			return 0;
		} else if (a.getLayer () > b.getLayer ()) {
			return -1;
		} else {
			return 1;
		}

	}

}
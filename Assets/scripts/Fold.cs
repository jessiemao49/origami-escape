using UnityEngine;
using System.Collections;

public class Fold : MonoBehaviour {
	private Vector3 startMouse;
	private Vector3 endMouse;
	private float zDistance = 5.0f;

	void Start () {
	
	}
	
	void Update () {
		if (Input.GetMouseButtonDown(0)) {			
			var mousePos = Input.mousePosition;
			mousePos.z = zDistance;
			startMouse = Camera.main.ScreenToWorldPoint(mousePos);
			Debug.Log ("mouse down");
		}
		if (Input.GetMouseButtonUp(0)) {
			Debug.Log ("mouse up");
			var mousePos = Input.mousePosition;
			mousePos.z = zDistance;
			endMouse = Camera.main.ScreenToWorldPoint(mousePos);
			Vector3 direction = (endMouse - startMouse);
			Debug.Log (startMouse);
			Debug.Log (endMouse);
			Debug.DrawLine(startMouse, endMouse, Color.green, 2);
			
		}
	}

}

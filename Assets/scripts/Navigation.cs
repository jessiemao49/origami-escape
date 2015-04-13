using UnityEngine;
using System.Collections;

public class Navigation : MonoBehaviour {
	GameObject cam;
	public GUISkin transparent;

	// Use this for initialization
	void Start () {
		cam = GameObject.Find ("Main Camera");	
	}
	
	private void OnGUI() {
		GUI.skin = transparent;
		if (GUI.Button (new Rect (0, 0, 60, Screen.height), "")) {
			cam.transform.Rotate(new Vector3(0, 1, 0), -90);
		}
		if (GUI.Button (new Rect (Screen.width-60, 0, 60, Screen.height), "")) {
			cam.transform.Rotate(new Vector3(0, 1, 0), 90);
		}
		GUI.skin = null;
	}
}

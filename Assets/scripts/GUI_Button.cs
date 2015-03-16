using UnityEngine;
using System.Collections;

public class GUI_Button : MonoBehaviour {

	public Texture2D buttonImage = null;
	public Paper paper;

	void Start () {
		paper = GetComponent<Paper> ();
	}

	
	private void OnGUI() 
	{
		if (GUI.Button (new Rect (15, 15, 100, 30), "Undo")) 
		{

		}
		if (GUI.Button (new Rect (15, 50, 100, 30), "Flip")) 
		{
			paper.flipPaper();

		}
	}
}

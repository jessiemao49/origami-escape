using UnityEngine;
using System.Collections;

public class GUI_Button : MonoBehaviour {

	public Texture2D buttonImage = null;
	public Paper paper;
	public TargetShape target;
	public Camera origamiCamera;
	private bool showScore = false;
	private string score = "";
	private Rect windowRect;

	void Start () {
		paper = GetComponent<Paper> ();
	}

	private void OnGUI() 
	{

		if (showScore) {
			windowRect = GUI.Window(0,windowRect,ShowPopup,"");
		}
		if (GUI.Button (new Rect (15, 15, 100, 30), "Restart")) 
		{
			paper.Restart();
		}
		if (GUI.Button (new Rect (15, 50, 100, 30), "Undo")) 
		{
			// TODO: IMPLEMENT UNDO
		}
		if (GUI.Button (new Rect (15, 85, 100, 30), "Flip"))
		{	
			paper.flipPaper();
		}
		if (GUI.Button (new Rect (15, 130, 100, 30), "Submit"))
		{
			showScore = true;
			score = ((int) (scoreCheck () * 100)).ToString () + "%";
			windowRect = new Rect(Screen.width / 2 - 115, Screen.height / 2 - 100, 200, 100);
		}
	}
	
	void ShowPopup(int id){
		GUILayout.Label ("\nYour score is: " + score);
		if(GUILayout.Button("OK"))
			showScore = false;
	}

	private float scoreCheck() {
		int sample_size = 150;
		float screen_dim = 12;
		int correct = 0;
		int wrong = 0;

		for (int i = 0; i <= sample_size; i++) {
			for (int j = 0; j <= sample_size; j++) {
				// Shoot a ray
				Vector3 o = origamiCamera.transform.position;
				Vector3 p = new Vector3(
					(float) i / (float) sample_size * screen_dim - (screen_dim / 2), 
					0,
					(float) j / (float) sample_size * screen_dim - (screen_dim / 2));
//				Debug.DrawLine (o,p,Color.white, 5);
				RaycastHit[] hits = Physics.RaycastAll(o, p-o);
				if (hits.Length > 0) {
					bool hitPaper = false;
					bool hitTarget = false;
					for (int k = 0; k < hits.Length; k++) {
						if (hits[k].collider.gameObject.tag == "paper") {
							hitPaper = true;
						}
						if (hits[k].collider.gameObject.tag == "target") {
							hitTarget = true;
						}
					}
					if (hitPaper && hitTarget) {
						correct++;
						Debug.DrawLine (o, p, Color.red, 10);
					} else if (hitPaper) {
						wrong++;
						Debug.DrawLine (o, p, Color.green, 10);
					} else if (hitTarget) {
						wrong++;
						Debug.DrawLine (o, p, Color.cyan, 10);
					}
				}
			}
		}
		return (float) correct / ((float) correct + (float) wrong);
	}
}

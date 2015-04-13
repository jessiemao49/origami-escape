using UnityEngine;
using System.Collections;

public class DummyResponse : MonoBehaviour {
	public Inventory inventory;
	public Texture2D icon;

	void Start () {
		inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
	}

	void OnMouseDown(){
		this.gameObject.SetActive (false);
		ItemClass obj = new ItemClass(inventory.getID(), this.gameObject.name, icon);
		inventory.addObj (obj);
		inventory.updateGUI ();
		inventory.printBag ();
	}   
}

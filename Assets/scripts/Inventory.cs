using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {
	public InventoryGUI gui;
	private System.Collections.Generic.List<ItemClass> bag;
	private int id;

	void Start () {
		id = 0;
		bag = new System.Collections.Generic.List<ItemClass> ();
	}

	public System.Collections.Generic.List<ItemClass> getBag() {
		return bag;
	}

	public bool hasObj(ItemClass obj) {
		return bag.Contains(obj);
	}

	public ItemClass getObjByName(string name) {
		foreach (ItemClass obj in bag) {
			if (obj.name == name) {
				return obj;
			}
		}
		return null;
	}

	public void printBag() {
		Debug.Log ("You have these items:");
		foreach (ItemClass obj in bag) {
			Debug.Log (obj.name);
		}
		Debug.Log ("==================");
	}

	public void addObj(ItemClass obj) {
		bag.Add (obj);
	}

	public void removeObj(ItemClass obj) {
		bag.Remove (obj);
	}

	public ItemClass removeObjByName(string name) {
		foreach (ItemClass obj in bag) {
			if (obj.name == name) {
				bag.Remove (obj);
				return obj;
			}
		}
		return null;
	}
	
	public ItemClass removeObjById(int id) {
		foreach (ItemClass obj in bag) {
			if (obj.id == id) {
				bag.Remove (obj);
				return obj;
			}
		}
		return null;
	}

	public void updateGUI() {
		for (int i = 0 ; i < bag.Count; i++) {
			Debug.Log (bag[i].name);
			gui.dictionary[bag[i].id] = bag[i].icon;
		}

	}

	public int count() { return bag.Count; }
	public int getID() { return id++; }
}

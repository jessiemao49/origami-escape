using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryGUI : MonoBehaviour {
	public Inventory inventory;
	public Dictionary<int, Texture2D> dictionary = new Dictionary<int, Texture2D>();
	public GUISkin inventory_gui;

	private int NUM_ITEMS = 6;

	void Start () {		
		dictionary = new Dictionary<int, Texture2D> () {
			{0, null},
			{1, null},
			{2, null},
			{3, null},
			{4, null},
			{5, null},
		};
	}

	void OnGUI() {
		GUI.skin = inventory_gui;
		int dy = Screen.height / 7;
		// w = 600
		// 100 100 100 100 100 100
		// 5 90 10 90 10 90 10 90 10 90 10 90 5
		int dx = Screen.width / 6 - 10;
		for (int i = 0; i < NUM_ITEMS; i++) {
			GUI.Button(new Rect(i * (dx+10) + 10, Screen.height - dy, dx, dy-10), dictionary[i]);
		}
		GUI.skin = null;
	}
}




using UnityEngine;
using System.Collections;

public class ItemClass {
	public string name;
	public int id;
	public Texture2D icon;
	
	public ItemClass(int ide, string nam, Texture2D ico) {
		name = nam;
		id = ide;
		icon = ico;
	}
}
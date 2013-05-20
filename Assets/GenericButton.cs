using UnityEngine;
using System.Collections;

public class GenericButton : MonoBehaviour {
	
	public Main main;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnMouseDown() {
		main.showMaze();
		main.enabled = true;	
	}
	
}

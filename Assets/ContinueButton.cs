using UnityEngine;
using System.Collections;

public class ContinueButton : MonoBehaviour {
	
	public Main main;
	
	
	// Use this for initialization
	void Start () {
		hide();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnMouseDown() {
		main.spawnPlayer();
	}
	
	public void show() {
		enabled=true;
	}
	
	public void hide() {
		enabled=false;
	}
	
	
}

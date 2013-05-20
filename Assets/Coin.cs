using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
		gameObject.transform.Rotate(0,Random.Range(0,30),0);
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.Rotate(0,1,0);
	}
}

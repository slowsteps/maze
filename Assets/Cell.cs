using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour {
	
	public bool isOpen = true;
	public int number;
	public int col;
	public int row;
	public bool isexit = false;
	public Grid grid;
	public Color mycolor;
	public GameObject mycoin;
	public bool visited = false;
	
	
	
	

	
	
	// Use this for initialization
	void Start () {
		
		//show();
	}
	
	public void setOpen() {
		this.renderer.material.color = new Color(0,0,0);
		//this.transform.Translate(0,-1f,0);
		
	}
	
	public void setClosed() {
		this.renderer.material.color = new Color(0.4f,0.2f,1f);
		this.transform.Translate(0,1f,0);
		isOpen = false;
	}
	
	public void setStart() {
		this.renderer.material.color = Color.green;
		this.transform.Translate(0,0,0);	
	}
	
	public void setExit() {
		this.renderer.material.color = new Color(1,1,1);
		this.transform.Translate(0,0,0);	
		isexit = true;
	}
	
	public void show() {
		this.renderer.material.color = new Color(0.4f,0.2f,1f);
		mycolor = this.renderer.material.color;
		if (mycoin!=null) mycoin.SetActive(true);
		if(!isOpen) {
			float delay = Random.Range(0,5);
			iTween.MoveFrom(this.gameObject,iTween.Hash("y",0,"time",10,"delay",delay));	
		}
	}
	
	public void hide() {
		if(!isOpen) {

			float delay1 = Random.Range(1,5);
			float delay2 = Random.Range(1,5);
			
			iTween.MoveTo(this.gameObject,iTween.Hash("y",0,"time",5*delay1,"delay",delay2));
			
		}
	}
	
	public void hideCoin() {
		if (mycoin!=null) mycoin.SetActive(false);
	}
	
	public void showCoin() {
		if (mycoin!=null) mycoin.SetActive(true);
	}
	
	public void onAvatararHit() {
		if (!visited && mycoin!=null) {
			
			visited = true;
			//iTween.ColorTo(mycoin,new Color(0,0,0,0),1);
			iTween.ColorTo(mycoin,iTween.Hash("a",0));
			iTween.MoveTo(mycoin,iTween.Hash("y",1,"oncomplete","onCoinComplete","oncompletetarget",this.gameObject));
			
		}
		 
	}
	
	private void onCoinComplete() {
		mycoin.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

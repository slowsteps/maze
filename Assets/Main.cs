using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
	
	
	//settings in component
	public Grid grid;
	public Camera fpscam;
	public Camera topviewcam;
	public GameObject sun;
	public GUIText turnsfield;
	public GUIText directionfield;
	public GUIText messagefield;
	public GUITexture continuebutton;
	public GUITexture startlevelbutton;
	
	public GameObject avatar;
	public GameObject robot;
	public GameObject coin;

	
	private const string GAMEOVER_EXIT = "exit reached";
	private const string GAMEOVER_NO_MORE_TURNS = "no more turns";
	
	
	private string direction;
	private const string UP="up";
	public string heading;
	public bool mapvisible=true;
	private int turns = 10;
	private const string MOVE_FORWARD="move forward";
	private const string MOVE_TURN="turn";
	private bool pressedLeft = false;
	private bool pressedRight = false;
	private bool gameover = false;
	private GameObject gameobject;
	private float stepsize = 0.02f;
	private GameObject myrobot;
	
	public GameObject exitarrow;
	private bool turning = true;
	private ArrayList savedpositions;
	private Cell savedCell;
	string positions = "";
	private Cell targetcell;
	private GameObject target=null;
	
	
	
	// Use this for initialization
	void Start () {
		
		savedpositions = new ArrayList();
		

		
		turnsfield.text = turns + " turns left";
		
		
		continuebutton.enabled = false;
		
		exitarrow = Instantiate(exitarrow) as GameObject;
		myrobot = Instantiate(robot) as GameObject;
		spawnPlayer();
	}
	
	
	
	public void spawnPlayer() {
		enabled = false; //pause update
		continuebutton.enabled = false;
		grid.createMaze(grid.level);
		avatar.transform.position = grid.startcell.transform.position;	
		avatar.transform.rotation = Quaternion.AngleAxis(0,Vector3.up);
		avatar.SetActive(false);
		heading = "up";
		grid.curcell = grid.startcell;
		turns = (int)grid.allowedturns[grid.level];
		turnsfield.text = turns + " turns";
		messagefield.text = "";
		resetCamera();		
		//print (robot.transform.position);
		showMap();
		
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKey(KeyCode.LeftArrow)) pressedLeft = true;
		else if (Input.GetKey(KeyCode.RightArrow)) pressedRight = true;
		else  directionfield.text = "";
		
		if (!mapvisible) {
			moveOneCell();
			moveCamera();
			animateExit();
		}
	}
	
	
	

	
	void moveOneCell() {
		
		
		
		gameobject = avatar; // convenience
		//avatar.SetActive(false);
		if (!target) target = getNextCell(); //needs a push for first cell		
		turning=false;
		
		
		
		float dist = Vector3.Distance(gameobject.transform.position,target.transform.position);
		
		if (dist<stepsize) {
			
			target = getNextCell();
		
			
			
			if 	(heading.Equals("up") ) 	gameobject.transform.Translate(stepsize*Vector3.forward,Space.World);
			else if (heading.Equals("left") ) 	gameobject.transform.Translate(stepsize*Vector3.left,Space.World);
			else if (heading.Equals("right") ) 	gameobject.transform.Translate(stepsize*Vector3.right,Space.World);
			else if (heading.Equals("down") ) 	gameobject.transform.Translate(stepsize*Vector3.back,Space.World);

		}
		else {
		
			if (Input.GetKey(KeyCode.LeftArrow)) 	turning = true;
			else if (Input.GetKey(KeyCode.RightArrow)) 	turning = true;
			else turning = false;

			gameobject.transform.position = Vector3.MoveTowards(gameobject.transform.position,target.transform.position,stepsize);
			myrobot.transform.position = gameobject.transform.position;
			//myrobot.transform.position = gameobject.transform.position + new Vector3(0,0.1f*Mathf.Sin(10*Time.timeSinceLevelLoad));
			myrobot.transform.rotation = gameobject.transform.rotation;
			myrobot.transform.Translate(0,0.6f,0);
			
			
			
			//grid.curcell = targetcell;
			
			
			savedpositions.Add(gameobject.transform.position);
			if (savedpositions.Count>50) savedpositions.RemoveAt(0);

		}
		
		//gameobject.transform.Translate(0,1f,0);
		//gameobject.transform.Translate(0,0.1f*Mathf.Sin(0.1f*Time.frameCount),0);
		
		if (targetcell.isexit) setGameOver(GAMEOVER_EXIT);
		//print(target.gameObject.transform.position);
		//print(" frame: " + Time.frameCount.ToString() + " pos=" +  gameobject.transform.position.x + " target=" +  target.transform.position.x);
		
	}
	
	
	private GameObject getNextCell() {
		
		savedCell = grid.curcell;
		//savedCell.renderer.material.color = savedCell.GetComponent<Cell>().mycolor;
		turning = false;

			//print ("getting next cell");
			if (pressedLeft && grid.getLeft().isOpen) {
				targetcell = grid.getLeft();
				target = grid.getLeft().gameObject;
				Turn("left");
				
			}
			else if (pressedRight && grid.getRight().isOpen) {
				targetcell = grid.getRight();
				target = grid.getRight().gameObject;
				Turn("right");
				
			}
			else if (grid.getForward().isOpen) {
				targetcell = grid.getForward();
				target = targetcell.gameObject;
				//print ("forward");
				pressedLeft = false;
				pressedRight = false;
			
			}
			else if (!grid.getForward().isOpen) {
				//print ("u-turn");
				targetcell = grid.getBack();
				target = grid.getBack().gameObject;
				Turn ("back");
			}
			else {
				targetcell = null;
			}
				
			grid.setCurCell(targetcell);
		
			//targetcell.renderer.material.color = Color.red;
			iTween.ColorTo(targetcell.gameObject,iTween.Hash("r",1f,"time",0.5,"oncomplete","onColorAnimComplete","oncompletetarget",this.gameObject));
			//iTween.ColorFrom(targetcell.gameObject,iTween.Hash("r",1f,"time",2));
			
			
			
		return target;
		
	}
	
	private void onColorAnimComplete() {
		iTween.ColorTo(targetcell.gameObject,iTween.Hash("r",0f,"time",10));	
	}
	
	public void Turn(string turndirection) {
		//print ("in turn");
		if (turning) return;
		turning = true;
		if 		(heading.Equals("up") && turndirection.Equals("right")) heading = "right";
		else if (heading.Equals("up") && turndirection.Equals("left")) heading = "left";
		else if (heading.Equals("up") && turndirection.Equals("back")) heading = "down";
		
		else if (heading.Equals("right") && turndirection.Equals("right")) heading = "down";
		else if (heading.Equals("right") && turndirection.Equals("left")) heading = "up";
		else if (heading.Equals("right") && turndirection.Equals("back")) heading = "left";
		
		else if (heading.Equals("left") && turndirection.Equals("right")) heading = "up";
		else if (heading.Equals("left") && turndirection.Equals("left")) heading = "down";
		else if (heading.Equals("left") && turndirection.Equals("back")) heading = "right";
		
		else if (heading.Equals("down") && turndirection.Equals("right")) heading = "left";
		else if (heading.Equals("down") && turndirection.Equals("left")) heading = "right";
		else if (heading.Equals("down") && turndirection.Equals("back")) heading = "up";
		
		
		float targetrotation=0;
		if (heading.Equals("up")) targetrotation = 0;
		if (heading.Equals("right")) targetrotation = 90;
		if (heading.Equals("down")) targetrotation = 180;
		if (heading.Equals("left")) targetrotation = 270;
		
		
		
		iTween.RotateTo(avatar,iTween.Hash("y",targetrotation,"time",1,"oncomplete","onRotateComplete","oncompletetarget",this.gameObject));
		

			
		turns--;
		if(turns<0) setGameOver(GAMEOVER_NO_MORE_TURNS);
		turnsfield.text = turns + " turns left";
		
		pressedLeft = false;
		pressedRight = false;
		
		
	}
	

	void onRotateComplete() {
		turning = false;	
	}
	
	void resetCamera() {
		fpscam.transform.position = grid.startcell.transform.position;
		fpscam.transform.Translate(0,0.8f,0);
		//enabled = true;
		iTween.MoveFrom(fpscam.gameObject,iTween.Hash("y",-3,"time",2,"oncomplete","onEnableCamera","oncompletetarget",this.gameObject));
	}
	
	void moveCamera() {
		
		if (savedpositions.Count > 0 ){
			fpscam.gameObject.transform.position = Vector3.Lerp((Vector3)savedpositions[0],avatar.transform.position,0.2f);
		}
		if (enabled) {
			//print ("before " + fpscam.transform.position);
			fpscam.transform.LookAt(avatar.transform,Vector3.up);
		}
		//print ("after " + fpscam.transform.position);
		
		fpscam.gameObject.transform.Translate(0,0.8f,0);
	
	}
	
	public void onEnableCamera() {
		//start update
		enabled = true;
	}
	
	
	void setGameOver(string reason) {
		//print ("gameover: " + reason);
		if (!gameover) {
			
			gameover = true;
			this.enabled = false;
			//next level
			if (reason.Equals(GAMEOVER_EXIT)) {
				messagefield.text = "Level complete - you found the exit!";
				grid.level++;
				grid.hideCells();
				iTween.MoveTo(fpscam.gameObject,iTween.Hash("y",2,"time",3,"oncomplete","showStartButton","oncompletetarget",this.gameObject));
				
				//iTween.RotateBy(fpscam.gameObject,iTween.Hash("y",1f,"time",6,"easetype",iTween.EaseType.linear));
			}
			//try again
			else if (reason.Equals(GAMEOVER_NO_MORE_TURNS)) {
				messagefield.text = "Level failed - no more turns";
				grid.hideCells();
				iTween.MoveTo(fpscam.gameObject,iTween.Hash("y",2,"time",3,"oncomplete","showStartButton","oncompletetarget",this.gameObject));
			
				//iTween.RotateBy(fpscam.gameObject,iTween.Hash("y",1f,"time",6,"easetype",iTween.EaseType.linear));

			}
			//if (grid.level<grid.maps.Count) continuebutton.enabled = true;
			//else messagefield.text = "Final level completed";
		}
	}
	
	public void showMap() {
		
		fpscam.gameObject.SetActive(false);
		topviewcam.gameObject.SetActive(true);
		sun.light.intensity = 1;
		startlevelbutton.enabled = true;
		mapvisible=true;
		exitarrow.SetActive(false);
		avatar.SetActive(false);
		
			foreach(Cell cell in grid.cells) {
				//iTween.PunchScale(cell.gameObject,new Vector3(1.5f,1.5f,1.5f),2);
				float rnd = (float)0.1*Random.Range(1,10);
				iTween.MoveFrom(cell.gameObject,iTween.Hash("x",-200,"time",0.4,"delay",rnd));
				iTween.ColorFrom(cell.gameObject,iTween.Hash("a",0,"time",1,"delay",rnd));
				cell.hideCoin();
			}
	}
	
	private void showStartButton() {
		continuebutton.enabled = true;
	}
	
	public void showMaze() {
		fpscam.gameObject.transform.position = grid.startcell.transform.position;
		
		fpscam.gameObject.SetActive(true);
		sun.light.intensity = 0.1f;
		mapvisible=false;
		startlevelbutton.enabled = false;
		grid.showCells();
		resetCamera();
		exitarrow.SetActive(true);
		avatar.SetActive(true);

		
	}
	
	
	
	void animateExit() {
		exitarrow.transform.Rotate(0,2,0);
	}
	

	
	
	
}


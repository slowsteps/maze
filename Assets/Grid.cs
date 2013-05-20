using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {
	
	public int rows;
	public int cols;
	public int numcells;
	public ArrayList allowedturns;
	
	
	public GameObject cell;
	public ArrayList map;
	public ArrayList maps;
	public ArrayList cells;

	public Main main;
	public Cell curcell;
	public Cell startcell;	
	public int level=0;
	
	public const string UP="up";
	public const string DOWN="up";
	public const string LEFT="up";
	public const string RIGHT="up";
	
	
	// Use this for initialization
	void Start () {
		
		
		//createMaze(level);
		
	
	}
	
	
	
	
	public void createMaze(int level) {
		
		print ("in createMaze");
		if(map!=null) destroyMaze();
	
		ArrayList map0 = new ArrayList{2,2,2,2,2,2,1,1,4,2,2,2,1,2,2,2,2,3,2,2,2,2,2,2,2};
		ArrayList map1 = new ArrayList{2,2,2,2,2,2,2,2,1,1,1,2,1,2,2,2,2,1,1,1,2,2,1,1,1,2,2,2,2,1,1,1,2,4,2,2,1,3,1,1,1,2,2,2,2,2,2,2,2};
		ArrayList map2 = new ArrayList{2,2,2,2,2,2,1,2,2,2,2,1,1,2,1,2,1,1,1,2,2,2,1,1,1,1,1,2,1,2,2,2,1,2,1,2,1,2,1,2,2,1,1,2,3,2,1,1,1,2,2,2,1,2,2,2,1,2,2,2,2,1,1,1,1,1,1,2,4,2,2,2,2,2,1,2,2,2,1,2,2,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2};	
		ArrayList map3 = new ArrayList{2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,1,2,1,2,1,2,2,2,1,2,1,2,1,2,2,1,2,3,2,1,2,1,2,1,2,1,2,1,2,
2,1,2,2,2,1,2,1,2,1,2,2,2,1,2,
2,1,1,1,1,1,1,1,1,1,2,1,1,1,2,
2,2,1,2,2,2,2,2,2,2,2,2,1,2,2,
2,1,1,1,1,1,2,4,2,1,1,1,1,1,2,
2,1,2,2,2,2,2,1,2,2,2,2,2,1,2,
2,1,1,1,2,1,1,1,1,1,1,1,1,1,2,
2,1,2,1,2,2,2,2,2,2,2,2,2,1,2,
2,1,2,1,1,1,1,1,1,1,1,1,2,1,2,
2,1,2,2,2,2,2,2,2,2,2,1,2,1,2,
2,1,1,1,1,1,1,1,1,1,1,1,1,1,2,
2,2,2,2,2,2,2,2,2,2,2,2,2,2,2};		
		maps = new ArrayList{map3,map1,map2,map3,map0};
		
		allowedturns = new ArrayList{30,10,20,99};
		
		map = (ArrayList)maps[level];
		//map = level0;
		level++;
		
		
		rows = (int)Mathf.Sqrt(map.Count);
		cols = rows;
		numcells = rows*cols;


		main.topviewcam.orthographicSize = (float) 1.5* (float) 0.5*rows;
		
		cells = new ArrayList();
		
		for (int i=0;i<numcells;i++) {
			GameObject mycell = Instantiate(cell) as GameObject;
			
				
			int hor = i % cols;
			int vert = (int)-Mathf.Floor(i/cols);
			mycell.transform.Translate(hor,0,vert);
			
			mycell.GetComponent<Cell>().grid = this;
			mycell.GetComponent<Cell>().number = i;
			mycell.GetComponent<Cell>().col = hor;
			mycell.GetComponent<Cell>().row = vert;
			
			
			if (i<map.Count) {
				if((int)map[i] == 4)	{
					mycell.GetComponent<Cell>().setExit();
					main.exitarrow.transform.position = mycell.transform.position + new Vector3(0,4,0);
					
				}
				else if((int)map[i] == 3)	{
					mycell.GetComponent<Cell>().setStart();
					startcell = mycell.GetComponent<Cell>();
					curcell = mycell.GetComponent<Cell>();
				}
				else if((int)map[i] == 2)	mycell.GetComponent<Cell>().setClosed();
				else if((int)map[i] == 1)	{
					mycell.GetComponent<Cell>().setOpen();
					mycell.GetComponent<Cell>().mycoin = Instantiate(main.coin,mycell.transform.position + new Vector3(0,0.6f,0),Quaternion.identity) as GameObject;
				}
			}
			
			cells.Add(mycell.GetComponent<Cell>());
			main.topviewcam.transform.position = new Vector3(cols/2,10,(-rows/2)+0.5f);				
			
			
		
		}
		
	}
	
	public void setCurCell(Cell incell) {
		curcell = incell;
		curcell.onAvatararHit();
	}
	
	public void destroyMaze() {
		print ("destroying cells");
		foreach (Cell cell in cells) {
			Destroy(cell.gameObject);
			Destroy(cell);
		}
		
	}
	
	public Cell getForward() {
		//test, only check up
		//one above 
		
		int index=0;
		if (main.heading.Equals("up")) index = curcell.number - rows;
		if (main.heading.Equals("down")) index = curcell.number + rows;
		if (main.heading.Equals("left")) index = curcell.number - 1;
		if (main.heading.Equals("right")) index = curcell.number + 1;
		Cell nextcell = cells[index] as Cell;
		
		//print("curcell.number:" + curcell.number + "nextcell.number:" + nextcell.number + " row:" + nextcell.row + " - col:" + nextcell.col + " - open:" + nextcell.isOpen);
		return nextcell;
	}
	
	
	public Cell getBack() {
		//test, only check up
		//one above 
		
		int index=0;
		if (main.heading.Equals("up")) index = curcell.number + rows;
		if (main.heading.Equals("down")) index = curcell.number - rows;
		if (main.heading.Equals("left")) index = curcell.number + 1;
		if (main.heading.Equals("right")) index = curcell.number - 1;
		Cell nextcell = cells[index] as Cell;
		
		//print("curcell.number:" + curcell.number + "nextcell.number:" + nextcell.number + " row:" + nextcell.row + " - col:" + nextcell.col + " - open:" + nextcell.isOpen);
		return nextcell;
	}
	
	public Cell getLeft() {
		
		//one cell to the left, relative to local camera transform 
		int index=0;
		if (main.heading.Equals("up")) index = curcell.number - 1;
		if (main.heading.Equals("down")) index = curcell.number + 1;
		if (main.heading.Equals("left")) index = curcell.number + rows;
		if (main.heading.Equals("right")) index = curcell.number - rows;
		Cell nextcell = cells[index] as Cell;

		//print("curcell.number:" + curcell.number + "nextcell.number:" + nextcell.number + " row:" + nextcell.row + " - col:" + nextcell.col + " - open:" + nextcell.isOpen);
		return nextcell;
	}
	
	public Cell getRight() {
		
		//one cell to the right, relative to local camera transform 
		int index=0;
		if (main.heading.Equals("up")) index = curcell.number + 1;
		if (main.heading.Equals("down")) index = curcell.number - 1;
		if (main.heading.Equals("left")) index = curcell.number - rows;
		if (main.heading.Equals("right")) index = curcell.number + rows;
		Cell nextcell = cells[index] as Cell;

		//print("curcell.number:" + curcell.number + "nextcell.number:" + nextcell.number + " row:" + nextcell.row + " - col:" + nextcell.col + " - open:" + nextcell.isOpen);
		return nextcell;
	}
	
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void showCells() {
		foreach(Cell cell in cells) {
				cell.show();
		}
	}
	
	public void hideCells() {
		foreach(Cell cell in cells) {
				cell.hide();
		}
	}
	
}

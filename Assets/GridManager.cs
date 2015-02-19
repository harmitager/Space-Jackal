using UnityEngine;
using System.Collections;

public class GridManager: MonoBehaviour
{
	//following public variable is used to store the hex model prefab;
	//instantiate it by dragging the prefab on this variable using unity editor
	public GameObject Square;
	public GameObject Ship;
	public GameObject Pirate;
	//next two variables can also be instantiated using unity editor
	public int gridWidthInSquares = 13;
	public int gridHeightInSquares = 13;
	
	//Hexagon tile width and height in game world
	private float squareWidth;
	private float squareHeight;
	
	//Method to initialise Hexagon width and height
	void setSizes()
	{
		//renderer component attached to the Hex prefab is used to get the current width and height
		squareWidth = Square.renderer.bounds.size.x;
		squareHeight = Square.renderer.bounds.size.z;
	}
	
	//Method to calculate the position of the first hexagon tile
	//The center of the hex grid is (0,0,0)
	Vector3 calcInitPos()
	{
		Vector3 initPos;
		//the initial position will be in the left upper corner
		initPos = new Vector3(-squareWidth * gridWidthInSquares / 2f + squareWidth / 2, 0,
		                      gridHeightInSquares / 2f * squareHeight - squareHeight / 2);
		
		return initPos;
	}
	
	//method used to convert hex grid coordinates to game world coordinates
	public Vector3 calcWorldCoord(Vector2 gridPos)
	{
		//Position of the first hex tile
		Vector3 initPos = calcInitPos();
		//Every second row is offset by half of the tile width
		float offset = 0;
		//if (gridPos.y % 2 != 0)
		//	offset = squareWidth / 2;
		
		float x =  initPos.x + offset + gridPos.x * squareWidth;
		//Every new line is offset in z direction by 3/4 of the hexagon height
		float z = initPos.z - gridPos.y * squareHeight;
		return new Vector3(x, 0, z);
	}
	
	//Finally the method which initialises and positions all the tiles
	void initGrid()
	{
		//Game object which is the parent of all the hex tiles
		GameObject squareGridGO = new GameObject("SquareGrid");
		
		for (float y = 0; y < gridHeightInSquares; y++)
		{
			for (float x = 0; x < gridWidthInSquares; x++)
			{
				//GameObject assigned to Hex public variable is cloned
				makeSquare(x,y,squareGridGO);
			}
		}
		makeSquare(gridWidthInSquares/2, -1, squareGridGO);
		makeSquare(gridWidthInSquares/2, gridHeightInSquares, squareGridGO);
		makeSquare(-1, gridHeightInSquares/2, squareGridGO);
		makeSquare(gridWidthInSquares, gridHeightInSquares/2, squareGridGO);
		makeShips ();
	}

	//Setup starting squares
	void makeSquare(float x,float y,GameObject squareGridGO)
	{
		GameObject square = (GameObject)Instantiate(Square);
		//Current position in grid
		Vector2 gridPos = new Vector2(x, y);
		square.transform.position = calcWorldCoord(gridPos);
		square.transform.parent = squareGridGO.transform;
	}

	void makeShips()
	{
		GameObject Ships = new GameObject ("Ships");
		makeShip (gridWidthInSquares / 2, -1, Ships);
		makeShip (gridWidthInSquares / 2, gridHeightInSquares, Ships);
		makeShip (-1, gridHeightInSquares/2, Ships);
		makeShip(gridWidthInSquares, gridHeightInSquares/2, Ships);
	}

	//Create a ship
	void makeShip(float x, float y, GameObject Ships)
	{
		GameObject ship = (GameObject)Instantiate(Ship);
		//Current position in grid
		Vector2 gridPos = new Vector2 (x, y);
		ship.transform.position = calcWorldCoord (gridPos);
		ship.transform.position -= transform.forward*2.5f;
		ship.transform.parent = Ships.transform;
	}

	//Place single starting pirate
	void makePirate(float x,float y, GameObject Ship)
	{
		GameObject pirate = (GameObject)Instantiate (Pirate);
		Vector2 gridPos = new Vector2 (x, y);

	}
	//The grid should be generated on game start
	void Start()
	{
		setSizes();
		initGrid();
	}
}
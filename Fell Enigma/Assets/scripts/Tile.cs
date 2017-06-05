using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

	public Vector2 gridPosition = Vector2.zero;

	public int movementCost = 1;

	public List<Tile> neighbours = new List<Tile>();


	public Color colour = Color.green;


	// Use this for initialization
	void Start () {
		GetComponent<Renderer>().material.color = colour;
		generateNeighbours();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/**
	 * Finds the neighbours of each tile
	 * @author Jeffrey Goh
	 * @version 1.0
	 * @updated 2/6/2017
	 */
	void generateNeighbours()
	{
		neighbours = new List<Tile>();
		//Up
		if (gridPosition.y < Grid.instance.tilesPerCol - 1)
		{
			Vector2 up = new Vector2(gridPosition.x, gridPosition.y + 1);
			neighbours.Add(Grid.instance.map[(int)up.x][(int)up.y]);
		}

		//Down
		if (gridPosition.y > 0)
		{
			Vector2 down = new Vector2(gridPosition.x, gridPosition.y - 1);
			neighbours.Add(Grid.instance.map[(int)down.x][(int)down.y]);
		}

		//Left
		if (gridPosition.x > 0)
		{
			Vector2 left = new Vector2(gridPosition.x - 1, gridPosition.y);
			neighbours.Add(Grid.instance.map[(int)left.x][(int)left.y]);
		}

		//Right
		if (gridPosition.x < Grid.instance.tilesPerRow - 1)
		{
			Vector2 right = new Vector2(gridPosition.x + 1, gridPosition.y);
			neighbours.Add(Grid.instance.map[(int)right.x][(int)right.y]);
		}
	}

	/**
	* Allows currently selected unit to move to an empty tile or to attack the unit that's on the tile
	* @author Jeffrey Goh
	* @version 1.0
	* @updated 2/6/2017
	*/
	private void OnMouseDown()
	{
		if (Grid.instance.units[Grid.instance.currentPlayer].isMoving)
		{
			Grid.instance.moveCurrentUnit(this);
		}
		else if (Grid.instance.units[Grid.instance.currentPlayer].isFighting)
		{
			Grid.instance.attackWithCurrentUnit(this);
		}
	}
}

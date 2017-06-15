using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

	public Vector2 gridPosition = Vector2.zero;

	public int movementCost;
    public int tileModifier;
    private TerrainS linkedTerrain;

	public List<Tile> neighbours = new List<Tile>();

	public Unit occupied = null;


	public Color colour;
    public float opacity;
    private Color defaultColour;


	// Use this for initialization
	void Start () {
        colour.a = opacity;
		GetComponent<Renderer>().material.color = colour;
        defaultColour = colour;
		generateNeighbours();
        detectTerrain();
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
		Debug.Log(gridPosition.x + " " + gridPosition.y);
		if (Grid.instance.currentPlayer != -1)
		{
			if (Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].isMoving)
			{
				Grid.instance.moveCurrentUnit(this);
			}
			else if (Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].isFighting)
			{
				Grid.instance.attackWithCurrentUnit(this);
			}
		}
	}

    /**
	* Resets tile to its default color
	* @author Wayne Neo
	* @version 1.0
	* @updated 6/6/2017
	*/
    public void resetDefaultColor()
    {
        GetComponent<Renderer>().material.color = defaultColour;
    }

    /**
	* Returns the default color
	* @author Wayne Neo
	* @version 1.0
	* @updated 6/6/2017
	*/
    public Color returnDefaultColor()
    {
        return defaultColour;
    }

    /**
   * Finds the terrain above and sets the corresponding values.
   * I have no way to confirm this is accurate. It seems to work (magically) though
   * @author Wayne Neo
   * @version 1.0
   * @updated 6/6/2017
   */
    private void detectTerrain()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1);
        this.linkedTerrain = colliders[0].gameObject.GetComponent<TerrainS>();
        movementCost = linkedTerrain.returnCost();
        tileModifier = linkedTerrain.returnModifier();
    }
}

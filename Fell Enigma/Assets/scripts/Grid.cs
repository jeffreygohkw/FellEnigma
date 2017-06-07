﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Grid : MonoBehaviour {

	public static Grid instance;
	
	public GameObject tilePrefab;
	public GameObject unitPrefab;
	public GameObject enemyPrefab;
    public GameObject terrainPrefab;

	public int tilesPerRow;
	public int tilesPerCol;

	public int currentPlayer = -1;

	public int currentTeam = 0;

	public int totalDone = 0;

	public BattleFormula battle = new BattleFormula();


	public List<List<Tile>> map = new List<List<Tile>>();
    public List<List<TerrainS>> mapT = new List<List<TerrainS>>();
    public List<List<Unit>> units = new List<List<Unit>>();

	public void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start ()
	{
		CreateTiles();
		CreatePlayers();
	}

	// Update is called once per frame
	void Update()
	{
		// Skip turn if entire team is dead
		if (units[currentTeam].Count == 0 || totalDone == units[currentTeam].Count)
		{
			nextTurn();
		}
		else
		{
			foreach (Unit u in units[currentTeam])
			{
				// If the unit is selected and alive
				if (u.currentHP > 0 && u.selected)
				{
					u.turnUpdate();
				}
			}
		}
	}

	/**
	 * v1.0
	 * Moves on to the next turn
	 * To be revamped
	 * 
	 * v1.1
	 * Added FE style turn order
	 * 
	 * @author Jeffrey Goh
	 * @version v1.1
	 * @updated 7/6/2017
	 */

	public void nextTurn()
	{
		foreach (Unit u in units[currentTeam])
		{
			if (u.currentHP > 0)
			{
				u.doneAction = false;
				u.selected = false;
			}
		}

		if (currentTeam < units.Count - 1)
		{
			currentTeam++;
		}
		else
		{
			currentTeam = 0;
		}

		currentPlayer = -1;
		totalDone = 0;
		foreach (Unit u in units[currentTeam])
		{
			if (u.currentHP == 0)
			{
				totalDone++;
			}
		}
		Debug.Log(currentTeam);
	}



	/**
	* Move the current unit to the destination tile
	* 
	* v1.1
	* Does not feature pathfinding, so the unit just moves in a straight line to the destination
	* 
	* v1.2
	* Moves in an L shape to the destination, vertical first
	* Can navigate around obstacles, and will pick the shortest path
	* 
	* v1.3
	* Minor collision bugfix
	* 
	* v1.4
	* Allow movement to tiles where units have died
	* 
	* @param destTile The destination tile
	* @author Jeffrey Goh
	* @version 1.4
	* @updated 7/6/2017
	*/
	public void moveCurrentUnit(Tile destTile)
	{
		if (destTile.GetComponent<Renderer>().material.color != destTile.colour)
		{
			// Don't let the unit move to a tile that's occupied
			foreach (List<Unit> i in units)
			{
				foreach (Unit u in i)
				{
					if (u.gridPosition == destTile.gridPosition && u.currentHP > 0)
					{
						return;
					}
				}
			}

			// Remove the green highlighted tiles
			removeTileHighlight();

			map[(int)units[currentTeam][currentPlayer].gridPosition.x][(int)units[currentTeam][currentPlayer].gridPosition.y].occupied = null;

			// Get the path from the unit's current position to its final position
			List <Tile> path = TilePathFinder.FindPath(map[(int)units[currentTeam][currentPlayer].gridPosition.x][(int)units[currentTeam][currentPlayer].gridPosition.y], units[currentTeam][currentPlayer].mov, map[(int)destTile.gridPosition.x][(int)destTile.gridPosition.y]).tileList;
			foreach (Tile t in path)
			{
				units[currentTeam][currentPlayer].positionQueue.Add(map[(int)t.gridPosition.x][(int)t.gridPosition.y].transform.position);
			}
			// Set gridPosition of the unit to the destination tile
			units[currentTeam][currentPlayer].gridPosition = destTile.gridPosition;

			map[(int)destTile.gridPosition.x][(int)destTile.gridPosition.y].occupied = units[currentTeam][currentPlayer];
		}
		else
		{
			Debug.Log("Invalid destination.");
		}
	}



	/**
	 * Highlights tiles on the grid
	 * 
	 * v1.1 
	 * Added min and max range
	 * 
	 * @param origin The location of the unit
	 * @highlightcolour Varies depending on the action
	 * @minActionRange The minimum range we can perform the action
	 * @maxActionRange The maximum range we can perform the action
	 * @move Whether the unit is moving or doing something else
	 * @author Jeffrey Goh
	 * @version 1.1
	 * @updated 2/6/2017
	 */
	public void highlightTilesAt(Vector2 origin, Color highlightcolour, int minActionRange, int maxActionRange, bool move)
	{
		List<Tile> highlightedTiles = TileHighlight.FindHighlight(map[(int)origin.x][(int)origin.y], minActionRange, maxActionRange, move);

		foreach (Tile t in highlightedTiles)
		{
			t.GetComponent<Renderer>().material.color = highlightcolour;
		}
	}



	/**
	 * Removes highlights on the grid
     * 
     * v 1.0
     * Manual change of highlight to green
     * 
     * v 1.1
     * Changed to user-defined color
     * 
     * 
	 * @author Jeffrey Goh
	 * @version 1.1
	 * @updated 6/6/2017 by Wayne Neo
	 */
	public void removeTileHighlight()
	{
		for (int i = 0; i < tilesPerRow; i++)
		{
			for (int j = 0; j < tilesPerCol; j++)
			{
				map[i][j].resetDefaultColor();
			}
		}
	}


	/**
	* Attacks the unit on the target tile
	* @param target destTile The tile our target is on
	* @author Jeffrey Goh
	* @version 1.0
	* @updated 2/6/2017
	*/
	public void attackWithCurrentUnit(Tile destTile)
	{
		if (destTile.GetComponent<Renderer>().material.color != destTile.returnDefaultColor())
		{
			Unit target = null;
			foreach (List<Unit> i in units)
			{
				foreach (Unit u in i)
				{
					if (u.gridPosition == destTile.gridPosition)
					{
						target = u;
						battle.attackWithCurrentUnit(target);
						break;
					}
				}
			}
		}
	}



	/**
	* Generates the grid
	* Change the tilesPerCol and tilesPerRow to change the dimensions of the grid
	* @author Jeffrey Goh
	* @version 1.0
	* @updated 2/6/2017
	*/
	void CreateTiles()
	{
		// Iterate through each column
		for (int i = 0; i < tilesPerCol; i++)
		{
			// A row of tiles
			List<Tile> row = new List<Tile>();
				
			// Iterate through each Row
			for (int j = 0; j < tilesPerRow; j++)
			{
				Tile tile = ((GameObject)Instantiate(tilePrefab, new Vector3(i - Mathf.Floor(tilesPerCol / 2), j - Mathf.Floor(tilesPerRow / 2), 0), Quaternion.Euler(new Vector3()))).GetComponent<Tile>();
				
				tile.gridPosition = new Vector2(i, j);
				// Add tile to the row
				row.Add(tile);
			}

			// Add the row to the map
			map.Add(row);
		}

        // Iterate through each column
        for (int i = 0; i < tilesPerCol; i++)
        {
            // A row of tiles
            List<TerrainS> rowT = new List<TerrainS>();

            // Iterate through each Row
            for (int j = 0; j < tilesPerRow; j++)
            {
                TerrainS terrain = ((GameObject)Instantiate(terrainPrefab, new Vector3(i - Mathf.Floor(tilesPerCol / 2), j - Mathf.Floor(tilesPerRow / 2), 1), Quaternion.Euler(new Vector3()))).GetComponent<TerrainS>();

                terrain.gridPosition = new Vector2(i, j);
                // Add tile to the row
                rowT.Add(terrain);
            }

            // Add the row to the map
            mapT.Add(rowT);
        }
    }



	/**
	* Add units, both allies and enemies to the map
	* Have to assign everything manually
	* 
	* v1.1 
	* Added teams 
	* Must assign the teams and indexes in the correct order
	* 
	* @author Jeffrey Goh
	* @version 1.1
	* @updated 7/6/2017
	*/
	void CreatePlayers()
	{
		PlayerUnit unit1 = ((GameObject)Instantiate(unitPrefab, new Vector3(0 - Mathf.Floor(tilesPerCol / 2), 0 - Mathf.Floor(tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<PlayerUnit>();
		unit1.gridPosition = new Vector2(0, 0);

		unit1.unitName = "Lyn";
		unit1.job = "Myrmidon";
		unit1.lvl = 1;
		unit1.exp = 0;
		unit1.maxHP = 16;
		unit1.currentHP = 16;
		unit1.strength = 4;
		unit1.mag = 0;
		unit1.skl = 7;
		unit1.spd = 9;
		unit1.luk = 5;
		unit1.def = 2;
		unit1.res = 0;
		unit1.con = 5;
		unit1.mov = 5;

		unit1.weaponMt = 5;
		unit1.weaponPhysical = true;
		unit1.weaponAcc = 90;
		unit1.weaponCrit = 0;
		unit1.weaponWt = 5;
		unit1.weaponMinRange = 1;
		unit1.weaponMaxRange = 1;

		map[0][0].occupied = unit1;

		unit1.team = 0;
		unit1.index = 0;


		PlayerUnit unit2 = ((GameObject)Instantiate(unitPrefab, new Vector3(0 - Mathf.Floor(tilesPerCol / 2), 1 - Mathf.Floor(tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<PlayerUnit>();
		unit2.gridPosition = new Vector2(0, 1);

		unit2.unitName = "Batta";
		unit2.job = "Brigand";
		unit2.lvl = 2;
		unit2.exp = 0;
		unit2.maxHP = 21;
		unit2.currentHP = 21;
		unit2.strength = 5;
		unit2.mag = 0;
		unit2.skl = 1;
		unit2.spd = 3;
		unit2.luk = 2;
		unit2.def = 3;
		unit2.res = 0;
		unit2.con = 10;
		unit2.mov = 5;

		unit2.weaponMt = 8;
		unit2.weaponPhysical = true;
		unit2.weaponAcc = 75;
		unit2.weaponCrit = 0;
		unit2.weaponWt = 10;
		unit2.weaponMinRange = 1;
		unit2.weaponMaxRange = 1;

		map[0][1].occupied = unit2;

		unit2.team = 1;
		unit2.index = 0;


		PlayerUnit unit3 = ((GameObject)Instantiate(unitPrefab, new Vector3(2 - Mathf.Floor(tilesPerCol / 2), 1 - Mathf.Floor(tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<PlayerUnit>();
		unit3.gridPosition = new Vector2(2, 1);

		unit3.unitName = "Lute";
		unit3.job = "Mage";
		unit3.lvl = 1;
		unit3.exp = 0;
		unit3.maxHP = 17;
		unit3.currentHP = 17;
		unit3.strength = 0;
		unit3.mag = 6;
		unit3.skl = 6;
		unit3.spd = 7;
		unit3.luk = 8;
		unit3.def = 3;
		unit3.res = 5;
		unit3.con = 3;
		unit3.mov = 5;

		unit3.weaponMt = 5;
		unit3.weaponPhysical = false;
		unit3.weaponAcc = 90;
		unit3.weaponCrit = 0;
		unit3.weaponWt = 4;
		unit3.weaponMinRange = 1;
		unit3.weaponMaxRange = 2;

		map[2][1].occupied = unit3;

		unit3.team = 2;
		unit3.index = 0;


		PlayerUnit unit4 = ((GameObject)Instantiate(unitPrefab, new Vector3(4 - Mathf.Floor(tilesPerCol / 2), 2 - Mathf.Floor(tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<PlayerUnit>();
		unit4.gridPosition = new Vector2(4, 2);

		unit4.unitName = "Rebecca";
		unit4.job = "Archer";
		unit4.lvl = 1;
		unit4.exp = 0;
		unit4.maxHP = 17;
		unit4.currentHP = 17;
		unit4.strength = 4;
		unit4.mag = 0;
		unit4.skl = 5;
		unit4.spd = 6;
		unit4.luk = 4;
		unit4.def = 3;
		unit4.res = 1;
		unit4.con = 5;
		unit4.mov = 5;

		unit4.weaponMt = 6;
		unit4.weaponPhysical = true;
		unit4.weaponAcc = 85;
		unit4.weaponCrit = 0;
		unit4.weaponWt = 5;
		unit4.weaponMinRange = 2;
		unit4.weaponMaxRange = 2;

		map[4][2].occupied = unit4;

		unit4.team = 0;
		unit4.index = 1;


		List<Unit> team0 = new List<Unit>();
		List<Unit> team1 = new List<Unit>();
		List<Unit> team2 = new List<Unit>();

		team0.Add(unit1);
		team0.Add(unit4);

		units.Add(team0);

		team1.Add(unit2);

		units.Add(team1);

		team2.Add(unit3);

		units.Add(team2);




		/*
		AIUnit enemy1 = ((GameObject)Instantiate(enemyPrefab, new Vector3(3 - Mathf.Floor(tilesPerCol / 2), 3 - Mathf.Floor(tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
		enemy1.gridPosition = new Vector2(3, 3);

		units.Add(enemy1);
		*/
	}
}

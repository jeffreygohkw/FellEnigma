using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Grid : MonoBehaviour {

	public static Grid instance;
	
	public GameObject tilePrefab;
	public GameObject unitPrefab;
	public GameObject enemyPrefab;
    public GameObject terrainPrefab;
    public TextAsset mapConfig;
    public Camera mainCam;
	public string mapName;

	public int tilesPerRow = 0;
	public int tilesPerCol = 0;

	public int currentPlayer = -1;

	public int currentTeam = 0;

	public List<int> AITeams = new List<int>();

	public int totalDone = 0;

	public BattleFormula battle = new BattleFormula();

	public List<List<Tile>> map = new List<List<Tile>>();
    public List<List<Unit>> units = new List<List<Unit>>();

	public Dictionary<Vector2, string[]> villageLoot = new Dictionary<Vector2, string[]>();

	public Dictionary<Vector2, Vector2> tavernAndSpawn = new Dictionary<Vector2, Vector2>();
	public int tavernLevel;
	

	public int gold = 1000;

	/*
	 * -1: None
	 * 0: MC
	 * 1: Naive Prince
	 * 2: Kind Soul
	 * 3: Young Rebel
	 * 4: Black Heart
	 */
	public int commander = -1;

	public void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start ()
	{
		Item.instance.initialiseItems();
        CreateTerrain();
		CreateTiles();
		CreatePlayers.generatePlayers(mapName);
		CreateBuildings.generateBuildings(mapName);
	}

	// Update is called once per frame
	void Update()
	{
        //Force shut game
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

		int status = WinCon.checkWinCon(mapName);
		//Win conditions
		
		if (status == 1)
		{
			foreach (List<Unit> u in units)
			{
				foreach (Unit v in u)
				{
					v.gameObject.SetActive(false);
				}
			}
			return;
		}
		
		else if (status == 2)
		{
			foreach (List<Unit> u in units)
			{
				foreach (Unit v in u)
				{
					v.gameObject.SetActive(false);
				}
			}
			return;
		}
		
		// Skip turn if entire team is dead
		if (units[currentTeam].Count == 0 || totalDone == units[currentTeam].Count || currentPlayer == units[currentTeam].Count)
		{
			nextTurn();
		}
		else
		{
			if (AITeams.Contains(currentTeam))
			{
				if (currentPlayer == -1)
				{
					currentPlayer++;
				}
				else if (units[currentTeam][currentPlayer].currentHP > 0 && !units[currentTeam][currentPlayer].doneAction)
				{
					units[currentTeam][currentPlayer].turnUpdate();
				}
				else
				{
					currentPlayer++;
				}
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
        resetCamera();
		removeTileHighlight();
		foreach (Unit u in units[currentTeam])
		{
			if (u.currentHP > 0)
			{
				u.doneMoving = false;
				u.doneAction = false;
				u.selected = false;
				u.willAttack = false;
				u.isMoving = false;
				u.isFighting = false;
				u.isHealing = false;
				u.activeStaffIndex = -1;
				u.highlighted = false;
				u.displayInventory = false;
				u.selectedItemIndex = -1;
				u.displayTavern = false;
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
			//Heal if on a fort
			else if (map[(int)u.gridPosition.x][(int)u.gridPosition.y].linkedTerrain.returnHeal() != 0)
			{
				float percent = (float)map[(int)u.gridPosition.x][(int)u.gridPosition.y].linkedTerrain.returnHeal() / (float)100;
				u.currentHP += (int)(percent * (float)u.maxHP);
				
				// Set the current HP to the max HP if it overshoots
				if (u.currentHP > u.maxHP)
				{
					u.currentHP = u.maxHP;
				}

				Debug.Log(u.unitName + " has healed for " + (int)(percent * (float)u.maxHP) + " HP.");
                CombatLog.instance.AddEvent(u.unitName + " has healed for " + (int)(percent * (float)u.maxHP) + " HP.");
                CombatLog.instance.PrintEvent();
				Debug.Log(u.unitName + "'s HP: " + u.currentHP + "/" + u.maxHP);
			}
		}
	}

	/**
	 * 
	 * 
	 * @author Jeffrey Goh
	 * @version v1.0
	 * @updated 24/6/2017
	 */
	void GameOver()
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
	* v1.5
	* AI compatibility
	* 
	* @param destTile The destination tile
	* @author Jeffrey Goh
	* @version 1.5
	* @updated 12/6/2017
	*/
	public void moveCurrentUnit(Tile destTile)
	{

		if ((destTile.GetComponent<Renderer>().material.color != destTile.colour || AITeams.Contains(currentTeam)) && (destTile.occupied == null || destTile.occupied == units[currentTeam][currentPlayer]))
		{
			// Don't let the unit move to a tile that's occupied

			// Remove the green highlighted tiles
			removeTileHighlight();

			map[(int)units[currentTeam][currentPlayer].gridPosition.x][(int)units[currentTeam][currentPlayer].gridPosition.y].occupied = null;

			// Get the path from the unit's current position to its final position
			List <Tile> path = TilePathFinder.FindPath(map[(int)units[currentTeam][currentPlayer].gridPosition.x][(int)units[currentTeam][currentPlayer].gridPosition.y], units[currentTeam][currentPlayer].mov, map[(int)destTile.gridPosition.x][(int)destTile.gridPosition.y], units[currentTeam][currentPlayer].allies).tileList;
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
		List<Tile> highlightedTiles = TileHighlight.FindHighlight(map[(int)origin.x][(int)origin.y], minActionRange, maxActionRange, units[currentTeam][currentPlayer].allies, move);

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
		for (int i = 0; i < tilesPerCol; i++)
		{
			for (int j = 0; j < tilesPerRow; j++)
			{
				map[i][j].resetDefaultColor();
			}
		}
	}


	/**
	* Attacks the unit on the target tile
	* 
	* v1.1
	* Added check for destTile being null and AI compatibility
	* 
	* @param target destTile The tile our target is on
	* @author Jeffrey Goh
	* @version 1.1
	* @updated 12/6/2017
	*/
	public void attackWithCurrentUnit(Tile destTile)
	{
		if (destTile == null) {
			return;
		}
		else if (destTile.GetComponent<Renderer>().material.color != destTile.returnDefaultColor() || AITeams.Contains(currentTeam))
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
	* Heals the unit on the target tile
	* 
	* @param target destTile The tile our target is on
	* @author Jeffrey Goh
	* @version 1.0
	* @updated 9/7/2017
	*/
	public void healWithCurrentUnit(Tile destTile, int staff)
	{
		if (destTile == null)
		{
			return;
		}
		else if (destTile.GetComponent<Renderer>().material.color != destTile.returnDefaultColor() || AITeams.Contains(currentTeam))
		{
			Unit target = null;
			foreach (List<Unit> i in units)
			{
				foreach (Unit u in i)
				{
					if (u.gridPosition == destTile.gridPosition)
					{
						target = u;
						battle.healWithCurrentUnit(target, Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].inventory[Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].activeStaffIndex]);
						break;
					}
				}
			}
		}
	}

	/**
	* Talk to the target unit
	* 
	* @param target destTile The tile our target is on
	* @author Jeffrey Goh
	* @version 1.0
	* @updated 9/7/2017
	*/
	public void talkWithCurrentUnit(Tile destTile)
	{
		if (destTile == null)
		{
			return;
		}
		else if (destTile.GetComponent<Renderer>().material.color != destTile.returnDefaultColor() || AITeams.Contains(currentTeam))
		{
			Unit target = null;
			foreach (List<Unit> i in units)
			{
				foreach (Unit u in i)
				{
					if (u.gridPosition == destTile.gridPosition)
					{
						target = u;
						talkWithCurrentUnit(target);
						break;
					}
				}
			}
		}
	}

	/**
	* Talk to the target unit
	* 
	* @param target destTile The tile our target is on
	* @author Jeffrey Goh
	* @version 1.0
	* @updated 9/7/2017
	*/
	public void talkWithCurrentUnit(Unit target)
	{
		if (target.currentHP <= 0)
		{
			return;
		}
		else if (target.canTalk.ContainsKey(units[currentPlayer][currentTeam].unitName))
		{
			//Recruit
			if (target.canTalk[units[currentPlayer][currentTeam].unitName] == 0)
			{
				PlayerUnit recruit = ((GameObject)Instantiate(Grid.instance.unitPrefab, new Vector3(target.gridPosition.x - Mathf.Floor(Grid.instance.tilesPerCol / 2), target.gridPosition.y - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<PlayerUnit>();
				recruit.gridPosition = new Vector2(target.gridPosition.x, target.gridPosition.y);

				recruit.unitName = target.unitName;
				recruit.job = target.job;
				recruit.isThief = target.isThief;
				recruit.lvl = target.lvl;
				recruit.exp = target.exp;
				recruit.maxHP = target.maxHP;
				recruit.currentHP = target.currentHP;
				recruit.strength = target.strength;
				recruit.mag = target.mag;
				recruit.skl = target.skl;
				recruit.spd = target.spd;
				recruit.luk = target.luk;
				recruit.def = target.def;
				recruit.res = target.res;
				recruit.con = target.con;
				recruit.mov = target.mov;

				recruit.hpG = target.hpG;
				recruit.strG = target.strG;
				recruit.magG = target.magG;
				recruit.sklG = target.sklG;
				recruit.spdG = target.spdG;
				recruit.lukG = target.lukG;
				recruit.defG = target.defG;
				recruit.resG = target.resG;

				foreach (string weapon in target.proficiency)
				{
					recruit.proficiency.Add(weapon);
				}

				for (int i = 0; i < target.inventory.Count; i++)
				{
					if (i == target.equippedIndex)
					{
						Item.instance.equipWeapon(recruit, target.inventory[i][0], target.inventory[i][1]);
					}
					else if (target.inventory[i].Length == 13)
					{
						Item.instance.addWeapon(recruit, target.inventory[i][0], target.inventory[i][1]);
					}
					else
					{
						Item.instance.addItem(recruit, target.inventory[i][0], target.inventory[i][1]);
					}
				}

				Grid.instance.map[(int)target.gridPosition.x][(int)target.gridPosition.y].occupied = recruit;

				recruit.team = 0;
				recruit.allies.Add(0);

				// The recruited unit gets a turn
				recruit.doneMoving = false;
				recruit.doneAction = false;
				recruit.selected = false;
				recruit.willAttack = false;
				recruit.isMoving = false;
				recruit.isFighting = false;
				recruit.isHealing = false;
				recruit.activeStaffIndex = -1;
				recruit.highlighted = false;
				recruit.displayInventory = false;
				recruit.selectedItemIndex = -1;

				recruit.index = units[currentTeam].Count;
				units[currentTeam].Add(recruit);

				removeTileHighlight();

				units[target.team][target.index].gameObject.SetActive(false);
				units[target.team].RemoveAt(target.index);

				units[currentTeam][currentPlayer].playerWait();
				Debug.Log(recruit.unitName + "has joined!");
			}
		}
	}

	/**
	 * v1.1
	 * By Jeffrey Goh
	 * Added Rotation of tiles so they aren't upside down, flipped terrain generation to be the same as the txt file
	 * 
	 * v1.2
	 * By Jeffrey Goh
	 * tilesPerRow and tilesPerCol's value is defined here, so they adapt to the size of the map instead of needing manual input
    * Reads text file MapConfig and generates the terrain
    * Based on CreateGrid
    * @author Wayne Neo
    * @version 1.2
    * @updated 24/6/2017
    */
	void CreateTerrain()
    {
        // Splits the text file into lines
        string[] lines = mapConfig.text.Split("\n"[0]);

		if (tilesPerCol == 0)
		{
			tilesPerCol = lines.Length;
		}
		System.Array.Reverse(lines);
        string[] line;

		// Iterate through each column
		for (int i = 0; i < tilesPerCol; i++)
			{
            // Splits the text file into strings containing individual integers
            line = lines[i].Split(" "[0]);

			if (tilesPerRow == 0)
			{
				tilesPerRow = line.Length;
			}
			// Iterate through each Row
			for (int j = 0; j < tilesPerRow; j++)
			{
                TerrainS terrain = ((GameObject)Instantiate(terrainPrefab, new Vector3(j - Mathf.Floor(tilesPerCol / 2), i - Mathf.Floor(tilesPerRow / 2), 1), Quaternion.Euler(new Vector3(0, 180, 0)))).GetComponent<TerrainS>();
                terrain.LoadTerrain(System.Int32.Parse(line[j]));
           
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
		for (int j = 0; j < tilesPerRow; j++)
		{
			// A row of tiles
			List<Tile> row = new List<Tile>();
				
			// Iterate through each Row
			for (int i = 0; i < tilesPerCol; i++)
			{
				Tile tile = ((GameObject)Instantiate(tilePrefab, new Vector3(j - Mathf.Floor(tilesPerCol / 2), i - Mathf.Floor(tilesPerRow / 2), 0), Quaternion.Euler(new Vector3()))).GetComponent<Tile>();
				
				tile.gridPosition = new Vector2(j, i);
				// Add tile to the row
				row.Add(tile);
			}

			// Add the row to the map
			map.Add(row);
		}
        
    }


    /**
	* A crude snap back of the camera to the first unit found
    * Will be improved
    * 
	* @author Wayne Neo
	* @version 1.0
	* @updated 25/6/2017
	*/
    public void resetCamera()
    {
        GameObject firstUnit = GameObject.FindGameObjectsWithTag("Player")[0];
        mainCam.transform.position = new Vector3(firstUnit.transform.position.x, firstUnit.transform.position.y, mainCam.transform.position.z);
    }
}

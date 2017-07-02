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

	public int tilesPerRow;
	public int tilesPerCol;

	public int currentPlayer = -1;

	public int currentTeam = 0;

	public List<int> AITeams = new List<int>();

	public int totalDone = 0;

	public BattleFormula battle = new BattleFormula();

	public List<List<Tile>> map = new List<List<Tile>>();
    public List<List<Unit>> units = new List<List<Unit>>();

	/*
	 * 0 = No wincon (deactivated)
	 * 1 = Rout
	 * 2 = Kill Boss
	 * 3 = Survive
	 * More to be added later
	 * */
	//public int winCon1;
	//public int winCon2;

	public bool victory = false;
	public bool failure = false;

	public void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start ()
	{
		Item.instance.initialiseWeapons();
        CreateTerrain();
		CreateTiles();
		CreatePlayers();
	}

	// Update is called once per frame
	void Update()
	{
        //Force shut game
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

		//Debug.Log(currentPlayer);
		if (units[2][0].currentHP <= 0)
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
		else if (units[1][0].currentHP <= 0)
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

		if ((destTile.GetComponent<Renderer>().material.color != destTile.colour || AITeams.Contains(currentTeam)) && destTile.occupied == null)
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
	 * v1.1
	 * By Jeffrey Goh
	 * Added Rotation of tiles so they aren't upside down, flipped terrain generation to be the same as the txt file
        * Reads text file MapConfig and generates the terrain
        * Based on CreateGrid
        * @author Wayne Neo
        * @version 1.1 
        * @updated 24/6/2017
        */
    void CreateTerrain()
    {
        // Splits the text file into lines
        string[] lines = mapConfig.text.Split("\n"[0]);
		System.Array.Reverse(lines);
        string[] line;

		// Iterate through each column
		for (int i = 0; i < tilesPerCol; i++)
			{
            // Splits the text file into strings containing individual integers
            line = lines[i].Split(" "[0]);
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
		PlayerUnit unit1 = ((GameObject)Instantiate(unitPrefab, new Vector3(2 - Mathf.Floor(tilesPerCol / 2), 5 - Mathf.Floor(tilesPerRow / 2),0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<PlayerUnit>();
		unit1.gridPosition = new Vector2(2, 5);

		unit1.unitName = "Karel";
		unit1.job = "Swordmaster";
		unit1.lvl = 19;
		unit1.exp = 0;
		unit1.maxHP = 44;
		unit1.currentHP = 44;
		unit1.strength = 20;
		unit1.mag = 5;
		unit1.skl = 28;
		unit1.spd = 23;
		unit1.luk = 18;
		unit1.def = 15;
		unit1.res = 13;
		unit1.con = 9;
		unit1.mov = 6;

		unit1.hpG = 210;
		unit1.strG = 130;
		unit1.magG = 0;
		unit1.sklG = 140;
		unit1.spdG = 140;
		unit1.lukG = 120;
		unit1.defG = 110;
		unit1.resG = 100;

		Item.instance.equipWeapon(unit1, "Sword", "WoDao");

		map[2][5].occupied = unit1;

		unit1.team = 0;
		unit1.allies.Add(0);
		unit1.index = 0;

		PlayerUnit unit2 = ((GameObject)Instantiate(unitPrefab, new Vector3(2 - Mathf.Floor(tilesPerCol / 2), 6 - Mathf.Floor(tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<PlayerUnit>();
		unit2.gridPosition = new Vector2(2, 6);

		unit2.unitName = "Sanaki";
		unit2.job = "Empress";
		unit2.lvl = 1;
		unit2.exp = 0;
		unit2.maxHP = 28;
		unit2.currentHP = 28;
		unit2.strength = 2;
		unit2.mag = 33;
		unit2.skl = 22;
		unit2.spd = 23;
		unit2.luk = 32;
		unit2.def = 10;
		unit2.res = 28;
		unit2.con = 4;
		unit2.mov = 6;

		unit2.hpG = 70;
		unit2.strG = 40;
		unit2.magG = 60;
		unit2.sklG = 60;
		unit2.spdG = 35;
		unit2.lukG = 55;
		unit2.defG = 30;
		unit2.resG = 50;

		Item.instance.equipWeapon(unit2, "Tome", "Fimbulvetr");

		map[2][6].occupied = unit2;

		unit2.team = 0;
		unit2.allies.Add(0);
		unit2.index = 1;

		PlayerUnit unit4 = ((GameObject)Instantiate(unitPrefab, new Vector3(1 - Mathf.Floor(tilesPerCol / 2), 6 - Mathf.Floor(tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<PlayerUnit>();
		unit4.gridPosition = new Vector2(1, 6);

		unit4.unitName = "Shinon";
		unit4.job = "Sniper";
		unit4.classBonusA = 20;
		unit4.classBonusB = 60;
		unit4.lvl = 13;
		unit4.exp = 0;
		unit4.maxHP = 43;
		unit4.currentHP = 43;
		unit4.strength = 21;
		unit4.mag = 7;
		unit4.skl = 28;
		unit4.spd = 24;
		unit4.luk = 15;
		unit4.def = 20;
		unit4.res = 14;
		unit4.con = 11;
		unit4.mov = 7;

		unit4.hpG = 50;
		unit4.strG = 40;
		unit4.magG = 15;
		unit4.sklG = 70;
		unit4.spdG = 65;
		unit4.lukG = 30;
		unit4.defG = 45;
		unit4.resG = 20;

		Item.instance.equipWeapon(unit4, "Bow", "KillerBow");

		map[1][6].occupied = unit4;

		unit4.team = 0;
		unit4.allies.Add(0);
		unit4.index = 2;

		AIUnit unit3 = ((GameObject)Instantiate(enemyPrefab, new Vector3(10 - Mathf.Floor(tilesPerCol / 2), 7 - Mathf.Floor(tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
		unit3.gridPosition = new Vector2(10, 7);
		unit3.ai_id = 2;

		unit3.unitName = "Nino";
		unit3.job = "Mage";
		unit3.classBonusA = 0;
		unit3.classBonusB = 0;
		unit3.lvl = 5;
		unit3.exp = 0;
		unit3.maxHP = 19;
		unit3.currentHP = 19;
		unit3.strength = 1;
		unit3.mag = 7;
		unit3.skl = 8;
		unit3.spd = 11;
		unit3.luk = 10;
		unit3.def = 4;
		unit3.res = 7;
		unit3.con = 3;
		unit3.mov = 5;

		unit3.hpG = 55;
		unit3.strG = 35;
		unit3.magG = 50;
		unit3.sklG = 55;
		unit3.spdG = 60;
		unit3.lukG = 45;
		unit3.defG = 15;
		unit3.resG = 50;

		Item.instance.equipWeapon(unit3, "Tome", "Elfire");

		map[10][7].occupied = unit3;

		unit3.team = 2;
		unit3.allies.Add(0);
		unit3.allies.Add(2);
		unit3.index = 0;




		AIUnit boss1 = ((GameObject)Instantiate(enemyPrefab, new Vector3(10 - Mathf.Floor(tilesPerCol / 2), 18 - Mathf.Floor(tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
		boss1.gridPosition = new Vector2(10, 18);
		boss1.ai_id = 2;

		boss1.unitName = "Lyon";
		boss1.job = "Necromancer";
		boss1.classBonusA = 20;
		boss1.classBonusB = 60;
		boss1.isBoss = 1;
		boss1.lvl = 20;
		boss1.exp = 0;
		boss1.maxHP = 60;
		boss1.currentHP = 120;
		boss1.strength = 25;
		boss1.mag = 22;
		boss1.skl = 13;
		boss1.spd = 11;
		boss1.luk = 4;
		boss1.def = 17;
		boss1.res = 19;
		boss1.con = 7;
		boss1.mov = 6;

		Item.instance.equipWeapon(boss1, "Tome", "Fenrir");

		map[10][18].occupied = boss1;

		boss1.team = 1;
		boss1.allies.Add(1);
		boss1.index = 0;

		AIUnit enemy1 = ((GameObject)Instantiate(enemyPrefab, new Vector3(10 - Mathf.Floor(tilesPerCol / 2), 13 - Mathf.Floor(tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
		enemy1.gridPosition = new Vector2(10, 13);
		enemy1.ai_id = 1;

		enemy1.unitName = "Riev";
		enemy1.job = "Bishop";
		enemy1.classBonusA = 20;
		enemy1.classBonusB = 60;
		enemy1.lvl = 17;
		enemy1.exp = 0;
		enemy1.maxHP = 51;
		enemy1.currentHP = 51;
		enemy1.strength = 0;
		enemy1.mag = 16;
		enemy1.skl = 22;
		enemy1.spd = 20;
		enemy1.luk = 11;
		enemy1.def = 16;
		enemy1.res = 20;
		enemy1.con = 7;
		enemy1.mov = 6;

		Item.instance.equipWeapon(enemy1, "Tome", "Aura");

		map[10][13].occupied = enemy1;

		enemy1.team = 1;
		enemy1.allies.Add(1);
		enemy1.index = 1;


		AIUnit enemy2 = ((GameObject)Instantiate(enemyPrefab, new Vector3(0 - Mathf.Floor(tilesPerCol / 2), 12 - Mathf.Floor(tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
		enemy2.gridPosition = new Vector2(0, 12);
		enemy2.ai_id = 0;

		enemy2.unitName = "Caellach";
		enemy2.job = "Hero";
		enemy2.classBonusA = 20;
		enemy2.classBonusB = 60;
		enemy2.lvl = 12;
		enemy2.exp = 0;
		enemy2.maxHP = 50;
		enemy2.currentHP = 50;
		enemy2.strength = 20;
		enemy2.mag = 0;
		enemy2.skl = 15;
		enemy2.spd = 14;
		enemy2.luk = 15;
		enemy2.def = 16;
		enemy2.res = 24;
		enemy2.con = 13;
		enemy2.mov = 6;

		Item.instance.equipWeapon(enemy2, "Axe", "Tomahawk");

		map[0][12].occupied = enemy2;

		enemy2.team = 1;
		enemy2.allies.Add(1);
		enemy2.index = 2;

		AIUnit enemy3 = ((GameObject)Instantiate(enemyPrefab, new Vector3(19 - Mathf.Floor(tilesPerCol / 2), 15 - Mathf.Floor(tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
		enemy3.gridPosition = new Vector2(19, 15);
		enemy3.ai_id = 0;

		enemy3.unitName = "Uhai";
		enemy3.job = "Nomadic Trooper";
		enemy2.classBonusA = 20;
		enemy3.classBonusB = 60;
		enemy3.lvl = 7;
		enemy3.exp = 0;
		enemy3.maxHP = 33;
		enemy3.currentHP = 33;
		enemy3.strength = 15;
		enemy3.mag = 0;
		enemy3.skl = 13;
		enemy3.spd = 12;
		enemy3.luk = 4;
		enemy3.def = 12;
		enemy3.res = 13;
		enemy3.con = 10;
		enemy3.mov = 8;

		Item.instance.equipWeapon(enemy3, "Bow", "Longbow");

		map[19][15].occupied = enemy3;

		enemy3.team = 1;
		enemy3.allies.Add(1);
		enemy3.index = 3;

		List<Unit> team0 = new List<Unit>();
		List<Unit> team1 = new List<Unit>();
		List<Unit> team2 = new List<Unit>();

        unit1.mainCam = unit2.mainCam = unit3.mainCam = unit4.mainCam = enemy1.mainCam = enemy2.mainCam = enemy3.mainCam = boss1.mainCam = mainCam;

		team0.Add(unit1);
		team0.Add(unit2);
		team0.Add(unit4);

		units.Add(team0);

		team1.Add(boss1);
		team1.Add(enemy1);
		team1.Add(enemy2);
		team1.Add(enemy3);

		units.Add(team1);


		team2.Add(unit3);

		units.Add(team2);


		AITeams.Add(1);
		AITeams.Add(2);

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

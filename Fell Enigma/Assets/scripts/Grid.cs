using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Grid : MonoBehaviour {

	public static Grid instance;
	
	public GameObject tilePrefab;
	public GameObject unitPrefab;
	public GameObject enemyPrefab;
    public GameObject terrainPrefab;
    public TextAsset mapConfig;
    public Camera mainCam;
	public string mapName;
	public string nextScene;

	public int tilesPerRow = 0;
	public int tilesPerCol = 0;

	public int currentPlayer = -1;

	public int currentTeam = 0;

	public List<int> AITeams = new List<int>();

	public int totalDone = 0;

	public BattleFormula battle = new BattleFormula();

	public List<List<Tile>> map = new List<List<Tile>>();
    public List<List<Unit>> units = new List<List<Unit>>();

	//1st element in int[] is the team that controls the village, the second is the number of captures to take over the village
	public Dictionary<Vector2, int[]> villageStatus = new Dictionary<Vector2, int[]>();

	public Dictionary<Vector2, Vector2> tavernAndSpawn = new Dictionary<Vector2, Vector2>();
	public int tavernLevel;

	public int gold;
	public int goldCap;

	/*
	 * -1: None
	 * 0: MC
	 * 1: Naive Prince
	 * 2: Kind Soul
	 * 3: Young Rebel
	 * 4: Black Heart
	 */
	public int commander;
	public int ultCharge;
	public bool ultActive = false;

	public bool waitUp = false;
	public int waitUpTime = 1;

	public Dictionary<Vector2, string> objectiveSpecificTiles = new Dictionary<Vector2, string>();
	public string objectiveComplete;

	/*
	 * 0: Still playing
	 * 1: Victory
	 * 2: Failure
	 */
	public int status = 0;

	public List<Unit> highlightedEnemies = new List<Unit>();

	public Dictionary<Vector2, string[]> chestLoot = new Dictionary<Vector2, string[]>();

	GameObject HUDCanvas;

	public void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start ()
	{
		HUDCanvas = GameObject.Find("HUDCanvas");
		HUDCanvas.SetActive(false);
		Item.instance.initialiseItems();
        CreateTerrain();
		CreateTiles();
		CreatePlayers.generatePlayers(mapName);
		CreateBuildings.generateBuildings(mapName);
		Debug.Log("Started");
	}

	// Update is called once per frame
	void Update()
	{
        //Force shut game
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

		//Skip Text
		if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
		{
			TextBoxManager.instance.setCurrentLine(TextBoxManager.instance.endAtLine);
			TextBoxManager.instance.disableTextBox();
		}

		if (waitUp || TextBoxManager.instance.isActive)
		{
			return;
		}

		bool newText = ActivateTextAtLine.instance.scriptUpdate();

		if (newText)
		{
			return;
		}

		//Go to next scene
		if (status == 1)
		{
			if (nextScene != null)
			{
				/*
				tilesPerRow = 0;
				tilesPerCol = 0;

				currentPlayer = -1;

				currentTeam = 0;
				map.Clear();
				units.Clear();
				villageStatus.Clear();
				tavernAndSpawn.Clear();
				highlightedEnemies.Clear();
				objectiveSpecificTiles.Clear();
				chestLoot.Clear();
				ultActive = false;

				waitUp = false;
				*/
				SceneManager.LoadScene(nextScene);
				return;
			}
			else
			{
				return;
			}
		}

		status = WinCon.checkWinCon(mapName);
		//Win conditions


		if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
		{
			TextBoxManager.instance.setCurrentLine(TextBoxManager.instance.endAtLine);
			TextBoxManager.instance.disableTextBox();
		}

		if (waitUp || TextBoxManager.instance.isActive)
		{
			return;
		}

		if (status == 1)
		{
			GameControl.instance.Save();
			HUDCanvas.SetActive(true);
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
			HUDCanvas.SetActive(true);
			foreach (List<Unit> u in units)
			{
				foreach (Unit v in u)
				{
					v.gameObject.SetActive(false);
				}
			}
			return;
		}

		// For highlighting of enemy range
		removeTerrainHighlight();

		foreach (Unit u in highlightedEnemies)
		{
			List<Tile> enemiesInRange = TileHighlight.FindAttackRange(Grid.instance.map[(int)u.gridPosition.x][(int)u.gridPosition.y], 1, u.mov, u.weaponMinRange, u.weaponMaxRange + u.weaponRangeBuff, u.allies, true, u.isFlying);
			foreach (Tile t in enemiesInRange)
			{
				t.linkedTerrain.GetComponent<Renderer>().material.color = Color.magenta;
			}
		}

		// To indicate ownership of village at a glance
		foreach (Vector2 v in villageStatus.Keys)
		{
			if (villageStatus[v][0] == -1)
			{
				//Non controlled are white
				Grid.instance.map[(int)v.x][(int)v.y].linkedTerrain.GetComponent<Renderer>().material.color = Color.grey;
			}
			if (villageStatus[v][0] == 0)
			{
				//Player controlled cities are cyan
				Grid.instance.map[(int)v.x][(int)v.y].linkedTerrain.GetComponent<Renderer>().material.color = Color.cyan;
			}
			else if (villageStatus[v][0] == 1)
			{
				//Enemy controlled are red
				Grid.instance.map[(int)v.x][(int)v.y].linkedTerrain.GetComponent<Renderer>().material.color = Color.red;
			}
			else if (villageStatus[v][0] == 2)
			{
				//Neutral controlled are yellow
				Grid.instance.map[(int)v.x][(int)v.y].linkedTerrain.GetComponent<Renderer>().material.color = Color.yellow;
			}
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
					if (waitUp)
					{
						startDelayCoroutine();
					}
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

	//To delay the execution of AI turns
	public void startDelayCoroutine()
	{
		StartCoroutine(delay(waitUpTime));
	}

	IEnumerator delay(int time)
	{
		if (waitUp)
		{
			yield return new WaitForSeconds((float)time);
			waitUp = false;
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
	 * v1.2
	 * Added income from villages
	 * 
	 * v1.3
	 * Added wait time for AI, player gets passive ult gain of 5 points at the start of their turn
	 * 
	 * @author Jeffrey Goh
	 * @version v1.3
	 * @updated 21/7/2017
	 */

	public void nextTurn()
	{
		resetCamera();
        removeTileHighlight();
		if (ultActive)
		{
			dispelUlt();
		}
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
			ultCharge += 5;
			if (ultCharge > 100)
			{
				ultCharge = 100;
			}
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

		//Gain 100 gold per village controlled on player turn (Needs playtesting and balancing)
		if (currentTeam == 0)
		{
			foreach (Vector2 k in villageStatus.Keys)
			{
				if (villageStatus[k][0] == 0)
				{
					gold += 100;
					if (gold > goldCap)
					{
						gold = goldCap;
					}
				}
			}
			Debug.Log("Current gold: " + gold);
		}
		if (AITeams.Contains(currentTeam))
		{
			waitUp = true;
			startDelayCoroutine();
			foreach (Unit u in units[currentTeam])
			{
				u.ai_id = u.ai_id_priority[0];
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
			List <Tile> path = TilePathFinder.FindPath(map[(int)units[currentTeam][currentPlayer].gridPosition.x][(int)units[currentTeam][currentPlayer].gridPosition.y], units[currentTeam][currentPlayer].mov, map[(int)destTile.gridPosition.x][(int)destTile.gridPosition.y], units[currentTeam][currentPlayer].allies, Grid.instance.units[currentTeam][currentPlayer].isFlying).tileList;
			//Debug.Log("Start");
			foreach (Tile t in path)
			{
				//Debug.Log(t.gridPosition.x + " " + t.gridPosition.y);
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
	public void highlightTilesAt(Vector2 origin, Color highlightcolour, int minActionRange, int maxActionRange, bool move, bool flying)
	{
		List<Tile> highlightedTiles = TileHighlight.FindHighlight(map[(int)origin.x][(int)origin.y], minActionRange, maxActionRange, units[currentTeam][currentPlayer].allies, move, flying);

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
     * v1.2
	 * Bugfix for non square maps
	 * 
	 * @author Jeffrey Goh
	 * @version 1.2
	 * @updated 30/7/2017 by Wayne Neo
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
	 * Removes highlights on terrain
     * 
     * v1.2
	 * Bugfix for non square maps
	 * 
	 * @author Jeffrey Goh
	 * @version 1.2
	 * @updated 30/7/2017 by Wayne Neo
	 */
	public void removeTerrainHighlight()
	{
		for (int i = 0; i < tilesPerRow; i++)
		{
			for (int j = 0; j < tilesPerCol; j++)
			{
				map[i][j].linkedTerrain.GetComponent<Renderer>().material = map[i][j].linkedTerrain.defaultColour;
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
	* v1.1
	* Bug fixes
	* 
	* @param target destTile The tile our target is on
	* @author Jeffrey Goh
	* @version 1.1
	* @updated 19/7/2017
	*/
	public void talkWithCurrentUnit(Unit target)
	{
		if (target.currentHP <= 0)
		{
			return;
		}
		else if (target.canTalk.ContainsKey(units[currentTeam][currentPlayer].unitName))
		{
			//Recruit
			if (target.canTalk[units[currentTeam][currentPlayer].unitName] == 0)
			{
				if (ActivateTextAtLine.instance.recruiting == 0)
				{
					ActivateTextAtLine.instance.recruiting = 1;
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
					Debug.Log(recruit.unitName + " has joined!");
				}
			}
		}
		else
		{
			Debug.Log("Can't talk");
		}
	}


	/**
	 * Casts an ultimate ability depending on the current commander
     * 
	 * Check if ult is inactive before continuing
	 * 
     * @author Jeffrey Goh
     * @version 1.1
     * @updated 21/7/2017
    */
	public void castUlt()
	{
		if (ultActive)
		{
			Debug.Log("Ultimate is already active.");
            CombatLog.instance.AddEvent("Ultimate is already active.");
            CombatLog.instance.PrintEvent();
            return;
		}
		ultCharge = 0;
		if (commander == 0)
		{
			//MC
			//Increases Attack and Move Range of all units by 1
			foreach (Unit u in units[currentTeam])
			{
				u.weaponRangeBuff = 1;
				u.mov += 1;
			}
			ultActive = true;
            EventManager.TriggerEvent("PlayUltSound");
			Debug.Log("All allies gained 1 extra mov and 1 extra weapon range!");
            CombatLog.instance.AddEvent("All allies gained 1 extra mov and 1 extra weapon range!");
            CombatLog.instance.PrintEvent();
        }
		else if (commander == 1)
		{
			//Naive Prince
			//Buff stats of all units on the team by 5
			//Everything from hp to res, no con or mov
			foreach (Unit u in units[currentTeam])
			{
				u.maxHP += 5;
				u.currentHP += 5;
				u.strength += 5;
				u.mag += 5;
				u.skl += 5;
				u.spd += 5;
				u.luk += 5;
				u.def += 5;
				u.res += 5;
			}
			ultActive = true;
            EventManager.TriggerEvent("PlayUltSound");
            Debug.Log("All allies gained 5 to all stats!");
            CombatLog.instance.AddEvent("All allies gained 5 to all stats!");
            CombatLog.instance.PrintEvent();
        }
		else if (commander == 2)
		{
			//Kind Soul
			//Restore 30% hp to all units on the team
			foreach (Unit u in units[currentTeam])
			{
				if (u.currentHP > 0)
				{
					u.currentHP += (int)((double)u.maxHP * 0.3);
					if (u.currentHP > u.maxHP)
					{
						u.currentHP = u.maxHP;
					}
				}
			}
			ultActive = true;
            EventManager.TriggerEvent("PlayUltSound");
            Debug.Log("All allies have been healed!");
            CombatLog.instance.AddEvent("All allies have been healed!");
            CombatLog.instance.PrintEvent();
        }
		else if (commander == 3)
		{
			//Young Rebel
			//Cancels the target's counterattack, improves all units' hit and crit
			//The code for the above is in BattleFormula, we just switch a bool to trigger it or not here
			foreach (Unit u in units[currentTeam])
			{
				u.rebelBuff = true;
			}
			ultActive = true;
            EventManager.TriggerEvent("PlayUltSound");
            Debug.Log("All allies no longer receive counterattacks, and have improved hit and crit rates!");
            CombatLog.instance.AddEvent("All allies no longer receive counterattacks, and have improved hit and crit rates!");
            CombatLog.instance.PrintEvent();
        }
		else if (commander == 4)
		{
			//Black Heart
			//Attacks every non friendly unit for 10 damage, non lethal
			for (int i = 0; i < units.Count; i++)
			{
				if (!units[currentTeam][0].allies.Contains(i))
				{
					foreach (Unit u in units[i])
					{
						if (u.currentHP > 1)
						{
							u.currentHP -= 10;
							if (u.currentHP < 1)
							{
								u.currentHP = 1;
							}
						}
					}
				}
			}
			ultActive = true;
            EventManager.TriggerEvent("PlayUltSound");
            Debug.Log("All enemies have taken 10 damage!");
            CombatLog.instance.AddEvent("All enemies have taken 10 damage!");
            CombatLog.instance.PrintEvent();
        }
		else
		{
			Debug.Log("No Commander set, can't cast ult.");
            CombatLog.instance.AddEvent("No Commander set, can't cast ult.");
            CombatLog.instance.PrintEvent();
        }
	}


	/**
	 * Dispels an ultimate ability at the end of turn
	 * 
	 * v1.1
	 * Checks if ult is active before continuing
	 * 
	 * @author Jeffrey Goh
	 * @version 1.1
	 * @updated 21/7/2017
	*/
	public void dispelUlt()
	{
		if (!ultActive)
		{
			Debug.Log("Ultimate is already deactivated.");
			return;
		}
		if (commander == 0)
		{
			//MC
			//Resets Attack Range of all units
			foreach (Unit u in units[currentTeam])
			{
				u.weaponRangeBuff = 0;
				u.mov -= 1;
			}
			ultActive = false;
		}
		else if (commander == 1)
		{
			//Naive Prince
			//Reset stats
			foreach (Unit u in units[currentTeam])
			{
				u.maxHP -= 5;
				u.currentHP -= 5;
				u.strength -= 5;
				u.mag -= 5;
				u.skl -= 5;
				u.spd -= 5;
				u.luk -= 5;
				u.def -= 5;
				u.res -= 5;
			}
			ultActive = false;
		}
		else if (commander == 2)
		{
			//Kind Soul
			//None needed
			ultActive = false;
		}
		else if (commander == 3)
		{
			//Young Rebel
			//Switch off rebelBuff
			foreach (Unit u in units[currentTeam])
			{
				u.rebelBuff = false;
			}
			ultActive = false;
		}
		else if (commander == 4)
		{
			//Black Heart
			//Nothing needed
			ultActive = false;
		}
		else
		{
			Debug.Log("No Commander set, can't dispel ult.");
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
	 * 
	 * v1.3
	 * By Jeffrey Goh
	 * Added villageStatus 
    * Reads text file MapConfig and generates the terrain
    * Based on CreateGrid
    * @author Wayne Neo
    * @version 1.3
    * @updated 15/7/2017
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
				if (System.Int32.Parse(line[j]) == 6)
				{
					int[] village = new int[] { -1, 2 };
					villageStatus.Add(new Vector2(j,i), village);
				}
			}
        }
		Debug.Log("Terrain Created");
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
		Debug.Log("Tiles Created");
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

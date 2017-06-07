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

	public int tilesPerRow;
	public int tilesPerCol;

	public int currentPlayer = 0;


	public List<List<Tile>> map = new List<List<Tile>>();
    public List<List<TerrainS>> mapT = new List<List<TerrainS>>();
    public List<Unit> units = new List<Unit>();

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
		// Skip turn if dead
		if (units[currentPlayer].currentHP <= 0)
		{
			nextTurn();
		}
		else
		{
			units[currentPlayer].turnUpdate();
		}
	}

	/**
	 * Moves on to the next turn
	 * To be revamped
	 * @author Jeffrey Goh
	 * @version v0.01
	 * @updated 2/6/2017
	 */

	public void nextTurn()
	{
		if (currentPlayer < units.Count - 1)
		{
			currentPlayer++;
		}
		else
		{
			currentPlayer = 0;
		}
		Debug.Log(currentPlayer);
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
	* @param destTile The destination tile
	* @author Jeffrey Goh
	* @version 1.3
	* @updated 5/6/2017
	*/
	public void moveCurrentUnit(Tile destTile)
	{
		if (destTile.GetComponent<Renderer>().material.color != destTile.colour)
		{
			// Don't let the unit move to a tile that's occupied
			foreach (Unit u in units)
			{
				if (u.gridPosition == destTile.gridPosition)
				{
					return;
				}
			}

			// Remove the green highlighted tiles
			removeTileHighlight();

			map[(int)units[currentPlayer].gridPosition.x][(int)units[currentPlayer].gridPosition.y].occupied = null;

			// Get the path from the unit's current position to its final position
			List <Tile> path = TilePathFinder.FindPath(map[(int)units[currentPlayer].gridPosition.x][(int)units[currentPlayer].gridPosition.y], units[currentPlayer].mov, map[(int)destTile.gridPosition.x][(int)destTile.gridPosition.y]).tileList;
			foreach (Tile t in path)
			{
				units[currentPlayer].positionQueue.Add(map[(int)t.gridPosition.x][(int)t.gridPosition.y].transform.position);
			}
			// Set gridPosition of the unit to the destination tile
			units[currentPlayer].gridPosition = destTile.gridPosition;

			map[(int)destTile.gridPosition.x][(int)destTile.gridPosition.y].occupied = units[currentPlayer];
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
	* Attacks the target unit, based on the equipped weapons
	* 
	* v 1.0
	* Basic combat formulas
	* 
	* v 1.1
	* Opponent will counterattack, checks for magic weapons
	* 
	* v1.2
	* Opponent will only counterattack if in range, attacker and defender will followup if attack speed >= 4
	* 
	* v1.3
	* Added max and min range instead of just 1 range
	* Fixed some bugs with range calculations
	* 
	* To do
	* Does not factor in terrain, supports, other misc things
	* No weapon triangle or followups and doesn't check range
	* 
	* @param target The target of the attack
	* @author Jeffrey Goh
	* @version 1.3
	* @updated 5/6/2017
	*/
	public void attackWithCurrentUnit(Unit target)
	{
		if (map[(int)target.gridPosition.x][(int)target.gridPosition.y].GetComponent<Renderer>().material.color != map[(int)target.gridPosition.x][(int)target.gridPosition.y].colour)
		{

			if (target != null)
			{
				Debug.Log("Calculating");

				// Range
				bool canCounter = false;

				int dist = Mathf.Abs((int)units[currentPlayer].gridPosition.x - (int)target.gridPosition.x) + Mathf.Abs((int)units[currentPlayer].gridPosition.y - (int)target.gridPosition.y);

				if (target.weaponMinRange <= dist && target.weaponMaxRange >= dist)
				{
					canCounter = true;
				}

				Debug.Log(target.weaponMaxRange + " " + target.weaponMaxRange + " " + dist);

				// Accuracy

				int attackerAcc = units[currentPlayer].weaponAcc + units[currentPlayer].skl * 2 + units[currentPlayer].luk / 2;
				int attackerAvd = units[currentPlayer].spd * 2 + units[currentPlayer].luk;
				int defenderAcc = target.weaponAcc + target.skl * 2 + target.luk / 2;
				int defenderAvd = target.spd * 2 + target.luk;

				int attackerHit = attackerAcc - defenderAvd;
				int defenderHit = defenderAcc - attackerAvd;

				int attackerAtk;
				int attackerDef;

				int defenderAtk;
				int defenderDef;

				// Damage for attacker
				// Str and Def if physical weapon equipped, Mag and Res otherwise
				if (units[currentPlayer].weaponPhysical)
				{
					attackerAtk = units[currentPlayer].strength + units[currentPlayer].weaponMt;
					defenderDef = target.def;
				}
				else
				{
					attackerAtk = units[currentPlayer].mag + units[currentPlayer].weaponMt;
					defenderDef = target.res;
				}

				// Damage for defender

				if (target.weaponPhysical)
				{
					defenderAtk = target.strength + target.weaponMt;
					attackerDef = units[currentPlayer].def;
				}
				else
				{
					defenderAtk = target.mag + target.weaponMt;
					attackerDef = units[currentPlayer].res;
				}

				int attackerDmg = attackerAtk - defenderDef;
				int defenderDmg = defenderAtk - attackerDef;

				// tink instead of doing negative damage and healing the enemy
				if (attackerDmg < 0)
				{
					attackerDmg = 0;
				}

				if (defenderDmg < 0)
				{
					defenderDmg = 0;
				}

				//Crits
				int attackerCritRate = units[currentPlayer].weaponCrit + units[currentPlayer].skl / 2;
				int attackerCritAvd = units[currentPlayer].luk;
				int defenderCritRate = target.weaponCrit + target.skl / 2;
				int defenderCritAvd = target.luk;
				int attackerCritChance = attackerCritRate - defenderCritAvd;
				int defenderCritChance = defenderCritRate - attackerCritAvd;

				// Keep crit chance within 0 - 100 to be safe
				if (attackerCritChance > 100)
				{
					attackerCritChance = 100;
				}
				else if (attackerCritChance < 0)
				{
					attackerCritChance = 0;
				}

				if (defenderCritChance > 100)
				{
					defenderCritChance = 100;
				}
				else if (defenderCritChance < 0)
				{
					defenderCritChance = 0;
				}

				//Attack Speed
				int atkBurden = units[currentPlayer].weaponWt - units[currentPlayer].con;
				if (atkBurden < 0)
				{
					atkBurden = 0;
				}

				int defBurden = target.weaponWt - target.con;
				if (defBurden < 0)
				{
					defBurden = 0;
				}

				int atkAS = units[currentPlayer].spd - atkBurden;
				int defAS = target.spd - defBurden;

				//The actual attack

				// Roll for hit
				int hit = Random.Range(1, 100);
				if (hit <= attackerHit)
				{
					Debug.Log("Name: " + units[currentPlayer].name + " Hit: " + attackerHit + " DMG: " + attackerDmg + " Crit: " + attackerCritChance);

					//Check for crits
					int crit = Random.Range(1, 100);

					Debug.Log("Hit Roll: " + hit + " Crit Roll: " + crit);

					int tempattackerDmg;

					if (crit <= attackerCritChance)
					{
						tempattackerDmg = attackerDmg * 3;
						Debug.Log(units[currentPlayer].name + " has critically hit " + target.name + " for " + tempattackerDmg + " damage!");
					}
					else
					{
						tempattackerDmg = attackerDmg;
						Debug.Log(units[currentPlayer].name + " has hit " + target.name + " for " + tempattackerDmg + " damage!");
					}

					target.currentHP -= tempattackerDmg;

				}
				// If miss
				else
				{
					Debug.Log("Name: " + units[currentPlayer].name + " Hit: " + attackerHit + " DMG: " + attackerDmg + " Crit: " + attackerCritChance);
					Debug.Log("Hit Roll: " + hit);

					Debug.Log(units[currentPlayer].name + " missed!");
				}

				// Check if target is dead
				if (target.currentHP <= 0)
				{
					target.currentHP = 0;
					Debug.Log(target.name + " has died!");

					// Deactivate menu GUI and set unit to not attacking after the attack
					units[currentPlayer].selected = false;
					units[currentPlayer].isFighting = false;
					removeTileHighlight();
					return;
				}
				else
				{
					Debug.Log(target.name + ": " + target.currentHP + "/" + target.maxHP + ", " + units[currentPlayer].name + ": " + units[currentPlayer].currentHP + "/" + units[currentPlayer].maxHP);
				}



				// Counterattack
				if (target.currentHP > 0 && canCounter)
				{
					Debug.Log(target.name + " counterattacks!");

					//Roll for hit
					int counterHit = Random.Range(1, 100);
					if (counterHit <= defenderHit)
					{
						Debug.Log("Name: " + target.name + " Hit: " + defenderHit + " DMG: " + defenderDmg + " Crit: " + defenderCritChance);

						//Check for crits
						int counterCrit = Random.Range(1, 100);

						Debug.Log("Hit Roll: " + counterHit + " Crit Roll: " + counterCrit);

						int tempdefenderDmg;

						if (counterCrit <= defenderCritChance)
						{
							tempdefenderDmg = defenderDmg * 3;
							Debug.Log(target.name + " has critically hit " + units[currentPlayer].name + " for " + tempdefenderDmg + " damage!");
						}
						else
						{
							tempdefenderDmg = defenderDmg;
							Debug.Log(target.name + " has hit " + units[currentPlayer].name + " for " + tempdefenderDmg + " damage!");
						}

						units[currentPlayer].currentHP -= tempdefenderDmg;

					}
					else
					{
						Debug.Log("Name: " + target.name + " Hit: " + defenderHit + " DMG: " + defenderDmg + " Crit: " + defenderCritChance);
						Debug.Log("Hit Roll: " + counterHit);

						Debug.Log(target.name + " missed!");
					}

					if (units[currentPlayer].currentHP <= 0)
					{
						units[currentPlayer].currentHP = 0;
						Debug.Log(units[currentPlayer].name + " has died!");

						// Deactivate menu GUI and set unit to not attacking after the attack
						units[currentPlayer].selected = false;
						units[currentPlayer].isFighting = false;
						removeTileHighlight();
						return;
					}
					else
					{
						Debug.Log(target.name + ": " + target.currentHP + "/" + target.maxHP + ", " + units[currentPlayer].name + ": " + units[currentPlayer].currentHP + "/" + units[currentPlayer].maxHP);
					}



					// Check if attacker is dead
					if (units[currentPlayer].currentHP <= 0)
					{
						units[currentPlayer].currentHP = 0;
						Debug.Log(units[currentPlayer].name + " has died!");
					}
					else
					{
						Debug.Log(target.name + ": " + target.currentHP + ", " + units[currentPlayer].name + ": " + units[currentPlayer].currentHP);
					}

				}



				//Follow up
				if (atkAS - defAS >= 4 && units[currentPlayer].currentHP > 0)
				{
					Debug.Log(units[currentPlayer].name + " does a follow-up attack!");

					// Roll for hit
					int hit1 = Random.Range(1, 100);
					if (hit1 <= attackerHit)
					{
						Debug.Log("Name: " + units[currentPlayer].name + " Hit: " + attackerHit + " DMG: " + attackerDmg + " Crit: " + attackerCritChance);

						//Check for crits
						int crit = Random.Range(1, 100);

						Debug.Log("Hit Roll: " + hit1 + " Crit Roll: " + crit);

						int tempattackerDmg;

						if (crit <= attackerCritChance)
						{
							tempattackerDmg = attackerDmg * 3;
							Debug.Log(units[currentPlayer].name + " has critically hit " + target.name + " for " + tempattackerDmg + " damage!");
						}
						else
						{
							tempattackerDmg = attackerDmg;
							Debug.Log(units[currentPlayer].name + " has hit " + target.name + " for " + tempattackerDmg + " damage!");
						}

						target.currentHP -= tempattackerDmg;

					}
					// If miss
					else
					{
						Debug.Log("Name: " + units[currentPlayer].name + " Hit: " + attackerHit + " DMG: " + attackerDmg + " Crit: " + attackerCritChance);
						Debug.Log("Hit Roll: " + hit1);

						Debug.Log(units[currentPlayer].name + " missed!");
					}

					// Check if target is dead
					if (target.currentHP <= 0)
					{
						target.currentHP = 0;
						Debug.Log(target.name + " has died!");
					}
					else
					{
						Debug.Log(target.name + ": " + target.currentHP + ", " + units[currentPlayer].name + ": " + units[currentPlayer].currentHP);
					}



					// Check if target is dead
					if (target.currentHP <= 0)
					{
						target.currentHP = 0;
						Debug.Log(target.name + " has died!");

						// Deactivate menu GUI and set unit to not attacking after the attack
						units[currentPlayer].selected = false;
						units[currentPlayer].isFighting = false;
						removeTileHighlight();
						return;
					}
					else
					{
						Debug.Log(target.name + ": " + target.currentHP + "/" + target.maxHP + ", " + units[currentPlayer].name + ": " + units[currentPlayer].currentHP + "/" + units[currentPlayer].maxHP);
					}
				}



				if (target.currentHP > 0 && canCounter && defAS - atkAS >= 4)
				{
					Debug.Log(target.name + " does a follow up counterattack!");
					// Counterattack

					//Roll for hit
					int counterHit = Random.Range(1, 100);
					if (counterHit <= defenderHit)
					{
						Debug.Log("Name: " + target.name + " Hit: " + defenderHit + " DMG: " + defenderDmg + " Crit: " + defenderCritChance);

						//Check for crits
						int counterCrit = Random.Range(1, 100);

						Debug.Log("Hit Roll: " + counterHit + " Crit Roll: " + counterCrit);

						int tempdefenderDmg;

						if (counterCrit <= defenderCritChance)
						{
							tempdefenderDmg = defenderDmg * 3;
							Debug.Log(target.name + " has critically hit " + units[currentPlayer].name + " for " + tempdefenderDmg + " damage!");
						}
						else
						{
							tempdefenderDmg = defenderDmg;
							Debug.Log(target.name + " has hit " + units[currentPlayer].name + " for " + tempdefenderDmg + " damage!");
						}

						units[currentPlayer].currentHP -= tempdefenderDmg;

					}
					else
					{
						Debug.Log("Name: " + target.name + " Hit: " + defenderHit + " DMG: " + defenderDmg + " Crit: " + defenderCritChance);
						Debug.Log("Hit Roll: " + counterHit);

						Debug.Log(target.name + " missed!");
					}

					if (units[currentPlayer].currentHP <= 0)
					{
						units[currentPlayer].currentHP = 0;
						Debug.Log(units[currentPlayer].name + " has died!");
					}
					else
					{
						Debug.Log(target.name + ": " + target.currentHP + ", " + units[currentPlayer].name + ": " + units[currentPlayer].currentHP);
					}


					// Check if attacker is dead
					if (units[currentPlayer].currentHP <= 0)
					{
						units[currentPlayer].currentHP = 0;
						Debug.Log(units[currentPlayer].name + " has died!");

						// Deactivate menu GUI and set unit to not attacking after the attack
						units[currentPlayer].selected = false;
						units[currentPlayer].isFighting = false;
						removeTileHighlight();
						return;
					}
					else
					{
						Debug.Log(target.name + ": " + target.currentHP + "/" + target.maxHP + ", " + units[currentPlayer].name + ": " + units[currentPlayer].currentHP + "/" + units[currentPlayer].maxHP);
					}
				}
			}
			
		}
		else
		{
			Debug.Log("Invalid target");
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
			foreach (Unit u in units)
			{
				if (u.gridPosition == destTile.gridPosition)
				{
					target = u;
					this.attackWithCurrentUnit(target);
					break;
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
	* @author Jeffrey Goh
	* @version 1.0
	* @updated 2/6/2017
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

		units.Add(unit1);

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

		units.Add(unit2);

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

		units.Add(unit3);


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


		units.Add(unit4);


		/*
		AIUnit enemy1 = ((GameObject)Instantiate(enemyPrefab, new Vector3(3 - Mathf.Floor(tilesPerCol / 2), 3 - Mathf.Floor(tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
		enemy1.gridPosition = new Vector2(3, 3);

		units.Add(enemy1);
		*/
	}
}

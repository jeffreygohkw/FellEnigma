using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Grid : MonoBehaviour {

	public static Grid instance;
	
	public GameObject tilePrefab;
	public GameObject unitPrefab;
	public GameObject enemyPrefab;

	public int tilesPerRow;
	public int tilesPerCol;

	public int currentPlayer = 0;


	List <List<Tile>> map = new List<List<Tile>>();
	public List <Unit> units = new List<Unit>();

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

	private void OnGUI()
	{
		units[currentPlayer].TurnOnGUI();
	}


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

	public void moveCurrentUnit(Tile destTile)
	{
		units[currentPlayer].moveTo = destTile.transform.position;
		units[currentPlayer].gridPosition = destTile.gridPosition;
	}

	public void attackWithCurrentUnit(Tile destTile)
	{
		Unit target = null;
		foreach (Unit u in units) {
			if (u.gridPosition == destTile.gridPosition)
			{
				target = u;
			}

			if (target != null)
			{
				Debug.Log("Calculating");
				//Combat formulas v1.1

				// v1: Basic 1 way combat

				// v1.1: added counterattacks
				
				// To-do
				// Does not factor in terrain, support bonuses or other misc things
				// Weapon Triangle?
				// No followups, or range

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
				}
				else
				{
					Debug.Log(target.name + ": " + target.currentHP + ", " + units[currentPlayer].name + ": " + units[currentPlayer].currentHP);
				}



				Debug.Log(target.name + " counterattacks!");
				if (target.currentHP > 0)
				{
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
				}

				break;
			}
		}
		// Deactivate menu GUI and set unit to not attacking after the attack
		units[currentPlayer].selected = false;
		units[currentPlayer].isFighting = false;

	}

	// To Generate Grid
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
	}

	// Add players to the map
	void CreatePlayers()
	{
		PlayerUnit unit1 = ((GameObject)Instantiate(unitPrefab, new Vector3(0 - Mathf.Floor(tilesPerCol / 2), 0 - Mathf.Floor(tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<PlayerUnit>();
		unit1.gridPosition = new Vector2(0, 0);

		unit1.name = "Lyn";
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


		units.Add(unit1);

		PlayerUnit unit2 = ((GameObject)Instantiate(unitPrefab, new Vector3(0 - Mathf.Floor(tilesPerCol / 2), 1 - Mathf.Floor(tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<PlayerUnit>();
		unit2.gridPosition = new Vector2(0, 1);

		unit2.name = "Batta";
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

		units.Add(unit2);

		PlayerUnit unit3 = ((GameObject)Instantiate(unitPrefab, new Vector3(2 - Mathf.Floor(tilesPerCol / 2), 1 - Mathf.Floor(tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<PlayerUnit>();
		unit3.gridPosition = new Vector2(2, 1);

		unit3.name = "Lute";
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

		units.Add(unit3);

		AIUnit enemy1 = ((GameObject)Instantiate(enemyPrefab, new Vector3(3 - Mathf.Floor(tilesPerCol / 2), 3 - Mathf.Floor(tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
		enemy1.gridPosition = new Vector2(3, 3);

		units.Add(enemy1);
	}
}

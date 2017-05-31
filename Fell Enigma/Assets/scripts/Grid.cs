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
				//Combat formulas v1

				//Accuracy
				int acc = units[currentPlayer].weaponAcc + units[currentPlayer].skl * 2 + units[currentPlayer].luk / 2;
				int avd = target.spd * 2 + target.luk;
				int trueHit = acc - avd;

				//Damage
				int atk = units[currentPlayer].strength + units[currentPlayer].equipped;
				int def = target.def;
				int dmg = atk - def;

				if (dmg < 0)
				{
					dmg = 0;
				}

				//Crits
				int critRate = units[currentPlayer].weaponCrit + units[currentPlayer].skl / 2;
				int critAvd = target.luk;
				int critChance = critRate - critAvd;

				if (critChance > 100)
				{
					critChance = 100;
				}
				else if (critChance < 0)
				{
					critChance = 0;
				}

				//The actual attack
				int hit = Random.Range(1, 100);
				if (hit <= trueHit)
				{
					Debug.Log("Name: " + units[currentPlayer].name + " Hit: " + trueHit + " DMG: " + dmg + " Crit: " + critChance);
					
					//Check for crits
					int crit = Random.Range(1, 100);

					Debug.Log("Hit Roll: " + hit + " Crit Roll: " + crit);


					if (crit <= critChance)
					{
						dmg *= 3;
						Debug.Log(units[currentPlayer].name + " has critically hit " + target.name + " for " + dmg + " damage!");
					}
					else
					{


						Debug.Log(units[currentPlayer].name + " has hit " + target.name + " for " + dmg + " damage!");
					}

					target.currentHP -= dmg;
					
				}
				else
				{
					Debug.Log("Name: " + units[currentPlayer].name + " Hit: " + trueHit + " DMG: " + dmg + " Crit: " + critChance);
					Debug.Log("Hit Roll: " + hit);

					Debug.Log(units[currentPlayer].name + " missed!");
				}

				if (target.currentHP <= 0)
				{
					target.currentHP = 0;
					Debug.Log(target.name + " has died!");
				}
				else
				{
					Debug.Log(target.name + ": " + target.currentHP + ", " + units[currentPlayer].name + ": " + units[currentPlayer].currentHP);
				}

				break;
			}
		}
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
		unit1.skl = 7;
		unit1.spd = 9;
		unit1.luk = 5;
		unit1.def = 2;
		unit1.res = 0;
		unit1.mov = 5;

		unit1.equipped = 5;
		unit1.weaponAcc = 90;
		unit1.weaponCrit = 0;



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
		unit2.skl = 1;
		unit2.spd = 3;
		unit2.luk = 2;
		unit2.def = 3;
		unit2.res = 0;
		unit2.mov = 5;

		unit2.equipped = 8;
		unit2.weaponAcc = 75;
		unit2.weaponCrit = 0;

		units.Add(unit2);

		AIUnit enemy1 = ((GameObject)Instantiate(enemyPrefab, new Vector3(3 - Mathf.Floor(tilesPerCol / 2), 3 - Mathf.Floor(tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
		enemy1.gridPosition = new Vector2(3, 3);

		units.Add(enemy1);
	}
}

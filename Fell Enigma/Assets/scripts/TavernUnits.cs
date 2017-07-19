using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernUnits : MonoBehaviour {

	// Use this for initialization
	void Start () {
		initUnits();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public TextAsset unitDatabase;

	public static Dictionary<string, List<string[]>> tavernUnits = new Dictionary<string, List<string[]>>();


	/**
	* Initialize the generic units that will spawn from the tavern
	* 
	* @param destTile The destination tile
	* @author Jeffrey Goh
	* @version 1.0
	* @updated 13/7/2017
	*/
	public void initUnits()
	{
		// Splits the text file into lines
		string[] lines = unitDatabase.text.Split("\r"[0]);
		string[] name;
		string[] weaponProf;
		string[] baseStats;
		string[] growths;
		List<string[]> unitData;

		for (int i = 0; i < lines.Length; i += 4)
		{
			name = lines[i].Split(" "[0]);
			weaponProf = lines[i + 1].Split(" "[0]);
			baseStats = lines[i + 2].Split(" "[0]);
			growths = lines[i + 3].Split(" "[0]);

			unitData = new List<string[]>();
			unitData.Add(name);
			unitData.Add(weaponProf);
			unitData.Add(baseStats);
			unitData.Add(growths);
			tavernUnits.Add(name[0], unitData);
		}
	}


	/**
	* Checks if a unit should spawn, then spawn the unit from the tavern
	* 
	* v1.1
	* Added increased stats if a unit is spawned at a higher level
	* 
	* v1.2
	* Fixed bug with mov and con
	* 
	* @param unitClass The class of the unit to spawn
	* @param tavernLocation The tavern this unit is being requested to spawn from
	* @author Jeffrey Goh
	* @version 1.2
	* @updated 19/7/2017
	*/
	public static void tavernSpawn(string unitClass, Vector2 tavernLocation)
	{
		if (Grid.instance.tavernAndSpawn.ContainsKey(tavernLocation))
		{
			if (!Grid.instance.map[(int)Grid.instance.tavernAndSpawn[tavernLocation].x][(int)Grid.instance.tavernAndSpawn[tavernLocation].y].occupied)
			{
				if (Grid.instance.tavernLevel * 500 <= Grid.instance.gold)
				{
					if (tavernUnits.ContainsKey(unitClass))
					{
						PlayerUnit unit1 = ((GameObject)Instantiate(Grid.instance.unitPrefab, new Vector3(Grid.instance.tavernAndSpawn[tavernLocation].x - Mathf.Floor(Grid.instance.tilesPerCol / 2), Grid.instance.tavernAndSpawn[tavernLocation].y - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<PlayerUnit>();
						unit1.gridPosition = new Vector2(Grid.instance.tavernAndSpawn[tavernLocation].x, Grid.instance.tavernAndSpawn[tavernLocation].y);

						unit1.unitName = "Mercenary";
						unit1.job = unitClass;
						unit1.lvl = Grid.instance.tavernLevel;
						unit1.exp = 0;
						unit1.maxHP = (int)(double.Parse(tavernUnits[unitClass][2][0]) + Grid.instance.tavernLevel * double.Parse(tavernUnits[unitClass][3][0]) * 0.01);
						unit1.currentHP = (int)(double.Parse(tavernUnits[unitClass][2][0]) + Grid.instance.tavernLevel * double.Parse(tavernUnits[unitClass][3][0]) * 0.01);
						unit1.strength = (int)(double.Parse(tavernUnits[unitClass][2][1]) + Grid.instance.tavernLevel * double.Parse(tavernUnits[unitClass][3][1]) * 0.01);
						unit1.mag = (int)(double.Parse(tavernUnits[unitClass][2][2]) + Grid.instance.tavernLevel * double.Parse(tavernUnits[unitClass][3][2]) * 0.01);
						unit1.skl = (int)(double.Parse(tavernUnits[unitClass][2][3]) + Grid.instance.tavernLevel * double.Parse(tavernUnits[unitClass][3][3]) * 0.01);
						unit1.spd = (int)(double.Parse(tavernUnits[unitClass][2][4]) + Grid.instance.tavernLevel * double.Parse(tavernUnits[unitClass][3][4]) * 0.01);
						unit1.luk = (int)(double.Parse(tavernUnits[unitClass][2][5]) + Grid.instance.tavernLevel * double.Parse(tavernUnits[unitClass][3][5]) * 0.01);
						unit1.def = (int)(double.Parse(tavernUnits[unitClass][2][6]) + Grid.instance.tavernLevel * double.Parse(tavernUnits[unitClass][3][6]) * 0.01);
						unit1.res = (int)(double.Parse(tavernUnits[unitClass][2][7]) + Grid.instance.tavernLevel * double.Parse(tavernUnits[unitClass][3][7]) * 0.01);
						unit1.con = int.Parse(tavernUnits[unitClass][2][9]);
						unit1.mov = int.Parse(tavernUnits[unitClass][2][8]);

						unit1.hpG = int.Parse(tavernUnits[unitClass][3][0]);
						unit1.strG = int.Parse(tavernUnits[unitClass][3][1]);
						unit1.magG = int.Parse(tavernUnits[unitClass][3][2]);
						unit1.sklG = int.Parse(tavernUnits[unitClass][3][3]);
						unit1.spdG = int.Parse(tavernUnits[unitClass][3][4]);
						unit1.lukG = int.Parse(tavernUnits[unitClass][3][5]);
						unit1.defG = int.Parse(tavernUnits[unitClass][3][6]);
						unit1.resG = int.Parse(tavernUnits[unitClass][3][7]);

						foreach (string s in tavernUnits[unitClass][1])
						{
							string iron = "Iron" + s;
							unit1.proficiency.Add(s);
							if (s == "Staff")
							{
								Item.instance.addItem(unit1, s, "Heal");
							}
							else if (s == "Tome")
							{
								Item.instance.equipWeapon(unit1, s, "Fire");
							}
							else
							{
								Item.instance.equipWeapon(unit1, s, iron);
							}
						}

						Grid.instance.map[(int)Grid.instance.tavernAndSpawn[tavernLocation].x][(int)Grid.instance.tavernAndSpawn[tavernLocation].y].occupied = unit1;

						unit1.team = 0;
						unit1.allies = Grid.instance.units[Grid.instance.currentTeam][0].allies;
						unit1.index = Grid.instance.units[Grid.instance.currentTeam].Count;
						unit1.mainCam = Grid.instance.mainCam;
						Grid.instance.units[Grid.instance.currentTeam].Add(unit1);

						//Pay up
						Grid.instance.gold -= Grid.instance.tavernLevel * 500;
						Debug.Log(unitClass + " spawned for " + Grid.instance.tavernLevel * 500 + " gold");
						Debug.Log(Grid.instance.gold + " gold left");

						//Wait
						Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].playerWait();
					}
					else
					{
						Debug.Log("Missing Class in Tavern.");
					}
				}
				else
				{
					Debug.Log("Not enough gold");
				}
			}
			else
			{
				Debug.Log("Spawn point is blocked");
			}
		}
		else
		{
			Debug.Log("Missing Tavern");
		}

	}
}

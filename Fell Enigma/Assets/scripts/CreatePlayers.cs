using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlayers : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/**
	* Add Grid.instance.units, both allies and enemies to the Grid.instance.map
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
	public static void generatePlayers(string mapName)
	{
		if (mapName == "tutorial")
		{
			//5, 25
			PlayerUnit unit1 = ((GameObject)Instantiate(Grid.instance.unitPrefab, new Vector3(5 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 25 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<PlayerUnit>();
			unit1.gridPosition = new Vector2(5, 25);

			unit1.unitName = "Naive Prince";
			unit1.job = "Prince";
			unit1.isHero = true;
			unit1.lvl = 1;
			unit1.exp = 0;
			unit1.maxHP = 18;
			unit1.currentHP = 18;
			unit1.strength = 7;
			unit1.mag = 0;
			unit1.skl = 5;
			unit1.spd = 6;
			unit1.luk = 3;
			unit1.def = 6;
			unit1.res = 2;
			unit1.con = 7;
			unit1.mov = 5;

			unit1.hpG = 80;
			unit1.strG = 60;
			unit1.magG = 20;
			unit1.sklG = 50;
			unit1.spdG = 40;
			unit1.lukG = 45;
			unit1.defG = 50;
			unit1.resG = 35;

			unit1.proficiency.Add("Sword");
			Item.instance.equipWeapon(unit1, "Sword", "IronSword");
			Item.instance.addItem(unit1, "Consumable", "Potion");

			Grid.instance.map[5][25].occupied = unit1;

			unit1.team = 0;
			unit1.allies.Add(0);
			unit1.index = 0;


			//5, 26
			PlayerUnit unit2 = ((GameObject)Instantiate(Grid.instance.unitPrefab, new Vector3(5 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 26 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<PlayerUnit>();
			unit2.gridPosition = new Vector2(5, 26);

			unit2.unitName = "Young Rebel";
			unit2.job = "Rogue";
			unit2.isHero = true;
			unit2.isThief = 1;
			//unit2.isFlying = true;
			unit2.lvl = 1;
			unit2.exp = 0;
			unit2.maxHP = 16;
			unit2.currentHP = 16;
			unit2.strength = 4;
			unit2.mag = 0;
			unit2.skl = 7;
			unit2.spd = 9;
			unit2.luk = 5;
			unit2.def = 4;
			unit2.res = 2;
			unit2.con = 5;
			unit2.mov = 5;

			unit2.hpG = 70;
			unit2.strG = 50;
			unit2.magG = 20;
			unit2.sklG = 60;
			unit2.spdG = 60;
			unit2.lukG = 50;
			unit2.defG = 20;
			unit2.resG = 25;

			unit2.proficiency.Add("Sword");
			Item.instance.equipWeapon(unit2, "Sword", "IronSword");
			Item.instance.addItem(unit2, "Key", "Lockpick");
			Item.instance.addItem(unit2, "Consumable", "Potion");

			Grid.instance.map[5][26].occupied = unit2;

			unit2.team = 0;
			unit2.allies.Add(0);
			unit2.index = 1;


			AIUnit ally1 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(14 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 15 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			ally1.gridPosition = new Vector2(14, 15);
			ally1.ai_id = 2;
			ally1.ai_id_priority.Add(2);
			ally1.canTalk.Add("Naive Prince", 0);
			ally1.canTalk.Add("Young Rebel", 0);

			ally1.unitName = "Kind Soul";
			ally1.job = "White Mage";
			ally1.isHero = true;
			ally1.lvl = 2;
			ally1.exp = 0;
			ally1.maxHP = 16;
			ally1.currentHP = 16;
			ally1.strength = 0;
			ally1.mag = 4;
			ally1.skl = 5;
			ally1.spd = 6;
			ally1.luk = 8;
			ally1.def = 2;
			ally1.res = 9;
			ally1.con = 4;
			ally1.mov = 5;

			ally1.hpG = 60;
			ally1.strG = 20;
			ally1.magG = 40;
			ally1.sklG = 35;
			ally1.spdG = 35;
			ally1.lukG = 70;
			ally1.defG = 10;
			ally1.resG = 80;

			ally1.proficiency.Add("Tome");
			ally1.proficiency.Add("Staff");
			Item.instance.addItem(ally1, "Staff", "Heal");
			Item.instance.addItem(ally1, "Consumable", "Potion");

			Grid.instance.map[14][15].occupied = ally1;

			ally1.team = 2;
			ally1.allies.Add(0);
			ally1.allies.Add(2);
			ally1.index = 0;



			AIUnit boss1 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(2 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 6 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			boss1.gridPosition = new Vector2(2, 6);
			boss1.ai_id = 2;
			boss1.ai_id_priority.Add(2);

			boss1.unitName = "Black Heart";
			boss1.job = "Black Mage";
			boss1.classBonusA = 0;
			boss1.classBonusB = 0;
			boss1.isBoss = 1;
			boss1.lvl = 2;
			boss1.exp = 0;
			boss1.maxHP = 22;
			boss1.currentHP = 1;
			boss1.strength = 4;
			boss1.mag = 6;
			boss1.skl = 4;
			boss1.spd = 8;
			boss1.luk = 3;
			boss1.def = 3;
			boss1.res = 7;
			boss1.con = 6;
			boss1.mov = 5;


			boss1.hpG = 50;
			boss1.strG = 20;
			boss1.magG = 80;
			boss1.sklG = 40;
			boss1.spdG = 55;
			boss1.lukG = 20;
			boss1.defG = 30;
			boss1.resG = 50;

			boss1.proficiency.Add("Tome");
			Item.instance.equipWeapon(boss1, "Tome", "Flux");

			Grid.instance.map[2][6].occupied = boss1;

			boss1.team = 1;
			boss1.allies.Add(1);
			boss1.index = 0;

			AIUnit enemy0 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(10 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 24 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			enemy0.gridPosition = new Vector2(10, 24);
			enemy0.ai_id = 0;
			enemy0.ai_id_priority.Add(0);

			enemy0.unitName = "Bandit";
			enemy0.job = "Bandit";
			enemy0.classBonusA = 0;
			enemy0.classBonusB = 0;
			enemy0.lvl = 1;
			enemy0.exp = 0;
			enemy0.maxHP = 16;
			enemy0.currentHP = 18;
			enemy0.strength = 5;
			enemy0.mag = 0;
			enemy0.skl = 16;
			enemy0.spd = 0;
			enemy0.luk = 0;
			enemy0.def = 1;
			enemy0.res = 0;
			enemy0.con = 12;
			enemy0.mov = 5;

			enemy0.proficiency.Add("Axe");
			Item.instance.equipWeapon(enemy0, "Axe", "BronzeAxe");

			Grid.instance.map[10][24].occupied = enemy0;


			enemy0.team = 1;
			enemy0.allies.Add(1);
			enemy0.index = 1;

			AIUnit enemy1 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(14 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 21 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			enemy1.gridPosition = new Vector2(14, 21);
			enemy1.ai_id = 2;
			enemy1.ai_id_priority.Add(2);

			enemy1.unitName = "Bandit";
			enemy1.job = "Bandit";
			enemy1.classBonusA = 0;
			enemy1.classBonusB = 0;
			enemy1.lvl = 1;
			enemy1.exp = 0;
			enemy1.maxHP = 18;
			enemy1.currentHP = 20;
			enemy1.strength = 5;
			enemy1.mag = 0;
			enemy1.skl = 0;
			enemy1.spd = 5;
			enemy1.luk = 0;
			enemy1.def = 1;
			enemy1.res = 0;
			enemy1.con = 12;
			enemy1.mov = 5;

			enemy1.proficiency.Add("Axe");
			Item.instance.equipWeapon(enemy1, "Axe", "IronAxe");

			Grid.instance.map[14][21].occupied = enemy1;

			enemy1.team = 1;
			enemy1.allies.Add(1);
			enemy1.index = 2;


			AIUnit enemy2 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(14 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 14 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			enemy2.gridPosition = new Vector2(14, 14);
			enemy2.ai_id = 1;
			enemy2.ai_id_priority.Add(1);

			enemy2.unitName = "Bandit";
			enemy2.job = "Bandit";
			enemy2.classBonusA = 0;
			enemy2.classBonusB = 0;
			enemy2.lvl = 1;
			enemy2.exp = 0;
			enemy2.maxHP = 20;
			enemy2.currentHP = 20;
			enemy2.strength = 0;
			enemy2.mag = 0;
			enemy2.skl = 1;
			enemy2.spd = 5;
			enemy2.luk = 0;
			enemy2.def = 3;
			enemy2.res = 0;
			enemy2.con = 12;
			enemy2.mov = 5;

			enemy2.proficiency.Add("Axe");
			Item.instance.equipWeapon(enemy2, "Axe", "BronzeAxe");

			Grid.instance.map[14][14].occupied = enemy2;

			enemy2.team = 1;
			enemy2.allies.Add(1);
			enemy2.index = 3;

			AIUnit enemy3 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(18 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 4 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			enemy3.gridPosition = new Vector2(18, 4);
			enemy3.ai_id = 4;

			enemy3.unitName = "Bandit Leader";
			enemy3.job = "Bandit";
			enemy3.ai_id = 4;
			enemy3.ai_id_priority.Add(4);
			enemy3.ai_id_priority.Add(0);

			enemy3.classBonusA = 0;
			enemy3.classBonusB = 0;
			enemy3.lvl = 3;
			enemy3.exp = 0;
			enemy3.maxHP = 24;
			enemy3.currentHP = 24;
			enemy3.strength = 7;
			enemy3.mag = 0;
			enemy3.skl = 3;
			enemy3.spd = 6;
			enemy3.luk = 2;
			enemy3.def = 5;
			enemy3.res = 2;
			enemy3.con = 12;
			enemy3.mov = 5;

			enemy3.proficiency.Add("Axe");
			Item.instance.equipWeapon(enemy3, "Axe", "IronAxe");

			Grid.instance.map[18][4].occupied = enemy3;

			enemy3.team = 1;
			enemy3.allies.Add(1);
			enemy3.index = 4;


			List<Unit> team0 = new List<Unit>();
			List<Unit> team1 = new List<Unit>();
			List<Unit> team2 = new List<Unit>();

			team0.Add(unit1);
			team0.Add(unit2);

			Grid.instance.units.Add(team0);

			team1.Add(boss1);
			team1.Add(enemy0);
			team1.Add(enemy1);
			team1.Add(enemy2);
			team1.Add(enemy3);

			Grid.instance.units.Add(team1);

			team2.Add(ally1);
			Grid.instance.units.Add(team2);


			Grid.instance.AITeams.Add(1);
			Grid.instance.AITeams.Add(2);


			foreach (List<Unit> team in Grid.instance.units)
			{
				foreach (Unit u in team)
				{
					u.mainCam = Grid.instance.mainCam;
				}
			}

			Grid.instance.resetCamera();
		}
		else if (mapName == "chapter1")
		{
			//Load Save
			GameControl.instance.Load();

			PlayerUnit unit1 = ((GameObject)Instantiate(Grid.instance.unitPrefab, new Vector3(2 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 17 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<PlayerUnit>();
			unit1.gridPosition = new Vector2(2, 17);

			unit1.unitName = GameControl.instance.npNameJob[0];
			unit1.job = GameControl.instance.npNameJob[1];
			unit1.isHero = GameControl.instance.npBoolData[0];
			unit1.isFlying = GameControl.instance.npBoolData[1];

			unit1.lvl = GameControl.instance.npIntData[0];
			unit1.exp = GameControl.instance.npIntData[1];
			unit1.maxHP = GameControl.instance.npIntData[2];
			if (GameControl.instance.npIntData[3] <= 0)
			{
				unit1.currentHP = 0;
			}
			else
			{
				unit1.currentHP = GameControl.instance.npIntData[2];
			}
			unit1.strength = GameControl.instance.npIntData[4];
			unit1.mag = GameControl.instance.npIntData[5];
			unit1.skl = GameControl.instance.npIntData[6];
			unit1.spd = GameControl.instance.npIntData[7];
			unit1.luk = GameControl.instance.npIntData[8];
			unit1.def = GameControl.instance.npIntData[9];
			unit1.res = GameControl.instance.npIntData[10];
			unit1.con = GameControl.instance.npIntData[11];
			unit1.mov = GameControl.instance.npIntData[12];

			unit1.hpG = GameControl.instance.npIntData[13];
			unit1.strG = GameControl.instance.npIntData[14];
			unit1.magG = GameControl.instance.npIntData[15];
			unit1.sklG = GameControl.instance.npIntData[16];
			unit1.spdG = GameControl.instance.npIntData[17];
			unit1.lukG = GameControl.instance.npIntData[18];
			unit1.defG = GameControl.instance.npIntData[19];
			unit1.resG = GameControl.instance.npIntData[20];

			unit1.proficiency = GameControl.instance.npProf;

			unit1.inventory = GameControl.instance.npInventory;
			unit1.equippedIndex = GameControl.instance.npIntData[21];
			unit1.weaponMt = GameControl.instance.npIntData[22];
			unit1.weaponPhysical = GameControl.instance.npBoolData[2];
			unit1.weaponAcc = GameControl.instance.npIntData[23];
			unit1.weaponCrit = GameControl.instance.npIntData[24];
			unit1.weaponWt = GameControl.instance.npIntData[25];
			unit1.weaponMinRange = GameControl.instance.npIntData[26];
			unit1.weaponMaxRange = GameControl.instance.npIntData[27];

			Grid.instance.map[2][17].occupied = unit1;

			unit1.team = 0;
			unit1.allies.Add(0);
			unit1.index = 0;


			PlayerUnit unit2 = ((GameObject)Instantiate(Grid.instance.unitPrefab, new Vector3(1 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 18 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<PlayerUnit>();
			unit2.gridPosition = new Vector2(1, 18);

			unit2.unitName = GameControl.instance.ksNameJob[0];
			unit2.job = GameControl.instance.ksNameJob[1];
			unit2.isHero = GameControl.instance.ksBoolData[0];
			unit2.isFlying = GameControl.instance.ksBoolData[1];

			unit2.lvl = GameControl.instance.ksIntData[0];
			unit2.exp = GameControl.instance.ksIntData[1];
			unit2.maxHP = GameControl.instance.ksIntData[2];
			if (GameControl.instance.ksIntData[3] <= 0)
			{
				unit2.currentHP = 0;
			}
			else
			{
				unit2.currentHP = GameControl.instance.ksIntData[2];
			}
			unit2.strength = GameControl.instance.ksIntData[4];
			unit2.mag = GameControl.instance.ksIntData[5];
			unit2.skl = GameControl.instance.ksIntData[6];
			unit2.spd = GameControl.instance.ksIntData[7];
			unit2.luk = GameControl.instance.ksIntData[8];
			unit2.def = GameControl.instance.ksIntData[9];
			unit2.res = GameControl.instance.ksIntData[10];
			unit2.con = GameControl.instance.ksIntData[11];
			unit2.mov = GameControl.instance.ksIntData[12];

			unit2.hpG = GameControl.instance.ksIntData[13];
			unit2.strG = GameControl.instance.ksIntData[14];
			unit2.magG = GameControl.instance.ksIntData[15];
			unit2.sklG = GameControl.instance.ksIntData[16];
			unit2.spdG = GameControl.instance.ksIntData[17];
			unit2.lukG = GameControl.instance.ksIntData[18];
			unit2.defG = GameControl.instance.ksIntData[19];
			unit2.resG = GameControl.instance.ksIntData[20];

			unit2.proficiency = GameControl.instance.ksProf;

			unit2.inventory = GameControl.instance.ksInventory;
			unit2.equippedIndex = GameControl.instance.ksIntData[21];
			unit2.weaponMt = GameControl.instance.ksIntData[22];
			unit2.weaponPhysical = GameControl.instance.ksBoolData[2];
			unit2.weaponAcc = GameControl.instance.ksIntData[23];
			unit2.weaponCrit = GameControl.instance.ksIntData[24];
			unit2.weaponWt = GameControl.instance.ksIntData[25];
			unit2.weaponMinRange = GameControl.instance.ksIntData[26];
			unit2.weaponMaxRange = GameControl.instance.ksIntData[27];

			Grid.instance.map[1][18].occupied = unit2;

			unit2.team = 0;
			unit2.allies.Add(0);
			unit2.index = 1;


			PlayerUnit unit3 = ((GameObject)Instantiate(Grid.instance.unitPrefab, new Vector3(2 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 18 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<PlayerUnit>();
			unit3.gridPosition = new Vector2(2, 18);

			unit3.unitName = GameControl.instance.yrNameJob[0];
			unit3.job = GameControl.instance.yrNameJob[1];
			unit3.isHero = GameControl.instance.yrBoolData[0];
			unit3.isFlying = GameControl.instance.yrBoolData[1];

			unit3.lvl = GameControl.instance.yrIntData[0];
			unit3.exp = GameControl.instance.yrIntData[1];
			unit3.maxHP = GameControl.instance.yrIntData[2];
			if (GameControl.instance.yrIntData[3] <= 0)
			{
				unit3.currentHP = 0;
			}
			else
			{
				unit3.currentHP = GameControl.instance.yrIntData[2];
			}
			unit3.strength = GameControl.instance.yrIntData[4];
			unit3.mag = GameControl.instance.yrIntData[5];
			unit3.skl = GameControl.instance.yrIntData[6];
			unit3.spd = GameControl.instance.yrIntData[7];
			unit3.luk = GameControl.instance.yrIntData[8];
			unit3.def = GameControl.instance.yrIntData[9];
			unit3.res = GameControl.instance.yrIntData[10];
			unit3.con = GameControl.instance.yrIntData[11];
			unit3.mov = GameControl.instance.yrIntData[12];

			unit3.hpG = GameControl.instance.yrIntData[13];
			unit3.strG = GameControl.instance.yrIntData[14];
			unit3.magG = GameControl.instance.yrIntData[15];
			unit3.sklG = GameControl.instance.yrIntData[16];
			unit3.spdG = GameControl.instance.yrIntData[17];
			unit3.lukG = GameControl.instance.yrIntData[18];
			unit3.defG = GameControl.instance.yrIntData[19];
			unit3.resG = GameControl.instance.yrIntData[20];

			unit3.proficiency = GameControl.instance.yrProf;

			unit3.inventory = GameControl.instance.yrInventory;
			unit3.equippedIndex = GameControl.instance.yrIntData[21];
			unit3.weaponMt = GameControl.instance.yrIntData[22];
			unit3.weaponPhysical = GameControl.instance.yrBoolData[2];
			unit3.weaponAcc = GameControl.instance.yrIntData[23];
			unit3.weaponCrit = GameControl.instance.yrIntData[24];
			unit3.weaponWt = GameControl.instance.yrIntData[25];
			unit3.weaponMinRange = GameControl.instance.yrIntData[26];
			unit3.weaponMaxRange = GameControl.instance.yrIntData[27];

			Grid.instance.map[2][18].occupied = unit3;

			unit3.team = 0;
			unit3.allies.Add(0);
			unit3.index = 2;


			PlayerUnit unit4 = ((GameObject)Instantiate(Grid.instance.unitPrefab, new Vector3(1 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 17 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<PlayerUnit>();
			unit4.gridPosition = new Vector2(1, 17);

			unit4.unitName = "Black Heart";
			unit4.job = "Black Mage";
			unit4.isHero = true;
			unit4.isFlying = false;

			unit4.lvl = 2;
			unit4.exp = 0;
			unit4.maxHP = 22;
			unit4.currentHP = 22;
			unit4.strength = 4;
			unit4.mag = 6;
			unit4.skl = 4;
			unit4.spd = 8;
			unit4.luk = 3;
			unit4.def = 3;
			unit4.res = 7;
			unit4.con = 6;
			unit4.mov = 5;


			unit4.hpG = 50;
			unit4.strG = 20;
			unit4.magG = 80;
			unit4.sklG = 40;
			unit4.spdG = 55;
			unit4.lukG = 20;
			unit4.defG = 30;
			unit4.resG = 50;

			unit4.proficiency.Add("Tome");
			Item.instance.equipWeapon(unit4, "Tome", "Flux");

			Grid.instance.map[1][17].occupied = unit4;

			unit4.team = 0;
			unit4.allies.Add(0);
			unit4.index = 3;


			AIUnit boss1 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(7 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 1 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			boss1.gridPosition = new Vector2(7, 1);
			boss1.ai_id = 2;
			boss1.ai_id_priority.Add(2);

			boss1.unitName = "Bandit Leader";
			boss1.job = "Bandit";
			boss1.classBonusA = 0;
			boss1.classBonusB = 0;
			boss1.isBoss = 1;
			boss1.lvl = 4;
			boss1.exp = 0;
			boss1.maxHP = 24;
			boss1.currentHP = 24;
			boss1.strength = 8;
			boss1.mag = 0;
			boss1.skl = 5;
			boss1.spd = 8;
			boss1.luk = 5;
			boss1.def = 6;
			boss1.res = 3;
			boss1.con = 12;
			boss1.mov = 5;

			boss1.proficiency.Add("Axe");
			Item.instance.equipWeapon(boss1, "Axe", "HandAxe");

			Grid.instance.map[2][6].occupied = boss1;

			boss1.team = 1;
			boss1.allies.Add(1);
			boss1.index = 0;


			AIUnit enemy1 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(13 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 4 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			enemy1.gridPosition = new Vector2(13, 4);
			enemy1.ai_id = 1;
			enemy1.ai_id_priority.Add(1);

			enemy1.unitName = "Bandit";
			enemy1.job = "Bandit";
			enemy1.classBonusA = 0;
			enemy1.classBonusB = 0;
			enemy1.isBoss = 0;
			enemy1.lvl = 3;
			enemy1.exp = 0;
			enemy1.maxHP = 24;
			enemy1.currentHP = 24;
			enemy1.strength = 6;
			enemy1.mag = 0;
			enemy1.skl = 1;
			enemy1.spd = 5;
			enemy1.luk = 0;
			enemy1.def = 4;
			enemy1.res = 0;
			enemy1.con = 12;
			enemy1.mov = 5;

			enemy1.proficiency.Add("Axe");
			Item.instance.equipWeapon(enemy1, "Axe", "SteelAxe");

			Grid.instance.map[13][4].occupied = enemy1;

			enemy1.team = 1;
			enemy1.allies.Add(1);
			enemy1.index = 1;


			AIUnit enemy2 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(2 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 3 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			enemy2.gridPosition = new Vector2(2, 3);
			enemy2.ai_id = 1;
			enemy2.ai_id_priority.Add(1);

			enemy2.unitName = "Bandit";
			enemy2.job = "Bandit";
			enemy2.classBonusA = 0;
			enemy2.classBonusB = 0;
			enemy2.isBoss = 0;
			enemy2.lvl = 3;
			enemy2.exp = 0;
			enemy2.maxHP = 24;
			enemy2.currentHP = 24;
			enemy2.strength = 6;
			enemy2.mag = 0;
			enemy2.skl = 1;
			enemy2.spd = 5;
			enemy2.luk = 0;
			enemy2.def = 4;
			enemy2.res = 0;
			enemy2.con = 12;
			enemy2.mov = 5;

			enemy2.proficiency.Add("Axe");
			Item.instance.equipWeapon(enemy2, "Axe", "SteelAxe");

			Grid.instance.map[2][3].occupied = enemy2;

			enemy2.team = 1;
			enemy2.allies.Add(1);
			enemy2.index = 2;


			AIUnit enemy3 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(8 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 3 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			enemy3.gridPosition = new Vector2(8, 3);
			enemy3.ai_id = 1;
			enemy3.ai_id_priority.Add(1);

			enemy3.unitName = "Bandit";
			enemy3.job = "Bandit";
			enemy3.classBonusA = 0;
			enemy3.classBonusB = 0;
			enemy3.isBoss = 0;
			enemy3.lvl = 3;
			enemy3.exp = 0;
			enemy3.maxHP = 24;
			enemy3.currentHP = 24;
			enemy3.strength = 6;
			enemy3.mag = 0;
			enemy3.skl = 1;
			enemy3.spd = 5;
			enemy3.luk = 0;
			enemy3.def = 4;
			enemy3.res = 0;
			enemy3.con = 12;
			enemy3.mov = 5;

			enemy3.proficiency.Add("Axe");
			Item.instance.equipWeapon(enemy3, "Axe", "IronAxe");

			Grid.instance.map[8][3].occupied = enemy3;

			enemy3.team = 1;
			enemy3.allies.Add(1);
			enemy3.index = 3;


			AIUnit enemy4 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(7 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 4 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			enemy4.gridPosition = new Vector2(7, 4);
			enemy4.ai_id = 1;
			enemy4.ai_id_priority.Add(1);

			enemy4.unitName = "Bandit";
			enemy4.job = "Bandit";
			enemy4.classBonusA = 0;
			enemy4.classBonusB = 0;
			enemy4.isBoss = 0;
			enemy4.lvl = 3;
			enemy4.exp = 0;
			enemy4.maxHP = 24;
			enemy4.currentHP = 24;
			enemy4.strength = 6;
			enemy4.mag = 0;
			enemy4.skl = 1;
			enemy4.spd = 5;
			enemy4.luk = 0;
			enemy4.def = 4;
			enemy4.res = 0;
			enemy4.con = 12;
			enemy4.mov = 5;

			enemy4.proficiency.Add("Axe");
			Item.instance.equipWeapon(enemy4, "Axe", "IronAxe");

			Grid.instance.map[7][4].occupied = enemy4;

			enemy4.team = 1;
			enemy4.allies.Add(1);
			enemy4.index = 4;


			AIUnit enemy5 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(6 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 3 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			enemy5.gridPosition = new Vector2(6, 3);
			enemy5.ai_id = 1;
			enemy5.ai_id_priority.Add(1);

			enemy5.unitName = "Bandit";
			enemy5.job = "Bandit";
			enemy5.classBonusA = 0;
			enemy5.classBonusB = 0;
			enemy5.isBoss = 0;
			enemy5.lvl = 3;
			enemy5.exp = 0;
			enemy5.maxHP = 24;
			enemy5.currentHP = 24;
			enemy5.strength = 6;
			enemy5.mag = 0;
			enemy5.skl = 1;
			enemy5.spd = 5;
			enemy5.luk = 0;
			enemy5.def = 4;
			enemy5.res = 0;
			enemy5.con = 12;
			enemy5.mov = 5;

			enemy5.proficiency.Add("Axe");
			Item.instance.equipWeapon(enemy5, "Axe", "IronAxe");

			Grid.instance.map[6][3].occupied = enemy5;

			enemy5.team = 1;
			enemy5.allies.Add(1);
			enemy5.index = 5;


			AIUnit enemy6 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(1 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 8 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			enemy6.gridPosition = new Vector2(1, 8);
			enemy6.ai_id = 1;
			enemy6.ai_id_priority.Add(1);

			enemy6.unitName = "Bandit";
			enemy6.job = "Bandit";
			enemy6.classBonusA = 0;
			enemy6.classBonusB = 0;
			enemy6.isBoss = 0;
			enemy6.lvl = 2;
			enemy6.exp = 0;
			enemy6.maxHP = 22;
			enemy6.currentHP = 22;
			enemy6.strength = 6;
			enemy6.mag = 0;
			enemy6.skl = 1;
			enemy6.spd = 5;
			enemy6.luk = 0;
			enemy6.def = 3;
			enemy6.res = 0;
			enemy6.con = 12;
			enemy6.mov = 5;

			enemy6.proficiency.Add("Axe");
			Item.instance.equipWeapon(enemy6, "Axe", "IronAxe");

			Grid.instance.map[1][8].occupied = enemy6;

			enemy6.team = 1;
			enemy6.allies.Add(1);
			enemy6.index = 6;


			AIUnit enemy7 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(3 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 8 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			enemy7.gridPosition = new Vector2(3, 8);
			enemy7.ai_id = 1;
			enemy7.ai_id_priority.Add(1);

			enemy7.unitName = "Bandit";
			enemy7.job = "Bandit";
			enemy7.classBonusA = 0;
			enemy7.classBonusB = 0;
			enemy7.isBoss = 0;
			enemy7.lvl = 2;
			enemy7.exp = 0;
			enemy7.maxHP = 22;
			enemy7.currentHP = 22;
			enemy7.strength = 6;
			enemy7.mag = 0;
			enemy7.skl = 1;
			enemy7.spd = 5;
			enemy7.luk = 0;
			enemy7.def = 3;
			enemy7.res = 0;
			enemy7.con = 12;
			enemy7.mov = 5;

			enemy7.proficiency.Add("Axe");
			Item.instance.equipWeapon(enemy7, "Axe", "IronAxe");

			Grid.instance.map[3][8].occupied = enemy7;

			enemy7.team = 1;
			enemy7.allies.Add(1);
			enemy7.index = 7;


			AIUnit enemy8 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(12 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 8 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			enemy8.gridPosition = new Vector2(12, 8);
			enemy8.ai_id = 1;
			enemy8.ai_id_priority.Add(1);

			enemy8.unitName = "Bandit";
			enemy8.job = "Bandit";
			enemy8.classBonusA = 0;
			enemy8.classBonusB = 0;
			enemy8.isBoss = 0;
			enemy8.lvl = 2;
			enemy8.exp = 0;
			enemy8.maxHP = 22;
			enemy8.currentHP = 22;
			enemy8.strength = 6;
			enemy8.mag = 0;
			enemy8.skl = 1;
			enemy8.spd = 5;
			enemy8.luk = 0;
			enemy8.def = 3;
			enemy8.res = 0;
			enemy8.con = 12;
			enemy8.mov = 5;

			enemy8.proficiency.Add("Axe");
			Item.instance.equipWeapon(enemy8, "Axe", "IronAxe");

			Grid.instance.map[12][8].occupied = enemy8;

			enemy8.team = 1;
			enemy8.allies.Add(1);
			enemy8.index = 8;


			AIUnit enemy9 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(13 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 11 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			enemy9.gridPosition = new Vector2(13, 11);
			enemy9.ai_id = 1;
			enemy9.ai_id_priority.Add(1);

			enemy9.unitName = "Bandit";
			enemy9.job = "Bandit";
			enemy9.classBonusA = 0;
			enemy9.classBonusB = 0;
			enemy9.isBoss = 0;
			enemy9.lvl = 2;
			enemy9.exp = 0;
			enemy9.maxHP = 20;
			enemy9.currentHP = 20;
			enemy9.strength = 5;
			enemy9.mag = 0;
			enemy9.skl = 1;
			enemy9.spd = 5;
			enemy9.luk = 0;
			enemy9.def = 3;
			enemy9.res = 0;
			enemy9.con = 12;
			enemy9.mov = 5;

			enemy9.proficiency.Add("Axe");
			Item.instance.equipWeapon(enemy9, "Axe", "IronAxe");

			Grid.instance.map[13][11].occupied = enemy9;

			enemy9.team = 1;
			enemy9.allies.Add(1);
			enemy9.index = 9;


			AIUnit enemy10 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(11 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 13 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			enemy10.gridPosition = new Vector2(11, 13);
			enemy10.ai_id = 1;
			enemy10.ai_id_priority.Add(1);

			enemy10.unitName = "Bandit";
			enemy10.job = "Bandit";
			enemy10.classBonusA = 0;
			enemy10.classBonusB = 0;
			enemy10.isBoss = 0;
			enemy10.lvl = 2;
			enemy10.exp = 0;
			enemy10.maxHP = 20;
			enemy10.currentHP = 20;
			enemy10.strength = 5;
			enemy10.mag = 0;
			enemy10.skl = 1;
			enemy10.spd = 5;
			enemy10.luk = 0;
			enemy10.def = 3;
			enemy10.res = 0;
			enemy10.con = 12;
			enemy10.mov = 5;

			enemy10.proficiency.Add("Axe");
			Item.instance.equipWeapon(enemy10, "Axe", "IronAxe");

			Grid.instance.map[11][13].occupied = enemy10;

			enemy10.team = 1;
			enemy10.allies.Add(1);
			enemy10.index = 10;


			AIUnit enemy11 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(12 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 12 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			enemy11.gridPosition = new Vector2(12, 12);
			enemy11.ai_id = 1;
			enemy11.ai_id_priority.Add(1);

			enemy11.unitName = "Bandit";
			enemy11.job = "Bandit";
			enemy11.classBonusA = 0;
			enemy11.classBonusB = 0;
			enemy11.isBoss = 0;
			enemy11.lvl = 2;
			enemy11.exp = 0;
			enemy11.maxHP = 20;
			enemy11.currentHP = 20;
			enemy11.strength = 5;
			enemy11.mag = 0;
			enemy11.skl = 1;
			enemy11.spd = 5;
			enemy11.luk = 0;
			enemy11.def = 3;
			enemy11.res = 0;
			enemy11.con = 12;
			enemy11.mov = 5;

			enemy11.proficiency.Add("Axe");
			Item.instance.equipWeapon(enemy11, "Axe", "IronAxe");

			Grid.instance.map[12][12].occupied = enemy11;

			enemy11.team = 1;
			enemy11.allies.Add(1);
			enemy11.index = 11;


			AIUnit enemy12 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(1 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 13 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			enemy12.gridPosition = new Vector2(1, 13);
			enemy12.ai_id = 1;
			enemy12.ai_id_priority.Add(1);

			enemy12.unitName = "Bandit";
			enemy12.job = "Bandit";
			enemy12.classBonusA = 0;
			enemy12.classBonusB = 0;
			enemy12.isBoss = 0;
			enemy12.lvl = 2;
			enemy12.exp = 0;
			enemy12.maxHP = 20;
			enemy12.currentHP = 20;
			enemy12.strength = 5;
			enemy12.mag = 0;
			enemy12.skl = 1;
			enemy12.spd = 5;
			enemy12.luk = 0;
			enemy12.def = 3;
			enemy12.res = 0;
			enemy12.con = 12;
			enemy12.mov = 5;

			enemy12.proficiency.Add("Axe");
			Item.instance.equipWeapon(enemy12, "Axe", "IronAxe");

			Grid.instance.map[1][13].occupied = enemy12;

			enemy12.team = 1;
			enemy12.allies.Add(1);
			enemy12.index = 12;


			AIUnit enemy13 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(3 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 11 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			enemy13.gridPosition = new Vector2(3, 11);
			enemy13.ai_id = 1;
			enemy13.ai_id_priority.Add(1);

			enemy13.unitName = "Bandit";
			enemy13.job = "Bandit";
			enemy13.classBonusA = 0;
			enemy13.classBonusB = 0;
			enemy13.isBoss = 0;
			enemy13.lvl = 2;
			enemy13.exp = 0;
			enemy13.maxHP = 20;
			enemy13.currentHP = 20;
			enemy13.strength = 5;
			enemy13.mag = 0;
			enemy13.skl = 1;
			enemy13.spd = 5;
			enemy13.luk = 0;
			enemy13.def = 3;
			enemy13.res = 0;
			enemy13.con = 12;
			enemy13.mov = 5;

			enemy13.proficiency.Add("Axe");
			Item.instance.equipWeapon(enemy13, "Axe", "IronAxe");

			Grid.instance.map[3][11].occupied = enemy13;

			enemy13.team = 1;
			enemy13.allies.Add(1);
			enemy13.index = 13;


			AIUnit enemy14 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(7 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 9 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			enemy14.gridPosition = new Vector2(7, 9);
			enemy14.ai_id = 1;
			enemy14.ai_id_priority.Add(1);

			enemy14.unitName = "Bandit";
			enemy14.job = "Bandit";
			enemy14.classBonusA = 0;
			enemy14.classBonusB = 0;
			enemy14.isBoss = 0;
			enemy14.lvl = 2;
			enemy14.exp = 0;
			enemy14.maxHP = 20;
			enemy14.currentHP = 20;
			enemy14.strength = 5;
			enemy14.mag = 0;
			enemy14.skl = 1;
			enemy14.spd = 5;
			enemy14.luk = 0;
			enemy14.def = 3;
			enemy14.res = 0;
			enemy14.con = 12;
			enemy14.mov = 5;

			enemy14.proficiency.Add("Axe");
			Item.instance.equipWeapon(enemy14, "Axe", "IronAxe");

			Grid.instance.map[7][9].occupied = enemy14;

			enemy14.team = 1;
			enemy14.allies.Add(1);
			enemy14.index = 13;

			List<Unit> team0 = new List<Unit>();
			List<Unit> team1 = new List<Unit>();

			team0.Add(unit1);
			team0.Add(unit2);
			team0.Add(unit3);
			team0.Add(unit4);

			team1.Add(boss1);
			team1.Add(enemy1);
			team1.Add(enemy2);
			team1.Add(enemy3);
			team1.Add(enemy4);
			team1.Add(enemy5);
			team1.Add(enemy6);
			team1.Add(enemy7);
			team1.Add(enemy8);
			team1.Add(enemy9);
			team1.Add(enemy10);
			team1.Add(enemy11);
			team1.Add(enemy12);
			team1.Add(enemy13);
			team1.Add(enemy14);

			Grid.instance.units.Add(team0);
			Grid.instance.units.Add(team1);

			Grid.instance.AITeams.Add(1);

			foreach (List<Unit> team in Grid.instance.units)
			{
				foreach (Unit u in team)
				{
					u.mainCam = Grid.instance.mainCam;
				}
			}

			Grid.instance.resetCamera();
		}
		else if (mapName == "chapter2")
		{
			//Load Save
			GameControl.instance.Load();

			PlayerUnit unit1 = ((GameObject)Instantiate(Grid.instance.unitPrefab, new Vector3(10 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 11 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<PlayerUnit>();
			unit1.gridPosition = new Vector2(10, 11);

			unit1.unitName = GameControl.instance.npNameJob[0];
			unit1.job = GameControl.instance.npNameJob[1];
			unit1.isHero = GameControl.instance.npBoolData[0];
			unit1.isFlying = GameControl.instance.npBoolData[1];

			unit1.lvl = GameControl.instance.npIntData[0];
			unit1.exp = GameControl.instance.npIntData[1];
			unit1.maxHP = GameControl.instance.npIntData[2];
			if (GameControl.instance.npIntData[3] <= 0)
			{
				unit1.currentHP = 0;
			}
			else
			{
				unit1.currentHP = GameControl.instance.npIntData[2];
			}
			unit1.strength = GameControl.instance.npIntData[4];
			unit1.mag = GameControl.instance.npIntData[5];
			unit1.skl = GameControl.instance.npIntData[6];
			unit1.spd = GameControl.instance.npIntData[7];
			unit1.luk = GameControl.instance.npIntData[8];
			unit1.def = GameControl.instance.npIntData[9];
			unit1.res = GameControl.instance.npIntData[10];
			unit1.con = GameControl.instance.npIntData[11];
			unit1.mov = GameControl.instance.npIntData[12];

			unit1.hpG = GameControl.instance.npIntData[13];
			unit1.strG = GameControl.instance.npIntData[14];
			unit1.magG = GameControl.instance.npIntData[15];
			unit1.sklG = GameControl.instance.npIntData[16];
			unit1.spdG = GameControl.instance.npIntData[17];
			unit1.lukG = GameControl.instance.npIntData[18];
			unit1.defG = GameControl.instance.npIntData[19];
			unit1.resG = GameControl.instance.npIntData[20];

			unit1.proficiency = GameControl.instance.npProf;

			unit1.inventory = GameControl.instance.npInventory;
			unit1.equippedIndex = GameControl.instance.npIntData[21];
			unit1.weaponMt = GameControl.instance.npIntData[22];
			unit1.weaponPhysical = GameControl.instance.npBoolData[2];
			unit1.weaponAcc = GameControl.instance.npIntData[23];
			unit1.weaponCrit = GameControl.instance.npIntData[24];
			unit1.weaponWt = GameControl.instance.npIntData[25];
			unit1.weaponMinRange = GameControl.instance.npIntData[26];
			unit1.weaponMaxRange = GameControl.instance.npIntData[27];

			Grid.instance.map[10][11].occupied = unit1;

			unit1.team = 0;
			unit1.allies.Add(0);
			unit1.index = 0;


			PlayerUnit unit2 = ((GameObject)Instantiate(Grid.instance.unitPrefab, new Vector3(11 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 11 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<PlayerUnit>();
			unit2.gridPosition = new Vector2(11, 11);

			unit2.unitName = GameControl.instance.ksNameJob[0];
			unit2.job = GameControl.instance.ksNameJob[1];
			unit2.isHero = GameControl.instance.ksBoolData[0];
			unit2.isFlying = GameControl.instance.ksBoolData[1];

			unit2.lvl = GameControl.instance.ksIntData[0];
			unit2.exp = GameControl.instance.ksIntData[1];
			unit2.maxHP = GameControl.instance.ksIntData[2];
			if (GameControl.instance.ksIntData[3] <= 0)
			{
				unit2.currentHP = 0;
			}
			else
			{
				unit2.currentHP = GameControl.instance.ksIntData[2];
			}
			unit2.strength = GameControl.instance.ksIntData[4];
			unit2.mag = GameControl.instance.ksIntData[5];
			unit2.skl = GameControl.instance.ksIntData[6];
			unit2.spd = GameControl.instance.ksIntData[7];
			unit2.luk = GameControl.instance.ksIntData[8];
			unit2.def = GameControl.instance.ksIntData[9];
			unit2.res = GameControl.instance.ksIntData[10];
			unit2.con = GameControl.instance.ksIntData[11];
			unit2.mov = GameControl.instance.ksIntData[12];

			unit2.hpG = GameControl.instance.ksIntData[13];
			unit2.strG = GameControl.instance.ksIntData[14];
			unit2.magG = GameControl.instance.ksIntData[15];
			unit2.sklG = GameControl.instance.ksIntData[16];
			unit2.spdG = GameControl.instance.ksIntData[17];
			unit2.lukG = GameControl.instance.ksIntData[18];
			unit2.defG = GameControl.instance.ksIntData[19];
			unit2.resG = GameControl.instance.ksIntData[20];

			unit2.proficiency = GameControl.instance.ksProf;

			unit2.inventory = GameControl.instance.ksInventory;
			unit2.equippedIndex = GameControl.instance.ksIntData[21];
			unit2.weaponMt = GameControl.instance.ksIntData[22];
			unit2.weaponPhysical = GameControl.instance.ksBoolData[2];
			unit2.weaponAcc = GameControl.instance.ksIntData[23];
			unit2.weaponCrit = GameControl.instance.ksIntData[24];
			unit2.weaponWt = GameControl.instance.ksIntData[25];
			unit2.weaponMinRange = GameControl.instance.ksIntData[26];
			unit2.weaponMaxRange = GameControl.instance.ksIntData[27];

			Grid.instance.map[11][11].occupied = unit2;

			unit2.team = 0;
			unit2.allies.Add(0);
			unit2.index = 1;


			PlayerUnit unit3 = ((GameObject)Instantiate(Grid.instance.unitPrefab, new Vector3(10 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 10 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<PlayerUnit>();
			unit3.gridPosition = new Vector2(10, 10);

			unit3.unitName = GameControl.instance.yrNameJob[0];
			unit3.job = GameControl.instance.yrNameJob[1];
			unit3.isHero = GameControl.instance.yrBoolData[0];
			unit3.isFlying = GameControl.instance.yrBoolData[1];

			unit3.lvl = GameControl.instance.yrIntData[0];
			unit3.exp = GameControl.instance.yrIntData[1];
			unit3.maxHP = GameControl.instance.yrIntData[2];
			if (GameControl.instance.yrIntData[3] <= 0)
			{
				unit3.currentHP = 0;
			}
			else
			{
				unit3.currentHP = GameControl.instance.yrIntData[2];
			}
			unit3.strength = GameControl.instance.yrIntData[4];
			unit3.mag = GameControl.instance.yrIntData[5];
			unit3.skl = GameControl.instance.yrIntData[6];
			unit3.spd = GameControl.instance.yrIntData[7];
			unit3.luk = GameControl.instance.yrIntData[8];
			unit3.def = GameControl.instance.yrIntData[9];
			unit3.res = GameControl.instance.yrIntData[10];
			unit3.con = GameControl.instance.yrIntData[11];
			unit3.mov = GameControl.instance.yrIntData[12];

			unit3.hpG = GameControl.instance.yrIntData[13];
			unit3.strG = GameControl.instance.yrIntData[14];
			unit3.magG = GameControl.instance.yrIntData[15];
			unit3.sklG = GameControl.instance.yrIntData[16];
			unit3.spdG = GameControl.instance.yrIntData[17];
			unit3.lukG = GameControl.instance.yrIntData[18];
			unit3.defG = GameControl.instance.yrIntData[19];
			unit3.resG = GameControl.instance.yrIntData[20];

			unit3.proficiency = GameControl.instance.yrProf;

			unit3.inventory = GameControl.instance.yrInventory;
			unit3.equippedIndex = GameControl.instance.yrIntData[21];
			unit3.weaponMt = GameControl.instance.yrIntData[22];
			unit3.weaponPhysical = GameControl.instance.yrBoolData[2];
			unit3.weaponAcc = GameControl.instance.yrIntData[23];
			unit3.weaponCrit = GameControl.instance.yrIntData[24];
			unit3.weaponWt = GameControl.instance.yrIntData[25];
			unit3.weaponMinRange = GameControl.instance.yrIntData[26];
			unit3.weaponMaxRange = GameControl.instance.yrIntData[27];

			Grid.instance.map[10][10].occupied = unit3;

			unit3.team = 0;
			unit3.allies.Add(0);
			unit3.index = 2;


			PlayerUnit unit4 = ((GameObject)Instantiate(Grid.instance.unitPrefab, new Vector3(4 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 17 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<PlayerUnit>();
			unit4.gridPosition = new Vector2(4, 17);

			unit4.unitName = GameControl.instance.bhNameJob[0];
			unit4.job = GameControl.instance.bhNameJob[1];
			unit4.isHero = GameControl.instance.bhBoolData[0];
			unit4.isFlying = GameControl.instance.bhBoolData[1];

			unit4.lvl = GameControl.instance.bhIntData[0];
			unit4.exp = GameControl.instance.bhIntData[1];
			unit4.maxHP = GameControl.instance.bhIntData[2];
			if (GameControl.instance.bhIntData[3] <= 0)
			{
				unit4.currentHP = 0;
			}
			else
			{
				unit4.currentHP = GameControl.instance.bhIntData[2];
			}
			unit4.strength = GameControl.instance.bhIntData[4];
			unit4.mag = GameControl.instance.bhIntData[5];
			unit4.skl = GameControl.instance.bhIntData[6];
			unit4.spd = GameControl.instance.bhIntData[7];
			unit4.luk = GameControl.instance.bhIntData[8];
			unit4.def = GameControl.instance.bhIntData[9];
			unit4.res = GameControl.instance.bhIntData[10];
			unit4.con = GameControl.instance.bhIntData[11];
			unit4.mov = GameControl.instance.bhIntData[12];

			unit4.hpG = GameControl.instance.bhIntData[13];
			unit4.strG = GameControl.instance.bhIntData[14];
			unit4.magG = GameControl.instance.bhIntData[15];
			unit4.sklG = GameControl.instance.bhIntData[16];
			unit4.spdG = GameControl.instance.bhIntData[17];
			unit4.lukG = GameControl.instance.bhIntData[18];
			unit4.defG = GameControl.instance.bhIntData[19];
			unit4.resG = GameControl.instance.bhIntData[20];

			unit4.proficiency = GameControl.instance.bhProf;

			unit4.inventory = GameControl.instance.bhInventory;
			unit4.equippedIndex = GameControl.instance.bhIntData[21];
			unit4.weaponMt = GameControl.instance.bhIntData[22];
			unit4.weaponPhysical = GameControl.instance.bhBoolData[2];
			unit4.weaponAcc = GameControl.instance.bhIntData[23];
			unit4.weaponCrit = GameControl.instance.bhIntData[24];
			unit4.weaponWt = GameControl.instance.bhIntData[25];
			unit4.weaponMinRange = GameControl.instance.bhIntData[26];
			unit4.weaponMaxRange = GameControl.instance.bhIntData[27];

			Grid.instance.map[4][17].occupied = unit4;

			unit4.team = 0;
			unit4.allies.Add(0);
			unit4.index = 3;


			AIUnit boss1 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(15 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 20 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			boss1.gridPosition = new Vector2(15, 20);
			boss1.ai_id = 2;
			boss1.ai_id_priority.Add(2);

			boss1.unitName = "Commander";
			boss1.job = "Armour Knight";
			boss1.classBonusA = 0;
			boss1.classBonusB = 0;
			boss1.isBoss = 1;
			boss1.lvl = 9;
			boss1.exp = 0;
			boss1.maxHP = 30;
			boss1.currentHP = 30;
			boss1.strength = 11;
			boss1.mag = 0;
			boss1.skl = 6;
			boss1.spd = 4;
			boss1.luk = 3;
			boss1.def = 13;
			boss1.res = 4;
			boss1.con = 13;
			boss1.mov = 4;

			boss1.proficiency.Add("Lance");
			Item.instance.equipWeapon(boss1, "Lance", "Javelin");

			Grid.instance.map[15][20].occupied = boss1;

			boss1.team = 1;
			boss1.allies.Add(1);
			boss1.index = 0;


			AIUnit enemy1 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(1 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 17 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			enemy1.gridPosition = new Vector2(1, 17);
			enemy1.ai_id = 1;
			enemy1.ai_id_priority.Add(2);

			enemy1.unitName = "Soldier";
			enemy1.job = "Armour Knight";
			enemy1.classBonusA = 0;
			enemy1.classBonusB = 0;
			enemy1.isBoss = 0;
			enemy1.lvl = 5;
			enemy1.exp = 0;
			enemy1.maxHP = 26;
			enemy1.currentHP = 26;
			enemy1.strength = 7;
			enemy1.mag = 0;
			enemy1.skl = 5;
			enemy1.spd = 3;
			enemy1.luk = 3;
			enemy1.def = 9;
			enemy1.res = 4;
			enemy1.con = 13;
			enemy1.mov = 4;

			enemy1.proficiency.Add("Lance");
			Item.instance.equipWeapon(enemy1, "Lance", "IronLance");

			Grid.instance.map[1][17].occupied = enemy1;

			enemy1.team = 1;
			enemy1.allies.Add(1);
			enemy1.index = 1;


			AIUnit enemy2 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(2 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 17 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			enemy2.gridPosition = new Vector2(2, 17);
			enemy2.ai_id = 1;
			enemy2.ai_id_priority.Add(2);

			enemy2.unitName = "Soldier";
			enemy2.job = "Armour Knight";
			enemy2.classBonusA = 0;
			enemy2.classBonusB = 0;
			enemy2.isBoss = 0;
			enemy2.lvl = 5;
			enemy2.exp = 0;
			enemy2.maxHP = 26;
			enemy2.currentHP = 26;
			enemy2.strength = 7;
			enemy2.mag = 0;
			enemy2.skl = 5;
			enemy2.spd = 3;
			enemy2.luk = 3;
			enemy2.def = 9;
			enemy2.res = 4;
			enemy2.con = 13;
			enemy2.mov = 4;

			enemy2.proficiency.Add("Lance");
			Item.instance.equipWeapon(enemy2, "Lance", "IronLance");

			Grid.instance.map[2][17].occupied = enemy2;

			enemy2.team = 1;
			enemy2.allies.Add(1);
			enemy2.index = 2;


			AIUnit enemy3 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(9 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 21 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			enemy3.gridPosition = new Vector2(9, 21);
			enemy3.ai_id = 1;
			enemy3.ai_id_priority.Add(2);

			enemy3.unitName = "Soldier";
			enemy3.job = "Armour Knight";
			enemy3.classBonusA = 0;
			enemy3.classBonusB = 0;
			enemy3.isBoss = 0;
			enemy3.lvl = 7;
			enemy3.exp = 0;
			enemy3.maxHP = 28;
			enemy3.currentHP = 28;
			enemy3.strength = 9;
			enemy3.mag = 0;
			enemy3.skl = 6;
			enemy3.spd = 3;
			enemy3.luk = 3;
			enemy3.def = 11;
			enemy3.res = 4;
			enemy3.con = 13;
			enemy3.mov = 4;

			enemy3.proficiency.Add("Lance");
			Item.instance.equipWeapon(enemy3, "Lance", "SteelLance");

			Grid.instance.map[9][21].occupied = enemy3;

			enemy3.team = 1;
			enemy3.allies.Add(1);
			enemy3.index = 3;


			AIUnit enemy4 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(9 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 20 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			enemy4.gridPosition = new Vector2(9, 20);
			enemy4.ai_id = 1;
			enemy4.ai_id_priority.Add(2);

			enemy4.unitName = "Soldier";
			enemy4.job = "Armour Knight";
			enemy4.classBonusA = 0;
			enemy4.classBonusB = 0;
			enemy4.isBoss = 0;
			enemy4.lvl = 7;
			enemy4.exp = 0;
			enemy4.maxHP = 28;
			enemy4.currentHP = 28;
			enemy4.strength = 9;
			enemy4.mag = 0;
			enemy4.skl = 6;
			enemy4.spd = 3;
			enemy4.luk = 3;
			enemy4.def = 11;
			enemy4.res = 4;
			enemy4.con = 13;
			enemy4.mov = 4;

			enemy4.proficiency.Add("Lance");
			Item.instance.equipWeapon(enemy4, "Lance", "SteelLance");

			Grid.instance.map[9][20].occupied = enemy4;

			enemy4.team = 1;
			enemy4.allies.Add(1);
			enemy4.index = 4;


			AIUnit enemy5 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(0 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 21 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			enemy5.gridPosition = new Vector2(0, 21);
			enemy5.ai_id = 1;
			enemy5.ai_id_priority.Add(2);

			enemy5.unitName = "Soldier";
			enemy5.job = "Archer";
			enemy5.classBonusA = 0;
			enemy5.classBonusB = 0;
			enemy5.isBoss = 0;
			enemy5.lvl = 5;
			enemy5.exp = 0;
			enemy5.maxHP = 20;
			enemy5.currentHP = 20;
			enemy5.strength = 6;
			enemy5.mag = 0;
			enemy5.skl = 8;
			enemy5.spd = 6;
			enemy5.luk = 5;
			enemy5.def = 5;
			enemy5.res = 4;
			enemy5.con = 5;
			enemy5.mov = 5;

			enemy5.proficiency.Add("Bow");
			Item.instance.equipWeapon(enemy5, "Bow", "IronBow");

			Grid.instance.map[0][21].occupied = enemy5;

			enemy5.team = 1;
			enemy5.allies.Add(1);
			enemy5.index = 5;


			AIUnit enemy6 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(2 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 22 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			enemy6.gridPosition = new Vector2(2, 22);
			enemy6.ai_id = 1;
			enemy6.ai_id_priority.Add(2);

			enemy6.unitName = "Soldier";
			enemy6.job = "Archer";
			enemy6.classBonusA = 0;
			enemy6.classBonusB = 0;
			enemy6.isBoss = 0;
			enemy6.lvl = 5;
			enemy6.exp = 0;
			enemy6.maxHP = 20;
			enemy6.currentHP = 20;
			enemy6.strength = 6;
			enemy6.mag = 0;
			enemy6.skl = 8;
			enemy6.spd = 6;
			enemy6.luk = 5;
			enemy6.def = 5;
			enemy6.res = 4;
			enemy6.con = 5;
			enemy6.mov = 5;

			enemy6.proficiency.Add("Bow");
			Item.instance.equipWeapon(enemy6, "Bow", "IronBow");

			Grid.instance.map[2][22].occupied = enemy6;

			enemy6.team = 1;
			enemy6.allies.Add(1);
			enemy6.index = 6;


			AIUnit enemy7 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(0 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 18 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			enemy7.gridPosition = new Vector2(0, 18);
			enemy7.ai_id = 1;
			enemy7.ai_id_priority.Add(2);

			enemy7.unitName = "Soldier";
			enemy7.job = "Mage";
			enemy7.classBonusA = 0;
			enemy7.classBonusB = 0;
			enemy7.isBoss = 0;
			enemy7.lvl = 5;
			enemy7.exp = 0;
			enemy7.maxHP = 18;
			enemy7.currentHP = 18;
			enemy7.strength = 0;
			enemy7.mag = 7;
			enemy7.skl = 6;
			enemy7.spd = 6;
			enemy7.luk = 5;
			enemy7.def = 3;
			enemy7.res = 7;
			enemy7.con = 3;
			enemy7.mov = 5;

			enemy7.proficiency.Add("Tome");
			Item.instance.equipWeapon(enemy7, "Tome", "Fire");

			Grid.instance.map[0][18].occupied = enemy7;

			enemy7.team = 1;
			enemy7.allies.Add(1);
			enemy7.index = 7;


			AIUnit enemy8 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(7 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 22 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			enemy8.gridPosition = new Vector2(7, 22);
			enemy8.ai_id = 1;
			enemy8.ai_id_priority.Add(2);

			enemy8.unitName = "Soldier";
			enemy8.job = "Mage";
			enemy8.classBonusA = 0;
			enemy8.classBonusB = 0;
			enemy8.isBoss = 0;
			enemy8.lvl = 7;
			enemy8.exp = 0;
			enemy8.maxHP = 22;
			enemy8.currentHP = 22;
			enemy8.strength = 0;
			enemy8.mag = 8;
			enemy8.skl = 6;
			enemy8.spd = 7;
			enemy8.luk = 5;
			enemy8.def = 4;
			enemy8.res = 8;
			enemy8.con = 3;
			enemy8.mov = 5;

			enemy8.proficiency.Add("Tome");
			Item.instance.equipWeapon(enemy8, "Tome", "Thunder");

			Grid.instance.map[7][22].occupied = enemy8;

			enemy8.team = 1;
			enemy8.allies.Add(1);
			enemy8.index = 8;


			AIUnit enemy9 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(4 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 24 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			enemy9.gridPosition = new Vector2(4, 24);
			enemy9.ai_id = 1;
			enemy9.ai_id_priority.Add(2);

			enemy9.unitName = "Soldier";
			enemy9.job = "Cavalry";
			enemy9.classBonusA = 0;
			enemy9.classBonusB = 0;
			enemy9.isBoss = 0;
			enemy9.lvl = 7;
			enemy9.exp = 0;
			enemy9.maxHP = 26;
			enemy9.currentHP = 26;
			enemy9.strength = 8;
			enemy9.mag = 0;
			enemy9.skl = 6;
			enemy9.spd = 6;
			enemy9.luk = 6;
			enemy9.def = 6;
			enemy9.res = 4;
			enemy9.con = 9;
			enemy9.mov = 7;

			enemy9.proficiency.Add("Sword");
			Item.instance.equipWeapon(enemy9, "Sword", "IronSword");

			Grid.instance.map[4][24].occupied = enemy9;

			enemy9.team = 1;
			enemy9.allies.Add(1);
			enemy9.index = 9;


			AIUnit enemy10 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(5 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 24 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			enemy10.gridPosition = new Vector2(5, 24);
			enemy10.ai_id = 1;
			enemy10.ai_id_priority.Add(2);

			enemy10.unitName = "Soldier";
			enemy10.job = "Cavalry";
			enemy10.classBonusA = 0;
			enemy10.classBonusB = 0;
			enemy10.isBoss = 0;
			enemy10.lvl = 7;
			enemy10.exp = 0;
			enemy10.maxHP = 26;
			enemy10.currentHP = 26;
			enemy10.strength = 8;
			enemy10.mag = 0;
			enemy10.skl = 6;
			enemy10.spd = 6;
			enemy10.luk = 6;
			enemy10.def = 6;
			enemy10.res = 4;
			enemy10.con = 9;
			enemy10.mov = 7;

			enemy10.proficiency.Add("Sword");
			Item.instance.equipWeapon(enemy10, "Sword", "IronSword");

			Grid.instance.map[5][24].occupied = enemy10;

			enemy10.team = 1;
			enemy10.allies.Add(1);
			enemy10.index = 10;


			AIUnit enemy11 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(15 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 21 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			enemy11.gridPosition = new Vector2(15, 21);
			enemy11.ai_id = 1;
			enemy11.ai_id_priority.Add(2);

			enemy11.unitName = "Soldier";
			enemy11.job = "Dragoon";
			enemy11.classBonusA = 0;
			enemy11.classBonusB = 0;
			enemy11.isBoss = 0;
			enemy11.isFlying = true;
			enemy11.lvl = 8;
			enemy11.exp = 0;
			enemy11.maxHP = 30;
			enemy11.currentHP = 30;
			enemy11.strength = 11;
			enemy11.mag = 0;
			enemy11.skl = 5;
			enemy11.spd = 5;
			enemy11.luk = 5;
			enemy11.def = 9;
			enemy11.res = 2;
			enemy11.con = 9;
			enemy11.mov = 7;

			enemy11.proficiency.Add("Axe");
			Item.instance.equipWeapon(enemy11, "Axe", "HandAxe");

			Grid.instance.map[15][21].occupied = enemy11;

			enemy11.team = 1;
			enemy11.allies.Add(1);
			enemy11.index = 11;


			AIUnit enemy12 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(14 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 22 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			enemy12.gridPosition = new Vector2(14, 22);
			enemy12.ai_id = 1;
			enemy12.ai_id_priority.Add(2);

			enemy12.unitName = "Soldier";
			enemy12.job = "Dragoon";
			enemy12.classBonusA = 0;
			enemy12.classBonusB = 0;
			enemy12.isBoss = 0;
			enemy12.isFlying = true;
			enemy12.lvl = 7;
			enemy12.exp = 0;
			enemy12.maxHP = 28;
			enemy12.currentHP = 28;
			enemy12.strength = 10;
			enemy12.mag = 0;
			enemy12.skl = 5;
			enemy12.spd = 5;
			enemy12.luk = 5;
			enemy12.def = 8;
			enemy12.res = 2;
			enemy12.con = 9;
			enemy12.mov = 7;

			enemy12.proficiency.Add("Axe");
			Item.instance.equipWeapon(enemy12, "Axe", "IronAxe");

			Grid.instance.map[14][22].occupied = enemy12;

			enemy12.team = 1;
			enemy12.allies.Add(1);
			enemy12.index = 12;


			AIUnit enemy13 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(14 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 19 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			enemy13.gridPosition = new Vector2(14, 19);
			enemy13.ai_id = 1;
			enemy13.ai_id_priority.Add(2);

			enemy13.unitName = "Soldier";
			enemy13.job = "Dragoon";
			enemy13.classBonusA = 0;
			enemy13.classBonusB = 0;
			enemy13.isBoss = 0;
			enemy13.isFlying = true;
			enemy13.lvl = 7;
			enemy13.exp = 0;
			enemy13.maxHP = 28;
			enemy13.currentHP = 28;
			enemy13.strength = 10;
			enemy13.mag = 0;
			enemy13.skl = 5;
			enemy13.spd = 5;
			enemy13.luk = 5;
			enemy13.def = 8;
			enemy13.res = 2;
			enemy13.con = 9;
			enemy13.mov = 7;

			enemy13.proficiency.Add("Axe");
			Item.instance.equipWeapon(enemy13, "Axe", "IronAxe");

			Grid.instance.map[14][19].occupied = enemy13;

			enemy13.team = 1;
			enemy13.allies.Add(1);
			enemy13.index = 13;


			AIUnit boss2 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(8 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 0 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			boss2.gridPosition = new Vector2(8, 0);
			boss2.ai_id = 1;
			boss2.ai_id_priority.Add(2);

			boss2.unitName = "Rebel Leader";
			boss2.job = "Sellsword";
			boss2.classBonusA = 0;
			boss2.classBonusB = 0;
			boss2.isBoss = 1;
			boss2.lvl = 9;
			boss2.exp = 0;
			boss2.maxHP = 28;
			boss2.currentHP = 28;
			boss2.strength = 8;
			boss2.mag = 0;
			boss2.skl = 8;
			boss2.spd = 9;
			boss2.luk = 4;
			boss2.def = 6;
			boss2.res = 5;
			boss2.con = 9;
			boss2.mov = 5;

			boss2.proficiency.Add("Sword");
			Item.instance.equipWeapon(boss2, "Sword", "SteelSword");

			Grid.instance.map[8][0].occupied = boss2;

			boss2.team = 2;
			boss2.allies.Add(2);
			boss2.index = 0;


			AIUnit e1 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(8 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 2 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			e1.gridPosition = new Vector2(8, 2);
			e1.ai_id = 1;
			e1.ai_id_priority.Add(2);

			e1.unitName = "Rebel";
			e1.job = "Sellsword";
			e1.classBonusA = 0;
			e1.classBonusB = 0;
			e1.isBoss = 0;
			e1.lvl = 8;
			e1.exp = 0;
			e1.maxHP = 28;
			e1.currentHP = 28;
			e1.strength = 7;
			e1.mag = 0;
			e1.skl = 8;
			e1.spd = 7;
			e1.luk = 4;
			e1.def = 6;
			e1.res = 5;
			e1.con = 8;
			e1.mov = 5;

			e1.proficiency.Add("Sword");
			Item.instance.equipWeapon(e1, "Sword", "SteelSword");

			Grid.instance.map[8][2].occupied = e1;

			e1.team = 2;
			e1.allies.Add(2);
			e1.index = 1;


			AIUnit e2 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(10 - Mathf.Floor(Grid.instance.tilesPerCol / 2),6 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			e2.gridPosition = new Vector2(10, 6);
			e2.ai_id = 1;
			e2.ai_id_priority.Add(2);

			e2.unitName = "Rebel";
			e2.job = "Archer";
			e2.classBonusA = 0;
			e2.classBonusB = 0;
			e2.isBoss = 0;
			e2.lvl = 6;
			e2.exp = 0;
			e2.maxHP = 24;
			e2.currentHP = 24;
			e2.strength = 7;
			e2.mag = 0;
			e2.skl = 8;
			e2.spd = 5;
			e2.luk = 4;
			e2.def = 5;
			e2.res = 4;
			e2.con = 5;
			e2.mov = 5;

			e2.proficiency.Add("Bow");
			Item.instance.equipWeapon(e2, "Bow", "IronBow");

			Grid.instance.map[10][6].occupied = e2;

			e2.team = 2;
			e2.allies.Add(2);
			e2.index = 2;


			AIUnit e3 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(5 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 4 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			e3.gridPosition = new Vector2(5, 4);
			e3.ai_id = 1;
			e3.ai_id_priority.Add(2);

			e3.unitName = "Rebel";
			e3.job = "Archer";
			e3.classBonusA = 0;
			e3.classBonusB = 0;
			e3.isBoss = 0;
			e3.lvl = 6;
			e3.exp = 0;
			e3.maxHP = 24;
			e3.currentHP = 24;
			e3.strength = 7;
			e3.mag = 0;
			e3.skl = 8;
			e3.spd = 5;
			e3.luk = 4;
			e3.def = 5;
			e3.res = 4;
			e3.con = 5;
			e3.mov = 5;

			e3.proficiency.Add("Bow");
			Item.instance.equipWeapon(e3, "Bow", "IronBow");

			Grid.instance.map[5][4].occupied = e3;

			e3.team = 2;
			e3.allies.Add(2);
			e3.index = 3;


			AIUnit e4 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(7 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 0 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			e4.gridPosition = new Vector2(7, 0);
			e4.ai_id = 1;
			e4.ai_id_priority.Add(2);

			e4.unitName = "Rebel";
			e4.job = "Fighter";
			e4.classBonusA = 0;
			e4.classBonusB = 0;
			e4.isBoss = 0;
			e4.lvl = 8;
			e4.exp = 0;
			e4.maxHP = 34;
			e4.currentHP = 34;
			e4.strength = 11;
			e4.mag = 0;
			e4.skl = 5;
			e4.spd = 5;
			e4.luk = 3;
			e4.def = 6;
			e4.res = 4;
			e4.con = 11;
			e4.mov = 5;

			e4.proficiency.Add("Axe");
			Item.instance.equipWeapon(e4, "Axe", "SteelAxe");

			Grid.instance.map[7][0].occupied = e4;

			e4.team = 2;
			e4.allies.Add(2);
			e4.index = 4;


			AIUnit e5 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(9 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 0 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			e5.gridPosition = new Vector2(9, 0);
			e5.ai_id = 1;
			e5.ai_id_priority.Add(2);

			e5.unitName = "Rebel";
			e5.job = "Fighter";
			e5.classBonusA = 0;
			e5.classBonusB = 0;
			e5.isBoss = 0;
			e5.lvl = 8;
			e5.exp = 0;
			e5.maxHP = 34;
			e5.currentHP = 34;
			e5.strength = 11;
			e5.mag = 0;
			e5.skl = 5;
			e5.spd = 5;
			e5.luk = 3;
			e5.def = 6;
			e5.res = 4;
			e5.con = 11;
			e5.mov = 5;

			e5.proficiency.Add("Axe");
			Item.instance.equipWeapon(e5, "Axe", "SteelAxe");

			Grid.instance.map[9][0].occupied = e5;

			e5.team = 2;
			e5.allies.Add(2);
			e5.index = 5;


			AIUnit e6 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(0 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 5 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			e6.gridPosition = new Vector2(0, 5);
			e6.ai_id = 1;
			e6.ai_id_priority.Add(2);

			e6.unitName = "Rebel";
			e6.job = "Fighter";
			e6.classBonusA = 0;
			e6.classBonusB = 0;
			e6.isBoss = 0;
			e6.lvl = 6;
			e6.exp = 0;
			e6.maxHP = 30;
			e6.currentHP = 30;
			e6.strength = 9;
			e6.mag = 0;
			e6.skl = 5;
			e6.spd = 4;
			e6.luk = 3;
			e6.def = 5;
			e6.res = 4;
			e6.con = 11;
			e6.mov = 5;

			e6.proficiency.Add("Axe");
			Item.instance.equipWeapon(e6, "Axe", "IronAxe");

			Grid.instance.map[0][5].occupied = e6;

			e6.team = 2;
			e6.allies.Add(2);
			e6.index = 6;


			AIUnit e7 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(6 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 5 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			e7.gridPosition = new Vector2(6, 5);
			e7.ai_id = 1;
			e7.ai_id_priority.Add(2);

			e7.unitName = "Rebel";
			e7.job = "Fighter";
			e7.classBonusA = 0;
			e7.classBonusB = 0;
			e7.isBoss = 0;
			e7.lvl = 6;
			e7.exp = 0;
			e7.maxHP = 30;
			e7.currentHP = 30;
			e7.strength = 9;
			e7.mag = 0;
			e7.skl = 5;
			e7.spd = 4;
			e7.luk = 3;
			e7.def = 5;
			e7.res = 4;
			e7.con = 11;
			e7.mov = 5;

			e7.proficiency.Add("Axe");
			Item.instance.equipWeapon(e7, "Axe", "IronAxe");

			Grid.instance.map[6][5].occupied = e7;

			e7.team = 2;
			e7.allies.Add(2);
			e7.index = 7;


			AIUnit e8 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(12 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 3 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			e8.gridPosition = new Vector2(12, 3);
			e8.ai_id = 1;
			e8.ai_id_priority.Add(2);

			e8.unitName = "Rebel";
			e8.job = "Fighter";
			e8.classBonusA = 0;
			e8.classBonusB = 0;
			e8.isBoss = 0;
			e8.lvl = 6;
			e8.exp = 0;
			e8.maxHP = 30;
			e8.currentHP = 30;
			e8.strength = 9;
			e8.mag = 0;
			e8.skl = 5;
			e8.spd = 4;
			e8.luk = 3;
			e8.def = 5;
			e8.res = 4;
			e8.con = 11;
			e8.mov = 5;

			e8.proficiency.Add("Axe");
			Item.instance.equipWeapon(e8, "Axe", "IronAxe");

			Grid.instance.map[12][3].occupied = e8;

			e8.team = 2;
			e8.allies.Add(2);
			e8.index = 8;


			AIUnit e9 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(13 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 2 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			e9.gridPosition = new Vector2(13, 2);
			e9.ai_id = 1;
			e9.ai_id_priority.Add(2);

			e9.unitName = "Rebel";
			e9.job = "Fighter";
			e9.classBonusA = 0;
			e9.classBonusB = 0;
			e9.isBoss = 0;
			e9.lvl = 6;
			e9.exp = 0;
			e9.maxHP = 30;
			e9.currentHP = 30;
			e9.strength = 9;
			e9.mag = 0;
			e9.skl = 5;
			e9.spd = 4;
			e9.luk = 3;
			e9.def = 5;
			e9.res = 4;
			e9.con = 11;
			e9.mov = 5;

			e9.proficiency.Add("Axe");
			Item.instance.equipWeapon(e9, "Axe", "IronAxe");

			Grid.instance.map[13][2].occupied = e9;

			e9.team = 2;
			e9.allies.Add(2);
			e9.index = 9;


			AIUnit e10 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(2 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 0 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			e10.gridPosition = new Vector2(2, 0);
			e10.ai_id = 1;
			e10.ai_id_priority.Add(2);

			e10.unitName = "Rebel";
			e10.job = "Shaman";
			e10.classBonusA = 0;
			e10.classBonusB = 0;
			e10.isBoss = 0;
			e10.lvl = 7;
			e10.exp = 0;
			e10.maxHP = 26;
			e10.currentHP = 26;
			e10.strength = 0;
			e10.mag = 8;
			e10.skl = 5;
			e10.spd = 5;
			e10.luk = 4;
			e10.def = 6;
			e10.res = 7;
			e10.con = 5;
			e10.mov = 5;

			e10.proficiency.Add("Tome");
			Item.instance.equipWeapon(e10, "Tome", "Elfire");

			Grid.instance.map[2][0].occupied = e10;

			e10.team = 2;
			e10.allies.Add(2);
			e10.index = 10;


			AIUnit e11 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(12 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 0 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			e11.gridPosition = new Vector2(12, 0);
			e11.ai_id = 1;
			e11.ai_id_priority.Add(2);

			e11.unitName = "Rebel";
			e11.job = "Shaman";
			e11.classBonusA = 0;
			e11.classBonusB = 0;
			e11.isBoss = 0;
			e11.lvl = 7;
			e11.exp = 0;
			e11.maxHP = 26;
			e11.currentHP = 26;
			e11.strength = 0;
			e11.mag = 8;
			e11.skl = 5;
			e11.spd = 5;
			e11.luk = 4;
			e11.def = 6;
			e11.res = 7;
			e11.con = 5;
			e11.mov = 5;

			e11.proficiency.Add("Tome");
			Item.instance.equipWeapon(e11, "Tome", "Elfire");

			Grid.instance.map[12][0].occupied = e11;

			e11.team = 2;
			e11.allies.Add(2);
			e11.index = 11;

			List<Unit> team0 = new List<Unit>();
			List<Unit> team1 = new List<Unit>();
			List<Unit> team2 = new List<Unit>();

			team0.Add(unit1);
			team0.Add(unit2);
			team0.Add(unit3);
			team0.Add(unit4);

			
			team1.Add(boss1);
			team1.Add(enemy1);
			team1.Add(enemy2);
			team1.Add(enemy3);
			team1.Add(enemy4);
			team1.Add(enemy5);
			team1.Add(enemy6);
			team1.Add(enemy7);
			team1.Add(enemy8);
			team1.Add(enemy9);
			team1.Add(enemy10);	
			team1.Add(enemy11);
			team1.Add(enemy12);
			team1.Add(enemy13);


			team2.Add(boss2);
			team2.Add(e1);
			team2.Add(e2);
			team2.Add(e3);
			team2.Add(e4);
			team2.Add(e5);
			team2.Add(e6);
			team2.Add(e7);
			team2.Add(e8);
			team2.Add(e9);
			team2.Add(e10);
			team2.Add(e11);

			Grid.instance.units.Add(team0);
			Grid.instance.units.Add(team1);
			Grid.instance.units.Add(team2);

			Grid.instance.AITeams.Add(1);
			Grid.instance.AITeams.Add(2);

			foreach (List<Unit> team in Grid.instance.units)
			{
				foreach (Unit u in team)
				{
					u.mainCam = Grid.instance.mainCam;
				}
			}

			Grid.instance.resetCamera();
		}
	}
}

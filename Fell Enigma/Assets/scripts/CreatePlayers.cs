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
		if (mapName == "milestone2")
		{
			PlayerUnit unit1 = ((GameObject)Instantiate(Grid.instance.unitPrefab, new Vector3(2 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 5 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<PlayerUnit>();
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

			unit1.proficiency.Add("Sword");
			Item.instance.equipWeapon(unit1, "Sword", "WoDao");
			Item.instance.addWeapon(unit1, "Lance", "Spear");

			Grid.instance.map[2][5].occupied = unit1;

			unit1.team = 0;
			unit1.allies.Add(0);
			unit1.index = 0;

			PlayerUnit unit2 = ((GameObject)Instantiate(Grid.instance.unitPrefab, new Vector3(2 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 6 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<PlayerUnit>();
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

			unit2.proficiency.Add("Tome");
			Item.instance.equipWeapon(unit2, "Tome", "Fimbulvetr");
			Item.instance.addItem(unit2, "StatBoost", "AngelicRobe");


			Grid.instance.map[2][6].occupied = unit2;

			unit2.team = 0;
			unit2.allies.Add(0);
			unit2.index = 1;

			PlayerUnit unit4 = ((GameObject)Instantiate(Grid.instance.unitPrefab, new Vector3(1 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 6 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<PlayerUnit>();
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

			unit4.proficiency.Add("Bow");
			Item.instance.addItem(unit4, "Consumable", "Potion");
			Item.instance.equipWeapon(unit4, "Bow", "KillerBow");
			Item.instance.equipWeapon(unit4, "Bow", "Longbow");

			Grid.instance.map[1][6].occupied = unit4;

			unit4.team = 0;
			unit4.allies.Add(0);
			unit4.index = 2;

			AIUnit unit3 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(10 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 7 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
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

			unit3.proficiency.Add("Tome");
			Item.instance.equipWeapon(unit3, "Tome", "Elfire");

			Grid.instance.map[10][7].occupied = unit3;

			unit3.team = 2;
			unit3.allies.Add(0);
			unit3.allies.Add(2);
			unit3.index = 0;




			AIUnit boss1 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(10 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 18 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
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
			boss1.currentHP = 60;
			boss1.strength = 25;
			boss1.mag = 22;
			boss1.skl = 13;
			boss1.spd = 11;
			boss1.luk = 4;
			boss1.def = 17;
			boss1.res = 19;
			boss1.con = 7;
			boss1.mov = 6;

			boss1.proficiency.Add("Tome");
			Item.instance.equipWeapon(boss1, "Tome", "Fenrir");

			Grid.instance.map[10][18].occupied = boss1;

			boss1.team = 1;
			boss1.allies.Add(1);
			boss1.index = 0;

			AIUnit enemy1 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(10 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 13 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
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

			enemy1.proficiency.Add("Tome");
			Item.instance.equipWeapon(enemy1, "Tome", "Aura");

			Grid.instance.map[10][13].occupied = enemy1;

			enemy1.team = 1;
			enemy1.allies.Add(1);
			enemy1.index = 1;


			AIUnit enemy2 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(0 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 12 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
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

			enemy2.proficiency.Add("Axe");
			Item.instance.equipWeapon(enemy2, "Axe", "Tomahawk");

			Grid.instance.map[0][12].occupied = enemy2;

			enemy2.team = 1;
			enemy2.allies.Add(1);
			enemy2.index = 2;

			AIUnit enemy3 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(19 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 15 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
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

			enemy3.proficiency.Add("Bow");
			enemy3.proficiency.Add("Sword");
			Item.instance.equipWeapon(enemy3, "Bow", "Longbow");

			Grid.instance.map[19][15].occupied = enemy3;

			enemy3.team = 1;
			enemy3.allies.Add(1);
			enemy3.index = 3;

			List<Unit> team0 = new List<Unit>();
			List<Unit> team1 = new List<Unit>();
			List<Unit> team2 = new List<Unit>();

			unit1.mainCam = unit2.mainCam = unit3.mainCam = unit4.mainCam = enemy1.mainCam = enemy2.mainCam = enemy3.mainCam = boss1.mainCam = Grid.instance.mainCam;

			team0.Add(unit1);
			team0.Add(unit2);
			team0.Add(unit4);

			Grid.instance.units.Add(team0);

			team1.Add(boss1);
			team1.Add(enemy1);
			team1.Add(enemy2);
			team1.Add(enemy3);

			Grid.instance.units.Add(team1);


			team2.Add(unit3);

			Grid.instance.units.Add(team2);


			Grid.instance.AITeams.Add(1);
			Grid.instance.AITeams.Add(2);
		}
		else if (mapName == "tutorial")
		{
			//5, 25
			PlayerUnit unit1 = ((GameObject)Instantiate(Grid.instance.unitPrefab, new Vector3(2 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 5 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<PlayerUnit>();
			unit1.gridPosition = new Vector2(2, 5);

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

			Grid.instance.map[2][5].occupied = unit1;

			unit1.team = 0;
			unit1.allies.Add(0);
			unit1.index = 0;


			//5, 26
			PlayerUnit unit2 = ((GameObject)Instantiate(Grid.instance.unitPrefab, new Vector3(13 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 15 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<PlayerUnit>();
			unit2.gridPosition = new Vector2(13, 15);

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

			Grid.instance.map[13][15].occupied = unit2;

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
	}
}

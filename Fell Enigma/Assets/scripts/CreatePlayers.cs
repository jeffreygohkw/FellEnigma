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
			Item.instance.addItem(unit4, "Consumable", "Herb");
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
			PlayerUnit unit1 = ((GameObject)Instantiate(Grid.instance.unitPrefab, new Vector3(14 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 28 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<PlayerUnit>();
			unit1.gridPosition = new Vector2(14, 28);

			unit1.unitName = "Prince";
			unit1.job = "Prince";
			unit1.lvl = 1;
			unit1.exp = 0;
			unit1.maxHP = 18;
			unit1.currentHP = 18;
			unit1.strength = 5;
			unit1.mag = 1;
			unit1.skl = 5;
			unit1.spd = 7;
			unit1.luk = 7;
			unit1.def = 5;
			unit1.res = 0;
			unit1.con = 7;
			unit1.mov = 5;

			unit1.hpG = 80;
			unit1.strG = 45;
			unit1.magG = 35;
			unit1.sklG = 50;
			unit1.spdG = 40;
			unit1.lukG = 45;
			unit1.defG = 30;
			unit1.resG = 35;

			unit1.proficiency.Add("Sword");
			Item.instance.equipWeapon(unit1, "Sword", "IronSword");
			Item.instance.addItem(unit1, "Consumable", "Herb");

			Grid.instance.map[14][28].occupied = unit1;

			unit1.team = 0;
			unit1.allies.Add(0);
			unit1.index = 0;



			PlayerUnit unit2 = ((GameObject)Instantiate(Grid.instance.unitPrefab, new Vector3(15 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 28 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<PlayerUnit>();
			unit2.gridPosition = new Vector2(15, 28);

			unit2.unitName = "White Heart";
			unit2.job = "White Mage";
			unit2.lvl = 2;
			unit2.exp = 0;
			unit2.maxHP = 19;
			unit2.currentHP = 19;
			unit2.strength = 0;
			unit2.mag = 6;
			unit2.skl = 6;
			unit2.spd = 8;
			unit2.luk = 2;
			unit2.def = 2;
			unit2.res = 6;
			unit2.con = 6;
			unit2.mov = 5;

			unit2.hpG = 55;
			unit2.strG = 10;
			unit2.magG = 50;
			unit2.sklG = 50;
			unit2.spdG = 40;
			unit2.lukG = 25;
			unit2.defG = 15;
			unit2.resG = 55;

			unit2.proficiency.Add("Tome");
			unit2.proficiency.Add("Staff");
			Item.instance.equipWeapon(unit2, "Tome", "Lightning");
			Item.instance.addItem(unit2, "Staff", "Heal");
			Item.instance.addItem(unit2, "Consumable", "Herb");

			Grid.instance.map[15][28].occupied = unit2;

			unit2.team = 0;
			unit2.allies.Add(0);
			unit2.index = 1;



			AIUnit boss1 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(2 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 6 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			boss1.gridPosition = new Vector2(2, 6);
			boss1.ai_id = 2;

			boss1.unitName = "Black Heart";
			boss1.job = "Black Mage";
			boss1.classBonusA = 0;
			boss1.classBonusB = 0;
			boss1.isBoss = 1;
			boss1.lvl = 2;
			boss1.exp = 0;
			boss1.maxHP = 24;
			boss1.currentHP = 24;
			boss1.strength = 4;
			boss1.mag = 8;
			boss1.skl = 14;
			boss1.spd = 7;
			boss1.luk = 9;
			boss1.def = 8;
			boss1.res = 10;
			boss1.con = 6;
			boss1.mov = 5;

			boss1.proficiency.Add("Tome");
			Item.instance.equipWeapon(boss1, "Tome", "Flux");

			Grid.instance.map[2][6].occupied = boss1;

			boss1.team = 1;
			boss1.allies.Add(1);
			boss1.index = 0;


			AIUnit enemy1 = ((GameObject)Instantiate(Grid.instance.enemyPrefab, new Vector3(17 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 22 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0), Quaternion.Euler(new Vector3(90, 0, 0)))).GetComponent<AIUnit>();
			enemy1.gridPosition = new Vector2(17, 22);
			enemy1.ai_id = 1;

			enemy1.unitName = "Bandit";
			enemy1.job = "Bandit";
			enemy1.classBonusA = 0;
			enemy1.classBonusB = 0;
			enemy1.lvl = 1;
			enemy1.exp = 0;
			enemy1.maxHP = 20;
			enemy1.currentHP = 20;
			enemy1.strength = 5;
			enemy1.mag = 0;
			enemy1.skl = 1;
			enemy1.spd = 5;
			enemy1.luk = 0;
			enemy1.def = 3;
			enemy1.res = 0;
			enemy1.con = 12;
			enemy1.mov = 5;

			enemy1.proficiency.Add("Axe");
			Item.instance.equipWeapon(enemy1, "Axe", "IronAxe");

			Grid.instance.map[17][22].occupied = enemy1;

			enemy1.team = 1;
			enemy1.allies.Add(1);
			enemy1.index = 1;


			List<Unit> team0 = new List<Unit>();
			List<Unit> team1 = new List<Unit>();

			team0.Add(unit1);
			team0.Add(unit2);

			Grid.instance.units.Add(team0);

			team1.Add(boss1);
			team1.Add(enemy1);

			Grid.instance.units.Add(team1);


			Grid.instance.AITeams.Add(1);

			foreach (List<Unit> team in Grid.instance.units)
			{
				foreach (Unit u in team)
				{
					u.mainCam = Grid.instance.mainCam;
				}
			}
		}

	}
}

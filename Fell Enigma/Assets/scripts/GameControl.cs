using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameControl : MonoBehaviour{

	PlayerData data;

	public static GameControl instance;

	public string nextScene;

	public List<string> mcProf;
	public List<string> mcNameJob;
	public List<int> mcIntData;
	public List<bool> mcBoolData;
	public List<string[]> mcInventory;

	public List<string> npProf;
	public List<string> npNameJob;
	public List<int> npIntData;
	public List<bool> npBoolData;
	public List<string[]> npInventory;

	public List<string> ksProf;
	public List<string> ksNameJob;
	public List<int> ksIntData;
	public List<bool> ksBoolData;
	public List<string[]> ksInventory;

	public List<string> yrProf;
	public List<string> yrNameJob;
	public List<int> yrIntData;
	public List<bool> yrBoolData;
	public List<string[]> yrInventory;

	public List<string> bhProf;
	public List<string> bhNameJob;
	public List<int> bhIntData;
	public List<bool> bhBoolData;
	public List<string[]> bhInventory;

	public int gold;

	public void Awake()
	{
		instance = this;

		nextScene = "";

		List<string> mcProf = new List<string>();
		List<string> mcNameJob = new List<string>();
		List<int> mcIntData = new List<int>();
		List<bool> mcBoolData = new List<bool>();
		List<string[]> mcInventory = new List<string[]>();

		List<string> npProf = new List<string>();
		List<string> npNameJob = new List<string>();
		List<int> npIntData = new List<int>();
		List<bool> npBoolData = new List<bool>();
		List<string[]> npInventory = new List<string[]>();

		List<string> ksProf = new List<string>();
		List<string> ksNameJob = new List<string>();
		List<int> ksIntData = new List<int>();
		List<bool> ksBoolData = new List<bool>();
		List<string[]> ksInventory = new List<string[]>();

		List<string> yrProf = new List<string>();
		List<string> yrNameJob = new List<string>();
		List<int> yrIntData = new List<int>();
		List<bool> yrBoolData = new List<bool>();
		List<string[]> yrInventory = new List<string[]>();

		List<string> bhProf = new List<string>();
		List<string> bhNameJob = new List<string>();
		List<int> bhIntData = new List<int>();
		List<bool> bhBoolData = new List<bool>();
		List<string[]> bhInventory = new List<string[]>();
}

	public void Save()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");
		data = new PlayerData();

		foreach (Unit u in Grid.instance.units[0])
		{
			if (Grid.instance.nextScene != "")
			{
				Debug.Log(Grid.instance.nextScene);
				data.nextScene = Grid.instance.nextScene;
			}

			if (u.unitName == "You")
			{
				data.mcProf = u.proficiency;

				data.mcNameJob.Clear();
				data.mcNameJob.Add(u.unitName);
				data.mcNameJob.Add(u.job);

				data.mcIntData.Clear();
				data.mcIntData.Add(u.lvl);
				data.mcIntData.Add(u.exp);
				data.mcIntData.Add(u.maxHP);
				data.mcIntData.Add(u.currentHP);
				data.mcIntData.Add(u.strength);
				data.mcIntData.Add(u.mag);
				data.mcIntData.Add(u.skl);
				data.mcIntData.Add(u.spd);
				data.mcIntData.Add(u.luk);
				data.mcIntData.Add(u.def);
				data.mcIntData.Add(u.res);
				data.mcIntData.Add(u.con);
				data.mcIntData.Add(u.mov);

				data.mcIntData.Add(u.hpG);
				data.mcIntData.Add(u.strG);
				data.mcIntData.Add(u.magG);
				data.mcIntData.Add(u.sklG);
				data.mcIntData.Add(u.spdG);
				data.mcIntData.Add(u.lukG);
				data.mcIntData.Add(u.defG);
				data.mcIntData.Add(u.resG);

				data.mcIntData.Add(u.equippedIndex);
				data.mcIntData.Add(u.weaponMt);
				data.mcIntData.Add(u.weaponAcc);
				data.mcIntData.Add(u.weaponCrit);
				data.mcIntData.Add(u.weaponWt);
				data.mcIntData.Add(u.weaponMinRange);
				data.mcIntData.Add(u.weaponMaxRange);

				data.mcBoolData.Clear();
				data.mcBoolData.Add(u.isHero);
				data.mcBoolData.Add(u.isFlying);
				data.mcBoolData.Add(u.weaponPhysical);
				
				data.mcInventory = u.inventory;
				Debug.Log("MC Saved");
			}
			else if (u.unitName == "Naive Prince")
			{
				data.npProf = u.proficiency;

				data.npNameJob.Clear();
				data.npNameJob.Add(u.unitName);
				data.npNameJob.Add(u.job);

				data.npIntData.Clear();
				data.npIntData.Add(u.lvl);
				data.npIntData.Add(u.exp);
				data.npIntData.Add(u.maxHP);
				data.npIntData.Add(u.currentHP);
				data.npIntData.Add(u.strength);
				data.npIntData.Add(u.mag);
				data.npIntData.Add(u.skl);
				data.npIntData.Add(u.spd);
				data.npIntData.Add(u.luk);
				data.npIntData.Add(u.def);
				data.npIntData.Add(u.res);
				data.npIntData.Add(u.con);
				data.npIntData.Add(u.mov);

				data.npIntData.Add(u.hpG);
				data.npIntData.Add(u.strG);
				data.npIntData.Add(u.magG);
				data.npIntData.Add(u.sklG);
				data.npIntData.Add(u.spdG);
				data.npIntData.Add(u.lukG);
				data.npIntData.Add(u.defG);
				data.npIntData.Add(u.resG);

				data.npIntData.Add(u.equippedIndex);
				data.npIntData.Add(u.weaponMt);
				data.npIntData.Add(u.weaponAcc);
				data.npIntData.Add(u.weaponCrit);
				data.npIntData.Add(u.weaponWt);
				data.npIntData.Add(u.weaponMinRange);
				data.npIntData.Add(u.weaponMaxRange);

				data.npBoolData.Clear();
				data.npBoolData.Add(u.isHero);
				data.npBoolData.Add(u.isFlying);
				data.npBoolData.Add(u.weaponPhysical);

				data.npInventory = u.inventory;
				Debug.Log("NP Saved");
			}
			else if (u.unitName == "Kind Soul")
			{
				data.ksProf = u.proficiency;

				data.ksNameJob.Clear();
				data.ksNameJob.Add(u.unitName);
				data.ksNameJob.Add(u.job);

				data.ksIntData.Clear();
				data.ksIntData.Add(u.lvl);
				data.ksIntData.Add(u.exp);
				data.ksIntData.Add(u.maxHP);
				data.ksIntData.Add(u.currentHP);
				data.ksIntData.Add(u.strength);
				data.ksIntData.Add(u.mag);
				data.ksIntData.Add(u.skl);
				data.ksIntData.Add(u.spd);
				data.ksIntData.Add(u.luk);
				data.ksIntData.Add(u.def);
				data.ksIntData.Add(u.res);
				data.ksIntData.Add(u.con);
				data.ksIntData.Add(u.mov);

				data.ksIntData.Add(u.hpG);
				data.ksIntData.Add(u.strG);
				data.ksIntData.Add(u.magG);
				data.ksIntData.Add(u.sklG);
				data.ksIntData.Add(u.spdG);
				data.ksIntData.Add(u.lukG);
				data.ksIntData.Add(u.defG);
				data.ksIntData.Add(u.resG);

				data.ksIntData.Add(u.equippedIndex);
				data.ksIntData.Add(u.weaponMt);
				data.ksIntData.Add(u.weaponAcc);
				data.ksIntData.Add(u.weaponCrit);
				data.ksIntData.Add(u.weaponWt);
				data.ksIntData.Add(u.weaponMinRange);
				data.ksIntData.Add(u.weaponMaxRange);

				data.ksBoolData.Clear();
				data.ksBoolData.Add(u.isHero);
				data.ksBoolData.Add(u.isFlying);
				data.ksBoolData.Add(u.weaponPhysical);

				data.ksInventory = u.inventory;
				Debug.Log("KS Saved");
			}
			else if (u.unitName == "Young Rebel")
			{
				data.yrProf = u.proficiency;

				data.yrNameJob.Clear();
				data.yrNameJob.Add(u.unitName);
				data.yrNameJob.Add(u.job);

				data.yrIntData.Clear();
				data.yrIntData.Add(u.lvl);
				data.yrIntData.Add(u.exp);
				data.yrIntData.Add(u.maxHP);
				data.yrIntData.Add(u.currentHP);
				data.yrIntData.Add(u.strength);
				data.yrIntData.Add(u.mag);
				data.yrIntData.Add(u.skl);
				data.yrIntData.Add(u.spd);
				data.yrIntData.Add(u.luk);
				data.yrIntData.Add(u.def);
				data.yrIntData.Add(u.res);
				data.yrIntData.Add(u.con);
				data.yrIntData.Add(u.mov);

				data.yrIntData.Add(u.hpG);
				data.yrIntData.Add(u.strG);
				data.yrIntData.Add(u.magG);
				data.yrIntData.Add(u.sklG);
				data.yrIntData.Add(u.spdG);
				data.yrIntData.Add(u.lukG);
				data.yrIntData.Add(u.defG);
				data.yrIntData.Add(u.resG);

				data.yrIntData.Add(u.equippedIndex);
				data.yrIntData.Add(u.weaponMt);
				data.yrIntData.Add(u.weaponAcc);
				data.yrIntData.Add(u.weaponCrit);
				data.yrIntData.Add(u.weaponWt);
				data.yrIntData.Add(u.weaponMinRange);
				data.yrIntData.Add(u.weaponMaxRange);

				data.yrBoolData.Clear();
				data.yrBoolData.Add(u.isHero);
				data.yrBoolData.Add(u.isFlying);
				data.yrBoolData.Add(u.weaponPhysical);

				data.yrInventory = u.inventory;
				Debug.Log("YR Saved");
			}
			else if (u.unitName == "Black Heart")
			{
				data.bhProf = u.proficiency;

				data.bhNameJob.Clear();
				data.bhNameJob.Add(u.unitName);
				data.bhNameJob.Add(u.job);

				data.bhIntData.Clear();
				data.bhIntData.Add(u.lvl);
				data.bhIntData.Add(u.exp);
				data.bhIntData.Add(u.maxHP);
				data.bhIntData.Add(u.currentHP);
				data.bhIntData.Add(u.strength);
				data.bhIntData.Add(u.mag);
				data.bhIntData.Add(u.skl);
				data.bhIntData.Add(u.spd);
				data.bhIntData.Add(u.luk);
				data.bhIntData.Add(u.def);
				data.bhIntData.Add(u.res);
				data.bhIntData.Add(u.con);
				data.bhIntData.Add(u.mov);

				data.bhIntData.Add(u.hpG);
				data.bhIntData.Add(u.strG);
				data.bhIntData.Add(u.magG);
				data.bhIntData.Add(u.sklG);
				data.bhIntData.Add(u.spdG);
				data.bhIntData.Add(u.lukG);
				data.bhIntData.Add(u.defG);
				data.bhIntData.Add(u.resG);

				data.bhIntData.Add(u.equippedIndex);
				data.bhIntData.Add(u.weaponMt);
				data.bhIntData.Add(u.weaponAcc);
				data.bhIntData.Add(u.weaponCrit);
				data.bhIntData.Add(u.weaponWt);
				data.bhIntData.Add(u.weaponMinRange);
				data.bhIntData.Add(u.weaponMaxRange);

				data.bhBoolData.Clear();
				data.bhBoolData.Add(u.isHero);
				data.bhBoolData.Add(u.isFlying);
				data.bhBoolData.Add(u.weaponPhysical);

				data.bhInventory = u.inventory;
				Debug.Log("BH Saved");
			}
			else
			{
				continue;
			}
		}
		data.gold = Grid.instance.gold;

		bf.Serialize(file, data);
		Debug.Log("Saved");
		file.Close();
	}

	public void Load()
	{
		if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
			data = (PlayerData)bf.Deserialize(file);

			nextScene = data.nextScene;

			mcProf = data.mcProf;
			mcNameJob = data.mcNameJob;
			mcIntData = data.mcIntData;
			mcBoolData = data.mcBoolData;
			mcInventory = data.mcInventory;

			npProf = data.npProf;
			npNameJob = data.npNameJob;
			npIntData = data.npIntData;
			npBoolData = data.npBoolData;
			npInventory = data.npInventory;

			ksProf = data.ksProf;
			ksNameJob = data.ksNameJob;
			ksIntData = data.ksIntData;
			ksBoolData = data.ksBoolData;
			ksInventory = data.ksInventory;

			yrProf = data.yrProf;
			yrNameJob = data.yrNameJob;
			yrIntData = data.yrIntData;
			yrBoolData = data.yrBoolData;
			yrInventory = data.yrInventory;

			bhProf = data.bhProf;
			bhNameJob = data.bhNameJob;
			bhIntData = data.bhIntData;
			bhBoolData = data.bhBoolData;
			bhInventory = data.bhInventory;

			gold = data.gold;
			Debug.Log("Loaded");
			file.Close();
		}
		else
		{
			Debug.Log("Missing savefile!");
		}
	}

	[Serializable]
	private class PlayerData
	{
		public string nextScene;

		public List<string> mcProf = new List<string>();
		public List<string> mcNameJob = new List<string>();
		public List<int> mcIntData = new List<int>();
		public List<bool> mcBoolData = new List<bool>();
		public List<string[]> mcInventory = new List<string[]>();

		public List<string> npProf = new List<string>();
		public List<string> npNameJob = new List<string>();
		public List<int> npIntData = new List<int>();
		public List<bool> npBoolData = new List<bool>();
		public List<string[]> npInventory = new List<string[]>();

		public List<string> ksProf = new List<string>();
		public List<string> ksNameJob = new List<string>();
		public List<int> ksIntData = new List<int>();
		public List<bool> ksBoolData = new List<bool>();
		public List<string[]> ksInventory = new List<string[]>();

		public List<string> yrProf = new List<string>();
		public List<string> yrNameJob = new List<string>();
		public List<int> yrIntData = new List<int>();
		public List<bool> yrBoolData = new List<bool>();
		public List<string[]> yrInventory = new List<string[]>();

		public List<string> bhProf = new List<string>();
		public List<string> bhNameJob = new List<string>();
		public List<int> bhIntData = new List<int>();
		public List<bool> bhBoolData = new List<bool>();
		public List<string[]> bhInventory = new List<string[]>();

		public int gold = 0;

	}
}


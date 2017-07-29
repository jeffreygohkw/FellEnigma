using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameControl : MonoBehaviour {

	public static GameControl control;

	public Unit MC;
	public Unit NP;
	public Unit KS;
	public Unit YR;
	public Unit BH;

	public int gold;

	private void Awake()
	{
		if (control == null)
		{
			DontDestroyOnLoad(gameObject);
			control = this;
		}
		else if (control != this)
		{
			Destroy(gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Save()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");
		PlayerData data = new PlayerData();

		foreach (Unit u in Grid.instance.units[0])
		{
			if (u.unitName == "Naive Prince")
			{
				data.NP = u;
			}
			else if (u.unitName == "Kind Soul")
			{
				data.KS = u;
			}
			else if (u.unitName == "Young Rebel")
			{
				data.YR = u;
			}
			else if (u.unitName == "Black Heart")
			{
				data.BH = u;
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
			PlayerData data = (PlayerData)bf.Deserialize(file);
			Debug.Log("Loaded");
			file.Close();
		}
	}
}

[Serializable]
class PlayerData
{
	public Unit MC;
	public Unit NP;
	public Unit KS;
	public Unit YR;
	public Unit BH;

	public int gold;

}
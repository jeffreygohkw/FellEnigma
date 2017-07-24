using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBuildings : MonoBehaviour
{
	

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}



	/**
	* Generates the buildings that will appear in the map
	* Have to assign everything manually
	* 
	* 
	* @author Jeffrey Goh
	* @version 1.0
	* @updated 12/7/2017
	*/
	public static void generateBuildings(string mapName)
	{
		if (mapName == "tutorial")
		{
			// Link Taverns and spawn point
			Grid.instance.tavernAndSpawn.Add(Grid.instance.map[16][12].gridPosition, Grid.instance.map[16][11].gridPosition);

			//Set level of units that can be recruited
			Grid.instance.tavernLevel = 1;

			Grid.instance.gold = 400;
			Grid.instance.goldCap = 1000;

			Grid.instance.commander = 0;
			Grid.instance.ultCharge = 70;
			ActivateTextAtLine.instance.startScript(0, 10);
		}
	}
}

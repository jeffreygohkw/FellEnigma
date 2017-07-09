using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCon : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/**
	 * 0 = Continue
	 * 1 = Victory
	 * 2 = Failure
	 * 
	 * @param mapName which map we are currently playing
	 * @author Jeffrey Goh
	 * @version v1.0
	 * @updated 9/7/2017
	 */
	public static int checkWinCon(string mapName)
	{
		if (mapName == "milestone2")
		{
			//If neutral unit dies
			if (Grid.instance.units[2][0].currentHP <= 0)
			{
				return 1;
			}
			//If boss dies
			else if (Grid.instance.units[1][0].currentHP <= 0)
			{
				return 2;
			}
			else
			{
				return 0;
			}
		}
		else if (mapName == "tutorial")
		{
			return 0;
		}
		else
		{
			return 0;
		}
	}
}

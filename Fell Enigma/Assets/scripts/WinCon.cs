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

	static bool textOut = false;

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
			bool rout = true;
			foreach (Unit u in Grid.instance.units[1])
			{
				//Boss dies
				if ((u.unitName == "Black Heart" && u.currentHP <= 0))
				{
					if (!textOut)
					{
						ActivateTextAtLine.instance.startScript(61, 80);
						textOut = true;
					}
					return 1;
				}
				if ((u.unitName == "Bandit Leader") && u.currentHP <= 0)
				{
					if (!textOut)
					{
						ActivateTextAtLine.instance.startScript(83, 101);
						textOut = true;
					}
					return 1;
				}
				if (u.currentHP > 0)
				{
					rout = false;
				}
			}

			bool heroSlain = false;
			foreach (List<Unit> team in Grid.instance.units)
			{
				foreach (Unit u in team)
				{
					if (u.isHero && u.currentHP <= 0)
					{
						heroSlain = true;
						break;
					}
				}
			}

			bool allCaptured = true;
			foreach (Vector2 v in Grid.instance.villageStatus.Keys)
			{
				if (Grid.instance.villageStatus[v][0] != 1)
				{
					allCaptured = false;
					break;
				}
			}

			//If a hero dies
			if (heroSlain)
			{
				if (!textOut)
				{
					ActivateTextAtLine.instance.startScript(104, 106);
					textOut = true;
				}
				return 2;
			}
			if (allCaptured)
			{
				if (!textOut)
				{
					ActivateTextAtLine.instance.startScript(109, 111);
					textOut = true;
				}
				return 2;
			}
			//Rout
			else if (rout)
			{
				return 1;
			}
			else
			{
				return 0;
			}
		}
		else if (mapName == "chapter1")
		{
			bool rout = true;
			foreach (Unit u in Grid.instance.units[1])
			{
				if (u.currentHP > 0)
				{
					rout = false;
					break;
				}
			}

			int heroSlain = 0;

			foreach (Unit u in Grid.instance.units[0])
			{
				if (u.currentHP <= 0 && u.isHero)
				{
					heroSlain++;
				}
			}

			if (heroSlain == 4)
			{
				ActivateTextAtLine.instance.startScript(19,20);
				return 2;
			}
			else if (Grid.instance.objectiveComplete == "Escape")
			{
				//ActivateTextAtLine.instance.startScript(,);
				return 1;
			}
			else if (rout)
			{
				//ActivateTextAtLine.instance.startScript(,);
				return 1;
			}
			else
			{
				return 0;
			}
		}
		else if (mapName == "chapter2")
		{	
			int heroSlain = 0;

			foreach (Unit u in Grid.instance.units[0])
			{
				if (u.currentHP <= 0 && u.isHero)
				{
					heroSlain++;
				}
			}

			if (heroSlain == 4)
			{
				ActivateTextAtLine.instance.startScript(11,12);
				return 2;
			}
			else if (Grid.instance.units[1][0].currentHP == 0)
			{
				//ActivateTextAtLine.instance.startScript(,);
				return 1;
			}
			else if (Grid.instance.units[2][0].currentHP <= 0)
			{
				//ActivateTextAtLine.instance.startScript(,);
				return 1;
			}
			else
			{
				return 0;
			}
		}
		else
		{
			return 0;
		}
	}
}

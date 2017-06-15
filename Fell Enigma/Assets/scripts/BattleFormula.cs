using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFormula
{
	/**
	* Attacks the target unit, based on the equipped weapons
	* 
	* v 1.0
	* Basic combat formulas
	* 
	* v 1.1
	* Opponent will counterattack, checks for magic weapons
	* 
	* v1.2
	* Opponent will only counterattack if in range, attacker and defender will followup if attack speed >= 4
	* 
	* v1.3
	* Added max and min range instead of just 1 range
	* Fixed some bugs with range calculations
	* 
	* v1.4
	* Updated variable names, stop the unit from taking any more actions after attacking
	* 
	* v1.5
	* AI compatibility
	* 
	* To do
	* Does not factor in terrain, supports, other misc things
	* No weapon triangle
	* 
	* @param target The target of the attack
	* @author Jeffrey Goh
	* @version 1.4
	* @updated 7/6/2017
	*/
	public void attackWithCurrentUnit(Unit target)
	{
		if (Grid.instance.map[(int)target.gridPosition.x][(int)target.gridPosition.y].GetComponent<Renderer>().material.color != Grid.instance.map[(int)target.gridPosition.x][(int)target.gridPosition.y].colour || Grid.instance.AITeams.Contains(Grid.instance.currentTeam))
		{

			if (target != null)
			{
				Debug.Log("Calculating");

				// Range
				bool canCounter = false;

				int dist = Mathf.Abs((int)Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].gridPosition.x - (int)target.gridPosition.x) + Mathf.Abs((int)Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].gridPosition.y - (int)target.gridPosition.y);

				if (target.weaponMinRange <= dist && target.weaponMaxRange >= dist)
				{
					canCounter = true;
				}

				// Accuracy

				int attackerAcc = Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].weaponAcc + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].skl * 2 + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].luk / 2;
				int attackerAvd = Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].spd * 2 + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].luk;
				int defenderAcc = target.weaponAcc + target.skl * 2 + target.luk / 2;
				int defenderAvd = target.spd * 2 + target.luk;

				int attackerHit = attackerAcc - defenderAvd;
				int defenderHit = defenderAcc - attackerAvd;

				int attackerAtk;
				int attackerDef;

				int defenderAtk;
				int defenderDef;

				// Damage for attacker
				// Str and Def if physical weapon equipped, Mag and Res otherwise
				if (Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].weaponPhysical)
				{
					attackerAtk = Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].strength + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].weaponMt;
					defenderDef = target.def;
				}
				else
				{
					attackerAtk = Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].mag + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].weaponMt;
					defenderDef = target.res;
				}

				// Damage for defender

				if (target.weaponPhysical)
				{
					defenderAtk = target.strength + target.weaponMt;
					attackerDef = Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].def;
				}
				else
				{
					defenderAtk = target.mag + target.weaponMt;
					attackerDef = Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].res;
				}

				int attackerDmg = attackerAtk - defenderDef;
				int defenderDmg = defenderAtk - attackerDef;

				// tink instead of doing negative damage and healing the enemy
				if (attackerDmg < 0)
				{
					attackerDmg = 0;
				}

				if (defenderDmg < 0)
				{
					defenderDmg = 0;
				}

				//Crits
				int attackerCritRate = Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].weaponCrit + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].skl / 2;
				int attackerCritAvd = Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].luk;
				int defenderCritRate = target.weaponCrit + target.skl / 2;
				int defenderCritAvd = target.luk;
				int attackerCritChance = attackerCritRate - defenderCritAvd;
				int defenderCritChance = defenderCritRate - attackerCritAvd;

				// Keep crit chance within 0 - 100 to be safe
				if (attackerCritChance > 100)
				{
					attackerCritChance = 100;
				}
				else if (attackerCritChance < 0)
				{
					attackerCritChance = 0;
				}

				if (defenderCritChance > 100)
				{
					defenderCritChance = 100;
				}
				else if (defenderCritChance < 0)
				{
					defenderCritChance = 0;
				}

				//Attack Speed
				int atkBurden = Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].weaponWt - Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].con;
				if (atkBurden < 0)
				{
					atkBurden = 0;
				}

				int defBurden = target.weaponWt - target.con;
				if (defBurden < 0)
				{
					defBurden = 0;
				}

				int atkAS = Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].spd - atkBurden;
				int defAS = target.spd - defBurden;

				//The actual attack

				// Roll for hit
				int hit = Random.Range(1, 100);
				if (hit <= attackerHit)
				{
					Debug.Log("Name: " + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " Hit: " + attackerHit + " DMG: " + attackerDmg + " Crit: " + attackerCritChance);

					//Check for crits
					int crit = Random.Range(1, 100);

					Debug.Log("Hit Roll: " + hit + " Crit Roll: " + crit);

					int tempattackerDmg;

					if (crit <= attackerCritChance)
					{
						tempattackerDmg = attackerDmg * 3;
						Debug.Log(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " has critically hit " + target.unitName + " for " + tempattackerDmg + " damage!");
					}
					else
					{
						tempattackerDmg = attackerDmg;
						Debug.Log(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " has hit " + target.unitName + " for " + tempattackerDmg + " damage!");
					}

					target.currentHP -= tempattackerDmg;

				}
				// If miss
				else
				{
					Debug.Log("Name: " + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " Hit: " + attackerHit + " DMG: " + attackerDmg + " Crit: " + attackerCritChance);
					Debug.Log("Hit Roll: " + hit);

					Debug.Log(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " missed!");
				}

				// Check if target is dead
				if (target.currentHP <= 0)
				{
					target.currentHP = 0;
					Debug.Log(target.unitName + " has died!");

					// Deactivate menu GUI and set unit to not attacking after the attack
					Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].selected = false;
					Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].isFighting = false;
					Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].doneAction = true;
					Grid.instance.totalDone++;
					if (!Grid.instance.AITeams.Contains(Grid.instance.currentTeam))
					{
						Grid.instance.currentPlayer = -1;
					}
					Grid.instance.removeTileHighlight();
					return;
				}
				else
				{
					Debug.Log(target.unitName + ": " + target.currentHP + "/" + target.maxHP + ", " + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + ": " + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].currentHP + "/" + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].maxHP);
				}



				// Counterattack
				if (target.currentHP > 0 && canCounter)
				{
					Debug.Log(target.unitName + " counterattacks!");

					//Roll for hit
					int counterHit = Random.Range(1, 100);
					if (counterHit <= defenderHit)
					{
						Debug.Log("Name: " + target.unitName + " Hit: " + defenderHit + " DMG: " + defenderDmg + " Crit: " + defenderCritChance);

						//Check for crits
						int counterCrit = Random.Range(1, 100);

						Debug.Log("Hit Roll: " + counterHit + " Crit Roll: " + counterCrit);

						int tempdefenderDmg;

						if (counterCrit <= defenderCritChance)
						{
							tempdefenderDmg = defenderDmg * 3;
							Debug.Log(target.unitName + " has critically hit " + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " for " + tempdefenderDmg + " damage!");
						}
						else
						{
							tempdefenderDmg = defenderDmg;
							Debug.Log(target.unitName + " has hit " + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " for " + tempdefenderDmg + " damage!");
						}

						Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].currentHP -= tempdefenderDmg;

					}
					else
					{
						Debug.Log("Name: " + target.unitName + " Hit: " + defenderHit + " DMG: " + defenderDmg + " Crit: " + defenderCritChance);
						Debug.Log("Hit Roll: " + counterHit);

						Debug.Log(target.unitName + " missed!");
					}

					if (Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].currentHP <= 0)
					{
						Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].currentHP = 0;
						Debug.Log(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " has died!");

						// Deactivate menu GUI and set unit to not attacking after the attack
						Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].selected = false;
						Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].isFighting = false;
						Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].doneAction = true;
						Grid.instance.totalDone++;
						if (!Grid.instance.AITeams.Contains(Grid.instance.currentTeam))
						{
							Grid.instance.currentPlayer = -1;
						}
						Grid.instance.removeTileHighlight();
						return;
					}
					else
					{
						Debug.Log(target.unitName + ": " + target.currentHP + "/" + target.maxHP + ", " + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + ": " + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].currentHP + "/" + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].maxHP);
					}



					// Check if attacker is dead
					if (Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].currentHP <= 0)
					{
						Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].currentHP = 0;
						Debug.Log(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " has died!");
						return;
					}
					else
					{
						Debug.Log(target.unitName + ": " + target.currentHP + ", " + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + ": " + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].currentHP);
					}

				}



				//Follow up
				if (atkAS - defAS >= 4 && Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].currentHP > 0)
				{
					Debug.Log(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " does a follow-up attack!");

					// Roll for hit
					int hit1 = Random.Range(1, 100);
					if (hit1 <= attackerHit)
					{
						Debug.Log("Name: " + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " Hit: " + attackerHit + " DMG: " + attackerDmg + " Crit: " + attackerCritChance);

						//Check for crits
						int crit = Random.Range(1, 100);

						Debug.Log("Hit Roll: " + hit1 + " Crit Roll: " + crit);

						int tempattackerDmg;

						if (crit <= attackerCritChance)
						{
							tempattackerDmg = attackerDmg * 3;
							Debug.Log(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " has critically hit " + target.unitName + " for " + tempattackerDmg + " damage!");
						}
						else
						{
							tempattackerDmg = attackerDmg;
							Debug.Log(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " has hit " + target.unitName + " for " + tempattackerDmg + " damage!");
						}

						target.currentHP -= tempattackerDmg;

					}
					// If miss
					else
					{
						Debug.Log("Name: " + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " Hit: " + attackerHit + " DMG: " + attackerDmg + " Crit: " + attackerCritChance);
						Debug.Log("Hit Roll: " + hit1);

						Debug.Log(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " missed!");
					}

					// Check if target is dead
					if (target.currentHP <= 0)
					{
						target.currentHP = 0;
						Debug.Log(target.unitName + " has died!");
					}
					else
					{
						Debug.Log(target.unitName + ": " + target.currentHP + ", " + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + ": " + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].currentHP);
					}



					// Check if target is dead
					if (target.currentHP <= 0)
					{
						target.currentHP = 0;
						Debug.Log(target.unitName + " has died!");

						// Deactivate menu GUI and set unit to not attacking after the attack
						Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].selected = false;
						Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].isFighting = false;
						Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].doneAction = true;
						Grid.instance.totalDone++;
						if (!Grid.instance.AITeams.Contains(Grid.instance.currentTeam))
						{
							Grid.instance.currentPlayer = -1;
						}
						Grid.instance.removeTileHighlight();
						return;
					}
					else
					{
						Debug.Log(target.unitName + ": " + target.currentHP + "/" + target.maxHP + ", " + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + ": " + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].currentHP + "/" + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].maxHP);
					}
				}



				if (target.currentHP > 0 && canCounter && defAS - atkAS >= 4)
				{
					Debug.Log(target.unitName + " does a follow up counterattack!");
					// Counterattack

					//Roll for hit
					int counterHit = Random.Range(1, 100);
					if (counterHit <= defenderHit)
					{
						Debug.Log("Name: " + target.unitName + " Hit: " + defenderHit + " DMG: " + defenderDmg + " Crit: " + defenderCritChance);

						//Check for crits
						int counterCrit = Random.Range(1, 100);

						Debug.Log("Hit Roll: " + counterHit + " Crit Roll: " + counterCrit);

						int tempdefenderDmg;

						if (counterCrit <= defenderCritChance)
						{
							tempdefenderDmg = defenderDmg * 3;
							Debug.Log(target.unitName + " has critically hit " + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " for " + tempdefenderDmg + " damage!");
						}
						else
						{
							tempdefenderDmg = defenderDmg;
							Debug.Log(target.unitName + " has hit " + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " for " + tempdefenderDmg + " damage!");
						}

						Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].currentHP -= tempdefenderDmg;

					}
					else
					{
						Debug.Log("Name: " + target.unitName + " Hit: " + defenderHit + " DMG: " + defenderDmg + " Crit: " + defenderCritChance);
						Debug.Log("Hit Roll: " + counterHit);

						Debug.Log(target.unitName + " missed!");
					}

					if (Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].currentHP <= 0)
					{
						Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].currentHP = 0;
						Debug.Log(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " has died!");
					}
					else
					{
						Debug.Log(target.unitName + ": " + target.currentHP + ", " + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + ": " + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].currentHP);
					}


					// Check if attacker is dead
					if (Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].currentHP <= 0)
					{
						Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].currentHP = 0;
						Debug.Log(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " has died!");

						// Deactivate menu GUI and set unit to not attacking after the attack
						Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].selected = false;
						Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].isFighting = false;
						Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].doneAction = true;
						Grid.instance.totalDone++;
						if (!Grid.instance.AITeams.Contains(Grid.instance.currentTeam))
						{
							Grid.instance.currentPlayer = -1;
						}
						Grid.instance.removeTileHighlight();
						return;
					}
					else
					{
						Debug.Log(target.unitName + ": " + target.currentHP + "/" + target.maxHP + ", " + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + ": " + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].currentHP + "/" + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].maxHP);
					}
				}
			}
			Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].selected = false;
			Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].isFighting = false;
			Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].doneAction = true;
			Grid.instance.totalDone++;
			if (!Grid.instance.AITeams.Contains(Grid.instance.currentTeam))
			{
				Grid.instance.currentPlayer = -1;
			}
			Grid.instance.removeTileHighlight();
		}
		else
		{
			Debug.Log("Invalid target");
		}
	}


	/**
	* Calculates estimated damage assuming no misses or crits
	* 
	* v 1.0
	* Based on v1.4 of attackWithCurrentUnit
	* 
	* To do
	* Does not factor in terrain, supports, other misc things
	* No weapon triangle
	* 
	* @param attacker The attacking unit
	* @param target The target of the attack
	* @author Jeffrey Goh
	* @version 1.0
	* @updated 12/6/2017
	*/
	public int battleForecast(Unit attacker, Unit target)
	{
		int attackerAtk;
		int attackerDef;

		int defenderAtk;
		int defenderDef;

		// Damage for attacker
		// Str and Def if physical weapon equipped, Mag and Res otherwise
		if (attacker.weaponPhysical)
		{
			attackerAtk = attacker.strength + attacker.weaponMt;
			defenderDef = target.def;
		}
		else
		{
			attackerAtk = attacker.mag + attacker.weaponMt;
			defenderDef = target.res;
		}

		// Damage for defender

		if (target.weaponPhysical)
		{
			defenderAtk = target.strength + target.weaponMt;
			attackerDef = attacker.def;
		}
		else
		{
			defenderAtk = target.mag + target.weaponMt;
			attackerDef = attacker.res;
		}

		int attackerDmg = attackerAtk - defenderDef;
		int defenderDmg = defenderAtk - attackerDef;

		// tink instead of doing negative damage and healing the enemy
		if (attackerDmg < 0)
		{
			attackerDmg = 0;
		}

		if (defenderDmg < 0)
		{
			defenderDmg = 0;
		}

		// Range
		bool canCounter = false;

		int dist = Mathf.Abs((int)Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].gridPosition.x - (int)target.gridPosition.x) + Mathf.Abs((int)Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].gridPosition.y - (int)target.gridPosition.y);

		if (target.weaponMinRange <= dist && target.weaponMaxRange >= dist)
		{
			canCounter = true;
		}

		//Attack Speed
		int atkBurden = attacker.weaponWt - attacker.con;
		if (atkBurden < 0)
		{
			atkBurden = 0;
		}

		int defBurden = target.weaponWt - target.con;
		if (defBurden < 0)
		{
			defBurden = 0;
		}

		int atkAS = attacker.spd - atkBurden;
		int defAS = target.spd - defBurden;

		int dmgDealt = attackerDmg;
		int dmgTaken = 0;

		if (canCounter)
		{
			dmgTaken += defenderDmg;
		}

		if (dmgTaken >= attacker.currentHP)
		{
			return dmgDealt;
		}

		if (atkAS - defAS >= 4)
		{
			dmgDealt += attackerDmg;
		}
		return dmgDealt;
	}
}
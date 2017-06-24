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
	* v1.6
	* Added terrain and exp
	* 
	* To do
	* Does not factor in supports, other misc things
	* No weapon triangle
	* 
	* @param target The target of the attack
	* @author Jeffrey Goh
	* @version 1.6
	* @updated 24/6/2017
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
				int attackerAvd = Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].spd * 2 + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].luk + Grid.instance.map[(int)Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].gridPosition.x][(int)Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].gridPosition.y].linkedTerrain.returnAvd();
				int defenderAcc = target.weaponAcc + target.skl * 2 + target.luk / 2;
				int defenderAvd = target.spd * 2 + target.luk + Grid.instance.map[(int)target.gridPosition.x][(int)target.gridPosition.y].linkedTerrain.returnAvd();

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
					defenderDef = target.def + Grid.instance.map[(int)target.gridPosition.x][(int)target.gridPosition.y].linkedTerrain.returnDef();
				}
				else
				{
					attackerAtk = Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].mag + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].weaponMt;
					defenderDef = target.res + Grid.instance.map[(int)target.gridPosition.x][(int)target.gridPosition.y].linkedTerrain.returnDef();
				}

				// Damage for defender

				if (target.weaponPhysical)
				{
					defenderAtk = target.strength + target.weaponMt;
					attackerDef = Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].def + Grid.instance.map[(int)Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].gridPosition.x][(int)Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].gridPosition.y].linkedTerrain.returnDef();
				}
				else
				{
					defenderAtk = target.mag + target.weaponMt;
					attackerDef = Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].res + Grid.instance.map[(int)Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].gridPosition.x][(int)Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].gridPosition.y].linkedTerrain.returnDef();
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

					// Add exp if attacker is a PlayerUnit
					if (Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer] is PlayerUnit)
					{
						this.battleEXP(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer], target, attackerDmg, true);
					}

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

						// Add exp if target is a PlayerUnit
						if (target is PlayerUnit)
						{
							this.battleEXP(target, Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer], defenderDmg, true);
						}
						return;
					}
					else
					{
						Debug.Log(target.unitName + ": " + target.currentHP + "/" + target.maxHP + ", " + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + ": " + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].currentHP + "/" + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].maxHP);
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

						// Add exp if attacker is a PlayerUnit
						if (Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer] is PlayerUnit)
						{
							this.battleEXP(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer], target, 2*attackerDmg, true);
						}

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
						// Add exp if target is a PlayerUnit
						if (target is PlayerUnit)
						{
							this.battleEXP(target, Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer], 2 * defenderDmg, true);
						}
						return;
					}
					else
					{
						Debug.Log(target.unitName + ": " + target.currentHP + "/" + target.maxHP + ", " + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + ": " + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].currentHP + "/" + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].maxHP);
					}
				}
				// Add exp if attacker is a PlayerUnit
				if (Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer] is PlayerUnit)
				{
					if (atkAS - defAS >= 4)
					{
						this.battleEXP(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer], target, 2 * attackerDmg, false);
					}
					else
					{
						this.battleEXP(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer], target, attackerDmg, false);
					}
				}

				// Add exp if target is a PlayerUnit
				if (target is PlayerUnit)
				{
					if (!canCounter)
					{
						this.battleEXP(target, Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer], 0, false);
					}
					else if (defAS - atkAS >= 4)
					{
						this.battleEXP(target, Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer], 2*defenderDmg, false);
					}
					else
					{
						this.battleEXP(target, Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer], defenderDmg, false);
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
			return;
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
	* v1.1
	* Added Terrain
	* 
	* To do
	* Does not factor in supports, other misc things
	* No weapon triangle
	* 
	* @param attacker The attacking unit
	* @param target The target of the attack
	* @author Jeffrey Goh
	* @version 1.1
	* @updated 24/6/2017
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
			defenderDef = target.def + Grid.instance.map[(int)target.gridPosition.x][(int)target.gridPosition.y].linkedTerrain.returnDef();
		}
		else
		{
			attackerAtk = attacker.mag + attacker.weaponMt;
			defenderDef = target.res + Grid.instance.map[(int)target.gridPosition.x][(int)target.gridPosition.y].linkedTerrain.returnDef();
		}

		// Damage for defender

		if (target.weaponPhysical)
		{
			defenderAtk = target.strength + target.weaponMt;
			attackerDef = attacker.def + Grid.instance.map[(int)attacker.gridPosition.x][(int)attacker.gridPosition.y].linkedTerrain.returnDef();
		}
		else
		{
			defenderAtk = target.mag + target.weaponMt;
			attackerDef = attacker.res + Grid.instance.map[(int)attacker.gridPosition.x][(int)attacker.gridPosition.y].linkedTerrain.returnDef();
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

	/**
	* Allows player units to gain exp and level up from combat
	* 
	* 
	* @param attacker The attacking unit
	* @param defender The target of the attack
	* @param damage The damage dealt in combat
	* @param defeated Whether the target was defeated
	* @author Jeffrey Goh
	* @version 1.0
	* @updated 24/6/2017
	*/
	public void battleEXP(Unit attacker, Unit defender, int damage, bool defeated)
	{
		if (attacker.lvl == 20)
		{
			return;
		}
		else
		{
			int expGain = 0;
			if (damage == 0)
			{
				expGain = 1;
			}
			else
			{
				int expDmg = (31 + (defender.lvl + defender.classBonusA) - (attacker.lvl + attacker.classBonusA)) / 3;

				if (defeated)
				{
					int expBase = ((defender.lvl * defender.classPower) + defender.classBonusB) - ((attacker.lvl + attacker.classPower) + attacker.classBonusB);
					int expKill = expDmg + expBase + 20 + defender.isBoss * 40 + defender.isThief;
					if (expKill <= 0)
					{
						expKill = 1;
					}
					else if (expKill > 100)
					{
						expKill = 100;
					}
					expGain = expKill;
				}
				else
				{
					if (expDmg <= 0)
					{
						expDmg = 1;
					}
					else if (expDmg > 100)
					{
						expDmg = 100;
					}
					expGain = expDmg;
				}
			}
			Debug.Log(attacker.unitName + " has gained " + expGain + " exp!");
			//Level up
			if (attacker.exp >= 100)
			{
				attacker.exp -= 100;
				attacker.lvl += 1;
				Debug.Log(attacker.unitName + " has levelled up!");
				int roll = Random.Range(1, 100);
				if (roll <= attacker.hpG)
				{
					attacker.currentHP += 1;
					attacker.maxHP += 1;
					Debug.Log(attacker.unitName + " has gained 1 HP!");
				}
				roll = Random.Range(1, 100);
				if (roll <= attacker.strG)
				{
					attacker.strength += 1;
					Debug.Log(attacker.unitName + " has gained 1 STR!");
				}
				roll = Random.Range(1, 100);
				if (roll <= attacker.magG)
				{
					attacker.mag += 1;
					Debug.Log(attacker.unitName + " has gained 1 MAG!");
				}
				roll = Random.Range(1, 100);
				if (roll <= attacker.sklG)
				{
					attacker.skl += 1;
					Debug.Log(attacker.unitName + " has gained 1 SKL!");
				}
				roll = Random.Range(1, 100);
				if (roll <= attacker.spdG)
				{
					attacker.spd += 1;
					Debug.Log(attacker.unitName + " has gained 1 SPD!");
				}
				roll = Random.Range(1, 100);
				if (roll <= attacker.lukG)
				{
					attacker.luk += 1;
					Debug.Log(attacker.unitName + " has gained 1 LUK!");
				}
				roll = Random.Range(1, 100);
				if (roll <= attacker.defG)
				{
					attacker.def += 1;
					Debug.Log(attacker.unitName + " has gained 1 DEF!");
				}
				roll = Random.Range(1, 100);
				if (roll <= attacker.resG)
				{
					attacker.res += 1;
					Debug.Log(attacker.unitName + " has gained 1 RES!");
				}
			}
			else
			{
				return;
			}
		}
	}
}
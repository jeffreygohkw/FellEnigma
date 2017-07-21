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
	* v1.7
	* Added Ult charge, 3 damage per point for damage done by player, 5 damage per point for player taking damage
	* 
	* @param target The target of the attack
	* @author Jeffrey Goh
	* @version 1.7
	* @updated 21/7/2017
	*/
	public void attackWithCurrentUnit(Unit target)
	{
		if (Grid.instance.map[(int)target.gridPosition.x][(int)target.gridPosition.y].GetComponent<Renderer>().material.color != Grid.instance.map[(int)target.gridPosition.x][(int)target.gridPosition.y].colour || Grid.instance.AITeams.Contains(Grid.instance.currentTeam))
		{

			if (target != null && Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].equippedIndex != -1)
			{
				Debug.Log("Calculating");

				// Range
				bool canCounter = false;

				int dist = Mathf.Abs((int)Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].gridPosition.x - (int)target.gridPosition.x) + Mathf.Abs((int)Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].gridPosition.y - (int)target.gridPosition.y);

				if (target.weaponMinRange <= dist && target.weaponMaxRange + target.weaponRangeBuff >= dist)
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

				//Cancel counterattack and upgrade hit and crit if rebelBuff is on for the attacker
				if (Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].rebelBuff)
				{
					canCounter = false;
					attackerHit += 50;
					attackerCritChance += 30;
				}



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
						CombatLog.instance.AddEvent(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " has critically hit " + target.unitName + " for " + tempattackerDmg + " damage!");
					}
					else
					{
						tempattackerDmg = attackerDmg;
						Debug.Log(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " has hit " + target.unitName + " for " + tempattackerDmg + " damage!");
						CombatLog.instance.AddEvent(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " has hit " + target.unitName + " for " + tempattackerDmg + " damage!");
					}

					
					//Ult charge
					if (Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer] is PlayerUnit)
					{
						if (tempattackerDmg > target.currentHP)
						{
							Grid.instance.ultCharge += target.currentHP / 3;
						}
						else
						{
							Grid.instance.ultCharge += tempattackerDmg / 3;
						}
					}
					else if (target is PlayerUnit)
					{
						if (tempattackerDmg > target.currentHP)
						{
							Grid.instance.ultCharge += target.currentHP / 5;
						}
						else
						{
							Grid.instance.ultCharge += tempattackerDmg / 5;
						}
					}

					if (Grid.instance.ultCharge > 100)
					{
						Grid.instance.ultCharge = 100;
					}

					target.currentHP -= tempattackerDmg;
				}
				// If miss
				else
				{
					Debug.Log("Name: " + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " Hit: " + attackerHit + " DMG: " + attackerDmg + " Crit: " + attackerCritChance);
					Debug.Log("Hit Roll: " + hit);

					Debug.Log(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " missed!");
					CombatLog.instance.AddEvent(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " missed!");
				}

				// Check if target is dead
				if (target.currentHP <= 0)
				{
					target.currentHP = 0;
					Debug.Log(target.unitName + " has died!");
                    CombatLog.instance.AddEvent(target.unitName + " has died!");

					// Add exp if attacker is a PlayerUnit
					if (Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer] is PlayerUnit)
					{
						this.battleEXP(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer], target, attackerDmg, true);
					}

					// Deactivate menu GUI and set unit to not attacking after the attack
					Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].playerWait();
					CombatLog.instance.PrintEvent();
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
                    CombatLog.instance.AddEvent(target.unitName + " counterattacks!");

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
                            CombatLog.instance.AddEvent(target.unitName + " has critically hit " + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " for " + tempdefenderDmg + " damage!");
						}
						else
						{
							tempdefenderDmg = defenderDmg;
							Debug.Log(target.unitName + " has hit " + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " for " + tempdefenderDmg + " damage!");
                            CombatLog.instance.AddEvent(target.unitName + " has hit " + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " for " + tempdefenderDmg + " damage!");
						}

						//Ult charge
						if (target is PlayerUnit)
						{
							if (tempdefenderDmg > target.currentHP)
							{
								Grid.instance.ultCharge += target.currentHP / 3;
							}
							else
							{
								Grid.instance.ultCharge += tempdefenderDmg / 3;
							}
						}
						else if (Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer] is PlayerUnit)
						{
							if (tempdefenderDmg > target.currentHP)
							{
								Grid.instance.ultCharge += target.currentHP / 5;
							}
							else
							{
								Grid.instance.ultCharge += tempdefenderDmg / 5;
							}
						}

						if (Grid.instance.ultCharge > 100)
						{
							Grid.instance.ultCharge = 100;
						}

						Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].currentHP -= tempdefenderDmg;

					}
					else
					{
						Debug.Log("Name: " + target.unitName + " Hit: " + defenderHit + " DMG: " + defenderDmg + " Crit: " + defenderCritChance);
						Debug.Log("Hit Roll: " + counterHit);

						Debug.Log(target.unitName + " missed!");
                        CombatLog.instance.AddEvent(target.unitName + " missed!");
                    }

					if (Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].currentHP <= 0)
					{
						Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].currentHP = 0;
						Debug.Log(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " has died!");
                        CombatLog.instance.AddEvent(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " has died!");

						// Add exp if attacker is a PlayerUnit
						if (Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer] is PlayerUnit)
						{
							this.battleEXP(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer], target, attackerDmg, true);
						}

						// Deactivate menu GUI and set unit to not attacking after the attack
						Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].playerWait();
						CombatLog.instance.PrintEvent();
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
                    CombatLog.instance.AddEvent(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " does a follow-up attack!");

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
                            CombatLog.instance.AddEvent(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " has critically hit " + target.unitName + " for " + tempattackerDmg + " damage!");
						}
						else
						{
							tempattackerDmg = attackerDmg;
							Debug.Log(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " has hit " + target.unitName + " for " + tempattackerDmg + " damage!");
                            CombatLog.instance.AddEvent(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " has hit " + target.unitName + " for " + tempattackerDmg + " damage!");
						}

						//Ult charge
						if (Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer] is PlayerUnit)
						{
							if (tempattackerDmg > target.currentHP)
							{
								Grid.instance.ultCharge += target.currentHP / 3;
							}
							else
							{
								Grid.instance.ultCharge += tempattackerDmg / 3;
							}
						}
						else if (target is PlayerUnit)
						{
							if (tempattackerDmg > target.currentHP)
							{
								Grid.instance.ultCharge += target.currentHP / 5;
							}
							else
							{
								Grid.instance.ultCharge += tempattackerDmg / 5;
							}
						}

						if (Grid.instance.ultCharge > 100)
						{
							Grid.instance.ultCharge = 100;
						}

						target.currentHP -= tempattackerDmg;

					}
					// If miss
					else
					{
						Debug.Log("Name: " + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " Hit: " + attackerHit + " DMG: " + attackerDmg + " Crit: " + attackerCritChance);
						Debug.Log("Hit Roll: " + hit1);

						Debug.Log(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " missed!");
                        CombatLog.instance.AddEvent(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " missed!");
					}



					// Check if target is dead
					if (target.currentHP <= 0)
					{
						target.currentHP = 0;
						Debug.Log(target.unitName + " has died!");
                        CombatLog.instance.AddEvent(target.unitName + " has died!");

						// Add exp if attacker is a PlayerUnit
						if (Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer] is PlayerUnit)
						{
							this.battleEXP(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer], target, attackerDmg, true);
						}

						// Deactivate menu GUI and set unit to not attacking after the attack
						Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].playerWait();

						CombatLog.instance.PrintEvent();
						return;
					}
					else
					{
						Debug.Log(target.unitName + ": " + target.currentHP + "/" + target.maxHP + ", " + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + ": " + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].currentHP + "/" + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].maxHP);
					}
				}


				// FOllowup Counter
				if (target.currentHP > 0 && canCounter && defAS - atkAS >= 4)
				{
					Debug.Log(target.unitName + " does a follow up counterattack!");
                    CombatLog.instance.AddEvent(target.unitName + " does a follow up counterattack!");
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
                            CombatLog.instance.AddEvent(target.unitName + " has critically hit " + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " for " + tempdefenderDmg + " damage!");
						}
						else
						{
							tempdefenderDmg = defenderDmg;
							Debug.Log(target.unitName + " has hit " + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " for " + tempdefenderDmg + " damage!");
                            CombatLog.instance.AddEvent(target.unitName + " has hit " + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " for " + tempdefenderDmg + " damage!");
						}

						//Ult charge
						if (target is PlayerUnit)
						{
							if (tempdefenderDmg > target.currentHP)
							{
								Grid.instance.ultCharge += target.currentHP / 3;
							}
							else
							{
								Grid.instance.ultCharge += tempdefenderDmg / 3;
							}
						}
						else if (Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer] is PlayerUnit)
						{
							if (tempdefenderDmg > target.currentHP)
							{
								Grid.instance.ultCharge += target.currentHP / 5;
							}
							else
							{
								Grid.instance.ultCharge += tempdefenderDmg / 5;
							}
						}

						if (Grid.instance.ultCharge > 100)
						{
							Grid.instance.ultCharge = 100;
						}

						Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].currentHP -= tempdefenderDmg;

					}
					else
					{
						Debug.Log("Name: " + target.unitName + " Hit: " + defenderHit + " DMG: " + defenderDmg + " Crit: " + defenderCritChance);
						Debug.Log("Hit Roll: " + counterHit);

						Debug.Log(target.unitName + " missed!");
                        CombatLog.instance.AddEvent(target.unitName + " missed!");
					}

					if (Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].currentHP <= 0)
					{
						Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].currentHP = 0;
						Debug.Log(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " has died!");
                        CombatLog.instance.AddEvent(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " has died!");
					}
					else
					{
						Debug.Log(target.unitName + ": " + target.currentHP + ", " + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + ": " + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].currentHP);
					}


					// Check if attacker is dead
					if (Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].currentHP <= 0)
					{
						Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].currentHP = 0;

						// Add exp if attacker is a PlayerUnit
						if (Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer] is PlayerUnit)
						{
							this.battleEXP(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer], target, attackerDmg, true);
						}

						// Deactivate menu GUI and set unit to not attacking after the attack
						Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].playerWait();
						CombatLog.instance.PrintEvent();
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

			// Deactivate menu GUI and set unit to not attacking after the attack
			Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].playerWait();
			CombatLog.instance.PrintEvent();
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
	* v1.2
	* Checks for equipped item
	* 
	* To do
	* Does not factor in supports, other misc things
	* No weapon triangle
	* 
	* @param attacker The attacking unit
	* @param target The target of the attack
	* @author Jeffrey Goh
	* @version 1.2
	* @updated 5/7/2017
	*/
	public int battleForecast(Unit attacker, Unit target)
	{
		if (attacker.equippedIndex != -1)
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

			if (target.weaponMinRange <= dist && target.weaponMaxRange + target.weaponRangeBuff >= dist)
			{
				canCounter = true;
			}

			//Cancel counterattack if rebelBuff is on for the attacker
			if (attacker.rebelBuff)
			{
				canCounter = false;
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
		else
		{
			return 0;
		}
	}

	/**
	* Allows player units to gain exp and level up from combat
	* 
	* v1.1
	* Updated EXP formula to account for over 100% growths
	* 
	* @param attacker The attacking unit
	* @param defender The target of the attack
	* @param damage The damage dealt in combat
	* @param defeated Whether the target was defeated
	* @author Jeffrey Goh
	* @version 1.1
	* @updated 2/7/2017
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
			attacker.exp += expGain;
			Debug.Log(attacker.unitName + " has gained " + expGain + " exp!");
            CombatLog.instance.AddEvent(attacker.unitName + " has gained " + expGain + " exp!");
			//Level up
			while (attacker.exp >= 100)
			{
				attacker.exp -= 100;
				attacker.lvl += 1;
				Debug.Log(attacker.unitName + " has levelled up to level " + attacker.lvl);
                CombatLog.instance.AddEvent(attacker.unitName + " has levelled up to level " + attacker.lvl);
				int roll;
				int statup = 0;
				int tempG = attacker.hpG;
				do
				{
					roll = Random.Range(1, 100);
					if (roll <= tempG)
					{
						statup += 1;
					}
					tempG -= 100;
				} while (tempG > 0);
				if (statup > 0)
				{
					attacker.maxHP += statup;
					attacker.currentHP += statup;
					Debug.Log(attacker.unitName + " has gained " + statup + " HP!");
                    CombatLog.instance.AddEvent(attacker.unitName + " has gained " + statup + " HP!");
                    statup = 0;
				}
				
				tempG = attacker.strG;
				do
				{
					roll = Random.Range(1, 100);
					if (roll <= tempG)
					{
						statup += 1;
					}
					tempG -= 100;
				} while (tempG > 0);
				if (statup > 0)
				{
					attacker.strength += statup;
					Debug.Log(attacker.unitName + " has gained " + statup + " STR!");
                    CombatLog.instance.AddEvent(attacker.unitName + " has gained " + statup + " STR!");
					statup = 0;
				}

				tempG = attacker.magG;
				do
				{
					roll = Random.Range(1, 100);
					if (roll <= tempG)
					{
						statup += 1;
					}
					tempG -= 100;
				} while (tempG > 0);
				if (statup > 0)
				{
					attacker.mag += statup;
					Debug.Log(attacker.unitName + " has gained " + statup + " MAG!");
                    CombatLog.instance.AddEvent(attacker.unitName + " has gained " + statup + " MAG!");

                    statup = 0;
				}

				tempG = attacker.sklG;
				do
				{
					roll = Random.Range(1, 100);
					if (roll <= tempG)
					{
						statup += 1;
					}
					tempG -= 100;
				} while (tempG > 0);
				if (statup > 0)
				{
					attacker.skl += statup;
					Debug.Log(attacker.unitName + " has gained " + statup + " SKL!");
                    CombatLog.instance.AddEvent(attacker.unitName + " has gained " + statup + " SKL!");
					statup = 0;
				}

				tempG = attacker.spdG;
				do
				{
					roll = Random.Range(1, 100);
					if (roll <= tempG)
					{
						statup += 1;
					}
					tempG -= 100;
				} while (tempG > 0);
				if (statup > 0)
				{
					attacker.spd += statup;
					Debug.Log(attacker.unitName + " has gained " + statup + " SPD!");
                    CombatLog.instance.AddEvent(attacker.unitName + " has gained " + statup + " SPD!");
					statup = 0;
				}

				tempG = attacker.lukG;
				do
				{
					roll = Random.Range(1, 100);
					if (roll <= tempG)
					{
						statup += 1;
					}
					tempG -= 100;
				} while (tempG > 0);
				if (statup > 0)
				{
					attacker.luk += statup;
					Debug.Log(attacker.unitName + " has gained " + statup + " LUK!");
                    CombatLog.instance.AddEvent(attacker.unitName + " has gained " + statup + " LUK!");
					statup = 0;
				}

				tempG = attacker.defG;
				do
				{
					roll = Random.Range(1, 100);
					if (roll <= tempG)
					{
						statup += 1;
					}
					tempG -= 100;
				} while (tempG > 0);
				if (statup > 0)
				{
					attacker.def += statup;
					Debug.Log(attacker.unitName + " has gained " + statup + " DEF!");
                    CombatLog.instance.AddEvent(attacker.unitName + " has gained " + statup + " DEF!");
					statup = 0;
				}

				tempG = attacker.resG;
				do
				{
					roll = Random.Range(1, 100);
					if (roll <= tempG)
					{
						statup += 1;
					}
					tempG -= 100;
				} while (tempG > 0);
				if (statup > 0)
				{
					attacker.res += statup;
					Debug.Log(attacker.unitName + " has gained " + statup + " RES!");
                    CombatLog.instance.AddEvent(attacker.unitName + " has gained " + statup + " RES!");
					statup = 0;
				}
			}
            CombatLog.instance.PrintEvent();
            return;
		}
	}

	/**
	* Heals the target unit, based on the equipped weapons
	*
	* 
	* @param target The target of the attack
	* @param staff The stats of the staff
	* @author Jeffrey Goh
	* @version 1.0
	* @updated 9/7/2017
	*/
	public void healWithCurrentUnit(Unit target, string[] staff)
	{
		if (Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].allies.Contains(target.team))
		{
			if (target.currentHP < target.maxHP)
			{
				int tempHP = target.currentHP;
				// Heal based on staff Mt + caster's Mag
				target.currentHP += int.Parse(staff[8]) + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].mag;

				//HP caps at target's maxHP
				if (target.currentHP > target.maxHP)
				{
					target.currentHP = target.maxHP;
				}

				Debug.Log(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " has healed " + target.unitName + " for " + (Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].currentHP - tempHP) + " HP!");
                CombatLog.instance.AddEvent(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " has healed " + target.unitName + " for " + (Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].currentHP - tempHP) + " HP!");
				Debug.Log(target.unitName + "'s HP: "+ Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].currentHP + "/" + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].maxHP);

				//Gain EXP based on staff used
				Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].exp += int.Parse(staff[7]);
				Debug.Log(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " has gained " + int.Parse(staff[7]) + " exp!");
                CombatLog.instance.AddEvent(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " has gained " + int.Parse(staff[7]) + " exp!");

				//Level Up
				while (Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].exp >= 100)
				{
					Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].exp -= 100;
					Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].lvl += 1;
					Debug.Log(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " has levelled up to level " + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].lvl);
                    CombatLog.instance.AddEvent(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " has levelled up to level " + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].lvl);
					int roll;
					int statup = 0;
					int tempG = Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].hpG;
					do
					{
						roll = Random.Range(1, 100);
						if (roll <= tempG)
						{
							statup += 1;
						}
						tempG -= 100;
					} while (tempG > 0);
					if (statup > 0)
					{
						Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].maxHP += statup;
						Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].currentHP += statup;
						Debug.Log(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " has gained " + statup + " HP!");
                        CombatLog.instance.AddEvent(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " has gained " + statup + " HP!");

                        statup = 0;
					}

					tempG = Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].strG;
					do
					{
						roll = Random.Range(1, 100);
						if (roll <= tempG)
						{
							statup += 1;
						}
						tempG -= 100;
					} while (tempG > 0);
					if (statup > 0)
					{
						Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].strength += statup;
						Debug.Log(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " has gained " + statup + " STR!");
                        CombatLog.instance.AddEvent(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " has gained " + statup + " STR!");
						statup = 0;
					}

					tempG = Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].magG;
					do
					{
						roll = Random.Range(1, 100);
						if (roll <= tempG)
						{
							statup += 1;
						}
						tempG -= 100;
					} while (tempG > 0);
					if (statup > 0)
					{
						Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].mag += statup;
						Debug.Log(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " has gained " + statup + " MAG!");
                        CombatLog.instance.AddEvent(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " has gained " + statup + " MAG!");
						statup = 0;
					}

					tempG = Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].sklG;
					do
					{
						roll = Random.Range(1, 100);
						if (roll <= tempG)
						{
							statup += 1;
						}
						tempG -= 100;
					} while (tempG > 0);
					if (statup > 0)
					{
						Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].skl += statup;
						Debug.Log(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " has gained " + statup + " SKL!");
                        CombatLog.instance.AddEvent(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " has gained " + statup + " SKL!");

                        statup = 0;
					}

					tempG = Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].spdG;
					do
					{
						roll = Random.Range(1, 100);
						if (roll <= tempG)
						{
							statup += 1;
						}
						tempG -= 100;
					} while (tempG > 0);
					if (statup > 0)
					{
						Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].spd += statup;
						Debug.Log(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " has gained " + statup + " SPD!");
                        CombatLog.instance.AddEvent(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " has gained " + statup + " SPD!");
						statup = 0;
					}

					tempG = Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].lukG;
					do
					{
						roll = Random.Range(1, 100);
						if (roll <= tempG)
						{
							statup += 1;
						}
						tempG -= 100;
					} while (tempG > 0);
					if (statup > 0)
					{
						Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].luk += statup;
						Debug.Log(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " has gained " + statup + " LUK!");
                        CombatLog.instance.AddEvent(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " has gained " + statup + " LUK!");
						statup = 0;
					}

					tempG = Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].defG;
					do
					{
						roll = Random.Range(1, 100);
						if (roll <= tempG)
						{
							statup += 1;
						}
						tempG -= 100;
					} while (tempG > 0);
					if (statup > 0)
					{
						Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].def += statup;
						Debug.Log(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " has gained " + statup + " DEF!");
                        CombatLog.instance.AddEvent(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " has gained " + statup + " DEF!");
						statup = 0;
					}

					tempG = Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].resG;
					do
					{
						roll = Random.Range(1, 100);
						if (roll <= tempG)
						{
							statup += 1;
						}
						tempG -= 100;
					} while (tempG > 0);
					if (statup > 0)
					{
						Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].res += statup;
						Debug.Log(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " has gained " + statup + " RES!");
                        CombatLog.instance.AddEvent(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName + " has gained " + statup + " RES!");
						statup = 0;
					}
				}
				Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].playerWait();
				Grid.instance.removeTileHighlight();
                CombatLog.instance.PrintEvent();
                return;
			}
			else
			{
				Debug.Log(target.unitName + "'s HP is full.");
                CombatLog.instance.AddEvent(target.unitName + "'s HP is full.");
                CombatLog.instance.PrintEvent();
			}
		}
		else
		{
			Debug.Log("Invalid target");
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIUnit : Unit
{
	Tile targetTile = null;

	/*
	 * 0: Aggressive
	 * 1: Passive
	 * 2: Stationary
	 * 3: Target (To be implemented)
	 */
	public int ai_id;

	// Use this for initialization
	void Start()
	{

	}

	/// Update is called once per frame
	void Update()
	{
		//If it is your team's turn
		if (Grid.instance.currentTeam == team)
		{
			// If the unit has finished its action, it is grey, otherwise, it is cyan
			if (doneAction)
			{
				GetComponent<Renderer>().material.color = Color.grey;
			}
			else
			{
				GetComponent<Renderer>().material.color = Color.magenta;
			}
		}
		else
		{
			//If it is not your team's turn, unit is grey
			GetComponent<Renderer>().material.color = Color.grey;
		}

		if (currentHP <= 0)
		{
            //Kept for debugging purposes
            //GetComponent<Renderer>().material.color = Color.red;

            //Object disappears if dead
            gameObject.SetActive(false);
           

			//Turn the tile this unit was standing on free
			Grid.instance.map[(int)gridPosition.x][(int)gridPosition.y].occupied = null;
		}
	}

	/**
	* The AI of the AIUnit
	* v1.0 
	* Movement, bsaically the same as PlayerUnit
	* 
	* v1.1
	* AIUnit will attack the weakest unit in range
	*
	* v1.2
	* Added Passive and Stationary AI
	* @author Jeffrey Goh
	* @version 1.2
	* @updated 24/6/2017
	*/
	public override void turnUpdate()
	{
		if (positionQueue.Count > 0)
		{
            mainCam.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, mainCam.transform.position.z);

            if (Vector3.Distance(positionQueue[0], transform.position) > 0.1f)
			{
				transform.position += ((Vector3)positionQueue[0] - transform.position).normalized * moveSpeed * Time.deltaTime;
                mainCam.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, mainCam.transform.position.z);
				if (Vector3.Distance(positionQueue[0], transform.position) <= 0.1f)
				{
					transform.position = positionQueue[0];
					positionQueue.RemoveAt(0);
					if (positionQueue.Count == 0)
					{
						isMoving = false;
						doneMoving = true;
					}
				}
			}
		}
		else if (doneMoving)
		{
			// If we have moved
			if (willAttack)
			{
				// If moved into position to attack, attack
				isFighting = true;
				Grid.instance.attackWithCurrentUnit(targetTile);
			}
			else
			{
				//End turn
				Grid.instance.removeTileHighlight();
				isMoving = false;
				isFighting = false;
				doneAction = true;
				targetTile = null;
				Grid.instance.totalDone++;
			}
		}
		else
		{
			targetTile = null;
			if (ai_id == 0)
			{
				//Agressive AI
				int tempMov = mov;
				// Find all enemies within range
				Dictionary<Tile, List<Tile>> enemiesInRange = TileHighlight.FindTarget(Grid.instance.map[(int)gridPosition.x][(int)gridPosition.y], 1, tempMov, weaponMinRange, weaponMaxRange, allies, true);
				List<Tile> target = new List<Tile>();

				// Find the best target to approach
				while (true)
				{
					// Find all enemies within range
					enemiesInRange = TileHighlight.FindTarget(Grid.instance.map[(int)gridPosition.x][(int)gridPosition.y], 1, tempMov, weaponMinRange, weaponMaxRange, allies, true);

					Debug.Log("In Range: " + enemiesInRange.Count);
					target = chooseTarget(enemiesInRange);
					if (target == null)
					{
						tempMov++;
						continue;
					}
					else if (target[0] == null)
					{
						tempMov++;
						continue;
					}
					else
					{
						break;
					}
				}

				//If we can reach a target, move to the appropriate tile
				if (target[1] != null && tempMov == mov)
				{
					isMoving = true;
					doneMoving = true;
					Grid.instance.moveCurrentUnit(target[1]);
					willAttack = true;
				}
				else
				{

					List<Tile> available = TileHighlight.FindHighlight(Grid.instance.map[(int)gridPosition.x][(int)gridPosition.y], 1, mov, allies, true);
					int distanceTo = 1;
					while (true)
					{
						// Otherwise, approach as close as possible

						List<Tile> possibilities = TileHighlight.FindHighlight(target[0], distanceTo, distanceTo, allies, true);

						foreach (Tile t in possibilities)
						{
							if (available.Contains(t))
							{
								isMoving = true;
								doneMoving = true;
								Grid.instance.moveCurrentUnit(t);
								return;
							}
						}
						distanceTo++;
					}
				}
			}
			else if (ai_id == 1)
			{
				//Passive AI
				// Replace this with 0 and don't increment for stationary enemies
				int tempMov = mov;
				// Find all enemies within range
				Dictionary<Tile, List<Tile>> enemiesInRange = TileHighlight.FindTarget(Grid.instance.map[(int)gridPosition.x][(int)gridPosition.y], 1, tempMov, weaponMinRange, weaponMaxRange, allies, true);
				List<Tile> target = new List<Tile>();

				// Find all enemies within range
				enemiesInRange = TileHighlight.FindTarget(Grid.instance.map[(int)gridPosition.x][(int)gridPosition.y], 1, tempMov, weaponMinRange, weaponMaxRange, allies, true);

				Debug.Log("In Range: " + enemiesInRange.Count);
				target = chooseTarget(enemiesInRange);

				if (target == null)
				{
					// End Turn
					Grid.instance.removeTileHighlight();
					isMoving = false;
					isFighting = false;
					doneAction = true;
					targetTile = null;
					Grid.instance.totalDone++;
				}

				//If we can reach a target, move to the appropriate tile
				if (target[1] != null && tempMov == mov)
				{
					isMoving = true;
					doneMoving = true;
					Grid.instance.moveCurrentUnit(target[1]);
					willAttack = true;
				}
				else
				{
					doneMoving = true;
					
				}
			}
			else if (ai_id == 2)
			{
				//Stationary AI
				int tempMov = 0;
				// Find all enemies within range
				Dictionary<Tile, List<Tile>> enemiesInRange = TileHighlight.FindTarget(Grid.instance.map[(int)gridPosition.x][(int)gridPosition.y], 1, tempMov, weaponMinRange, weaponMaxRange, allies, true);
				List<Tile> target = new List<Tile>();

				// Find all enemies within range
				enemiesInRange = TileHighlight.FindTarget(Grid.instance.map[(int)gridPosition.x][(int)gridPosition.y], 1, tempMov, weaponMinRange, weaponMaxRange, allies, true);

				Debug.Log("In Range: " + enemiesInRange.Count);
				target = chooseTarget(enemiesInRange);

				if (target == null)
				{
					// End Turn
					Grid.instance.removeTileHighlight();
					isMoving = false;
					isFighting = false;
					doneAction = true;
					targetTile = null;
					Grid.instance.totalDone++;
				}

				if (target[1] != null)
				{
					doneMoving = true;
					willAttack = true;
				}
			}
		}
			base.turnUpdate();
	}

	/**
	 * v1.1
	 * Bug fixes for units that deal 0 damage
	 * 
	 * Choose a target to move to and attack
	 * Returns null if there are no units in range
	 * Returns the tile containing the unit that this unit will target and the tile this unit will attack from
	 * Returns the unit this unit can deal the most damage to if this unit cannot reach any unit in range due to obstructions, and null since it cannot actually hit the target
	 * @param enemiesInRange The tiles of enemies we want to consider to choose as our target
	 * @author Jeffrey Goh
	 * @version 1.1
	 * @updated 24/6/2017
	 */
	public List<Tile> chooseTarget(Dictionary<Tile, List<Tile>> enemiesInRange)
	{
		Tile primeTarget = null;
		int originalCount = enemiesInRange.Count;

		List<Tile> toReturn = new List<Tile>();

		if (enemiesInRange.Count > 0)
		{
			while (!doneAction)
			{
				if (enemiesInRange.Count == 0)
				{
					// We return primeTarget instead of null since there exists a unit that is in range, but cannot be attacked
					toReturn.Add(primeTarget);
					toReturn.Add(null);

					return toReturn;
				}
				else
				{
					// Attack if any units in range
					// Attack a unit that will die if no misses, crits or any special effects
					// if none will die, attack the unit that will take the most damage
					bool hasKillableTarget = false;
					bool targetWillDie = false;
					int maxDmg = -1;

					// Determine which unit is the target 
					foreach (Tile t in enemiesInRange.Keys)
					{
						targetWillDie = false;
						int dmgDealt = Grid.instance.battle.battleForecast(this, t.occupied);
						if (dmgDealt >= t.occupied.currentHP)
						{
							// If it will kill a target, then that target has a higher priority over any target that will not die
							targetWillDie = true;
							dmgDealt = t.occupied.currentHP;
						}

						if (targetWillDie && !hasKillableTarget)
						{
							// If there were no other targets that would die, set the current target to be the base of comparison
							targetTile = t;
							maxDmg = dmgDealt;
							hasKillableTarget = true;
						}
						else if (((targetWillDie && hasKillableTarget) || (!targetWillDie && !hasKillableTarget)) && dmgDealt > maxDmg)
						{
							// If the target would die and there were other targets that would die
							// Or if both this target and no other target so far would die
							// And if attacking the current target will deal more damage
							// Set maxDmg and targetTile to be the current target's
							maxDmg = dmgDealt;
							targetTile = t;
						}
					}

					// To keep track of the number of occupied tiles that would have otherwise allowed this unit to hit the target
					int occupiedCount = 0;
					if (enemiesInRange.ContainsKey(targetTile))
					{
						foreach (Tile x in enemiesInRange[targetTile])
						{
							if ((x.occupied == null || x == Grid.instance.map[(int)gridPosition.x][(int)gridPosition.y]))
							{
								toReturn.Add(targetTile);
								toReturn.Add(x);
								return toReturn;
							}
							else
							{
								occupiedCount++;
							}
						}


						// If every tile is occupied, remove this target and recalculate
						if (occupiedCount == enemiesInRange[targetTile].Count)
						{
							if (enemiesInRange.Count == originalCount)
							{
								primeTarget = targetTile;
							}
							enemiesInRange.Remove(targetTile);
							continue;
						}
					}
					else
					{
						enemiesInRange.Remove(targetTile);
						continue;
					}
				}
			}
			// If action was taken, return null
			return null;
		}
		else
		{
			// If there are no enemies in range to begin with, return null
			return null;
		}
	}

	private void OnMouseDown()
	{
		if (Grid.instance.currentPlayer != -1)
		{
			if (Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].isFighting && Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer] != this)
			{
				Grid.instance.battle.attackWithCurrentUnit(this);
			}
			else if (Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].isHealing && Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer] != this)
			{
				Grid.instance.battle.healWithCurrentUnit(this, Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].inventory[Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].activeStaffIndex]);
			}
			else if (Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].isTalking && canTalk.ContainsKey(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].unitName) && Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer] != this)
			{
				Grid.instance.talkWithCurrentUnit(this);
			}
		}
	}

	public override void OnGUI()
	{
		base.OnGUI();
	}
}

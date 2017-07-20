using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AIUnit : Unit
{
	public Tile targetTile;
	public Tile objectiveTile = null;
	public Unit objectiveUnit = null;



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
			if (doneAction)
			{
				if (allies.Contains(0))
				{
					//If ally has done action
					GetComponent<Renderer>().material.color = Color.white;
				}
				else
				{
					//If enemy has done action
					GetComponent<Renderer>().material.color = Color.magenta;
				}
			}
			else
			{
				if (allies.Contains(0))
				{
					//If ally
					GetComponent<Renderer>().material.color = Color.green;
				}
				else
				{
					GetComponent<Renderer>().material.color = Color.red;
				}
			}
		}
		else
		{
			//If it is not your team's turn
			if (allies.Contains(0))
			{
				//If ally
				GetComponent<Renderer>().material.color = Color.white;
			}
			else
			{
				GetComponent<Renderer>().material.color = Color.magenta;
			}
		}

		if (currentHP <= 0)
		{
            //Kept for debugging purposes
            //GetComponent<Renderer>().material.color = Color.red;

            //Object disappears if dead
            gameObject.SetActive(false);

			if (Grid.instance.highlightedEnemies.Contains(this))
			{
				Grid.instance.highlightedEnemies.Remove(this);
			}
           

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
	* 
	* v1.3
	* Updated movement to fix rare bug
	* 
	* @author Jeffrey Goh
	* @version 1.3
	* @updated 19/7/2017
	*/
	public override void turnUpdate()
	{
		if (positionQueue.Count > 0)
		{

			mainCam.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, mainCam.transform.position.z);

			if (Vector3.Distance(positionQueue[0], transform.position) > 0.1f)
			{
				transform.position = Vector3.MoveTowards(transform.position, (Vector3)positionQueue[0], moveSpeed * Time.deltaTime);
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
			else if (foundCity)
			{
				if (Grid.instance.villageStatus[gridPosition][0] != team)
				{
					Grid.instance.villageStatus[gridPosition][1] -= 1;
					Debug.Log("Capturing Village");
					if (Grid.instance.villageStatus[gridPosition][1] == 0)
					{
						//Convert the village to your side
						Grid.instance.villageStatus[gridPosition][0] = team;
						Grid.instance.villageStatus[gridPosition][1] = 2;
						Debug.Log("Village has been captured.");
					}
					foundCity = false;
					playerWait();
				}
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
				int tempMove = 1;
				int tempMov = mov;
				// Find all enemies within range
				Dictionary<Tile, List<Tile>> enemiesInRange = TileHighlight.FindTarget(Grid.instance.map[(int)gridPosition.x][(int)gridPosition.y], tempMove, tempMov, weaponMinRange, weaponMaxRange + weaponRangeBuff, allies, true, isFlying);
				List<Tile> target = new List<Tile>();

				// Find the best target to approach
				while (true)
				{
					// Find all enemies within range
					enemiesInRange = TileHighlight.FindTarget(Grid.instance.map[(int)gridPosition.x][(int)gridPosition.y], tempMove, tempMov, weaponMinRange, weaponMaxRange + weaponRangeBuff, allies, true, isFlying);

					//Debug.Log("In Range: " + enemiesInRange.Count);
					target = chooseTarget(enemiesInRange);
					if (target == null)
					{
						tempMove = tempMov;
						tempMov++;
						continue;
					}
					else if (target[0] == null)
					{
						tempMove = tempMov;
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

					List<Tile> available = TileHighlight.FindHighlight(Grid.instance.map[(int)gridPosition.x][(int)gridPosition.y], 1, mov, allies, true, isFlying);
					int distanceTo = 1;
					while (true)
					{
						// Otherwise, approach as close as possible

						List<Tile> possibilities = TileHighlight.FindHighlight(target[0], distanceTo, distanceTo, allies, true, isFlying);

						foreach (Tile t in possibilities)
						{
							if (available.Contains(t))
							{
								isMoving = true;
								doneMoving = true;
								Grid.instance.moveCurrentUnit(t);
								return;
							}
							else if (t.gridPosition == gridPosition)
							{
								doneMoving = true;
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
				// Find all enemies within range
				Dictionary<Tile, List<Tile>> enemiesInRange = TileHighlight.FindTarget(Grid.instance.map[(int)gridPosition.x][(int)gridPosition.y], 1, mov, weaponMinRange, weaponMaxRange + weaponRangeBuff, allies, true, isFlying);
				List<Tile> target = new List<Tile>();

				// Find all enemies within range
				enemiesInRange = TileHighlight.FindTarget(Grid.instance.map[(int)gridPosition.x][(int)gridPosition.y], 1, mov, weaponMinRange, weaponMaxRange + weaponRangeBuff, allies, true, isFlying);

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
				else if (target[1] != null)
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
				// Find all enemies within range
				Dictionary<Tile, List<Tile>> enemiesInRange = TileHighlight.FindTarget(Grid.instance.map[(int)gridPosition.x][(int)gridPosition.y], 0, 0, weaponMinRange, weaponMaxRange + weaponRangeBuff, allies, true, isFlying);
				List<Tile> target = new List<Tile>();

				//Debug.Log("In Range: " + enemiesInRange.Count);
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

				else if (target[1] != null)
				{
					doneMoving = true;
					willAttack = true;
				}
			}
			else if (ai_id == 3)
			{
				//Target AI
				int tempMov = mov;
				// Find all enemies within range
				Dictionary<Tile, List<Tile>> enemiesInRange = TileHighlight.FindTarget(Grid.instance.map[(int)gridPosition.x][(int)gridPosition.y], 1, tempMov, weaponMinRange, weaponMaxRange + weaponRangeBuff, allies, true, isFlying);
				List<Tile> target = new List<Tile>();

				// Find the best target to approach
				while (true)
				{
					// Find all enemies within range
					enemiesInRange = TileHighlight.FindTarget(Grid.instance.map[(int)gridPosition.x][(int)gridPosition.y], 1, tempMov, weaponMinRange, weaponMaxRange + weaponRangeBuff, allies, true, isFlying);

					//Debug.Log("In Range: " + enemiesInRange.Count);
					target = chooseTarget(enemiesInRange);
					if (target == null)
					{
						target = new List<Tile>();
						if (objectiveUnit != null)
						{
							target.Add(Grid.instance.map[(int)objectiveUnit.gridPosition.x][(int)objectiveUnit.gridPosition.y]);
							break;
						}
						else if (objectiveTile != null)
						{
							target.Add(objectiveTile);
							break;
						}
						else
						{
							Debug.Log("No target");
							break;
						}
					}
					else
					{
						break;
					}
				}

				//If we can reach a target, move to the appropriate tile
				if (target.Count > 1)
				{
					if (target[1] != null && tempMov == mov)
					{
						isMoving = true;
						doneMoving = true;
						Grid.instance.moveCurrentUnit(target[1]);
						willAttack = true;
					}
				}
				//If we are already on the target tile with no enemies in range, stay put
				else if (Grid.instance.map[(int)gridPosition.x][(int)gridPosition.y] == objectiveTile)
				{
					doneMoving = true;
				}
				//Move to the objective tile if we can reach
				else if (TileHighlight.FindHighlight(Grid.instance.map[(int)gridPosition.x][(int)gridPosition.y], 1, tempMov, allies, true, isFlying).Contains(target[0]))
				{
					isMoving = true;
					doneMoving = true;
					Grid.instance.moveCurrentUnit(target[0]);
				}
				else
				{

					List<Tile> available = TileHighlight.FindHighlight(Grid.instance.map[(int)gridPosition.x][(int)gridPosition.y], 1, mov, allies, true, isFlying);
					int distanceTo = 1;
					while (true)
					{
						// Otherwise, approach as close as possible

						List<Tile> possibilities = TileHighlight.FindHighlight(target[0], distanceTo, distanceTo, allies, true, isFlying);

						foreach (Tile t in possibilities)
						{
							if (available.Contains(t))
							{
								isMoving = true;
								doneMoving = true;
								Grid.instance.moveCurrentUnit(t);
								return;
							}
							else if (t.gridPosition == gridPosition)
							{
								doneMoving = true;
								return;
							}
						}
						distanceTo++;
					}
				}
			}
			else if (ai_id == 4)
			{
				if (Grid.instance.villageStatus.ContainsKey(gridPosition))
				{
					if (Grid.instance.villageStatus[gridPosition][0] != team)
					{
						doneMoving = true;
						foundCity = true;
						return;
					}
				}
				//City seeking AI
				int tempMove = 1;
				int tempMov = mov;
				// Find all enemies within range
				List<Tile> citiesInRange = TileHighlight.FindCities(Grid.instance.map[(int)gridPosition.x][(int)gridPosition.y], tempMove, tempMov, allies, isFlying);

				// Find the best target to approach
				while (true)
				{
					// Find all enemies within range
					citiesInRange = TileHighlight.FindCities(Grid.instance.map[(int)gridPosition.x][(int)gridPosition.y], tempMove, tempMov, allies, isFlying);

					if (citiesInRange.Count == 0)
					{
						tempMove = tempMov;
						tempMov++;
						continue;
					}
					else
					{
						break;
					}

				}
				if (tempMove == 1)
				{
					//If there are cities in range
					foreach (Tile t in citiesInRange)
					{
						if (t.occupied == null)
						{
							isMoving = true;
							doneMoving = true;
							Grid.instance.moveCurrentUnit(t);
							foundCity = true;
							return;
						}
					}
						Debug.Log("There");
						ai_id = ai_id_priority[1];
				}
				else
				{
					Debug.Log("Here");
					List<Tile> available = TileHighlight.FindHighlight(Grid.instance.map[(int)gridPosition.x][(int)gridPosition.y], 1, mov, allies, true, isFlying);
					int distanceTo = 1;
					while (true)
					{
						// Otherwise, approach as close as possible

						List<Tile> possibilities = TileHighlight.FindHighlight(citiesInRange[0], distanceTo, distanceTo, allies, true, isFlying);

						foreach (Tile t in possibilities)
						{
							if (available.Contains(t))
							{
								isMoving = true;
								doneMoving = true;
								Grid.instance.moveCurrentUnit(t);
								return;
							}
							else if (t.gridPosition == gridPosition)
							{
								doneMoving = true;
								return;
							}
						}
						distanceTo++;
					}
				}
			}
			base.turnUpdate();
		}
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

	/**
	* Makes the current unit wait
	* Done to avoid typing this out every time
	* @author Jeffrey Goh
	* @version 1.0
	* @updated 11/7/2017
	*/
	public override void playerWait()
	{
		Grid.instance.removeTileHighlight();
		isMoving = false;
		isFighting = false;
		isHealing = false;
		activeStaffIndex = -1;
		doneAction = true;
		selected = false;
		displayInventory = false;
		selectedItemIndex = -1;
		isTalking = false;

		Grid.instance.totalDone++;
		Grid.instance.currentPlayer = -1;

		base.playerWait();
	}

	private void OnMouseDown()
	{
		if (!EventSystem.current.IsPointerOverGameObject())
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
			else
			{
				//Danger Range
				if (Grid.instance.highlightedEnemies.Contains(this))
				{
					Grid.instance.highlightedEnemies.Remove(this);
				}
				else
				{
					Grid.instance.highlightedEnemies.Add(this);
				}
			}
		}
	}

	public override void OnGUI()
	{
		base.OnGUI();
	}
}

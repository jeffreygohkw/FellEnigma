using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : Unit
{

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame

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
				GetComponent<Renderer>().material.color = Color.cyan;
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

			//Delete this unit from the list of units
			//Grid.instance.units[team].RemoveAt(index);

			//Shift the index of each unit after this unit in the list down by 1
			/*
			foreach (Unit u in Grid.instance.units[team])
			{
				if (u.index >= index)
				{
					u.index--;
				}
			}
			*/

			if (Grid.instance.currentPlayer == index)
			{
				Grid.instance.currentPlayer = -1;
			}
		}
	}


	/**
	* Move the current unit to the destination tile
	* Moves in an L shape to the destination, vertical first
	* Can navigate around obstacles, and will pick the shortest path
	* @param destTile The destination tile
	* @author Jeffrey Goh
	* @version 1.2
	* @updated 2/6/2017
	*/
	public override void turnUpdate()
	{
		if (positionQueue.Count > 0)
		{
			if (Vector3.Distance(positionQueue[0], transform.position) > 0.1f)
			{
				transform.position += ((Vector3)positionQueue[0] - transform.position).normalized * moveSpeed * Time.deltaTime;

				if (Vector3.Distance(positionQueue[0], transform.position) <= 0.1f)
				{
					transform.position = positionQueue[0];
					positionQueue.RemoveAt(0);
					if (positionQueue.Count == 0)
					{
						isMoving = false;
					}
				}
			}
		}


		//v1.1
		/*
		// Move to its destination
		if (Vector3.Distance(moveTo, transform.position) > 0.1f)
		{
			Debug.Log("End");
			transform.position += (moveTo - transform.position).normalized * moveSpeed * Time.deltaTime;

			// When the unit has reached its destination
			if (Vector3.Distance(moveTo, transform.position) <= 0.1f)
			{
				transform.position = moveTo;

				// Reset unit status after moving
				isMoving = false;
			}
		}
		*/
		base.turnUpdate();
	}


	private void OnMouseDown()
	{
		if (Grid.instance.currentTeam == team)
		{
			selected = !selected;
			if (selected)
			{
				Grid.instance.currentPlayer = index;
			}
		}
		if (Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].isFighting && Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer] != this)
		{
			Grid.instance.battle.attackWithCurrentUnit(this);
		}
		isMoving = false;
		isFighting = false;
	}


	/**
	* GUI buttons
	* 
	* v1.1
	* Added Wait
	* 
	* @param destTile The destination tile
	* @author Jeffrey Goh
	* @version 1.1
	* @updated 2/6/2017
	*/
	public override void OnGUI()
	{
		if (selected && !doneAction)
		{
			Rect buttonRect = new Rect(0, Screen.height - 200, 150, 50);

			//Move
			if (GUI.Button(buttonRect, "Move"))
			{
				Grid.instance.removeTileHighlight();
				isFighting = false;

				if (isMoving)
				{
					isMoving = false;
					Grid.instance.removeTileHighlight();
				}
				else
				{
					isMoving = true;
					Grid.instance.highlightTilesAt(gridPosition, new Vector4(0f,1f,0f,0.5f), 1, mov, true);
				}
			}


			buttonRect = new Rect(0, Screen.height - 150, 150, 50);

			//Attack
			if (GUI.Button(buttonRect, "Attack"))
			{
				Grid.instance.removeTileHighlight();
				isMoving = false;

				if (isFighting)
				{
					isFighting = false;
					Grid.instance.removeTileHighlight();
				}
				else
				{
					isFighting = true;
					Grid.instance.highlightTilesAt(gridPosition, new Vector4(1f,0f,0f,0.5f), weaponMinRange, weaponMaxRange, false);
				}
			}


			buttonRect = new Rect(0, Screen.height - 100, 150, 50);
			//Wait
			if (GUI.Button(buttonRect, "Wait"))
			{
				Grid.instance.removeTileHighlight();
				isMoving = false;
				isFighting = false;
				doneAction = true;
				Grid.instance.totalDone++;
			}



			buttonRect = new Rect(0, Screen.height - 50, 150, 50);

			//End Turn

			if (GUI.Button(buttonRect, "End"))
			{
				Grid.instance.removeTileHighlight();
				isMoving = false;
				isFighting = false;
				selected = false;
				Grid.instance.nextTurn();
			}
			base.OnGUI();
		}
	}
}
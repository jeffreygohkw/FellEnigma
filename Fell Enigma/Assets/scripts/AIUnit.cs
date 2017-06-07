using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIUnit : Unit
{

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
			Grid.instance.units[team].RemoveAt(index);

			//Shift the index of each unit after this unit in the list down by 1
			foreach (Unit u in Grid.instance.units[team])
			{
				if (u.index >= index)
				{
					u.index--;
				}
			}
		}
	}

	/**
	* Move the current unit to the destination tile
	* Moves in an L shape to the destination, vertical first
	* Can navigate around obstacles, and will pick the shortest path
	* @param destTile The destination tile
	* @author Jeffrey Goh
	* @version 1.0
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
		
		base.turnUpdate();
	}

	public override void OnGUI()
	{
		


		base.OnGUI();
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : Unit
{
	List<Vector2> lastPosition = new List<Vector2>();
	public bool displayInventory = false;
	public int selectedItemIndex = -1;
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
			// If the unit has finished its action, it is grey, if it is currently selected, it is blue, otherwise, it is cyan
			if (doneAction)
			{
				GetComponent<Renderer>().material.color = Color.grey;
			}
			else if (selected)
			{
				GetComponent<Renderer>().material.color = Color.blue;
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
			lastPosition.Clear();
		}

		if (currentHP <= 0)
		{
            //Kept for debugging purposes
            //GetComponent<Renderer>().material.color = Color.red;

            //Object disappears if dead
            this.GetComponent<StatsUI>().UI.enabled = false;
            gameObject.SetActive(false);

			//Turn the tile this unit was standing on free
			Grid.instance.map[(int)gridPosition.x][(int)gridPosition.y].occupied = null;

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
	* 
	* v1.3
	* Added compatibility for undo move, units can now only move once per turn
	* @param destTile The destination tile
	* @author Jeffrey Goh
	* @version 1.3
	* @updated 2/7/2017
	*/
	public override void turnUpdate()
	{
		if (!doneMoving)
		{
			lastPosition.Add(gridPosition);
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
							doneMoving = true;
							
						}
					}
				}
			}
		}
		base.turnUpdate();
	}


	private void OnMouseDown()
	{
		if (Grid.instance.currentTeam == team)
		{
			foreach (Unit u in Grid.instance.units[Grid.instance.currentTeam])
			{
				if (u.selected && u != this)
				{
					// Don't select this unit if another unit is selected
					return;
				}
			}
			if (!doneAction)
			{
				selected = !selected;
			}
			if (selected && !doneAction)
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
	* v1.2
	* Added Undo Move
	* 
	* @param destTile The destination tile
	* @author Jeffrey Goh
	* @version 1.2
	* @updated 2/7/2017
	*/
	public override void OnGUI()
	{
		if (selected && !doneAction)
		{
			Rect buttonRect = new Rect(0, Screen.height - 250, 150, 50);

			if (!doneMoving)
			{
				//Move
				if (GUI.Button(buttonRect, "Move"))
				{
					if (!doneMoving)
					{
						displayInventory = false;
						selectedItemIndex = -1;
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
							Grid.instance.highlightTilesAt(gridPosition, new Vector4(0f, 1f, 0f, 0.5f), 1, mov, true);
						}
					}
				}
			}
			else 
			{
				//Undo Move
				if (GUI.Button(buttonRect, "Undo Move"))
				{
					if (lastPosition.Count != 0)
					{
						displayInventory = false;
						selectedItemIndex = -1;
						Grid.instance.map[(int)gridPosition.x][(int)gridPosition.y].occupied = null;
						transform.position = Grid.instance.map[(int)lastPosition[0].x][(int)lastPosition[0].y].transform.position;
						this.gridPosition = lastPosition[0];
						Grid.instance.map[(int)gridPosition.x][(int)gridPosition.y].occupied = this;
					}
					lastPosition.Clear();
					doneMoving = false;
				}
			}
			buttonRect = new Rect(0, Screen.height - 200, 150, 50);

			//Attack
			if (GUI.Button(buttonRect, "Attack"))
			{
				displayInventory = false;
				selectedItemIndex = -1;
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

			buttonRect = new Rect(0, Screen.height - 150, 150, 50);

			//Item
			if (GUI.Button(buttonRect, "Item"))
			{
				displayInventory = !displayInventory;
			}
			if (displayInventory)
			{
				Debug.Log(inventory.Count);
				if (inventory.Count >= 1)
				{
					Debug.Log(inventory[0][1]);
					buttonRect = new Rect(151, Screen.height - 150, 150, 50);
					if (GUI.Button(buttonRect, inventory[1][0]))
					{
						if (selectedItemIndex != 0)
						{
							selectedItemIndex = 0;
						}
						else
						{
							selectedItemIndex = -1;
						}
					}
				}

				
				if (inventory.Count >= 2)
				{
					buttonRect = new Rect(150, Screen.height - 100, 150, 50);
					if (GUI.Button(buttonRect, inventory[1][1]))
					{
						if (selectedItemIndex != 1)
						{
							selectedItemIndex = 1;
						}
						else
						{
							selectedItemIndex = -1;
						}
					}
				}
					
				if (inventory.Count >= 3)
				{
					buttonRect = new Rect(150, Screen.height - 50, 150, 50);
					if (GUI.Button(buttonRect, inventory[2][1]))
					{
						if (selectedItemIndex != 2)
						{
							selectedItemIndex = 2;
						}
						else
						{
							selectedItemIndex = -1;
						}
					}
				}
			}

			buttonRect = new Rect(0, Screen.height - 100, 150, 50);
			//Wait
			if (GUI.Button(buttonRect, "Wait"))
			{
				Grid.instance.removeTileHighlight();
				lastPosition.Clear();
				isMoving = false;
				isFighting = false;
				doneAction = true;
				selected = false;
				displayInventory = false;
				selectedItemIndex = -1;
				Grid.instance.totalDone++;
			}



			buttonRect = new Rect(0, Screen.height - 50, 150, 50);

			//End Turn

			if (GUI.Button(buttonRect, "End"))
			{
				Grid.instance.removeTileHighlight();
				lastPosition.Clear();
				isMoving = false;
				isFighting = false;
				selected = false;
				doneAction = true;
				displayInventory = false;
				selectedItemIndex = -1;
				Grid.instance.nextTurn();
			}
			base.OnGUI();
		}
	}


}
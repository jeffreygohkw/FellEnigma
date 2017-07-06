using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : Unit
{
	List<Vector2> lastPosition = new List<Vector2>();

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
	 * Equips an item in the PlayerUnit's inventory
	 * 
	 * v1.1
	 * Checks for weapon type
	 * 
	 * @param index index of the item
	 * @author Jeffrey Goh
	 * @version 1.1
	 * @updated 6/7/2017
	 */
	public void equipItem(int index)
	{
		// Don't equip if an invalid index is passed in
		if (inventory.Count < index)
		{
			Debug.Log("Invalid index");
		}
		else
		{
			//If not weapon, don't equip
			if (inventory[index].Length != 13)
			{
				Debug.Log("Not a weapon");
			}
			else if (!proficiency.Contains(inventory[index][0]))
			{
				Debug.Log("Cannot equip a weapon of this type.");
			}
			else
			{
				// Equip their weapon
				weaponMt = int.Parse(inventory[index][6]);

				if (inventory[index][0] != "Tome")
				{
					weaponPhysical = true;
				}
				else
				{
					weaponPhysical = false;
				}
				weaponAcc = int.Parse(inventory[index][7]);
				weaponCrit = int.Parse(inventory[index][8]);
				weaponWt = int.Parse(inventory[index][5]);
				weaponMinRange = int.Parse(inventory[index][3]);
				weaponMaxRange = int.Parse(inventory[index][4]);
				equippedIndex = index;
				Debug.Log(inventory[index][1] + " equipped!");
			}
		}
	}

	/**
	 * Uses an item in the PlayerUnit's inventory
	 * 
	 * @param index index of the item
	 * @author Jeffrey Goh
	 * @version 1.0
	 * @updated 6/7/2017
	 */
	public void useItem(int index)
	{
		// Don't equip if an invalid index is passed in
		if (inventory.Count < index)
		{
			Debug.Log("Invalid index");
		}
		else
		{
			if (inventory[index][0] == "Staff")
			{
				//Heal code here
			}
			else if (inventory[index][0] == "StatBoost")
			{
				if (inventory[index][5] == "maxHP")
				{
					maxHP += int.Parse(inventory[index][4]);
					currentHP += int.Parse(inventory[index][4]);
				}
				else if (inventory[index][5] == "strength")
				{
					strength += int.Parse(inventory[index][4]);
				}
				else if (inventory[index][5] == "mag")
				{
					mag += int.Parse(inventory[index][4]);
				}
				else if (inventory[index][5] == "skl")
				{
					skl += int.Parse(inventory[index][4]);
				}
				else if (inventory[index][5] == "spd")
				{
					spd += int.Parse(inventory[index][4]);
				}
				else if (inventory[index][5] == "def")
				{
					def += int.Parse(inventory[index][4]);
				}
				else if (inventory[index][5] == "luk")
				{
					luk += int.Parse(inventory[index][4]);
				}
				else if (inventory[index][5] == "res")
				{
					res += int.Parse(inventory[index][4]);
				}
				else if (inventory[index][5] == "mov")
				{
					mov += int.Parse(inventory[index][4]);
				}
				else if (inventory[index][5] == "con")
				{
					con += int.Parse(inventory[index][4]);
				}
				else
				{
					Debug.Log("Something's wrong with your stat booster");
					return;
				}
				discardItem(index);
				selectedItemIndex = -1;
			}
			else if (inventory[index][0] == "Consumable")
			{
				if (currentHP == maxHP)
				{
					Debug.Log("HP is full!");
				}
				else
				{
					currentHP += int.Parse(inventory[index][4]);
					if (currentHP > maxHP)
					{
						currentHP = maxHP;
					}
					discardItem(index);
					selectedItemIndex = -1;
				}
			}
			else if (inventory[index][0] == "Key")
			{
				// Unlock door code here
			}
			else
			{
				Debug.Log("Wrong Item type");
			}
		}
	}

	/**
		* Discards an item in the PlayerUnit's inventory
		* @param index index of the item
		* @author Jeffrey Goh
		* @version 1.0
		* @updated 5/7/2017
		*/
	public void discardItem(int index)
	{
		// Don't discard if an invalid index is passed in
		if (inventory.Count < index)
		{
			Debug.Log("Invalid index");
		}
		else
		{
			// Discard their weapon
			inventory.RemoveAt(index);
			// Make the unit unequipped if you discard their weapon
			if (equippedIndex == index)
			{
				equippedIndex = -1;
				weaponMinRange = 0;
				weaponMaxRange = 0;
				weaponMt = 0;
				weaponWt = 0;
				weaponCrit = 0;
			}
		}
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
						isFighting = false;
						Grid.instance.removeTileHighlight();
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

			// The actual Items
			if (displayInventory)
			{
				if (inventory.Count >= 1)
				{
					buttonRect = new Rect(151, Screen.height - 150, 150, 50);
					if (GUI.Button(buttonRect, inventory[0][1]))
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

			//What you can do with the items
			//Currently assuming weapons
			if (selectedItemIndex != -1 && displayInventory)
			{
				buttonRect = new Rect(300, Screen.height - 100, 150, 50);
				if (inventory[selectedItemIndex].Length == 13)
				{
					if (GUI.Button(buttonRect, "Equip"))
					{
						equipItem(selectedItemIndex);
					}
				}
				else
				{
					if (GUI.Button(buttonRect, "Use"))
					{
						useItem(selectedItemIndex);
					}
				}
					buttonRect = new Rect(300, Screen.height - 50, 150, 50);
				if (GUI.Button(buttonRect, "Discard"))
				{
					discardItem(selectedItemIndex);
					selectedItemIndex = -1;
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
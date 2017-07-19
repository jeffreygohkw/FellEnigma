using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerUnit : Unit
{
	List<Vector2> lastPosition = new List<Vector2>();
    private bool onceisEnough = false;

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
                if (!onceisEnough)
                {
                    EventManager.TriggerEvent("DeselectUnit");
                    EventManager.TriggerEvent("DeselectUnitStats");
                    EventManager.TriggerEvent("ItemUIOFF");
                    EventManager.TriggerEvent("AttackUnitStatsOFF");
                    EventManager.TriggerEvent("HealUnitStatsOFF");
                    ActionOtherUI.instance.OffAllUI();
                    EventManager.StopListening("MoveUnit", MoveUnit);
                    EventManager.StopListening("UndoMoveUnit", UndoMoveUnit);
                    EventManager.StopListening("AttackUnit", AttackUnit);
                    EventManager.StopListening("ItemUnit", ItemUnit);
                    EventManager.StopListening("WaitUnit", WaitUnit);
                    EventManager.StopListening("EndUnit", EndUnit);
                    EventManager.StopListening("OtherUnit", OtherUnit);
                    EventManager.StopListening("TalkUnit", TalkUnit);
                    EventManager.StopListening("CapUnit", CapUnit);
                    EventManager.StopListening("TavUnit", TavUnit);
                    EventManager.StopListening("ObjUnit", ObjUnit);
                    EventManager.StopListening("EquipUseItem", EquipUseItem);
                    EventManager.StopListening("DiscardItem", DiscardItem);
                    onceisEnough = true;
                }
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
            onceisEnough = false;
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
                            EventManager.TriggerEvent("MovedUnit");
                        }
					}
				}
			}
		}
		base.turnUpdate();
	}


	private void OnMouseDown()
	{
		if (!EventSystem.current.IsPointerOverGameObject())
		{
			if (Grid.instance.currentTeam == team)
			{
				foreach (Unit u in Grid.instance.units[Grid.instance.currentTeam])
				{
					if (u.selected && u != this)
					{
						if (Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].isHealing && Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer] != this)
						{
							Grid.instance.battle.healWithCurrentUnit(this, Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].inventory[Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].activeStaffIndex]);
						}
						return;
					}
				}
				if (selected && doneMoving)
				{
					playerWait();
				}
				if (!doneAction)
				{
					selected = !selected;
					if (selected)
					{
						EventManager.TriggerEvent("SelectUnit");
						EventManager.TriggerEvent("SelectUnitStats");
                        EventManager.StartListening("MoveUnit", MoveUnit);
                        EventManager.StartListening("UndoMoveUnit", UndoMoveUnit);
                        EventManager.StartListening("AttackUnit", AttackUnit);
                        EventManager.StartListening("ItemUnit", ItemUnit);
                        EventManager.StartListening("WaitUnit", WaitUnit);
                        EventManager.StartListening("EndUnit", EndUnit);
                        EventManager.StartListening("OtherUnit", OtherUnit);
                        EventManager.StartListening("TalkUnit", TalkUnit);
                        EventManager.StartListening("CapUnit", CapUnit);
                        EventManager.StartListening("TavUnit", TavUnit);
                        EventManager.StartListening("ObjUnit", ObjUnit);
                        EventManager.StartListening("EquipUseItem", EquipUseItem);
                        EventManager.StartListening("DiscardItem", DiscardItem);
                    }
					else
					{
						EventManager.TriggerEvent("DeselectUnit");
						EventManager.TriggerEvent("DeselectUnitStats");
                        EventManager.TriggerEvent("ItemUIOFF");
                        EventManager.TriggerEvent("HealUnitStatsOFF");
                        EventManager.TriggerEvent("AttackUnitStatsOFF");
                        ActionOtherUI.instance.OffAllUI();
                        EventManager.StopListening("MoveUnit", MoveUnit);
                        EventManager.StopListening("UndoMoveUnit", UndoMoveUnit);
                        EventManager.StopListening("AttackUnit", AttackUnit);
                        EventManager.StopListening("ItemUnit", ItemUnit);
                        EventManager.StopListening("WaitUnit", WaitUnit);
                        EventManager.StopListening("EndUnit", EndUnit);
                        EventManager.StopListening("OtherUnit", OtherUnit);
                        EventManager.StopListening("TalkUnit", TalkUnit);
                        EventManager.StopListening("CapUnit", CapUnit);
                        EventManager.StopListening("TavUnit", TavUnit);
                        EventManager.StopListening("ObjUnit", ObjUnit);
                        EventManager.StopListening("EquipUseItem", EquipUseItem);
                        EventManager.StopListening("DiscardItem", DiscardItem);
                    }
				}
				if (selected && !doneAction)
				{
					Grid.instance.currentPlayer = index;
				}
			}
			if (Grid.instance.currentPlayer != -1)
			{
				if (Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].isFighting && Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer] != this)
				{
					Grid.instance.battle.attackWithCurrentUnit(this);
				}
			}
			else
			{
				isMoving = false;
				isFighting = false;
				isHealing = false;
				activeStaffIndex = -1;
			}
		}
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
	 * v1.1
	 * Added Healing
	 * 
	 * v1.2
	 * Prints a line when a consumable is used
	 * 
	 * @param index index of the item
	 * @author Jeffrey Goh
	 * @version 1.2
	 * @updated 16/7/2017
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
				if (proficiency.Contains("Staff"))
				{
					isHealing = !isHealing;
					if (isHealing)
					{
						if (activeStaffIndex != index)
						{
                            EventManager.TriggerEvent("HealUnitStatsON");
							activeStaffIndex = index;
							Grid.instance.highlightTilesAt(gridPosition, Color.yellow, int.Parse(inventory[index][3]), int.Parse(inventory[index][4]), false, isFlying);
						}
					}
					else
					{
                        EventManager.TriggerEvent("HealUnitStatsOFF");
                        activeStaffIndex = -1;
						Grid.instance.removeTileHighlight();
					}
					Debug.Log(activeStaffIndex);

				}
				else
				{
					Debug.Log(unitName + " cannot use staves!");
					CombatLog.instance.AddEvent(unitName + " cannot use staves!");
					CombatLog.instance.PrintEvent();

				}
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
                EventManager.TriggerEvent("ItemUIOFF");
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
					int tempHP = currentHP;
					currentHP += int.Parse(inventory[index][4]);
					if (currentHP > maxHP)
					{
						currentHP = maxHP;
					}
					tempHP = currentHP - tempHP;
					Debug.Log(unitName + " has healed for " + tempHP + " HP!");
					CombatLog.instance.AddEvent(unitName + " has healed for " + tempHP + " HP!");
					CombatLog.instance.PrintEvent();
					discardItem(index);
					selectedItemIndex = -1;
                    EventManager.TriggerEvent("ItemUIOFF");
                }
			}
			else if (inventory[index][0] == "Key")
			{
				// Unlock door code here
				if (inventory[index][1] == "ChestKey")
				{
					if (Grid.instance.map[(int)gridPosition.x][(int)gridPosition.y].linkedTerrain.returnName() == "Chest")
					{
						if (Grid.instance.chestLoot.ContainsKey(gridPosition))
						{
							//Discard Key
							discardItem(selectedItemIndex);
							//Add Item
							inventory.Add(Grid.instance.chestLoot[gridPosition]);
							//Change Chest to OpenChest
							Grid.instance.map[(int)gridPosition.x][(int)gridPosition.y].linkedTerrain.LoadTerrain(10);

							Debug.Log("Acquired " + Grid.instance.chestLoot[gridPosition][1]);
							CombatLog.instance.AddEvent("Acquired " + Grid.instance.chestLoot[gridPosition][1]);
							CombatLog.instance.PrintEvent();
                            EventManager.TriggerEvent("ItemUIOFF");
                            playerWait();
						}
						else
						{
							Debug.Log("Chest loot not found");
						}
					}
				}
				else if (inventory[index][1] == "DoorKey")
				{
					bool unlockedADoor = false;
					foreach (Tile t in Grid.instance.map[(int)gridPosition.x][(int)gridPosition.y].neighbours)
					{
						if (t.linkedTerrain.returnName() == "Door")
						{
							unlockedADoor = true;
							Grid.instance.map[(int)t.gridPosition.x][(int)t.gridPosition.y].linkedTerrain.LoadTerrain(12);
						}
					}
					if (unlockedADoor)
					{
						//Discard Key
						discardItem(selectedItemIndex);
                        EventManager.TriggerEvent("ItemUIOFF");
                        playerWait();

						Debug.Log("Door unlocked");
						CombatLog.instance.AddEvent("Door unlocked");
						CombatLog.instance.PrintEvent();
					}
					else
					{
						Debug.Log("No doors found");
					}
				}
				else if (inventory[index][1] == "Lockpick")
				{
					//Both Chest Key and Door Key
					if (Grid.instance.map[(int)gridPosition.x][(int)gridPosition.y].linkedTerrain.returnName() == "Chest")
					{
						if (Grid.instance.chestLoot.ContainsKey(gridPosition))
						{
							//Discard Key
							discardItem(selectedItemIndex);
							//Add Item
							inventory.Add(Grid.instance.chestLoot[gridPosition]);
							//Change Chest to OpenChest
							Grid.instance.map[(int)gridPosition.x][(int)gridPosition.y].linkedTerrain.LoadTerrain(10);

							Debug.Log("Acquired " + Grid.instance.chestLoot[gridPosition][1]);
							CombatLog.instance.AddEvent("Acquired " + Grid.instance.chestLoot[gridPosition][1]);
							CombatLog.instance.PrintEvent();
                            EventManager.TriggerEvent("ItemUIOFF");
                            playerWait();
						}
						else
						{
							Debug.Log("Chest is empty");
							CombatLog.instance.AddEvent("Acquired " + Grid.instance.chestLoot[gridPosition][1]);
							CombatLog.instance.PrintEvent();
						}
					}

					bool unlockedADoor = false;
					foreach (Tile t in Grid.instance.map[(int)gridPosition.x][(int)gridPosition.y].neighbours)
					{
						if (t.linkedTerrain.returnName() == "Door")
						{
							unlockedADoor = true;
							Grid.instance.map[(int)t.gridPosition.x][(int)t.gridPosition.y].linkedTerrain.LoadTerrain(12);
						}
					}
					if (unlockedADoor)
					{
						//Discard Key
						discardItem(selectedItemIndex);
                        EventManager.TriggerEvent("ItemUIOFF");
                        playerWait();

						Debug.Log("Door unlocked");
						CombatLog.instance.AddEvent("Door unlocked");
						CombatLog.instance.PrintEvent();
					}
					else
					{
						Debug.Log("No doors found");
					}
				}
				else
				{
					Debug.Log("There's nothing to unlock");
					CombatLog.instance.AddEvent("Door unlocked");
					CombatLog.instance.PrintEvent();
				}
			}
			else
			{
				Debug.Log("Wrong Item type");
			}
		}
	}

	/**
	* Discards an item in the PlayerUnit's inventory
	* 
	* v1.1
	* Prints a line when an object is discarded
	* 
	* @param index index of the item
	* @author Jeffrey Goh
	* @version 1.1
	* @updated 16/7/2017
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
			string name = inventory[index][1];
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
			Debug.Log(name + " discarded");
			CombatLog.instance.AddEvent(name + " discarded");
			CombatLog.instance.PrintEvent();
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
		lastPosition.Clear();
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


    /**
	* GUI buttons
	* 
	* v1.1
	* Added Wait
	* 
	* v1.2
	* Added Undo Move
	* 
	* v1.3
	* Added Objective Specific, Inventory, Talk, Visit, Tavern
	* 
	* @param destTile The destination tile
	* @author Jeffrey Goh
	* @version 1.3
	* @updated 15/7/2017
	*/

    public override void OnGUI()
	{
		if (selected && !doneAction)
		{
			Rect buttonRect = new Rect(0, Screen.height - 400, 150, 50);
			if (Grid.instance.ultCharge == 100 && Grid.instance.commander >= 0 && Grid.instance.commander <= 4 && Grid.instance.ultActive == false)
			{
				if (GUI.Button(buttonRect, "Ult"))
				{
					Grid.instance.castUlt();
				}
			}


			buttonRect = new Rect(0, Screen.height - 350, 150, 50);
			foreach (Tile t in Grid.instance.map[(int)gridPosition.x][(int)gridPosition.y].neighbours)
			{
				if (t.occupied)
				{
					if (t.occupied.canTalk.ContainsKey(this.unitName))
					{
						//Talk to other units
						if (GUI.Button(buttonRect, "Talk"))
						{
							isMoving = false;
							isFighting = false;
							isHealing = false;
							activeStaffIndex = -1;
							displayInventory = false;
							selectedItemIndex = -1;
							displayTavern = false;

							Debug.Log("Talking");
							isTalking = !isTalking;
							if (isTalking)
							{
								Grid.instance.highlightTilesAt(gridPosition, Color.yellow, 1, 1, false, isFlying);
							}
							else
							{
								Grid.instance.removeTileHighlight();
							}
						}
					}
				}
			}

			buttonRect = new Rect(0, Screen.height - 300, 150, 50);
			if (Grid.instance.map[(int)gridPosition.x][(int)gridPosition.y].linkedTerrain.returnName() == "Village" && Grid.instance.villageStatus.ContainsKey(gridPosition))
			{
				//Visit
				if (GUI.Button(buttonRect, "Capture"))
				{
					// Check if reward is gold
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
					}
					else
					{
						Debug.Log("Village has already been captured");
					}
				}
			}
			else if (Grid.instance.map[(int)gridPosition.x][(int)gridPosition.y].linkedTerrain.returnName() == "Tavern" && Grid.instance.tavernAndSpawn.ContainsKey(gridPosition))
			{
				//Tavern
				if (GUI.Button(buttonRect, "Tavern"))
				{
					displayTavern = !displayTavern;
					displayInventory = false;
					selectedItemIndex = -1;
					Grid.instance.removeTileHighlight();
					isMoving = false;
					isFighting = false;
					isHealing = false;
					activeStaffIndex = -1;
					isTalking = false;
				}
			}
			else if (Grid.instance.objectiveSpecificTiles.ContainsKey(gridPosition))
			{
				//Seize, escape etc.
				if (GUI.Button(buttonRect, Grid.instance.objectiveSpecificTiles[gridPosition]))
				{
					Grid.instance.objectiveComplete = Grid.instance.objectiveSpecificTiles[gridPosition];
				}
			}

			if (displayTavern)
			{
				int count = 1;
				foreach (string s in TavernUnits.tavernUnits.Keys)
				{
					count++;
					buttonRect = new Rect(150, Screen.height - 50 * count, 150, 50);
					if (GUI.Button(buttonRect, s))
					{
						TavernUnits.tavernSpawn(s, gridPosition);
					}
				}
			}

			buttonRect = new Rect(0, Screen.height - 250, 150, 50);

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
						isHealing = false;
						activeStaffIndex = -1;
						isTalking = false;
						displayTavern = false;

						if (isMoving)
						{
							isMoving = false;
							Grid.instance.removeTileHighlight();
						}
						else
						{
							isMoving = true;
							Grid.instance.highlightTilesAt(gridPosition, new Vector4(0f, 1f, 0f, 0.5f), 1, mov, true, isFlying);
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
						isHealing = false;
						activeStaffIndex = -1;
						isTalking = false;
						displayTavern = false;

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
			if (equippedIndex != -1)
			{
				if (GUI.Button(buttonRect, "Attack"))
				{
					displayInventory = false;
					selectedItemIndex = -1;
					Grid.instance.removeTileHighlight();
					isMoving = false;
					isHealing = false;
					activeStaffIndex = -1;
					isTalking = false;
					displayTavern = false;


					if (isFighting)
					{
						isFighting = false;
						Grid.instance.removeTileHighlight();
					}
					else
					{
						isFighting = true;
						Grid.instance.highlightTilesAt(gridPosition, new Vector4(1f, 0f, 0f, 0.5f), weaponMinRange, weaponMaxRange + weaponRangeBuff, false, isFlying);
					}
				}
			}

			buttonRect = new Rect(0, Screen.height - 150, 150, 50);

			//Item
			if (GUI.Button(buttonRect, "Item"))
			{
				displayInventory = !displayInventory;
				if (isFighting || isMoving)
				{
					Grid.instance.removeTileHighlight();
					isHealing = false;
					activeStaffIndex = -1;
				}
				if (!displayInventory)
				{
					Grid.instance.removeTileHighlight();
					isHealing = false;
					activeStaffIndex = -1;
					selectedItemIndex = -1;
				}
			}


			// The actual Items
			if (displayInventory)
			{
				isMoving = false;
				isFighting = false;
				isTalking = false;

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
				playerWait();
			}



			buttonRect = new Rect(0, Screen.height - 50, 150, 50);

			//End Turn

			if (GUI.Button(buttonRect, "End"))
			{
				Grid.instance.removeTileHighlight();
				lastPosition.Clear();
				isMoving = false;
				isFighting = false;
				isHealing = false;
				activeStaffIndex = -1;
				selected = false;
				doneAction = true;
				displayInventory = false;
				selectedItemIndex = -1;
				isTalking = false;
				displayTavern = false;

				Grid.instance.nextTurn();
				Grid.instance.currentPlayer = -1;
			}
			base.OnGUI();
            
        }
        
        
    }



    /**
    * EventMangager: Activates Move function when the move button is pressed
    * 
    * v1.1
    * updated to follow original
    * 
    * @author Jeffery Goh
    * @version 1.1
    * @updated on 19/7/17 by Wayne
    */
    void MoveUnit()
    {
        if (selected && !doneAction)
        {
            if (!doneMoving)
            {
                displayInventory = false;
                selectedItemIndex = -1;
                Grid.instance.removeTileHighlight();
                isFighting = false;
                isHealing = false;
                activeStaffIndex = -1;
                isTalking = false;
                displayTavern = false;
                if (isMoving)
                {
                    isMoving = false;
                    Grid.instance.removeTileHighlight();
                }
                else
                {
                    isMoving = true;
                    Grid.instance.highlightTilesAt(gridPosition, new Vector4(0f, 1f, 0f, 0.5f), 1, mov, true, isFlying);
                }
            }
        }
    }

    /**
    * EventManager: Activates Undo Move function when the Undo Move button is pressed
    * 
    * @author Jeffery Goh
    * @version 1.0
    * @updated on 19/7/17 by Wayne
    */ 
    void UndoMoveUnit()
    {
        if (selected && !doneAction)
        {
            if (doneMoving)
            {
                if (lastPosition.Count != 0)
                {
                    isFighting = false;
                    isHealing = false;
                    activeStaffIndex = -1;
                    isTalking = false;
                    displayTavern = false;

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
                EventManager.TriggerEvent("UndoMovedUnit");
            }
        }
    }

    /**
     * EventManager: Activates the Attack function when the Attack button is pressed
     *
     * @author Jeffrey Goh
     * @version 1.0
     * @updated by 19/7/17 by Wayne
     */
    void AttackUnit()
    {
        if (selected && !doneAction)
        {
            displayInventory = false;
            selectedItemIndex = -1;
            Grid.instance.removeTileHighlight();
            isMoving = false;
            isHealing = false;
            activeStaffIndex = -1;

            if (isFighting)
            {
                isFighting = false;
                Grid.instance.removeTileHighlight();
                EventManager.TriggerEvent("AttackUnitStatsOFF");
            }
            else
            {
                isFighting = true;
                Grid.instance.highlightTilesAt(gridPosition, new Vector4(1f, 0f, 0f, 0.5f), weaponMinRange, weaponMaxRange, false, isFlying);
            }
        }
    }

    /**
    * EventManager: Activates the Item function when the Item button is pressed
    *
    * @author Jeffrey Goh
    * @version 1.0
    * @updated by 19/7/17 by Wayne
    */
    void ItemUnit()
    {
        if (selected && !doneAction)
        {
            ItemUI.instance.setUnit(this);
            displayInventory = !displayInventory;
            if (isFighting || isMoving)
            {
                Grid.instance.removeTileHighlight();
                isHealing = false;
                activeStaffIndex = -1;
            }

            if (displayInventory)
            {
                isMoving = false;
                isFighting = false;
                isTalking = false;
                EventManager.TriggerEvent("ItemUION");
            }
            else
            {
                EventManager.TriggerEvent("ItemUIOFF");
                Grid.instance.removeTileHighlight();
                isHealing = false;
                activeStaffIndex = -1;
                selectedItemIndex = -1;
            }
        }
    }


    /**
    * EventManager: Activates the Equip/Use function when the Equip/Use button is pressed
    *
    * @author Jeffrey Goh
    * @version 1.0
    * @updated by 19/7/17 by Wayne
    */
    void EquipUseItem()
    {
        selectedItemIndex = ItemUI.instance.getItemIndex();
        if (inventory[selectedItemIndex].Length == 13)
        {
            equipItem(selectedItemIndex);
        }
        else
        {
            useItem(selectedItemIndex);
        }

        displayInventory = !displayInventory;
    }


    /**
    * EventManager: Activates the Discard Item function when the Discard Item button is pressed
    *
    * @author Jeffrey Goh
    * @version 1.0
    * @updated by 19/7/17 by Wayne
    */
    void DiscardItem()
    {
        selectedItemIndex = ItemUI.instance.getItemIndex();
        discardItem(selectedItemIndex);
        selectedItemIndex = -1;
        EventManager.TriggerEvent("ItemUIOFF");
        displayInventory = !displayInventory;
    }


    /**
    * EventManager: Activates the Other Actions function when the Actions button is pressed.
    * This searches for the available actions the unit can take and updates the ActionsOtherUI accordingly
    *
    * @author Jeffrey Goh
    * @version 1.0
    * @updated by 19/7/17 by Wayne
    */
    void OtherUnit()
    {
        if (selected && !doneAction)
        {
            foreach (Tile t in Grid.instance.map[(int)gridPosition.x][(int)gridPosition.y].neighbours)
            {
                if (t.occupied && t.occupied.canTalk.ContainsKey(this.unitName))
                {
                    ActionOtherUI.instance.canTalk = true;
                }
            }

            if (Grid.instance.map[(int)gridPosition.x][(int)gridPosition.y].linkedTerrain.returnName() == "Village" && Grid.instance.villageStatus.ContainsKey(gridPosition))
            {
                ActionOtherUI.instance.canCap = true;
            }
            else if (Grid.instance.map[(int)gridPosition.x][(int)gridPosition.y].linkedTerrain.returnName() == "Tavern" && Grid.instance.tavernAndSpawn.ContainsKey(gridPosition))
            {
                ActionOtherUI.instance.canTav = true;
            }
            else if (Grid.instance.objectiveSpecificTiles.ContainsKey(gridPosition))
            {
                ActionOtherUI.instance.canOther = true;
            }

            if (!ActionOtherUI.instance.canOther)
            {
                ActionOtherUI.instance.ToggleUI();
            }
            else
            {
                ActionOtherUI.instance.ToggleUI(Grid.instance.objectiveSpecificTiles[gridPosition]);
            }
        }
    }

    /**
    * EventManager: Activates the Talk function when the Talk button is pressed
    *
    * @author Jeffrey Goh
    * @version 1.0
    * @updated by 19/7/17 by Wayne
    */
    void TalkUnit()
    {
        if (selected && !doneAction)
        {
            isMoving = false;
            isFighting = false;
            isHealing = false;
            activeStaffIndex = -1;
            displayInventory = false;
            selectedItemIndex = -1;
            displayTavern = false;

            Debug.Log("Talking");
            isTalking = !isTalking;
            if (isTalking)
            {
                Grid.instance.highlightTilesAt(gridPosition, Color.yellow, 1, 1, false, isFlying);
            }
            else
            {
                Grid.instance.removeTileHighlight();
            }
        }
    }

    /**
    * EventManager: Activates the Capture function when the Capture button is pressed
    *
    * @author Jeffrey Goh
    * @version 1.0
    * @updated by 19/7/17 by Wayne
    */
    void CapUnit()
    {
        if (selected && !doneAction)
        {
            // Check if reward is gold
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
            }
            else
            {
                Debug.Log("Village has already been captured");
            }
        }
    }

    /**
    * EventManager: Activates the Tavern function when the Tavern button is pressed
    *
    * @author Jeffrey Goh
    * @version 1.0
    * @updated by 19/7/17 by Wayne
    */
    void TavUnit()
    {

        if (selected && !doneAction)
        {
            displayTavern = !displayTavern;
            ActionOtherUI.instance.ToggleTavUI(this);

            displayInventory = false;
            selectedItemIndex = -1;
            Grid.instance.removeTileHighlight();
            isMoving = false;
            isFighting = false;
            isHealing = false;
            activeStaffIndex = -1;
            isTalking = false;
        }
    }


    /**
    * EventManager: Activates the Special Objective function when the Special Objective button is pressed
    *
    * @author Jeffrey Goh
    * @version 1.0
    * @updated by 19/7/17 by Wayne
    */
    void ObjUnit()
    {
        if (selected && !doneAction)
        {
            Grid.instance.objectiveComplete = Grid.instance.objectiveSpecificTiles[gridPosition];
        }
    }


    /**
    * EventManager: Activates the Wait function when the Wait button is pressed
    *
    * @author Jeffrey Goh
    * @version 1.0
    * @updated by 19/7/17 by Wayne
    */
    void WaitUnit()
    {
        if (selected && !doneAction)
        {
            Grid.instance.removeTileHighlight();
            lastPosition.Clear();
            isMoving = false;
            isFighting = false;
            isHealing = false;
            activeStaffIndex = -1;
            doneAction = true;
            selected = false;
            displayInventory = false;
            selectedItemIndex = -1;
            EventManager.TriggerEvent("DeselectUnit");
            EventManager.TriggerEvent("DeselectUnitStats");
            EventManager.TriggerEvent("AttackUnitStatsOFF");
            EventManager.TriggerEvent("HealUnitStatsOFF");
            ActionOtherUI.instance.OffAllUI();
            EventManager.StopListening("MoveUnit", MoveUnit);
            EventManager.StopListening("UndoMoveUnit", UndoMoveUnit);
            EventManager.StopListening("AttackUnit", AttackUnit);
            EventManager.StopListening("ItemUnit", ItemUnit);
            EventManager.StopListening("WaitUnit", WaitUnit);
            EventManager.StopListening("EndUnit", EndUnit);
            EventManager.StopListening("OtherUnit", OtherUnit);
            EventManager.StopListening("TalkUnit", TalkUnit);
            EventManager.StopListening("CapUnit", CapUnit);
            EventManager.StopListening("TavUnit", TavUnit);
            EventManager.StopListening("ObjUnit", ObjUnit);
            EventManager.StopListening("EquipUseItem", EquipUseItem);
            EventManager.StopListening("DiscardItem", DiscardItem);
            Grid.instance.totalDone++;

        }
    }

    /**
    * EventManager: Activates the End function when the End button is pressed
    *
    * @author Jeffrey Goh
    * @version 1.0
    * @updated by 19/7/17 by Wayne
    */
    void EndUnit()
    {
        if (selected && !doneAction)
        {
            Grid.instance.removeTileHighlight();
            lastPosition.Clear();
            isMoving = false;
            isFighting = false;
            isHealing = false;
            activeStaffIndex = -1;
            selected = false;
            doneAction = true;
            displayInventory = false;
            selectedItemIndex = -1;
            EventManager.TriggerEvent("DeselectUnit");
            EventManager.TriggerEvent("DeselectUnitStats");
            EventManager.TriggerEvent("AttackUnitStatsOFF");
            EventManager.TriggerEvent("HealUnitStatsOFF");
            ActionOtherUI.instance.OffAllUI();
            EventManager.StopListening("MoveUnit", MoveUnit);
            EventManager.StopListening("UndoMoveUnit", UndoMoveUnit);
            EventManager.StopListening("AttackUnit", AttackUnit);
            EventManager.StopListening("ItemUnit", ItemUnit);
            EventManager.StopListening("WaitUnit", WaitUnit);
            EventManager.StopListening("EndUnit", EndUnit);
            EventManager.StopListening("OtherUnit", OtherUnit);
            EventManager.StopListening("TalkUnit", TalkUnit);
            EventManager.StopListening("CapUnit", CapUnit);
            EventManager.StopListening("TavUnit", TavUnit);
            EventManager.StopListening("ObjUnit", ObjUnit);
            EventManager.StopListening("EquipUseItem", EquipUseItem);
            EventManager.StopListening("DiscardItem", DiscardItem);
            Grid.instance.nextTurn();
        }
    }

}
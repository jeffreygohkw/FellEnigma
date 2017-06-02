using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : Unit
{
	public GameObject Menu;
	

	// Use this for initialization
	void Start()
	{
		Menu = GameObject.Find("Canvas");
	}

	// Update is called once per frame
	void Update()
	{
		if (Grid.instance.units[Grid.instance.currentPlayer] == this)
		{
			GetComponent<Renderer>().material.color = Color.yellow;
		}
		else
		{
			GetComponent<Renderer>().material.color = Color.grey;
		}

		if (currentHP <= 0)
		{
			GetComponent<Renderer>().material.color = Color.red;
			//gameObject.SetActive(false);
		}
	}

	public override void turnUpdate()
	{
		// Move to its destination
		if (Vector3.Distance(moveTo, transform.position) > 0.1f)
		{
			transform.position += (moveTo - transform.position).normalized * moveSpeed * Time.deltaTime;

			// When the unit has reached its destination
			if (Vector3.Distance(moveTo, transform.position) <= 0.1f)
			{
				transform.position = moveTo;

				// Reset unit status after moving
				isMoving = false;
			}
		}
		base.turnUpdate();
	}

	private void OnMouseDown()
	{
		selected = !selected;

		if (Grid.instance.units[Grid.instance.currentPlayer].isFighting && Grid.instance.units[Grid.instance.currentPlayer] != this)
		{
			Grid.instance.attackWithCurrentUnit(this);
		}
		isMoving = false;
		isFighting = false;
	}


	public override void OnGUI()
	{
		if (selected && Grid.instance.units[Grid.instance.currentPlayer] == this)
		{
			Rect buttonRect = new Rect(0, Screen.height - 150, 150, 50);

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
					Grid.instance.highlightTilesAt(gridPosition, Color.blue, mov, true);
				}
			}


			buttonRect = new Rect(0, Screen.height - 100, 150, 50);

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
					Grid.instance.highlightTilesAt(gridPosition, Color.red, weaponRange, false);
				}
			}


			buttonRect = new Rect(0, Screen.height - 50, 150, 50);

			//End Turn

			if (GUI.Button(buttonRect, "End"))
			{
				Grid.instance.removeTileHighlight();
				isMoving = false;
				isFighting = false;
				Grid.instance.nextTurn();
			}
			base.OnGUI();
		}
	}
}
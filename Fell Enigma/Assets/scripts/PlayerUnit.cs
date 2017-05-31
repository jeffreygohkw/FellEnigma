using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : Unit
{
	public GameObject Menu;
	private bool selected = false;

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
			}
		}
		base.turnUpdate();
	}

	private void OnMouseDown()
	{
		selected = !selected;
		TurnOnGUI();
	}

	public override void TurnOnGUI()
	{
		/*
		if (Grid.instance.units[Grid.instance.currentPlayer] == this) {
			Menu.SetActive(selected);
			Debug.Log("Done");
		}
		*/

		Rect buttonRect = new Rect(0, Screen.height - 150, 150, 50);

		//Move
		if (GUI.Button(buttonRect, "Move"))
		{
			isMoving = isMoving ? false : true;
			isFighting = false;
		}


		buttonRect = new Rect(0, Screen.height - 100, 150, 50);

		//Attack
		if (GUI.Button(buttonRect, "Attack"))
		{
			isMoving = false;
			isFighting = isFighting ? false : true;
		}


		buttonRect = new Rect(0, Screen.height - 50, 150, 50);

		//End Turn

		if (GUI.Button(buttonRect, "End"))
		{
			isMoving = false;
			isFighting = false;
			Grid.instance.nextTurn();
		}	
		base.TurnOnGUI();
		
	}
}
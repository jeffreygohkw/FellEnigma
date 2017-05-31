using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIUnit : Unit
{

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

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
				Grid.instance.nextTurn();
			}

		}
		else
		{
			moveTo = new Vector3(2 - Mathf.Floor(Grid.instance.tilesPerCol / 2), 0 - Mathf.Floor(Grid.instance.tilesPerRow / 2), 0);
		}

		base.turnUpdate();
	}

	public override void TurnOnGUI()
	{
		


		base.TurnOnGUI();
	}
}

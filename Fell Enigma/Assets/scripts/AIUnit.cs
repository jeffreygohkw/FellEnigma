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

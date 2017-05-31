using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

	public Vector2 gridPosition = Vector2.zero;


	// Use this for initialization
	void Start () {
		GetComponent<Renderer>().material.color = Color.green;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnMouseEnter()
	{
		if (Grid.instance.units[Grid.instance.currentPlayer].isMoving)
		{
			GetComponent<Renderer>().material.color = Color.blue;
		}
		else if (Grid.instance.units[Grid.instance.currentPlayer].isFighting)
		{
			GetComponent<Renderer>().material.color = Color.red;
		}

	}

	void OnMouseExit()
	{
		GetComponent<Renderer>().material.color = Color.green;
	}

	private void OnMouseDown()
	{
		if (Grid.instance.units[Grid.instance.currentPlayer].isMoving)
		{
			Grid.instance.moveCurrentUnit(this);
		}
		else if (Grid.instance.units[Grid.instance.currentPlayer].isFighting)
		{
			Grid.instance.attackWithCurrentUnit(this);
		}
	}
}

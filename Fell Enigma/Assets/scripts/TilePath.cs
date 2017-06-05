using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePath
{
	public List<Tile> tileList = new List<Tile>();

	public int costofPath = 0;

	public Tile lastTile;

	public TilePath()
	{

	}

	/**
	 * Clones the TilePath
	 * @param path The Tilepath to clone
	 * @author Jeffrey Goh
	 * @version 1.1
	 * @updated 5/6/2017
	 */
	public TilePath(TilePath path)
	{
		foreach (Tile t in path.tileList) {
			tileList.Add(t);
		}
		costofPath = path.costofPath;
		lastTile = path.lastTile;
	}

	// Add the cost of traversing the tile, seperate from addTile since attacking and moving counts differently
	public void addCost(int cost)
	{
		costofPath += cost;
	}

	// Add the tile to the Tilepath
	public void addTile(Tile t)
	{
		tileList.Add(t);
		lastTile = t;
	}
}
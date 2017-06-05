using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHighlight {

	public TileHighlight() 
	{

	}

	/**
	 * Uses Dijkstra's Formula to find which tiles can be traversed
	 * 
	 * v1.1
	 * Added min and max range (for archers)
	 * 
	 * @param originTile The tile our unit is on
	 * @param minrange The minimum move or attack range of the unit
	 * @param maxrange The maximum move or attack range of the unit
	 * @param moving whether we are calculating attack or movement
	 * @author Jeffrey Goh
	 * @version v1.1
	 * @updated 2/6/2017
	 */
	public static List<Tile> FindHighlight(Tile originTile, int minRange, int maxRange, bool moving)
	{
		// List of tiles to highlight
		List<Tile> closed = new List<Tile>();

		// List of valid paths left that could be taken
		List<TilePath> open = new List<TilePath>();

		List<Tile> toDelete = new List<Tile>();

		// Our path starts from the current location
		TilePath originPath = new TilePath();
		originPath.addTile(originTile);

		// Add the original tile, but don't factor in its own movecost
		open.Add(originPath);

		// Add original tile for now to help with the algorithm
		closed.Add(originTile);

		// Loop while there are valid paths left
		while (open.Count > 0)
		{
			// Get a valid path
			TilePath current = open[0];
			open.Remove(open[0]);

			// For each neighbour
			foreach (Tile t in current.lastTile.neighbours)
			{
				// If the neighbour is in closed, that tile can be visited and we continue
				if (closed.Contains(t))
				{
					continue;
				}

				// Otherwise, make a clone of the current path
				TilePath newTilePath = new TilePath(current);

				if (current.costofPath < minRange)
				{
					toDelete.Add(current.lastTile);
				}

				// If moving and the neighbor's movement cost exceeds the unit's movement range, we continue
				if (moving)
				{
					if (current.costofPath + t.movementCost > maxRange || t.occupied != null)
					{
						continue;
					}

					// Otherwise, we add that tile and its movement cost to the newTilePath
					newTilePath.addTile(t);
					newTilePath.addCost(t.movementCost);

				}
				else
				{
					// If we are attacking, we ignore the tile's movement cost, each tile will effectively cost 1
					// If the tile exceeds the attack range of the unit. we continue
					if (current.costofPath + 1 > maxRange)
					{
						continue;
					}
					// Otherwise, we add that tile and its movement cost to newTilePath
					newTilePath.addTile(t);
					newTilePath.addCost(1);
				}

				// We add newTilePath to open so we can consider it again
				open.Add(newTilePath);

				// We add t to closed as we now know this tile can be visited
				closed.Add(t);
			}
		}
		// Remove the origin tile as we don't want to attack ourselves or move to our current location
		closed.Remove(originTile);
		foreach (Tile t in toDelete)
		{
			closed.Remove(t);
		}
		return closed;
	}
}

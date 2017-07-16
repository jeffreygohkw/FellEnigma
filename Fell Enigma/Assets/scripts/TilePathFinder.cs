using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePathFinder : MonoBehaviour {

	/**
	 * Uses BFS to find the shortest path to a tile
	 * 
	 * v1.1
	 * Allowed allies to pass through each other
	 * 
	 * v1.2
	 * Added Flying compatibility
	 * 
	 * @param originTile The tile our unit is on
	 * @param range The move or attack range of the unit
	 * @param destinationTile The tile we want our unit to move to
	 * @param allies A list containing the allied teams of the current unit
	 * @author Jeffrey Goh
	 * @version v1.2
	 * @updated 15/7/2017
	 */
	public static TilePath FindPath(Tile originTile, int range, Tile destinationTile, List<int> allies, bool flying)
	{
		// List of tiles to highlight
		List<Tile> closed = new List<Tile>();

		// List of valid paths left that could be taken
		List<TilePath> open = new List<TilePath>();

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

			// If we found the destination tile, return the path
			if (current.lastTile == destinationTile)
			{
				current.tileList.RemoveAt(0);
				return current;
			}


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

				int movCost = t.movementCost;
				if (flying)
				{
					movCost = 1;
				}
				if (current.costofPath + movCost > range)
				{
					continue;
				}

				if (t.occupied != null)
				{
					if (!allies.Contains(t.occupied.team))
					{
						continue;
					}
				}

                if (!t.checkPassable())
                {
                    continue;
                }

				// Otherwise, we add that tile and its movement cost to the newTilePath
				newTilePath.addTile(t);
				newTilePath.addCost(movCost);


				// We add newTilePath to open so we can consider it again
				open.Add(newTilePath);

			}
		}
		return null;
	}
}

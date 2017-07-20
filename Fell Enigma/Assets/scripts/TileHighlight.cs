using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHighlight {

	public TileHighlight() 
	{

	}

	/**
	 * Uses BFS to find which tiles can be traversed
	 * 
	 * v1.1
	 * Added min and max range (for archers)
	 * 
	 * v1.2
	 * Fixed archer ranges when at the edge of the map
	 * 
	 * v1.3
	 * Allows allies to pass through each other
	 * 
	 * v1.4
	 * Fixed a bug with movement costs
	 * 
	 * v1.5
	 * Added flying compatibility
	 * 
	 * @param originTile The tile our unit is on
	 * @param minrange The minimum move or attack range of the unit
	 * @param maxrange The maximum move or attack range of the unit
	 * @param allies A list containing the allied teams of the current unit
	 * @param moving whether we are calculating attack or movement
	 * @author Jeffrey Goh
	 * @version v1.5
	 * @updated 16/7/2017
	 */
	public static List<Tile> FindHighlight(Tile originTile, int minRange, int maxRange, List<int> allies, bool moving, bool flying)
	{
		// List of tiles to highlight
		List<Tile> closed = new List<Tile>();

		Dictionary<Tile, int> costToReach = new Dictionary<Tile, int>();


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
		costToReach.Add(originTile, 0);


		// Loop while there are valid paths left
		while (open.Count > 0)
		{
			// Get a valid path
			TilePath current = open[0];
			open.Remove(open[0]);

			// For each neighbour
			foreach (Tile t in current.lastTile.neighbours)
			{
				// If the current costofPath is below the minRange, we mark the current lastTile to be set to not be highlighted
				if (current.costofPath < minRange)
				{
					toDelete.Add(current.lastTile);
				}

				// If the neighbour is in closed, that tile can be visited and we continue
				if (closed.Contains(t))
				{
					if (current.costofPath > costToReach[t])
					{
						continue;
					}
					else
					{
						costToReach[t] = current.costofPath;
					}
				}

				// Otherwise, make a clone of the current path
				TilePath newTilePath = new TilePath(current);

				// If moving and the neighbor's movement cost exceeds the unit's movement range, we continue
				if (moving)
				{
					int movCost = t.movementCost;
					if (flying)
					{
						movCost = 1;
					}
					if (current.costofPath + movCost > maxRange)
					{
						continue;
					}

					if (t.occupied != null)
					{
						if (allies.Contains(t.occupied.team))
						{
							toDelete.Add(t);
						}
						else
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
				if (costToReach.ContainsKey(t))
				{
					if (costToReach[t] < current.costofPath)
					{
						costToReach[t] = current.costofPath;
					}
				}
				else
				{
					costToReach.Add(t, current.costofPath);
				}
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

	/**
	 * Uses BFS to find which tiles can be traversed
	 * 
	 * v1.1
	 * Added min and max range (for archers)
	 * 
	 * v1.2
	 * Fixed archer ranges when at the edge of the map
	 * 
	 * v1.3
	 * Allows allies to pass through each other
	 * 
	 * v1.4
	 * Fixed a bug with movement costs
	 * 
	 * v1.5
	 * Added flying compatibility
	 * 
	 * @param originTile The tile our unit is on
	 * @param minrange The minimum move or attack range of the unit
	 * @param maxrange The maximum move or attack range of the unit
	 * @param allies A list containing the allied teams of the current unit
	 * @param moving whether we are calculating attack or movement
	 * @author Jeffrey Goh
	 * @version v1.5
	 * @updated 16/7/2017
	 */
	public static List<Tile> FindHighlightForCities(Tile originTile, int minRange, int maxRange, List<int> allies, bool moving, bool flying)
	{
		// List of tiles to highlight
		List<Tile> closed = new List<Tile>();

		Dictionary<Tile, int> costToReach = new Dictionary<Tile, int>();


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
		costToReach.Add(originTile, 0);


		// Loop while there are valid paths left
		while (open.Count > 0)
		{
			// Get a valid path
			TilePath current = open[0];
			open.Remove(open[0]);

			// For each neighbour
			foreach (Tile t in current.lastTile.neighbours)
			{
				// If the current costofPath is below the minRange, we mark the current lastTile to be set to not be highlighted
				if (current.costofPath < minRange)
				{
					toDelete.Add(current.lastTile);
				}

				// If the neighbour is in closed, that tile can be visited and we continue
				if (closed.Contains(t))
				{
					if (current.costofPath > costToReach[t])
					{
						continue;
					}
					else
					{
						costToReach[t] = current.costofPath;
					}
				}

				// Otherwise, make a clone of the current path
				TilePath newTilePath = new TilePath(current);

				// If moving and the neighbor's movement cost exceeds the unit's movement range, we continue
				if (moving)
				{
					int movCost = t.movementCost;
					if (flying)
					{
						movCost = 1;
					}
					if (current.costofPath + movCost > maxRange)
					{
						continue;
					}


					if (!t.checkPassable())
					{
						continue;
					}
					// Otherwise, we add that tile and its movement cost to the newTilePath
					newTilePath.addTile(t);
					newTilePath.addCost(movCost);

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
				if (costToReach.ContainsKey(t))
				{
					if (costToReach[t] < current.costofPath)
					{
						costToReach[t] = current.costofPath;
					}
				}
				else
				{
					costToReach.Add(t, current.costofPath);
				}
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

	/**
	 * Uses Find Highlight to return a dictionary containing the tiles that the unit can attack
	 * For AI usage mainly
	 * 
	 * @param originTile The tile our unit is on
	 * @param minrange The minimum move range of the unit
	 * @param maxrange The maximum move range of the unit
	 * @param minAttack The minimum attack range of the unit
	 * @param maxAttack The maximum attack range of the unit
	 * @param allies A list containing the allied teams of the current unit
	 * @param attacking Whether the unit is attacking or not
	 * @param flying Whether the unit is flying or not
	 * @author Jeffrey Goh
	 * @version v1.0
	 * @updated 12/6/2017
	 */
	public static Dictionary<Tile, List<Tile>> FindTarget(Tile originTile, int minRange, int maxRange, int minAttack, int maxAttack, List<int> allies, bool attacking, bool flying)
	{
		List<Tile> moveRange = FindHighlight(originTile, minRange, maxRange, allies, true, flying);
		moveRange.Add(originTile);

		// Dictionary of tiles
		// Key is a tile that can be attacked
		// The value is a list of the tiles that the unit can attack the key tile from
		Dictionary<Tile, List<Tile>> targets = new Dictionary<Tile, List<Tile>>();

		foreach (Tile t in moveRange)
		{
			// For each tile in the unit's move range
			if (t.occupied == null || t == originTile)
			{
				//If tile is not occupied, find the tiles it can attack from that tile
				List<Tile> attackRange = FindHighlight(t, minAttack, maxAttack, allies, false, flying);
				foreach (Tile a in attackRange)
				{
					if (a.occupied != null && a.occupied.currentHP > 0)
					{
						//For each tile it can attack from tile t
						if (targets.ContainsKey(a))
						{
							// If that tile coud already be attacked from another location, add t to the list of tiles under the key a
							targets[a].Add(t);
						}
						else if ((!allies.Contains(a.occupied.team) && attacking) || (allies.Contains(a.occupied.team) && !attacking))
						{
							
							// If a tile contains a hostile unit and we are attacking or if a tile contains an allied unit and we are performing a friendly action
							// We add that tile to the dictionary with key a and value of a List of tiles containing t
							List<Tile> temp = new List<Tile>();
							temp.Add(t);
							targets.Add(a, temp);
						}
						else
						{
							continue;
						}
					}
				}
			}
		}
		return targets;
	}

	/**
	 * Uses Find Highlight to return a List of Tiles containing capturable cities
	 * For AI usage mainly
	 * 
	 * @param originTile The tile our unit is on
	 * @param minrange The minimum move range of the unit
	 * @param maxrange The maximum move range of the unit
	 * @param allies A list containing the allied teams of the current unit
	 * @param flying Whether the unit is glying or not
	 * @author Jeffrey Goh
	 * @version v1.0
	 * @updated 19/7/2017
	 */
	public static List<Tile> FindCities(Tile originTile, int minRange, int maxRange, List<int> allies, bool flying)
	{
		List<Tile> moveRange = FindHighlightForCities(originTile, minRange, maxRange, allies, true, flying);

		List<Tile> toReturn =  new List<Tile>();

		bool allCaptured = true;
		foreach (Vector2 v in Grid.instance.villageStatus.Keys)
		{
			if (Grid.instance.villageStatus[v][0] != Grid.instance.currentTeam)
			{
				allCaptured = false;
				break;
			}
		}

		if (allCaptured)
		{
			return toReturn;
		}
		// Dictionary of tiles
		// Key is a tile that can be attacked
		// The value is a list of the tiles that the unit can attack the key tile from
		Dictionary<Tile, List<Tile>> targets = new Dictionary<Tile, List<Tile>>();

		foreach (Tile t in moveRange)
		{
			if (t.linkedTerrain.returnName() == "Village" && Grid.instance.villageStatus[t.gridPosition][0] != Grid.instance.currentTeam)
			{
				toReturn.Add(t);
			}
		}
		return toReturn;
	}


	/**
	 * Uses Find Highlight to return a list of tiles that is in the total attack range of the unit
	 * 
	 * @param originTile The tile our unit is on
	 * @param minrange The minimum move range of the unit
	 * @param maxrange The maximum move range of the unit
	 * @param minAttack The minimum attack range of the unit
	 * @param maxAttack The maximum attack range of the unit
	 * @param allies A list containing the allied teams of the current unit
	 * @param attacking Whether the unit is attacking or not
	 * @author Jeffrey Goh
	 * @version v1.0
	 * @updated 15/7/2017
	 */
	public static List<Tile> FindAttackRange(Tile originTile, int minRange, int maxRange, int minAttack, int maxAttack, List<int> allies, bool attacking, bool flying)
	{
		List<Tile> moveRange = FindHighlight(originTile, minRange, maxRange, allies, true, flying);
		moveRange.Add(originTile);

		List<Tile> output = new List<Tile>();

		foreach (Tile t in moveRange)
		{
			// For each tile in the unit's move range
			if (t.occupied == null || t == originTile)
			{
				//If tile is not occupied, find the tiles it can attack from that tile
				List<Tile> attackRange = FindHighlight(t, minAttack, maxAttack, allies, false, flying);
				foreach (Tile a in attackRange)
				{
					if (output.Contains(a))
					{
						continue;
					}
					else
					{
						output.Add(a);
					}
				}
			}
		}
		return output;
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TerrainS : MonoBehaviour
{

    private int config;
    private bool isPassable;
	

    public Material[] list;
	public string[] nameList;
    public int[] movementCostList;
    public int[] avdList;
	public int[] defList;
	public int[] healList;

	public Material defaultColour;


	/* Type 0: Normal tile
     * Type 1: Impassable tile
     * Type 2: Event tile
     * 
     */
	public int[] typeList;


    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    /**
    * Renders the terrain
    * Requires manual updating of materials in inspector. No safety net if out of array.
    * Trying to figure out how to seperate the Terrain prefab for different levels
    * Otherwise, have to load every material on one prefab...which is pretty bad
    * 
    * 
    * 
    * @param i is extracted from the text file mapConfig found in Grid
    * @author Wayne Neo
    * @version 1.1
    * @updated on 9/6/17
    */
    public void LoadTerrain(int i)
    {
		defaultColour = list[i];

		this.config = i;
        GetComponent<Renderer>().material = list[config];
        if (typeList[config] == 1)
        {
            this.isPassable = false;
        }
        else
        {
            this.isPassable = true;
        }
       
    }



	public string returnName()
	{
		return nameList[config];
	}
	public int returnCost()
    {
        return movementCostList[config];
    }

    public bool returnPassable()
    {
        return this.isPassable;
    }

    public int returnAvd()
    {
        return this.avdList[config];
    }

	public int returnDef()
	{
		return this.defList[config];
	}

	public int returnHeal()
	{
		return this.healList[config];
	}

}

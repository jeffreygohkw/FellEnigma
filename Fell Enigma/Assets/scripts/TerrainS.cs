using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TerrainS : MonoBehaviour
{

    private int config;
    private bool passable;

    public Material[] list;
    public int[] movementCost;
    public int[] modifier;


    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    /**
    * Renders the terrain and sets isPassable (should be seperated)
    * Requires manual updating of materials in inspector. No safety net if out of array.
    * Trying to figure out how to seperate the Terrain prefab for different levels
    * Otherwise, have to load every material on one prefab...which is pretty bad
    * @param i is extracted from the text file mapConfig found in Grid
    * @author Wayne Neo
    * @version 1.0
    * @updated on 9/6/17
    */
    public void LoadTerrain(int i)
    {
        this.config = i;
        GetComponent<Renderer>().material = list[config];

        if (movementCost[config] == -1)
        {
            this.passable = false;
        }
        else
        {
            this.passable = true;
        }
    }


    public int returnCost()
    {
        return movementCost[config];
    }

    public bool isPassable()
    {
        return this.passable;
    }

    public int returnModifier()
    {
        return this.modifier[config];
    }
   
}

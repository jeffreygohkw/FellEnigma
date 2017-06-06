using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainS : MonoBehaviour
{

    public Vector2 gridPosition = Vector2.zero;

    public Material[] list;


    // Use this for initialization
    void Start()
    {
        GetComponent<Renderer>().material = list[0];
    }

    // Update is called once per frame
    void Update()
    {

    }

   
}

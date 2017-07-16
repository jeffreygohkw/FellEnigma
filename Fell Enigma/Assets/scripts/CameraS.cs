using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraS : MonoBehaviour {

    public float Boundary = 0.05f; // distance from edge scrolling starts
    public float speed = 5;
    private float min_x;
    private float max_x;
    private float min_y;
    private float max_y;
    private float horiSize = 0;
    private float vertSize = 0;
    private bool justOnce = true;

    private Vector3 newPos;

	// Use this for initialization
	void Start ()
    {
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        // Now camera adjusts according to the grid size. Hopefully this works for all cases
        if (horiSize == 0 && vertSize == 0)
        {
            horiSize = Grid.instance.tilesPerRow;
            vertSize = Grid.instance.tilesPerCol;
        }

        if (justOnce)
        {
            min_x = transform.position.x - horiSize + 23.5f;
            max_x = transform.position.x + horiSize - 24;
            min_y = transform.position.y - vertSize + 16.5f;
            max_y = transform.position.y + vertSize - 20.6f;
            justOnce = false;
        }

        newPos = transform.position;

        // Movement by arrow keys
        if (Input.GetKey(KeyCode.RightArrow))
        {
            newPos.x += speed * Time.deltaTime; // move on +X axis
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            newPos.x -= speed * Time.deltaTime; // move on -X axis
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            newPos.y -= speed * Time.deltaTime; // move on -Y axis
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            newPos.y += speed * Time.deltaTime; // move on +Y axis
        }

        // Movement by moving mouse to edge of screen
        // Deactivated temporarily till can find right balance with UI
        /*if (Input.mousePosition.x > screenDim.x - Boundary)
        { 
            newPos.x += speed * Time.deltaTime; // move on +X axis
        }
        if (Input.mousePosition.x < 0 + Boundary)
        {
            newPos.x -= speed * Time.deltaTime; // move on -X axis
        }
        if (Input.mousePosition.y > screenDim.y - Boundary)
        {
            newPos.y += speed * Time.deltaTime; // move on +Y axis
        }
        if (Input.mousePosition.y < 0 + Boundary)
        {
            newPos.y -= speed * Time.deltaTime; // move on -Y axis
        }
        */

        transform.position = new Vector3(Mathf.Clamp(newPos.x, min_x, max_x), Mathf.Clamp(newPos.y, min_y, max_y), transform.position.z);
    }
}

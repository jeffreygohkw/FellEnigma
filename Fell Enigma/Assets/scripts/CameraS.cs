using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraS : MonoBehaviour {

    public float Boundary = 0.05f; // distance from edge scrolling starts
    public float speed = 5;
    public float min_x;
    public float max_x;
    public float min_y;
    public float max_y;

    private Vector2 screenDim;
    private Vector3 newPos;

	// Use this for initialization
	void Start ()
    {
        screenDim = new Vector2(Screen.width, Screen.height);
        min_x = transform.position.x - 5;
        max_x = transform.position.x + 1;
        min_y = transform.position.y - 10;
        max_y = transform.position.y + 4;
    }
	
	// Update is called once per frame
	void Update ()
    {

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

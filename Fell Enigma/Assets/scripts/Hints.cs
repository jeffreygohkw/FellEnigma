using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hints : MonoBehaviour {
    private CanvasGroup thisCanvas;
    private bool isOpen = false;

	// Use this for initialization
	void Start () {
        thisCanvas = this.GetComponent<CanvasGroup>();
	}
	
	// Update is called once per frame
	void Update () {
        // When H is pressed, toggle help window
        if (Input.GetKeyDown(KeyCode.H) )
        {
            isOpen = !isOpen;
        }
        // If mouse is clicked when help window is open
        else if (isOpen && Input.GetKeyDown(KeyCode.Mouse0))
        {
            isOpen = false;
        }

        if (isOpen)
        {
            thisCanvas.alpha = 1f;
        }
        else
        {
            thisCanvas.alpha = 0f;
        }
   
	}
}

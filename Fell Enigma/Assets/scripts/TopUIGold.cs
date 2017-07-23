using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopUIGold : MonoBehaviour {

    private Text textBar;

	// Use this for initialization
	void Start () {
        textBar = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        // Updates the text UI
        textBar.text = Grid.instance.gold.ToString() + "g";
    }

}

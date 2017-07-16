using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFullLog : MonoBehaviour {
    private bool hasClicked = false;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ButtonPress()
    {
        if (!hasClicked)
        {
            CombatLog.instance.ShowFullLog();
        }
        else
        {
            CombatLog.instance.HideFullLog();
        }
        hasClicked = !hasClicked;
    }
}

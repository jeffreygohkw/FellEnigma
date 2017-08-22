using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BPMenuSelection : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PressBanter()
    {
        // Empty for now
    }

    public void PressStatus()
    {
        EventManager.TriggerEvent("ToggleStatus");
    }

    public void PressShop()
    {
        EventManager.TriggerEvent("ToggleShop");
    }

    public void PressNext()
    {
        EventManager.TriggerEvent("ToggleNext");
    }
}

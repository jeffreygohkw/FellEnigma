using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerActionOther : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // If talk button is pressed, linked in inspector. Activates function in PlayerUnit
    public void talkClick()
    {
        EventManager.TriggerEvent("TalkUnit");
    }

    // If Objective button is pressed, linked in inspector. Activates respective function in PlayerUnit
    public void objClick()
    {
        if (ActionOtherUI.instance.canCap)
            EventManager.TriggerEvent("CapUnit");
        else if (ActionOtherUI.instance.canTav)
            EventManager.TriggerEvent("TavUnit");
        else if (ActionOtherUI.instance.canOther)
            EventManager.TriggerEvent("ObjUnit");
    }


}

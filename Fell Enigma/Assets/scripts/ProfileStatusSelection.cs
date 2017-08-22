using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileStatusSelection : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PressUnit1()
    {
        EventManager.TriggerEvent("SelectUnit1");
    }

    public void PressUnit2()
    {
        EventManager.TriggerEvent("SelectUnit2");
    }

    public void PressUnit3()
    {
        EventManager.TriggerEvent("SelectUnit3");
    }

    public void PressUnit4()
    {
        EventManager.TriggerEvent("SelectUnit4");
    }

    public void PressItem1()
    {
        EventManager.TriggerEvent("SelectItem1");
    }

    public void PressItem2()
    {
        EventManager.TriggerEvent("SelectItem2");
    }

    public void PressItem3()
    {
        EventManager.TriggerEvent("SelectItem3");
    }

    public void PressDis()
    {
        EventManager.TriggerEvent("DiscardItem");
    }

}

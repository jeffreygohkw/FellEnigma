using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actions : MonoBehaviour {
    public bool hasMoved = false;


    // Use this for initialization
    void Start()
    {
       
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // If Move button is pressed, linked in Inspector
    public void moveClick()
    {
        if (!hasMoved)
        {
            EventManager.TriggerEvent("MoveUnit");
        }
        else
        {
            EventManager.TriggerEvent("UndoMoveUnit");
        }
    }

    // If Attack button is pressed, linked in Inspector
    public void attackClick()
    {
        EventManager.TriggerEvent("AttackUnit");
        EventManager.TriggerEvent("AttackUnitStats");
    }

    // If Item button is pressed, linked in Inspector
    public void itemClick()
    {
        EventManager.TriggerEvent("ItemUnit");
    }

    // If Wait button is pressed, linked in Inspector
    public void waitClick()
    {
        EventManager.TriggerEvent("WaitUnit");
    }

    // If End button is pressed, linked in Inspector
    public void endClick()
    {
        EventManager.TriggerEvent("EndUnit");
    }
}

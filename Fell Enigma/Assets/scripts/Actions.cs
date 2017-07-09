using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actions : MonoBehaviour {
    public bool hasMoved = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

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

    public void attackClick()
    {
        EventManager.TriggerEvent("AttackUnit");
    }

    public void itemClick()
    {
        EventManager.TriggerEvent("ItemUnit");
    }

    public void waitClick()
    {
        EventManager.TriggerEvent("WaitUnit");
    }

    public void endClick()
    {
        EventManager.TriggerEvent("EndUnit");
    }
}

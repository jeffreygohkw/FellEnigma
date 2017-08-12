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
        EventManager.TriggerEvent("AttackUnitStatsOFF");
        EventManager.TriggerEvent("ItemUIOFF");
        ActionOtherUI.instance.OffAllUI();
        if (!hasMoved)
        {
            EventManager.TriggerEvent("MoveUnit");
            Debug.Log("MoveUnit Button called");
        }
        else
        {
            EventManager.TriggerEvent("UndoMoveUnit");
            Debug.Log("UndoMoveUnit Button called");
        }
    }

    // If Attack button is pressed, linked in Inspector
    public void attackClick()
    {
        ActionOtherUI.instance.OffAllUI();
        EventManager.TriggerEvent("ItemUIOFF");
        EventManager.TriggerEvent("AttackUnit");
    }

    // If Item button is pressed, linked in Inspector
    public void itemClick()
    {
        ActionOtherUI.instance.OffAllUI();
        EventManager.TriggerEvent("AttackUnitStatsOFF");
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

    // If Other button is pressed, linked in Inspector
    public void otherClick()
    {
        EventManager.TriggerEvent("AttackUnitStatsOFF");
        EventManager.TriggerEvent("ItemUIOFF");
        EventManager.TriggerEvent("OtherUnit");
    }

    // If Ult button is pressed, linked in Inspector
    public void ultClick()
    {
        if (Grid.instance.ultCharge == 100 && Grid.instance.commander >= 0 && Grid.instance.commander <= 4 && Grid.instance.ultActive == false)
        {
            Grid.instance.castUlt();
        }
        else
        {
            if (!Grid.instance.ultActive)
            {
                CombatLog.instance.AddEvent("Current Ult Charge: " + Grid.instance.ultCharge);
                CombatLog.instance.PrintEvent();
            }
            else
            {
                CombatLog.instance.AddEvent("Ultimate is currently active");
                CombatLog.instance.PrintEvent();
            }
        }
    }

}

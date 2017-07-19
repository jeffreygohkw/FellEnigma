using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerItem : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // When the first item slot is selected, linked in inspector
    public void selectItem0()
    {
        if (ItemUI.instance.getItemIndex() != 0)
        {
            ItemUI.instance.setItemIndex(0);
            ItemUI.instance.setItemSelected(true);
        }
        else
            ItemUI.instance.setItemIndex(-1);
    }

    // When the second item slot is selected, linked in inspector
    public void selectItem1()
    {
        if (ItemUI.instance.getItemIndex() != 1)
        {
            ItemUI.instance.setItemIndex(1);
            ItemUI.instance.setItemSelected(true);
        }
        else
            ItemUI.instance.setItemIndex(-1);

    }

    // When the third item slot is selected, linked in inspector
    public void selectItem2()
    {
        if (ItemUI.instance.getItemIndex() != 2)
        {
            ItemUI.instance.setItemIndex(2);
            ItemUI.instance.setItemSelected(true);
        }
        else
            ItemUI.instance.setItemIndex(-1);
    }

    // When equip/use is selected, linked in inspector
    public void equipUseItem()
    {
        EventManager.TriggerEvent("EquipUseItem");
    }

    // When discard is selected, linked in inspector
    public void discardItem()
    {
        EventManager.TriggerEvent("DiscardItem");
    }

}

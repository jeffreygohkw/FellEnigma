using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BPShopButtons : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
	}

    private void Awake()
    {

    }

    // Update is called once per frame
    void Update () {
		
	}

    public void PressShopItem0()
    {
        BPShopInventory.instance.selectedItemIndex = 0;
    }

    public void PressShopItem1()
    {
        BPShopInventory.instance.selectedItemIndex = 1;
    }

    public void PressShopItem2()
    {
        BPShopInventory.instance.selectedItemIndex = 2;
    }

    public void PressShopItem3()
    {
        BPShopInventory.instance.selectedItemIndex = 3;
    }

    public void PressShopItem4()
    {
        BPShopInventory.instance.selectedItemIndex = 4;
    }

    public void PressShopItem5()
    {
        BPShopInventory.instance.selectedItemIndex = 5;
    }

    public void PressShopCharNP()
    {
        BPShopInventory.instance.BuyItem("Naive Prince");
    }

    public void PressShopCharYR()
    {
        BPShopInventory.instance.BuyItem("Young Rebel");
    }

    public void PressShopCharKS()
    {
        BPShopInventory.instance.BuyItem("Kind Soul");
    }

    public void PressShopCharBH()
    {
        BPShopInventory.instance.BuyItem("Black Heart");
    }
}

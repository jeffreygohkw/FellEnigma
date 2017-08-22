using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BPStatus : MonoBehaviour {

    private Button[] buttons = new Button[3];
    private Button buttonDis;
    private Text stats;
    private Text[] buttonsText = new Text[3];
    private Text itemDes;

    private RawImage[] profiles = new RawImage[4];

    private int selectedItemIndex = -1;

    private string unitName;
    private string job;
    private int lvl;
    private int exp;
    private int maxHP;
    private int strength;
    private int mag; 
    private int skl;
    private int spd;
    private int luk;
    private int def;
    private int res;
    private int con;
    private int mov;
    private List<string[]> inventory;
    private int equippedIndex;
    private int weaponMt;
    //private bool weaponPhysical;
    private int weaponAcc;
    private int weaponCrit;
    private int weaponWt;
    private int weaponMinRange;
    private int weaponMaxRange;

	// Use this for initialization
	void Start () {
        buttons[0] = this.GetComponentsInChildren<Button>()[0];
        buttons[1] = this.GetComponentsInChildren<Button>()[1];
        buttons[2] = this.GetComponentsInChildren<Button>()[2];
        buttonDis = this.GetComponentsInChildren<Button>()[3];

        stats = this.GetComponentsInChildren<Text>()[0];
        buttonsText[0] = this.GetComponentsInChildren<Text>()[1];
        buttonsText[1] = this.GetComponentsInChildren<Text>()[2];
        buttonsText[2] = this.GetComponentsInChildren<Text>()[3];
        itemDes = this.GetComponentsInChildren<Text>()[4];

        profiles[0] = this.GetComponentsInChildren<RawImage>()[0];
        profiles[1] = this.GetComponentsInChildren<RawImage>()[1];
        profiles[2] = this.GetComponentsInChildren<RawImage>()[2];
        profiles[3] = this.GetComponentsInChildren<RawImage>()[3];

        EventManager.StartListening("IntStatus", IntStatus);
        EventManager.StartListening("SelectUnit1", SelectUnit1);
        EventManager.StartListening("SelectUnit2", SelectUnit2);
        EventManager.StartListening("SelectUnit3", SelectUnit3);
        EventManager.StartListening("SelectUnit4", SelectUnit4);
        EventManager.StartListening("SelectItem1", SelectItem1);
        EventManager.StartListening("SelectItem2", SelectItem2);
        EventManager.StartListening("SelectItem3", SelectItem3);
        EventManager.StartListening("DiscardItem", DiscardItem);
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    /**
    * EventManager: Initialises the profiles of the main characters, checking for death
    *
    * @author Wayne Neo
    * @version 1.0
    * @updated on 22/08/17
    */
    void IntStatus()
    {
        if (BPMain.instance.npDead)
        {
            profiles[0].color = new Color(0.28f,0f,0f,1f);
        }
        if (BPMain.instance.yrDead)
        {
            profiles[1].color = new Color(0.28f, 0f, 0f, 1f);
        }
        if (BPMain.instance.ksDead)
        {
            profiles[2].color = new Color(0.28f, 0f, 0f, 1f);
        }
        if (BPMain.instance.bhDead)
        {
            profiles[3].color = new Color(0.28f, 0f, 0f, 1f);
        }
    }


    /**
    * EventManager: Selects unit 1 and displays status if not dead
    *
    * @author Wayne Neo
    * @version 1.0
    * @updated on 22/08/17
    */
    void SelectUnit1() {

        if (profiles[0].color != new Color(0.28f, 0f, 0f, 1f))
        {
            unitName = GameControl.instance.npNameJob[0];
            job = GameControl.instance.npNameJob[1];

            lvl = GameControl.instance.npIntData[0];
            exp = GameControl.instance.npIntData[1];
            maxHP = GameControl.instance.npIntData[2];
            strength = GameControl.instance.npIntData[4];
            mag = GameControl.instance.npIntData[5];
            skl = GameControl.instance.npIntData[6];
            spd = GameControl.instance.npIntData[7];
            luk = GameControl.instance.npIntData[8];
            def = GameControl.instance.npIntData[9];
            res = GameControl.instance.npIntData[10];
            con = GameControl.instance.npIntData[11];
            mov = GameControl.instance.npIntData[12];
            inventory = GameControl.instance.npInventory;
            equippedIndex = GameControl.instance.npIntData[21];
            weaponMt = GameControl.instance.npIntData[22];
            //weaponPhysical = GameControl.instance.npBoolData[2];
            weaponAcc = GameControl.instance.npIntData[23];
            weaponCrit = GameControl.instance.npIntData[24];
            weaponWt = GameControl.instance.npIntData[25];
            weaponMinRange = GameControl.instance.npIntData[26];
            weaponMaxRange = GameControl.instance.npIntData[27];
            updateStats();
        }
    }

    /**
    * EventManager: Selects unit 2 and displays status if not dead
    *
    * @author Wayne Neo
    * @version 1.0
    * @updated on 22/08/17
    */
    void SelectUnit2()
    {
        if (profiles[1].color != new Color(0.28f, 0f, 0f, 1f))
        {
            unitName = GameControl.instance.yrNameJob[0];
            job = GameControl.instance.yrNameJob[1];

            lvl = GameControl.instance.yrIntData[0];
            exp = GameControl.instance.yrIntData[1];
            maxHP = GameControl.instance.yrIntData[2];
            strength = GameControl.instance.yrIntData[4];
            mag = GameControl.instance.yrIntData[5];
            skl = GameControl.instance.yrIntData[6];
            spd = GameControl.instance.yrIntData[7];
            luk = GameControl.instance.yrIntData[8];
            def = GameControl.instance.yrIntData[9];
            res = GameControl.instance.yrIntData[10];
            con = GameControl.instance.yrIntData[11];
            mov = GameControl.instance.yrIntData[12];
            inventory = GameControl.instance.yrInventory;
            equippedIndex = GameControl.instance.yrIntData[21];
            updateStats();
        }
    }

    /**
    * EventManager: Selects unit 3 and displays status if not dead
    *
    * @author Wayne Neo
    * @version 1.0
    * @updated on 22/08/17
    */
    void SelectUnit3()
    {
        if (profiles[2].color != new Color(0.28f, 0f, 0f, 1f))
        {
            unitName = GameControl.instance.ksNameJob[0];
            job = GameControl.instance.ksNameJob[1];

            lvl = GameControl.instance.ksIntData[0];
            exp = GameControl.instance.ksIntData[1];
            maxHP = GameControl.instance.ksIntData[2];
            strength = GameControl.instance.ksIntData[4];
            mag = GameControl.instance.ksIntData[5];
            skl = GameControl.instance.ksIntData[6];
            spd = GameControl.instance.ksIntData[7];
            luk = GameControl.instance.ksIntData[8];
            def = GameControl.instance.ksIntData[9];
            res = GameControl.instance.ksIntData[10];
            con = GameControl.instance.ksIntData[11];
            mov = GameControl.instance.ksIntData[12];
            inventory = GameControl.instance.ksInventory;
            equippedIndex = GameControl.instance.ksIntData[21];
            updateStats();
        }
    }

    /**
    * EventManager: Selects unit 4 and displays status if not dead
    *
    * @author Wayne Neo
    * @version 1.0
    * @updated on 22/08/17
    */
    void SelectUnit4()
    {
        if (profiles[3].color != new Color(0.28f, 0f, 0f, 1f))
        {
            unitName = GameControl.instance.bhNameJob[0];
            job = GameControl.instance.bhNameJob[1];

            lvl = GameControl.instance.bhIntData[0];
            exp = GameControl.instance.bhIntData[1];
            maxHP = GameControl.instance.bhIntData[2];
            strength = GameControl.instance.bhIntData[4];
            mag = GameControl.instance.bhIntData[5];
            skl = GameControl.instance.bhIntData[6];
            spd = GameControl.instance.bhIntData[7];
            luk = GameControl.instance.bhIntData[8];
            def = GameControl.instance.bhIntData[9];
            res = GameControl.instance.bhIntData[10];
            con = GameControl.instance.bhIntData[11];
            mov = GameControl.instance.bhIntData[12];
            inventory = GameControl.instance.bhInventory;
            equippedIndex = GameControl.instance.bhIntData[21];

            updateStats();
        }
    }

    /**
    * Helper method to update/refresh the stats portion of status
    *
    * @author Wayne Neo
    * @version 1.0
    * @updated on 22/08/17
    */
    private void updateStats()
    {
        int count = 0;

        selectedItemIndex = -1;
        buttonDis.interactable = false;
        itemDes.text = " ";
        stats.text = unitName + " (JOB: " + job + " LVL:" + lvl + ")\nSTR: " + strength +
            " (Stat that determines physical damage dealt)\nMAG: " + mag + " (Stat that determines magical damage dealt)\nSKL: " +
            skl + " (Stat that determines accuracy and critical hit rate)\nSPD: " + spd +
            " (Stat that determines evasion and follow-up attacks)\nLUK: " + luk +
            " (Stat that affects various factors in battle)\nDEF: " + def + " (Stat that determines physical damage received)\nRES: " +
            res + " (Stat that determines magical damage received)";

        foreach (string[] item in inventory)
        {
            buttons[count].interactable = true;
            buttonsText[count].text = item[1];
            count++;
        }

        
        for (int i = count; i <= 2; i++)
        {
            buttons[count].interactable = false;
            buttonsText[count].text = "Item " + count;
        }
    }

    /**
    * Helper method to obtain description from respective categories of items/weapons
    *
    * @author Wayne Neo
    * @version 1.0
    * @updated on 22/08/17
    */
    private void getItemDes()
    {
        if(inventory[selectedItemIndex][0].Equals("Staff"))
        {
            itemDes.text = obtainItemDes(10);
        }
        else if (inventory[selectedItemIndex][0].Equals("StatBoost"))
        {
            itemDes.text = obtainItemDes(6);
        }
        else if (inventory[selectedItemIndex][0].Equals("Consumable"))
        {
            itemDes.text = obtainItemDes(5);
        }
        else if (inventory[selectedItemIndex][0].Equals("Key"))
        {
            itemDes.text = obtainItemDes(4);
        }
        else if (inventory[selectedItemIndex].Length == 14)
        {
            itemDes.text = obtainItemDes(13);
        }
        else
        {
            itemDes.text = "N/A";
        }
    }


    /**
    * EventManager: Selects item 1 and allows the discard button to be pressed
    *
    * @author Wayne Neo
    * @version 1.0
    * @updated on 22/08/17
    */
    void SelectItem1()
    {
        selectedItemIndex = 0;
        buttonDis.interactable = true;
        getItemDes();
    }


    /**
    * EventManager: Selects item 2 and allows the discard button to be pressed
    *
    * @author Wayne Neo
    * @version 1.0
    * @updated on 22/08/17
    */
    void SelectItem2()
    {
        selectedItemIndex = 1;
        buttonDis.interactable = true;
        getItemDes();
    }


    /**
    * EventManager: Selects item 3 and allows the discard button to be pressed
    *
    * @author Wayne Neo
    * @version 1.0
    * @updated on 22/08/17
    */
    void SelectItem3()
    {
        selectedItemIndex = 2;
        buttonDis.interactable = true;
        getItemDes();
    }

    /**
    * EventManager: Discards item selected
    *
    * @author Wayne Neo
    * @version 1.0
    * @updated on 22/08/17
    */
    void DiscardItem()
    {
        itemDes.text = inventory[selectedItemIndex][1] + " discarded";
        buttonDis.interactable = false;
        if (unitName == "Naive Prince")
        {
            GameControl.instance.npInventory.RemoveAt(selectedItemIndex);
            if (equippedIndex == selectedItemIndex)
            {
                GameControl.instance.npIntData[21] = -1;
                GameControl.instance.npIntData[22] = 0; // weapon MT
                //GameControl.instance.npBoolData[2]; // weaponPhysical
                GameControl.instance.npIntData[23] = 0; // weaponAcc
                GameControl.instance.npIntData[24] = 0; // weaponCrit
                GameControl.instance.npIntData[25] = 0; // weaponWt
                GameControl.instance.npIntData[26] = 0; // weaponMinRange
                GameControl.instance.npIntData[27] = 0; // weaponMaxRange
            }
        }
        else if (unitName == "Young Rebel")
        {
            GameControl.instance.yrInventory.RemoveAt(selectedItemIndex);
            if (equippedIndex == selectedItemIndex)
            {
                GameControl.instance.yrIntData[21] = -1;
                GameControl.instance.yrIntData[22] = 0; // weapon MT
                //GameControl.instance.yrBoolData[2]; // weaponPhysical
                GameControl.instance.yrIntData[23] = 0; // weaponAcc
                GameControl.instance.yrIntData[24] = 0; // weaponCrit
                GameControl.instance.yrIntData[25] = 0; // weaponWt
                GameControl.instance.yrIntData[26] = 0; // weaponMinRange
                GameControl.instance.yrIntData[27] = 0; // weaponMaxRange
            }
        }
        else if (unitName == "Kind Soul")
        {
            GameControl.instance.ksInventory.RemoveAt(selectedItemIndex);
            if (equippedIndex == selectedItemIndex)
            {
                GameControl.instance.ksIntData[21] = -1;
                GameControl.instance.ksIntData[22] = 0; // weapon MT
                //GameControl.instance.ksBoolData[2]; // weaponPhysical
                GameControl.instance.ksIntData[23] = 0; // weaponAcc
                GameControl.instance.ksIntData[24] = 0; // weaponCrit
                GameControl.instance.ksIntData[25] = 0; // weaponWt
                GameControl.instance.ksIntData[26] = 0; // weaponMinRange
                GameControl.instance.ksIntData[27] = 0; // weaponMaxRange
            }
        }
        else if (unitName == "Black Heart")
        {
            GameControl.instance.bhInventory.RemoveAt(selectedItemIndex);
            if (equippedIndex == selectedItemIndex)
            {
                GameControl.instance.bhIntData[21] = -1;
                GameControl.instance.bhIntData[22] = 0; // weapon MT
                //GameControl.instance.bhBoolData[2]; // weaponPhysical
                GameControl.instance.bhIntData[23] = 0; // weaponAcc
                GameControl.instance.bhIntData[24] = 0; // weaponCrit
                GameControl.instance.bhIntData[25] = 0; // weaponWt
                GameControl.instance.bhIntData[26] = 0; // weaponMinRange
                GameControl.instance.bhIntData[27] = 0; // weaponMaxRange
            }
        }
        updateStats();
    }

    /**
    * Extracts the description text from the item  
    *
    * @param i the number of words in the specific category of Item
    * @author Wayne Neo
    * @version 1.0
    * @updated on 10/8/17
    */
    private string obtainItemDes(int i)
    {
        string[] words;
        string mainLine = "";
        words = inventory[selectedItemIndex][i].Split("$"[0]);

        foreach (string word in words)
        {
            mainLine += (word + " ");
        }

        return mainLine;
    }
}

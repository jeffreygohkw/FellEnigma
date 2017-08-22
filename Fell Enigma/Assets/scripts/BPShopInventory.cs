using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BPShopInventory : MonoBehaviour {

    public static BPShopInventory instance;

    private Dictionary<string, Dictionary<string, string[]>> weaponCatalog = new Dictionary<string, Dictionary<string, string[]>>();
    public TextAsset database;

    private Dictionary<string, Dictionary<string, string[]>> itemCatalog = new Dictionary<string, Dictionary<string, string[]>>();
    public TextAsset itemDatabase;


    public int selectedItemIndex = -1;
    private int[] cost;

    public string[] shopItemsType;
    public string[] shopItemsName;
    public string[] shopWeaponsType;
    public string[] shopWeaponsName;

    private Text[] shopButtons;
    public Text shopLog;

    private Button[] charButtons = new Button[4];

	// Use this for initialization
	void Start () {
        shopButtons = new Text[shopItemsType.Length + shopWeaponsType.Length];
        cost = new int[shopItemsType.Length + shopWeaponsType.Length];

        for (int i = 0; i < shopItemsType.Length + shopWeaponsType.Length; i++)
        {
            shopButtons[i] = this.GetComponentsInChildren<Text>()[i];
        }


        for (int i = 0; i < 3; i++)
        {
            charButtons[i] = this.GetComponentsInChildren<Button>()[shopItemsType.Length + shopWeaponsType.Length + i];
        }
	}

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update () {

        // Check for death
        if (selectedItemIndex != -1)
        {
            if (!BPMain.instance.npDead)
            {
                charButtons[0].interactable = true;
            }
            if (!BPMain.instance.yrDead)
            {
                charButtons[1].interactable = true;
            }
            /*if (BPMain.instance.ksDead)
            {
                charButtons[2].interactable = false;
            }
            if (BPMain.instance.bhDead)
            {
                charButtons[3].interactable = false;
            }*/
        }
        else
        {
            // No item selected
            for (int i = 0; i < 3; i++)
            {
                charButtons[i].interactable = false;
            }
        }


    }

    /**
    * Initialises the item/weapon database by reading the text assets
    * Copied directly from Item class
    *
    * @author Wayne Neo
    * @version 1.0
    * @updated on 22/08/17
    */
    public void initialiseItems()
    {
        // Splits the text file into lines
        string[] lines = database.text.Split("\r"[0]);
        string[] line;
        string[] itemLines = itemDatabase.text.Split("\r"[0]);

        Dictionary<string, string[]> type = new Dictionary<string, string[]>();
        // Iterate through the text file
        for (int i = 2; i < lines.Length; i++)
        {
            // Splits the text file into strings
            line = lines[i].Split(" "[0]);
            if (line.Length == 1)
            {
                // The weapon types are seperated by the name of the weapon type
                // Add each weapon type to the weaponCatalog and reset the dictionary for the next type
                weaponCatalog.Add(line[0], type);
                type = new Dictionary<string, string[]>();
            }
            else
            {
                //Add the weapon to type with the key being the weapon's name
                type.Add(line[1], line);
            }
        }

        for (int i = 2; i < itemLines.Length; i++)
        {
            // Splits the text file into strings
            line = itemLines[i].Split(" "[0]);
            if (line.Length == 1)
            {
                // The weapon types are seperated by the name of the item type
                // Add each item type to the itemCatalog and reset the dictionary for the next type
                itemCatalog.Add(line[0], type);
                type = new Dictionary<string, string[]>();
            }
            else
            {
                //Add the weapon to type with the key being the item's name
                type.Add(line[1], line);
            }
        }
    }


    /**
    * Initialises the shop based on the list set in the Inspector
    * Assumed that the list and buttons in Inspector match each other in terms of count
    *
    * @author Wayne Neo
    * @version 1.0
    * @updated on 22/08/17
    */
    public void initialiseShop()
    {
       int i = 0;
       int j = 0;
       while (i < shopItemsType.Length)
       {
            if (itemCatalog.ContainsKey(shopItemsType[j]) && itemCatalog[shopItemsType[j]].ContainsKey(shopItemsName[j]))
            {
                if (itemCatalog[shopItemsType[j]][shopItemsName[j]][0].Equals("Staff"))
                {
                    
                    string[] words;
                    string itemDes = "";
                    words = itemCatalog[shopItemsType[j]][shopItemsName[j]][10].Split("$"[0]);

                    foreach (string word in words)
                    {
                        itemDes += (word + " ");
                    }

                    shopButtons[i].text = itemCatalog[shopItemsType[j]][shopItemsName[j]][1] + " (Category: Staff, Cost: " + itemCatalog[shopItemsType[j]][shopItemsName[j]][9] 
                        + ", " + itemDes;
                    cost[i] = System.Int32.Parse(itemCatalog[shopItemsType[j]][shopItemsName[j]][9]);
                }
                else if (itemCatalog[shopItemsType[j]][shopItemsName[j]][0].Equals("StatBoost"))
                {
                    string[] words;
                    string itemDes = "";
                    words = itemCatalog[shopItemsType[j]][shopItemsName[j]][6].Split("$"[0]);

                    foreach (string word in words)
                    {
                        itemDes += (word + " ");
                    }

                    shopButtons[i].text = itemCatalog[shopItemsType[j]][shopItemsName[j]][1] + " (Category: StatBoost, Cost: " + itemCatalog[shopItemsType[j]][shopItemsName[j]][3]
                        + ", " + itemDes;
                    cost[i] = System.Int32.Parse(itemCatalog[shopItemsType[j]][shopItemsName[j]][3]);
                }
                else if (itemCatalog[shopItemsType[j]][shopItemsName[j]][0].Equals("Consumable"))
                {
                    string[] words;
                    string itemDes = "";
                    words = itemCatalog[shopItemsType[j]][shopItemsName[j]][5].Split("$"[0]);

                    foreach (string word in words)
                    {
                        itemDes += (word + " ");
                    }

                    shopButtons[i].text = itemCatalog[shopItemsType[j]][shopItemsName[j]][1] + " (Category: Consumable, Cost: " +itemCatalog[shopItemsType[j]][shopItemsName[j]][4]
                        + ", " + itemDes;
                    cost[i] = System.Int32.Parse(itemCatalog[shopItemsType[j]][shopItemsName[j]][4]);
                }
            }
            i++;
            j++;
       }
       j = 0;
       while (j < shopWeaponsType.Length)
       {
           if (weaponCatalog.ContainsKey(shopWeaponsType[j]) && weaponCatalog[shopWeaponsType[j]].ContainsKey(shopWeaponsName[j]))
           {
               shopButtons[i].text = weaponCatalog[shopWeaponsType[j]][shopWeaponsName[j]][1] + " (Category: " + weaponCatalog[shopWeaponsType[j]][shopWeaponsName[j]][0] + ", Range: "
                   + weaponCatalog[shopWeaponsType[j]][shopWeaponsName[j]][3] + "-" + weaponCatalog[shopWeaponsType[j]][shopWeaponsName[j]][4] + ", WT: " 
                   + weaponCatalog[shopWeaponsType[j]][shopWeaponsName[j]][5] + ", MT: " + weaponCatalog[shopWeaponsType[j]][shopWeaponsName[j]][6] + ", HIT: " 
                   + weaponCatalog[shopWeaponsType[j]][shopWeaponsName[j]][7] + ", CRIT: " + weaponCatalog[shopWeaponsType[j]][shopWeaponsName[j]][8] + ", Cost: "
                   + weaponCatalog[shopWeaponsType[j]][shopWeaponsName[j]][11] + ")";
                cost[i] = System.Int32.Parse(weaponCatalog[shopWeaponsType[j]][shopWeaponsName[j]][11]);
            }
           i++;
           j++;
       }
    }

    /**
    * Purchases item for the selected unit, if possible
    *
    * @param unit string containing character's name. Manually inputted in buttons
    * @author Wayne Neo
    * @version 1.0
    * @updated on 22/08/17
    */
    public void BuyItem(string unit)
    {
        // Check for space
        bool gotSpace = false;
        if (unit == "Naive Prince" && GameControl.instance.npInventory.Count <= 3)
        {
            gotSpace = true;
        }
        else if (unit == "Young Rebel" && GameControl.instance.yrInventory.Count <= 3)
        {
            gotSpace = true;
        }
        else if (unit == "Kind Soul" && GameControl.instance.ksInventory.Count <= 3)
        {
            gotSpace = true;
        }
        else if (unit == "Young Rebel" && GameControl.instance.bhInventory.Count <= 3)
        {
            gotSpace = true;
        }

        // After checking for space, check for gold
        if (!gotSpace)
        {
            shopLog.text = "This unit has no space available, go to Status to discard";
        }
        else if (GameControl.instance.gold < cost[selectedItemIndex])
        {
            shopLog.text = "Not enough gold to purchase";
        }
        else if (GameControl.instance.gold >= cost[selectedItemIndex])
        {
            GameControl.instance.gold -= cost[selectedItemIndex];
            if (unit == "Naive Prince")
            {
                // If item bought is a item
                if (selectedItemIndex < shopItemsType.Length)
                {
                    GameControl.instance.npInventory.Add(itemCatalog[shopItemsType[selectedItemIndex]][shopItemsName[selectedItemIndex]]);
                }
                else // Item is a weapon
                {
                    GameControl.instance.npInventory.Add(weaponCatalog[shopWeaponsType[selectedItemIndex - shopItemsType.Length]][shopWeaponsName[selectedItemIndex - shopItemsType.Length]]);
                }
            }
            else if (unit == "Young Rebel")
            {
                // If item bought is a item
                if (selectedItemIndex < shopItemsType.Length)
                {
                    GameControl.instance.yrInventory.Add(itemCatalog[shopItemsType[selectedItemIndex]][shopItemsName[selectedItemIndex]]);
                }
                else // Item is a weapon
                {
                    GameControl.instance.yrInventory.Add(weaponCatalog[shopWeaponsType[selectedItemIndex - shopItemsType.Length]][shopWeaponsName[selectedItemIndex - shopItemsType.Length]]);
                }
            }
            else if (unit == "Kind Soul")
            {
                // If item bought is a item
                if (selectedItemIndex < shopItemsType.Length)
                {
                    GameControl.instance.ksInventory.Add(itemCatalog[shopItemsType[selectedItemIndex]][shopItemsName[selectedItemIndex]]);
                }
                else // Item is a weapon
                {
                    GameControl.instance.ksInventory.Add(weaponCatalog[shopWeaponsType[selectedItemIndex - shopItemsType.Length]][shopWeaponsName[selectedItemIndex - shopItemsType.Length]]);
                }
            }
            else if (unit == "Black Heart")
            {
                // If item bought is a item
                if (selectedItemIndex < shopItemsType.Length)
                {
                    GameControl.instance.bhInventory.Add(itemCatalog[shopItemsType[selectedItemIndex]][shopItemsName[selectedItemIndex]]);
                }
                else // Item is a weapon
                {
                    GameControl.instance.bhInventory.Add(weaponCatalog[shopWeaponsType[selectedItemIndex - shopItemsType.Length]][shopWeaponsName[selectedItemIndex - shopItemsType.Length]]);
                }
            }

            shopLog.text = "Item purchased";
            selectedItemIndex = -1;
        }
    }
}

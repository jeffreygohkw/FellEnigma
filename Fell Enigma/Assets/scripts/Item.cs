using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

	public static Item instance;

	public void Awake()
	{
		instance = this;
	}

	private Dictionary<string, Dictionary<string, string[]>> weaponCatalog = new Dictionary<string, Dictionary<string, string[]>>();
	public TextAsset database;

	private Dictionary<string, Dictionary<string, string[]>> itemCatalog = new Dictionary<string, Dictionary<string, string[]>>();
	public TextAsset itemDatabase;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

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
	 * Adds a fresh item to the character's inventory if he has space in it
	 * 
	 * @param character The character to equip the weapon
	 * @weaponType The type of weapon
	 * @weaponName The name of the weapon
	 * @author Jeffrey Goh
	 * @version v1.0
	 * @updated 2/7/2017
	 */
	public void addItem(Unit character, string itemType, string itemName)
	{
		if (!itemCatalog.ContainsKey(itemType))
		{
			Debug.Log("Wrong itemType");
		}
		else if (!itemCatalog[itemType].ContainsKey(itemName))
		{
			Debug.Log("Wrong itemName");
		}
		else if (character.inventory.Count >= character.inventorySize)
		{
			Debug.Log("Discard an item first");
		}
		else
		{
			character.inventory.Add(itemCatalog[itemType][itemName]);
		}
	}

	/**
	* Adds a fresh weapon from the catalog to the character if the character's inventory is empty
	* 
	* @param character The character to equip the weapon
	* @weaponType The type of weapon
	* @weaponName The name of the weapon
	* @author Jeffrey Goh
	* @version v1.0
	* @updated 2/7/2017
	*/
	public void addWeapon(Unit character, string weaponType, string weaponName)
	{
		if (!weaponCatalog.ContainsKey(weaponType))
		{
			Debug.Log("Wrong weaponType");
		}
		else if (!weaponCatalog[weaponType].ContainsKey(weaponName))
		{
			Debug.Log("Wrong weaponName");
		}
		else
		{
			if (character.inventory.Count < character.inventorySize)
			{
				character.inventory.Add(weaponCatalog[weaponType][weaponName]);
			}
			else
			{
				Debug.Log("Inventory is full");
			}
		}
	}

	/**
	* Equips a character with a fresh weapon from the catalog
	* 
	* v1.1
	* Checks for weapon type
	* 
	* @param character The character to equip the weapon
	* @weaponType The type of weapon
	* @weaponName The name of the weapon
	* @author Jeffrey Goh
	* @version v1.1
	* @updated 6/7/2017
	*/
	public void equipWeapon(Unit character, string weaponType, string weaponName)
	{
		if (!weaponCatalog.ContainsKey(weaponType))
		{
			Debug.Log("Wrong weaponType");
		}
		else if (!weaponCatalog[weaponType].ContainsKey(weaponName))
		{
			Debug.Log("Wrong weaponName");
		}
		else if (!character.proficiency.Contains(weaponType))
		{
			Debug.Log(character.unitName + " cannot equip a weapon of this type.");
		}
		else
		{
			if (character.inventory.Count < character.inventorySize)
			{
				character.inventory.Add(weaponCatalog[weaponType][weaponName]);
				string[] weaponStats = weaponCatalog[weaponType][weaponName];

				character.weaponMt = int.Parse(weaponStats[6]);

				if (weaponType != "Tome")
				{
					character.weaponPhysical = true;
				}
				else
				{
					character.weaponPhysical = false;
				}
				character.weaponAcc = int.Parse(weaponStats[7]);
				character.weaponCrit = int.Parse(weaponStats[8]);
				character.weaponWt = int.Parse(weaponStats[5]);
				character.weaponMinRange = int.Parse(weaponStats[3]);
				character.weaponMaxRange = int.Parse(weaponStats[4]);
				character.equippedIndex = character.inventory.Count - 1;
			}
			else
			{
				Debug.Log("Inventory is full");
			}
		}
	}



}

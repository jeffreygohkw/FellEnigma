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

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void initialiseWeapons()
	{
		// Splits the text file into lines
		string[] lines = database.text.Split("\r"[0]);
		string[] line;
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
	}

	/**
	 * Adds a fresh weapon to the character's inventory if he has space in it
	 * 
	 * @param character The character to equip the weapon
	 * @weaponType The type of weapon
	 * @weaponName The name of the weapon
	 * @author Jeffrey Goh
	 * @version v1.0
	 * @updated 2/7/2017
	 */
	public void addToInventory(Unit character, string weaponType, string weaponName)
	{
		if (!weaponCatalog.ContainsKey(weaponType))
		{
			Debug.Log("Wrong weaponType");
		}
		else if (!weaponCatalog[weaponType].ContainsKey(weaponName))
		{
			Debug.Log("Wrong weaponName");
		}
		else if (character.inventory.Count >= character.inventorySize)
		{
			Debug.Log("Discard an item first");
		}
		else
		{
			character.inventory.Add(weaponCatalog[weaponType][weaponName]);
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
	* @param character The character to equip the weapon
	* @weaponType The type of weapon
	* @weaponName The name of the weapon
	* @author Jeffrey Goh
	* @version v1.0
	* @updated 2/7/2017
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
		else
		{
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
		}
	}
}

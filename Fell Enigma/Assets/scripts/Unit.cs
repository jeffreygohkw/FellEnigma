using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour {

	public Vector2 gridPosition = Vector2.zero;

	public bool selected = false;

	public int team = 0;
	public List<int> allies = new List<int>();
	public int index;

	public Vector3 moveTo;
	public float moveSpeed = 10.0f;

	public bool isMoving = false;
	public bool isFighting = false;
	public bool isHealing = false;
	public bool isTalking = false;
	public int activeStaffIndex = -1;
	public bool doneAction = false;
	public bool doneMoving = false;
	public bool willAttack = false;
	public bool highlighted = false;
	public bool displayInventory = false;
	public int selectedItemIndex = -1;
	public bool displayTavern = false;
	public bool foundCity = false;

	

	/*
	 * 0: Recruit
	 * 1: ???
	 */
	public Dictionary<string, int> canTalk = new Dictionary<string, int>();

	public List<string> proficiency = new List<string>();

	// Unit stats
	public string unitName;
	public string job;
	//1 for true
	public int isBoss = 0;
	public int isThief = 0;
	public bool isHero = false;
	public int classPower = 3;
	public int classBonusA;
	public int classBonusB = 0;
	public int lvl;
	public int exp;
	public int maxHP;
	public int currentHP;
	public int strength;
	public int mag;
	public int skl;
	public int spd;
	public int luk;
	public int def;
	public int res;
	public int con;
	public int mov;

	public bool isFlying = false;
	

	//Growths
	public int hpG;
	public int strG;
	public int magG;
	public int sklG;
	public int spdG;
	public int lukG;
	public int defG;
	public int resG;

	// Weapon stats
	public int equippedIndex = -1;
	public int weaponMt;
	public bool weaponPhysical;
	public int weaponAcc;
	public int weaponCrit;
	public int weaponWt;
	public int weaponMinRange;
	public int weaponMaxRange;


	public int inventorySize = 3;
	public List<string[]> inventory = new List<string[]>();

	//Ults
	public int weaponRangeBuff = 0;
	public bool rebelBuff = false;

	/*
	 * 0: Aggressive
	 * 1: Passive
	 * 2: Stationary
	 * 3: Target
	 * 4: City seeking
	 */
	public int ai_id;
	public List<int> ai_id_priority = new List<int>();



	//movement animation
	public List<Vector2> positionQueue = new List<Vector2>();

    public Camera mainCam;

	private void Awake()
	{
		moveTo = transform.position;
		/*
		stats.Add("maxHP", maxHP);
		stats.Add("strength", strength);
		stats.Add("mag", mag);
		stats.Add("skl", skl);
		stats.Add("spd", spd);
		stats.Add("luk", luk);
		stats.Add("def", def);
		stats.Add("res", res);
		stats.Add("mov", mov);
		stats.Add("con", con);
		*/
	}

	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{
		
	}

	public virtual void turnUpdate()
	{
		
	}

	public virtual void OnGUI()
	{

    }

	public virtual void playerWait()
	{

	}

    // Updates UI only when mouseOver
   public void OnMouseOver()
   {
        if (this.currentHP != 0)
        EventManager.TriggerEvent("GetStats");    
   }

    public void OnMouseExit()
    {
        EventManager.TriggerEvent("RemoveStats");
    }
    

}

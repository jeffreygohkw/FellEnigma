using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

	public Vector2 gridPosition = Vector2.zero;

	public bool selected = false;

	public int team;
	public List<int> allies = new List<int>();
	public int index;

	public Vector3 moveTo;
	public float moveSpeed = 10.0f;

	public bool isMoving = false;
	public bool isFighting = false;
	public bool doneAction = false;
	public bool doneMoving = false;
	public bool willAttack = false;

	public bool movementType;

	// Unit stats
	public string unitName;
	public string job;
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

	// Weapon stats
	public int weaponMt;
	public bool weaponPhysical;
	public int weaponAcc;
	public int weaponCrit;
	public int weaponWt;
	public int weaponMinRange;
	public int weaponMaxRange;

	//movement animation
	public List<Vector2> positionQueue = new List<Vector2>();


	private void Awake()
	{
		moveTo = transform.position;
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



}

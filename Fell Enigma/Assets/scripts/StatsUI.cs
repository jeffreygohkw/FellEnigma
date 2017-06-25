using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI: MonoBehaviour {

    public Canvas statsWindow;
    private Canvas UI;
    private Slider healthBar;
    private Text displayName;
    private Text displayStats;
    private Unit thisUnit;

	// Use this for initialization
	void Start ()
    {
        // Obtains Unit component (necessary for this to work)
        if (this.CompareTag("Player"))
        {
            thisUnit = this.GetComponent<PlayerUnit>();
        }
        else if (this.CompareTag("Enemy"))
        {
            thisUnit = this.GetComponent<AIUnit>();
        }
		else if (this.CompareTag("Stationary"))
		{
			thisUnit = this.GetComponent<StationaryUnit>();
		}

        // Obtains the various components under the UI prefab
        // Note that Text components need to be in order (aka don't change the order in Inspector)
		UI = Instantiate(statsWindow, new Vector3(Screen.width,Screen.height,0), Quaternion.Euler(new Vector3(90, 0, 0)));
        healthBar = UI.GetComponentInChildren<Slider>();
        displayName = UI.GetComponentsInChildren<Text>()[0];
        displayStats = UI.GetComponentsInChildren<Text>()[1];
        loadStats();
        UI.enabled = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
    }

    // Updates and enables UI 
    public void OnMouseOver()
    {
        updateData();
        UI.enabled = true;
    }

    // Disables UI
    public void OnMouseExit()
    {
        UI.enabled = false;
    }


    /**
    * Updates the health bar in the UI
    * 
    * @author Wayne Neo
    * @version 1.0
    * @updated on 25/6/17
    */
    private void updateData()
    {
        healthBar.value =  Mathf.Floor(((float) thisUnit.currentHP / (float) thisUnit.maxHP) * 100);
    }

    /**
    * Initial loading of parameters and name
    * Assuming name and parameters are not changed ingame
    * 
    * @author Wayne Neo
    * @version 1.0
    * @updated on 25/6/17
    */
    private void loadStats()
    {
        displayName.text = thisUnit.unitName;
        displayStats.text = "HP = " + thisUnit.currentHP.ToString() + "/" + thisUnit.maxHP.ToString() + " STR = " + thisUnit.strength.ToString() + " MAG = " + thisUnit.mag.ToString() + " SKL = " + thisUnit.skl.ToString() + "\n"
            + " SPD = " + thisUnit.spd.ToString() + " LUK = " + thisUnit.luk.ToString() + " DEF = " + thisUnit.def.ToString() + " RES = " + thisUnit.res.ToString();
    }
}

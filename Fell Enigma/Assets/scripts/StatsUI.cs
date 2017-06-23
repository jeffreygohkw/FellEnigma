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
        if (this.CompareTag("Player"))
        {
            thisUnit = this.GetComponent<PlayerUnit>();
        }
        else if (this.CompareTag("Enemy"))
        {
            thisUnit = this.GetComponent<AIUnit>();
        }

        UI = Instantiate(statsWindow, this.transform);
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

    public void OnMouseOver()
    {
        updateData();
        UI.enabled = true;
    }

    public void OnMouseExit()
    {
        UI.enabled = false;
    }

    private void updateData()
    {
        healthBar.value =  Mathf.Floor(((float) thisUnit.currentHP / (float) thisUnit.maxHP) * 100);
    }

    private void loadStats()
    {
        displayName.text = thisUnit.unitName;
        displayStats.text = "ATK = " + thisUnit.strength.ToString() + " MAG = " + thisUnit.mag.ToString() + " SPD = " + thisUnit.spd.ToString() + "\n"
            + "LUK = " + thisUnit.luk.ToString() + " DEF = " + thisUnit.def.ToString() + " RES = " + thisUnit.res.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour {

    // UI Elements
    private Slider healthBar;
    private Text displayName;
    private Text displayStats;

    private Unit currUnit;
    private bool isSelected;

	// Use this for initialization
	void Start () {
        EventManager.StartListening("GetStats", GetStats);

        // Obtains the various components under the UI prefab
        // Note that Text components need to be in order (aka don't change the order in Inspector)
        
        healthBar = this.GetComponentInChildren<Slider>();
        displayName = this.GetComponentsInChildren<Text>()[0];
        displayStats = this.GetComponentsInChildren<Text>()[1];
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void GetStats()
    {
        Collider[] colliders = Physics.OverlapSphere(new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y,0), 0.5f);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.CompareTag("Player"))
            {
                this.currUnit = colliders[i].gameObject.GetComponent<PlayerUnit>();
                break;
            }
            else if (colliders[i].gameObject.CompareTag("Enemy"))
            {
                this.currUnit = colliders[i].gameObject.GetComponent<AIUnit>();
                break;
            }
            else if (colliders[i].gameObject.CompareTag("Stationary"))
            {
                this.currUnit = colliders[i].gameObject.GetComponent<StationaryUnit>();
                break;
            }
        }

        displayName.text = currUnit.unitName;
        healthBar.value = Mathf.Floor(((float)currUnit.currentHP / (float)currUnit.maxHP) * 100);
        displayStats.text = "HP = " + currUnit.currentHP.ToString() + "/" + currUnit.maxHP.ToString() + " STR = " + currUnit.strength.ToString() + " MAG = " + currUnit.mag.ToString() + " SKL = " + currUnit.skl.ToString() + "\n"
            + " SPD = " + currUnit.spd.ToString() + " LUK = " + currUnit.luk.ToString() + " DEF = " + currUnit.def.ToString() + " RES = " + currUnit.res.ToString();
    }
}

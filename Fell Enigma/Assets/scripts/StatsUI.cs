using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour {

    // UI Elements
    private RawImage profile;
    private Slider healthBar;
    private Text displayName;
    private Text displayStats;

    private Unit currUnit;
    private Unit selectedUnit;
    private bool unitIsSelected = false;
    private bool isAttacking = false;

	// Use this for initialization
	void Start () {
        EventManager.StartListening("GetStats", GetStats);
        EventManager.StartListening("RemoveStats", RemoveStats);
        EventManager.StartListening("SelectUnitStats", SelectUnitStats);
       

        // Obtains the various components under the UI prefab
        // Note that Text components need to be in order (aka don't change the order in Inspector)
        profile = this.GetComponentInChildren<RawImage>();
        healthBar = this.GetComponentInChildren<Slider>();
        displayName = this.GetComponentsInChildren<Text>()[0];
        displayStats = this.GetComponentsInChildren<Text>()[1];

        OffUI();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void GetStats()
    {
        Collider[] colliders = Physics.OverlapSphere(new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0), 0.1f);
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

        if (!isAttacking)
        {
            displayName.text = currUnit.unitName;
            healthBar.value = Mathf.Floor(((float)currUnit.currentHP / (float)currUnit.maxHP) * 100);
            displayStats.text = "HP = " + currUnit.currentHP.ToString() + "/" + currUnit.maxHP.ToString() + " STR = " + currUnit.strength.ToString() + " MAG = " + currUnit.mag.ToString() + " SKL = " + currUnit.skl.ToString() + "\n"
                + " SPD = " + currUnit.spd.ToString() + " LUK = " + currUnit.luk.ToString() + " DEF = " + currUnit.def.ToString() + " RES = " + currUnit.res.ToString();
            OnUI();
        }
        else
        {
        }
    }

    void RemoveStats()
    {
        if (!unitIsSelected)
        {
            OffUI();
        }
        else
        {
            displayName.text = selectedUnit.unitName;
            healthBar.value = Mathf.Floor(((float)selectedUnit.currentHP / (float)selectedUnit.maxHP) * 100);
            displayStats.text = "HP = " + selectedUnit.currentHP.ToString() + "/" + selectedUnit.maxHP.ToString() + " STR = " + selectedUnit.strength.ToString() + " MAG = " + selectedUnit.mag.ToString() + " SKL = " + selectedUnit.skl.ToString() + "\n"
                + " SPD = " + selectedUnit.spd.ToString() + " LUK = " + selectedUnit.luk.ToString() + " DEF = " + selectedUnit.def.ToString() + " RES = " + selectedUnit.res.ToString();
        }
    }

    void SelectUnitStats()
    {
        selectedUnit = currUnit;
        unitIsSelected = true;
        EventManager.StartListening("DeselectUnitStats", DeselectUnitStats);
        EventManager.StopListening("SelectUnitStats", SelectUnitStats);
    }

    void DeselectUnitStats()
    {
        unitIsSelected = false;
        EventManager.StopListening("DeselectUnitStats", DeselectUnitStats);
        EventManager.StartListening("SelectUnitStats", SelectUnitStats);
        OffUI();
    }

    void unitAttack()
    {
        isAttacking = true;
    }

    private void OffUI()
    {
        this.GetComponent<CanvasGroup>().alpha = 0f;
    }

    private void OnUI()
    {
        this.GetComponent<CanvasGroup>().alpha = 1f;
    }
}

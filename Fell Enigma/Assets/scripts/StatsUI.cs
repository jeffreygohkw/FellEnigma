using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour {

    // UI Elements
    private CanvasGroup canvasA;
    private Slider healthBar;
    private Text displayName;
    private Text displayStats;

    // Battle Forecast
    private CanvasGroup canvasB;
    private RawImage enemyProfile;
    private Image attIcon;
    private Image hrIcon;
    private Image attIconB;
    private Image hrIconB;

    // Profile Pic
    private CanvasGroup canvasC;
    private RawImage profile;

    private Text meAttUI;
    private Text enemyAttUI;

    private Unit currUnit;
    private Unit selectedUnit;
    private bool unitIsSelected = false;
    private bool unitIsAttacking = false;
    private bool unitIsHealing = false;


    // Use this for initialization
    void Start() {
        EventManager.StartListening("GetStats", GetStats);
        EventManager.StartListening("RemoveStats", RemoveStats);
        EventManager.StartListening("SelectUnitStats", SelectUnitStats);
        EventManager.StartListening("AttackUnitStats", AttackUnitStats);

        // Obtains the various components under the UI prefab
        // Note that components need to be in order (aka don't change the order in Inspector)
        canvasA = this.GetComponentsInChildren<CanvasGroup>()[0];
        displayName = this.GetComponentsInChildren<Text>()[0];
        displayStats = this.GetComponentsInChildren<Text>()[1];
        healthBar = this.GetComponentInChildren<Slider>();

        canvasB = this.GetComponentsInChildren<CanvasGroup>()[1];
        enemyProfile = this.GetComponentsInChildren<RawImage>()[1];
        /*attIcon = this.GetComponentsInChildren<Image>()[0];
        attIconB = this.GetComponentsInChildren<Image>()[1];
        hrIcon = this.GetComponentsInChildren<Image>()[2];
        hrIconB = this.GetComponentsInChildren<Image>()[3];*/
        meAttUI = this.GetComponentsInChildren<Text>()[2];
        enemyAttUI = this.GetComponentsInChildren<Text>()[3];

        canvasC = this.GetComponentsInChildren<CanvasGroup>()[2];
        profile = this.GetComponentsInChildren<RawImage>()[0];


        OffUI();
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    /**
    * Finds the nearest Unit the mouse is hovering at and updates the UI correspondingly. Called when mouseover.
    * 
    * v1.1
    * Reduced radius of OverlapSphere
    * 
    * v1.2
    * Added Battle Forecast. Note a minor bug, where the battleforecast will run once before the unit deselects itself.
    * 
    * 
    * @author Wayne Neo
    * @version 1.2
    * @updated on 10/7/17
    */
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

        if (unitIsAttacking) //Battle forecast
        {
            // Copied from BattleFormula
            int dmgtoMe = Grid.instance.battle.battleForecast(currUnit, selectedUnit);
            int dmgtoEnemy = Grid.instance.battle.battleForecast(selectedUnit, currUnit);
            int attackerAcc = selectedUnit.weaponAcc + selectedUnit.skl * 2 + selectedUnit.luk / 2;
            int attackerAvd = selectedUnit.spd * 2 + selectedUnit.luk + Grid.instance.map[(int)Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].gridPosition.x][(int)Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].gridPosition.y].linkedTerrain.returnAvd();
            int defenderAcc = currUnit.weaponAcc + currUnit.skl * 2 + currUnit.luk / 2;
            int defenderAvd = currUnit.spd * 2 + currUnit.luk + Grid.instance.map[(int)currUnit.gridPosition.x][(int)currUnit.gridPosition.y].linkedTerrain.returnAvd();

            int myHr = attackerAcc - defenderAvd;
            int enemyHr = defenderAcc - attackerAvd;

            meAttUI.text = (dmgtoEnemy.ToString() + "\n" + myHr.ToString());
            enemyAttUI.text = (dmgtoMe.ToString() + "\n" + myHr.ToString());
            OnForecast();
        }
        else //Normal hover
        {
            displayName.text = currUnit.unitName;
            healthBar.value = Mathf.Floor(((float)currUnit.currentHP / (float)currUnit.maxHP) * 100);
            displayStats.text = "HP = " + currUnit.currentHP.ToString() + "/" + currUnit.maxHP.ToString() + " STR = " + currUnit.strength.ToString() + " MAG = " + currUnit.mag.ToString() + " SKL = " + currUnit.skl.ToString() + "\n"
                + " SPD = " + currUnit.spd.ToString() + " LUK = " + currUnit.luk.ToString() + " DEF = " + currUnit.def.ToString() + " RES = " + currUnit.res.ToString();
            OnStats();
        }

    }
    /**
     * Called when the mouse no longer hovers over a unit
     * 
     * v1.1
     * Now stays when a unit is selected
     * 
     * @author Wayne Neo
     * @version 1.1
     * @updated on 9/7/17
     */
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
            OnStats();
        }
        
    }

    /**
    * Sets the most recent Unit hovered to be the 'selected' unit to remember, switches the listening as well
    * 
    * @author Wayne Neo
    * @version 1.0
    * @updated on 9/7/17
    */
    void SelectUnitStats()
    {
        selectedUnit = currUnit;
        unitIsSelected = true;
        EventManager.StartListening("DeselectUnitStats", DeselectUnitStats);
        EventManager.StopListening("SelectUnitStats", SelectUnitStats);
    }

    /**
    * Deactivates UI, switches the listening as well
    * 
    * v1.1
    * Added attacking and healing resets
    * 
    * @author Wayne Neo
    * @version 1.1
    * @updated on 10/7/17
    */
    void DeselectUnitStats()
    {
        unitIsSelected = false;
        unitIsAttacking = false;
        unitIsHealing = false;
        EventManager.StopListening("DeselectUnitStats", DeselectUnitStats);
        EventManager.StartListening("SelectUnitStats", SelectUnitStats);
        OffUI();
    }

    /**
    * Updates bool for GetStats
    * 
    * @author Wayne Neo
    * @version 1.0
    * @updated on 10/7/17
    */
    void AttackUnitStats()
    {
        unitIsAttacking = !unitIsAttacking;
    }

    /**
    * Deactivates UI
    * 
    * 
    * @author Wayne Neo
    * @version 1.0
    * @updated on 9/7/17
    */
    private void OffUI()
    {
        canvasA.alpha = 0f;
        canvasB.alpha = 0f;
        canvasC.alpha = 0f;
    }

    /**
    * Activates Stats UI
    * 
    * @author Wayne Neo
    * @version 1.0
    * @updated on 9/7/17
    */
    private void OnStats()
    {
        canvasA.alpha = 1f;
        canvasB.alpha = 0f;
        canvasC.alpha = 1f;
    }
    /**
    * Activates Forecast UI
    * 
    * @author Wayne Neo
    * @version 1.0
    * @updated on 10/7/17
    */
    private void OnForecast()
    {
        canvasA.alpha = 0f;
        canvasB.alpha = 1f;
        canvasC.alpha = 1f;
    }

}

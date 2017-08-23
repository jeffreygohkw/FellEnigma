using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour {

    // UI Elements
    private CanvasGroup canvasA;
    private Slider healthBar;
    private Slider expBar;
    private Text displayName;
    private Text displayStats;

    // Battle Forecast
    private CanvasGroup canvasB;
    private RawImage enemyProfile;

    private Text myForecastUI;
    private Text enemyForecastUI;
    private Text myForecastStatsUI;
    private Text enemyForecastStatsUI;

    // Profile Pic
    private CanvasGroup canvasC;
    private RawImage displayProfile;
    public Texture[] profiles = new Texture[8];
    private Rect defaultRect;
   

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
        EventManager.StartListening("AttackUnitStatsON", AttackUnitStatsON);
        EventManager.StartListening("AttackUnitStatsOFF", AttackUnitStatsOFF);
        EventManager.StartListening("HealUnitStatsON", HealUnitStatsON);
        EventManager.StartListening("HealUnitStatsOFF", HealUnitStatsOFF);

        // Obtains the various components under the UI prefab
        // Note that components need to be in order (aka don't change the order in Inspector)

        // Canvas A: Healthbar,stats,name, exp Bar
        canvasA = this.GetComponentsInChildren<CanvasGroup>()[0];
        displayName = this.GetComponentsInChildren<Text>()[0];
        displayStats = this.GetComponentsInChildren<Text>()[1];
        healthBar = this.GetComponentsInChildren<Slider>()[0];
        expBar = this.GetComponentsInChildren<Slider>()[1];

        // Canvas B: Battle/Heal Forecast
        canvasB = this.GetComponentsInChildren<CanvasGroup>()[1];
        enemyProfile = this.GetComponentsInChildren<RawImage>()[0];
        /*attIcon = this.GetComponentsInChildren<Image>()[0];
        attIconB = this.GetComponentsInChildren<Image>()[1];
        hrIcon = this.GetComponentsInChildren<Image>()[2];
        hrIconB = this.GetComponentsInChildren<Image>()[3];*/
        myForecastUI = this.GetComponentsInChildren<Text>()[2];
        enemyForecastUI = this.GetComponentsInChildren<Text>()[3];
        myForecastStatsUI = this.GetComponentsInChildren<Text>()[4];
        enemyForecastStatsUI = this.GetComponentsInChildren<Text>()[5];

        // Canvas C: My profile pic
        canvasC = this.GetComponentsInChildren<CanvasGroup>()[2];
        displayProfile = this.GetComponentsInChildren<RawImage>()[1];

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
    * v1.3
    * Added Heal Forecast and Profile Functionality Expanded Battle Forecast.
    * 
    * v1.4
    * Added the case where enemy cannot counterattack
    * 
    * v1.5
    * Added EXP Bar
    * 
    * @author Wayne Neo
    * @version 1.5
    * @updated on 12/8/17
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
            if (currUnit != selectedUnit && !(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].allies.Contains(currUnit.team)))
            {
                // Copied from BattleFormula
                int dmgtoMe = Grid.instance.battle.battleForecast(currUnit, selectedUnit);
                int dmgtoEnemy = Grid.instance.battle.battleForecast(selectedUnit, currUnit);
                int attackerAcc = selectedUnit.weaponAcc + selectedUnit.skl * 2 + selectedUnit.luk / 2;
                int attackerAvd = selectedUnit.spd * 2 + selectedUnit.luk + Grid.instance.map[(int)Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].gridPosition.x][(int)Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].gridPosition.y].linkedTerrain.returnAvd();
                int defenderAcc = currUnit.weaponAcc + currUnit.skl * 2 + currUnit.luk / 2;
                int defenderAvd = currUnit.spd * 2 + currUnit.luk + Grid.instance.map[(int)currUnit.gridPosition.x][(int)currUnit.gridPosition.y].linkedTerrain.returnAvd();

                int myHr = Mathf.Min(attackerAcc - defenderAvd,100);
                int enemyHr = Mathf.Min(defenderAcc - attackerAvd,100);

                int attackerCritRate = Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].weaponCrit + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].skl / 2;
                int attackerCritAvd = Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].luk;
                int defenderCritRate = currUnit.weaponCrit + currUnit.skl / 2;
                int defenderCritAvd = currUnit.luk;
                int myCrit = Mathf.Max(attackerCritRate - defenderCritAvd, 0);
                int enemyCrit = Mathf.Max(defenderCritRate - attackerCritAvd, 0);

                int dist = Mathf.Abs((int)Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].gridPosition.x - (int)currUnit.gridPosition.x) + Mathf.Abs((int)Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].gridPosition.y - (int)currUnit.gridPosition.y);

                // If enemy cannot counter
                if (currUnit.weaponMinRange > dist || currUnit.weaponMaxRange + currUnit.weaponRangeBuff < dist || Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].rebelBuff)
                {
                    myForecastUI.text = "No change";
                }
                else
                {
                    myForecastUI.text = (selectedUnit.currentHP).ToString() + " => " + Mathf.Max((selectedUnit.currentHP - dmgtoMe), 0).ToString();
                }
                enemyForecastUI.text = (currUnit.currentHP).ToString() + " => " + Mathf.Max((currUnit.currentHP - dmgtoEnemy), 0).ToString();

                // Seperated due to spacing in UI
                if (myHr == 100)
                {
                    if (selectedUnit.spd - currUnit.spd >= 4)
                    {
                        myForecastStatsUI.text = selectedUnit.inventory[selectedUnit.equippedIndex][1] + " (x2)" + "\n" + myHr.ToString() + "%\t\t" + myCrit.ToString() + "%";
                    }
                    else
                    {
                        myForecastStatsUI.text = selectedUnit.inventory[selectedUnit.equippedIndex][1] + "\n" + myHr.ToString() + "%\t\t" + myCrit.ToString() + "%";
                    }
                }
                else
                {
                    if (selectedUnit.spd - currUnit.spd >= 4)
                    {
                        myForecastStatsUI.text = "\t" + selectedUnit.inventory[selectedUnit.equippedIndex][1] + " (x2)" + "\n" + myHr.ToString() + "%\t\t\t" + myCrit.ToString() + "%";
                    }
                    else
                    {
                        myForecastStatsUI.text = "\t" + selectedUnit.inventory[selectedUnit.equippedIndex][1] + "\n" + myHr.ToString() + "%\t\t\t" + myCrit.ToString() + "%";
                    }
                }

                if (currUnit.weaponMinRange > dist || currUnit.weaponMaxRange + currUnit.weaponRangeBuff < dist || Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].rebelBuff)
                    {
                    enemyForecastStatsUI.text = currUnit.inventory[currUnit.equippedIndex][1] + "\n" + "-" + "\t\t -";
                    }
                else if (enemyHr == 100)
                {
                    if (currUnit.spd - selectedUnit.spd >= 4)
                    {
                        enemyForecastStatsUI.text = currUnit.inventory[currUnit.equippedIndex][1] + " (x2)" + "\n" + enemyCrit.ToString() + "%\t\t" + enemyHr.ToString() + "%";
                    }
                    else
                    {
                        enemyForecastStatsUI.text = currUnit.inventory[currUnit.equippedIndex][1] + "\n" + enemyCrit.ToString() + "%\t\t" + enemyHr.ToString() + "%";
                    }
                }
                else
                {
                    if (currUnit.spd - selectedUnit.spd >= 4)
                    {
                        enemyForecastStatsUI.text = "\t" + currUnit.inventory[currUnit.equippedIndex][1] + " (x2)" + "\n" + enemyCrit.ToString() + "%\t\t\t" + enemyHr.ToString() + "%";
                    }
                    else
                    {
                        enemyForecastStatsUI.text = "\t" + currUnit.inventory[currUnit.equippedIndex][1] + "\n" + enemyCrit.ToString() + "%\t\t\t" + enemyHr.ToString() + "%";
                    }
                }

                // Profile picture loading
                if (currUnit.unitName.Equals("Naive Prince"))
                {
                    enemyProfile.texture = profiles[0];
                    enemyProfile.uvRect = new Rect(0.68f, 0.38f, 0.3f, 0.55f);
                }
                else if (currUnit.unitName.Equals("Kind Soul"))
                {
                    enemyProfile.texture = profiles[1];
                    enemyProfile.uvRect = new Rect(0.55f, 0.3f, 0.4f, 0.55f);
                }
                else if (currUnit.unitName.Equals("Young Rebel"))
                {
                    enemyProfile.texture = profiles[2];
                    enemyProfile.uvRect = new Rect(0.67f, 0.38f, 0.28f, 0.55f);
                }
                else if (currUnit.unitName.Equals("Black Heart"))
                {
                    enemyProfile.texture = profiles[3];
                    enemyProfile.uvRect = new Rect(0.68f, 0.38f, 0.3f, 0.55f);
                }
                else if (currUnit.unitName.Equals("Bandit") || currUnit.unitName.Equals("Bandit Leader") || currUnit.unitName.Equals("Rebel") || currUnit.unitName.Equals("Rebel Leader"))
                {
                    enemyProfile.texture = profiles[5];
                    enemyProfile.uvRect = new Rect(0, 0.2f, 1, 0.8f);
                }
				else if (currUnit.unitName.Equals("Soldier") || currUnit.unitName.Equals("Commander") || currUnit.unitName.Equals("Mercenary"))
				{
					enemyProfile.texture = profiles[6];
					enemyProfile.uvRect = new Rect(0, 0.2f, 1, 0.8f);
				}

				OnForecast();
            }
        }
        else if (unitIsHealing) //Heal forecast
        {
            if (currUnit != selectedUnit && (Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].allies.Contains(currUnit.team)))
            {
                int amtHealed = int.Parse(Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].inventory[Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].activeStaffIndex][8]) + Grid.instance.units[Grid.instance.currentTeam][Grid.instance.currentPlayer].mag;
                myForecastUI.text = "Healing";
                myForecastStatsUI.text = " ";
                enemyForecastUI.text = currUnit.currentHP + " => " + Mathf.Min(currUnit.currentHP + amtHealed, currUnit.maxHP).ToString();
                enemyForecastStatsUI.text = " ";

                if (currUnit.unitName.Equals("Naive Prince"))
                {
                    enemyProfile.texture = profiles[0];
                    enemyProfile.uvRect = new Rect(0.68f, 0.38f, 0.3f, 0.55f);
                }
                else if (currUnit.unitName.Equals("Kind Soul"))
                {
                    enemyProfile.texture = profiles[1];
                    enemyProfile.uvRect = new Rect(0.55f, 0.3f, 0.4f, 0.55f);
                }
                else if (currUnit.unitName.Equals("Young Rebel"))
                {
                    enemyProfile.texture = profiles[2];
                    enemyProfile.uvRect = new Rect(0.67f, 0.38f, 0.28f, 0.55f);
                }
                else if (currUnit.unitName.Equals("Black Heart"))
                {
                    enemyProfile.texture = profiles[3];
                    enemyProfile.uvRect = new Rect(0.68f, 0.38f, 0.3f, 0.55f);
                }
                else if (currUnit.unitName.Equals("Mercenary"))
                {
                    enemyProfile.texture = profiles[6];
                    enemyProfile.uvRect = new Rect(0, 0.2f, 1, 0.8f);
                }


                OnForecast();
            }
        }
        else //Normal hover
        {
            if (currUnit.unitName.Equals("Naive Prince"))
            {
                displayProfile.texture = profiles[0];
                displayProfile.uvRect = new Rect(0.68f, 0.38f, 0.3f, 0.55f);
            }
            else if (currUnit.unitName.Equals("Kind Soul"))
            {
                displayProfile.texture = profiles[1];
                displayProfile.uvRect = new Rect(0.55f, 0.3f, 0.4f, 0.55f);
            }
            else if (currUnit.unitName.Equals("Young Rebel"))
            {
                displayProfile.texture = profiles[2];
                displayProfile.uvRect = new Rect(0.67f, 0.38f, 0.28f, 0.55f);
            }
            else if (currUnit.unitName.Equals("Black Heart"))
            {
                displayProfile.texture = profiles[3];
                displayProfile.uvRect = new Rect(0.68f, 0.38f, 0.3f, 0.55f);
            }
			else if (currUnit.unitName.Equals("Bandit") || currUnit.unitName.Equals("Bandit Leader") || currUnit.unitName.Equals("Rebel") || currUnit.unitName.Equals("Rebel Leader"))
			{
				displayProfile.texture = profiles[5];
				displayProfile.uvRect = new Rect(0, 0.2f, 1, 0.8f);
			}
			else if (currUnit.unitName.Equals("Soldier") || currUnit.unitName.Equals("Commander") || currUnit.unitName.Equals("Mercenary"))
			{
				displayProfile.texture = profiles[6];
				displayProfile.uvRect = new Rect(0, 0.2f, 1, 0.8f);
			}

			displayName.text = currUnit.unitName;
            healthBar.value = Mathf.Floor(((float)currUnit.currentHP / (float)currUnit.maxHP) * 100);
            expBar.value = currUnit.exp;
            displayStats.text = "HP = " + currUnit.currentHP.ToString() + "/" + currUnit.maxHP.ToString() + " STR = " + currUnit.strength.ToString() + " MAG = " + currUnit.mag.ToString() + " SKL = " + currUnit.skl.ToString() + "\n"
                + " SPD = " + currUnit.spd.ToString() + " LUK = " + currUnit.luk.ToString() + " DEF = " + currUnit.def.ToString() + " RES = " + currUnit.res.ToString();
            OnStats();
        }

    }
    /**
     * EventManager: Called when the mouse no longer hovers over a unit
     * 
     * v1.1
     * Now stays when a unit is selected
     * 
     * v1.2
     * Updated for profile change
     * 
     * @author Wayne Neo
     * @version 1.2
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
            if (selectedUnit.unitName.Equals("Naive Prince"))
            {
                displayProfile.texture = profiles[0];
                displayProfile.uvRect = new Rect(0.68f, 0.38f, 0.3f, 0.55f);
            }
            else if (selectedUnit.unitName.Equals("Kind Soul"))
            {
                displayProfile.texture = profiles[1];
                displayProfile.uvRect = new Rect(0.55f, 0.3f, 0.4f, 0.55f);
            }
            else if (selectedUnit.unitName.Equals("Young Rebel"))
            {
                displayProfile.texture = profiles[2];
                displayProfile.uvRect = new Rect(0.67f, 0.38f, 0.28f, 0.55f);
            }
            else if (selectedUnit.unitName.Equals("Black Heart"))
            {
                displayProfile.texture = profiles[3];
                displayProfile.uvRect = new Rect(0.68f, 0.38f, 0.3f, 0.55f);
            }


            displayName.text = selectedUnit.unitName;
            healthBar.value = Mathf.Floor(((float)selectedUnit.currentHP / (float)selectedUnit.maxHP) * 100);
            displayStats.text = "HP = " + selectedUnit.currentHP.ToString() + "/" + selectedUnit.maxHP.ToString() + " STR = " + selectedUnit.strength.ToString() + " MAG = " + selectedUnit.mag.ToString() + " SKL = " + selectedUnit.skl.ToString() + "\n"
                + " SPD = " + selectedUnit.spd.ToString() + " LUK = " + selectedUnit.luk.ToString() + " DEF = " + selectedUnit.def.ToString() + " RES = " + selectedUnit.res.ToString();
            OnStats();
        }
        
    }

    /**
    * EventManager: Sets the most recent Unit hovered to be the 'selected' unit to remember, switches the listening as well
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
    * EventManager: Deactivates UI, switches the listening as well
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
    * EventManager: Updates bool for GetStats
    * 
    * @author Wayne Neo
    * @version 1.0
    * @updated on 10/7/17
    */
    void AttackUnitStatsON()
    {
        unitIsAttacking = true;
    }

    /**
    * EventManager: Updates bool for GetStats
    * 
    * @author Wayne Neo
    * @version 1.0
    * @updated on 10/7/17
    */
    void AttackUnitStatsOFF()
    {
        unitIsAttacking = false;
    }

    /**
    * EventManager: Updates bool for GetStats
    * 
    * @author Wayne Neo
    * @version 1.0
    * @updated on 19/7/17
    */
    void HealUnitStatsON()
    {
        unitIsHealing = true;
    }

    /**
    * EventManager: Updates bool for GetStats
    * 
    * @author Wayne Neo
    * @version 1.0
    * @updated on 19/7/17
    */
    void HealUnitStatsOFF()
    {
        unitIsHealing = false;
    }

    /**
    * Deactivates All UI
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
    * Activates Forecast UI for attacking
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

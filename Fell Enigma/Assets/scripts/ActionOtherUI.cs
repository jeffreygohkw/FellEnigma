using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionOtherUI : MonoBehaviour {
    public static ActionOtherUI instance;
    
    private CanvasGroup firstWindow;
    private CanvasGroup talkButton;
    private CanvasGroup objButton;
    private CanvasGroup tavWindow;

    private Text objText;

    public bool canTalk = false;
    public bool canCap = false;
    public bool canTav = false;
    public bool canOther = false;

    private Unit thisUnit;
	// Use this for initialization
	void Start () {

        // Obtains the various components under the UI prefab
        // Note that components need to be in order (aka don't change the order in Inspector)

        firstWindow = this.GetComponentsInChildren<CanvasGroup>()[0];
        talkButton = this.GetComponentsInChildren<CanvasGroup>()[1];
        objButton = this.GetComponentsInChildren<CanvasGroup>()[2];
        tavWindow = this.GetComponentsInChildren<CanvasGroup>()[3];

        objText = this.GetComponentsInChildren<Text>()[1];
    }

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update () {
		
	}

    /**
     * Toggles the UI when the Actions button is selected 
     * 
     * 
     * @author Wayne Neo
     * @version 1.0
     * @updated on 19/7/17
     */
    public void ToggleUI()
    {
        if (firstWindow.alpha == 0)
        {
            firstWindow.alpha = 1f;
            firstWindow.interactable = true;
            firstWindow.blocksRaycasts = true;


            if (canTalk)
            {
                talkButton.alpha = 1f;
                talkButton.interactable = true;
                talkButton.blocksRaycasts = true;
            }
            if (canCap)
            {
                objText.text = "Capture";
                objButton.alpha = 1f;
                objButton.interactable = true;
                objButton.blocksRaycasts = true;

            }
            else if (canTav)
            {
                objText.text = "Tavern";
                objButton.alpha = 1f;
                objButton.interactable = true;
                objButton.blocksRaycasts = true;
            }
            else if (canOther)
            {
                Debug.LogError("This should not appear");
            }
        }
        else
        {
            canTalk = canCap = canTav = canOther = false;
            firstWindow.alpha = 0;
            firstWindow.interactable = false;
            firstWindow.blocksRaycasts = false;
            talkButton.alpha = 0;
            talkButton.interactable = false;
            talkButton.blocksRaycasts = false;
            objButton.alpha = 0;
            objButton.interactable = false;
            objButton.blocksRaycasts = false;
        }

    }

    /**
     * Overrides the above function when a string is required, aka for special objectives tile
     * 
     * @param text Name of the special objective
     * @author Wayne Neo
     * @version1.0
     * @updated on 19/7/17
     */
    public void ToggleUI(string text)
    {
        if (firstWindow.alpha == 0)
        {
            firstWindow.alpha = 1f;
            firstWindow.interactable = true;
            firstWindow.blocksRaycasts = true;
            objText.text = text;
            objButton.alpha = 1f;
            objButton.interactable = true;
            objButton.blocksRaycasts = true;
        }
        else
        {
            canTalk = canCap = canTav = canOther = false;
            firstWindow.alpha = 0;
            firstWindow.interactable = false;
            firstWindow.blocksRaycasts = false;
            talkButton.alpha = 0;
            talkButton.interactable = false;
            talkButton.blocksRaycasts = false;
            objButton.alpha = 0;
            objButton.interactable = false;
            objButton.blocksRaycasts = false;
        }
    }

    /**
    *Toggles the Tavern UI when the Tavern button is pressed
    * 
    * @param unit To know what tavern tile is the unit standing on
    * @author Wayne Neo
    * @version 1.0
    * @updated on 19/7/17
    */
    public void ToggleTavUI(Unit unit)
    {
        thisUnit = unit;
        if (tavWindow.alpha == 0)
        {
            tavWindow.alpha = 1;
            tavWindow.interactable = true;
            tavWindow.blocksRaycasts = true;
        }
        else
        {
            tavWindow.alpha = 0;
            tavWindow.interactable = false;
            tavWindow.blocksRaycasts = false;
        }
        
    }

    /**
     * Deactivates all UI
     * 
     * @author Wayne Neo
     * @version 1.0
     * @updated on 19/7/17
     */
    public void OffAllUI()
    {
        canTalk = canCap = canTav = canOther = false;
        firstWindow.alpha = 0;
        firstWindow.interactable = false;
        firstWindow.blocksRaycasts = false;
        talkButton.alpha = 0;
        talkButton.interactable = false;
        talkButton.blocksRaycasts = false;
        objButton.alpha = 0;
        objButton.interactable = false;
        objButton.blocksRaycasts = false;
        tavWindow.alpha = 0;
        tavWindow.interactable = false;
        tavWindow.blocksRaycasts = false;
    }

    private void OnGUI()
    {
        // If tavern is open
        if (tavWindow.alpha == 1)
        {
            int count = 1;
            foreach (string s in TavernUnits.tavernUnits.Keys)
            {
                count++;
                Rect buttonRect = new Rect(Screen.width/1.28f, Screen.height/1.4f - ((Screen.height / 15) * count), Screen.width/6, Screen.height/15);
                if (GUI.Button(buttonRect, s))
                {
                    TavernUnits.tavernSpawn(s, thisUnit.gridPosition);
                }
            }
        }
    }

}

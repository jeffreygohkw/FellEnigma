using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour {

    public static ItemUI instance;
    private int selectedItemIndex;

    private CanvasGroup[] buttons = new CanvasGroup[3];
    private Text[] buttonsText = new Text[3]; 
    private CanvasGroup buttons2;
    private Text buttons2Text;
    private CanvasGroup window;

    private Unit selectedUnit;
    private bool itemIsSelected = false;

    // Use this for initialization
    void Start () {
        EventManager.StartListening("ItemUION", ItemUION);
        EventManager.StartListening("ItemUIOFF", ItemUIOFF);

        // Obtains the various components under the UI prefab
        // Note that components need to be in order (aka don't change the order in Inspector)
        window = this.GetComponentsInChildren<CanvasGroup>()[0];

        for (int i = 0; i <= 2; i++)
        {
            buttons[i] = this.GetComponentsInChildren<CanvasGroup>()[i + 1];
            buttonsText[i] = this.GetComponentsInChildren<Text>()[i];
        }

        
        buttons2 = this.GetComponentsInChildren<CanvasGroup>()[4];        
        buttons2Text = this.GetComponentsInChildren<Text>()[3];
        selectedItemIndex = -1;
    }

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update () {
        
        // If no item is selected
        if (selectedItemIndex == -1)
        {
            buttons2.alpha = 0;
            buttons2.interactable = false;
            buttons2.blocksRaycasts = false;
            itemIsSelected = false;
        }

        if (itemIsSelected)
        {
            // Item selected is a weapon
            if (selectedUnit.inventory[selectedItemIndex].Length == 13)
            {
                buttons2Text.text = "Equip";
            }
            else
            {
                buttons2Text.text = "Use";
            }

            // Activates second window
            buttons2.alpha = 1;
            buttons2.interactable = true;
            buttons2.blocksRaycasts = true;
        }

    }
    /**
    * EventManager: Activates the first ItemUI window
    * 
    * v1.1
    * Resets selectedItemIndex as well.
    * 
    * @author Wayne Neo
    * @version 1.1
    * @updated on 19/7/17
    */
    void ItemUION()
    {
        for (int i = 0; i < selectedUnit.inventory.Count; i++) {

            buttonsText[i].text = selectedUnit.inventory[i][1];
            buttons[i].alpha = 1;
            buttons[i].interactable = true;
            buttons[i].blocksRaycasts = true;
        }

        window.alpha = 1;
        window.interactable = true;
        window.interactable = true;
        selectedItemIndex = -1;

    }

    /**
     * EventManager: Deactivates all ItemUI 
     *
     * @author Wayne Neo
     * @version 1.0
     * @updated on 19/7/17
     */
    void ItemUIOFF()
    {
        for (int i = 0; i < 3; i++)
        {
            buttons[i].alpha = 0;
            buttons[i].interactable = false;
            buttons[i].blocksRaycasts = false;
        }

        window.alpha = 0;
        window.interactable = false;
        window.interactable = false;
        selectedItemIndex = -1;

    }

    // Sets selected item index whose function in playerUnit checks
    public void setItemIndex(int i)
    {
        selectedItemIndex = i;
    }

    // Returns selected item index whose function in playerUnit checks
    public int getItemIndex()
    {
        return selectedItemIndex;
    }

    // Sets selected unit whose function in playerUnit checks
    public void setUnit(Unit dude)
    {
        selectedUnit = dude;
    }

    // Returns accordingly if item is selected, whose function in playerUnit checks
    public void setItemSelected(bool value)
    {
        itemIsSelected = value;
    }
}

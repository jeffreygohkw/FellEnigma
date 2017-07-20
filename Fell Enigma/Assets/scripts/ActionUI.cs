using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ActionUI : MonoBehaviour
{
    private CanvasGroup thisCanvas;
    private Text moveText;
    private Image ultButton;

    // Use this for initialization
    void Start()
    {
        EventManager.StartListening("SelectUnit", SelectUnit);
        EventManager.StartListening("MovedUnit", MovedUnit);
        
        // Obtains the UI elements
        thisCanvas = this.GetComponent<CanvasGroup>();
        moveText = GetComponentsInChildren<Text>()[0];
        ultButton = this.GetComponentInChildren<Image>();
        OffUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (Grid.instance.ultCharge == 100)
        {
            ultButton.color = Color.green;
        }
        else
        {
            ultButton.color = Color.white;
        }
    }

    /**
    * Activates UI, switches the listening for selection
    * 
    * 
    * @author Wayne Neo
    * @version 1.0
    * @updated on 9/7/17
    */
    void SelectUnit()
    {
        OnUI();

        EventManager.TriggerEvent("SelectUnitStats");

        EventManager.StopListening("SelectUnit", SelectUnit);
        EventManager.StartListening("MovedUnit", MovedUnit);
        EventManager.StartListening("DeselectUnit", DeselectUnit);
    }

    /**
    * Deactivates UI, switches the listening for selection and resets the Move button
    * 
    * 
    * @author Wayne Neo
    * @version 1.0
    * @updated on 9/7/17
    */
    void DeselectUnit()
    {
        OffUI();
        EventManager.TriggerEvent("DeselectUnitStats");
        EventManager.TriggerEvent("AttackUnitStatsOFF");
        EventManager.TriggerEvent("ItemUIOFF");
        ActionOtherUI.instance.OffAllUI();

        moveText.GetComponent<Text>().text = "Move";
        this.GetComponentInChildren<Actions>().hasMoved = false;
        EventManager.StartListening("SelectUnit", SelectUnit);
        EventManager.StopListening("DeselectUnit", DeselectUnit);
    }

    /**
    * Changes Move button, switches the listening for MovedUnit
    * 
    * 
    * @author Wayne Neo
    * @version 1.0
    * @updated on 9/7/17
    */
    void MovedUnit()
    {
        moveText.GetComponent<Text>().text = "Undo Move";
        this.GetComponentInChildren<Actions>().hasMoved = true;
        EventManager.StartListening("UndoMovedUnit", UndoMovedUnit);
        EventManager.StopListening("MovedUnit", MovedUnit);
    }

    /**
    * Changes Undo Move button, switches the listening for MovedUnit
    * 
    * 
    * @author Wayne Neo
    * @version 1.0
    * @updated on 9/7/17
    */
    void UndoMovedUnit()
    {
        moveText.GetComponent<Text>().text = "Move";
        this.GetComponentInChildren<Actions>().hasMoved = false;
        EventManager.StopListening("UndoMovedUnit", UndoMovedUnit);
        EventManager.StartListening("MovedUnit", MovedUnit);
    }



    /**
    * Causes the UI to appear and be selectable
    * 
    * 
    * @author Wayne Neo
    * @version 1.0
    * @updated on 9/7/17
    */
    private void OnUI()
    {
        thisCanvas.alpha = 1f;
        thisCanvas.interactable = true;
        thisCanvas.blocksRaycasts = true;
    }

    /**
    * Causes the UI to disappear and be unselectable
    * 
    * 
    * @author Wayne Neo
    * @version 1.0
    * @updated on 9/7/17
    */
    private void OffUI()
    {
        thisCanvas.alpha = 0f;
        thisCanvas.interactable = false;
        thisCanvas.blocksRaycasts = false;
    }
}
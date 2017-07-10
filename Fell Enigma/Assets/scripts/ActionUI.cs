using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ActionUI : MonoBehaviour
{
    private CanvasGroup thisCanvas;
    private Text moveText;

    private bool hasMoved;


    // Use this for initialization
    void Start()
    {
        EventManager.StartListening("SelectUnit", SelectUnit);
        EventManager.StartListening("MovedUnit", MovedUnit);
        
        // Obtains the UI elements
        thisCanvas = this.GetComponent<CanvasGroup>();
        moveText = GetComponentsInChildren<Text>()[0];
        OffUI();
    }

    // Update is called once per frame
    void Update()
    {

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
        EventManager.StartListening("SelectUnit", SelectUnit);
        EventManager.StopListening("DeselectUnit", DeselectUnit);
        hasMoved = false;
        moveText.GetComponent<Text>().text = "Move";
        this.GetComponentInChildren<Actions>().hasMoved = false;
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
        hasMoved = true;
        moveText.GetComponent<Text>().text = "Undo Move";
        this.GetComponentInChildren<Actions>().hasMoved = true;
        EventManager.StartListening("UndoMovedUnit", UndoMovedUnit);
        EventManager.StopListening("MovedUnit", MovedUnit);
        Debug.Log("MovedUnit ran!");
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
        hasMoved = false;
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
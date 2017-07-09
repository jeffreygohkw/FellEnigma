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
        

        thisCanvas = this.GetComponent<CanvasGroup>();
        moveText = GetComponentsInChildren<Text>()[0];
        OffUI();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SelectUnit()
    {
        OnUI();
        EventManager.StopListening("SelectUnit", SelectUnit);
        EventManager.StartListening("DeselectUnit", DeselectUnit);
    }

    void DeselectUnit()
    {
        OffUI();
        EventManager.StartListening("SelectUnit", SelectUnit);
        EventManager.StopListening("DeselectUnit", DeselectUnit);
        hasMoved = false;
        moveText.GetComponent<Text>().text = "Move";
        this.GetComponentInChildren<Actions>().hasMoved = false;
    }

    void MovedUnit()
    {
        hasMoved = true;
        moveText.GetComponent<Text>().text = "Undo Move";
        this.GetComponentInChildren<Actions>().hasMoved = true;
        EventManager.StartListening("UndoMovedUnit", UndoMovedUnit);
        EventManager.StopListening("MovedUnit", MovedUnit);

    }

    void UndoMovedUnit()
    {
        hasMoved = false;
        moveText.GetComponent<Text>().text = "Move";
        this.GetComponentInChildren<Actions>().hasMoved = false;
        EventManager.StopListening("UndoMovedUnit", UndoMovedUnit);
        EventManager.StartListening("MovedUnit", MovedUnit);
    }




    private void OnUI()
    {
        thisCanvas.alpha = 1f;
        thisCanvas.interactable = true;
        thisCanvas.blocksRaycasts = true;
    }

    private void OffUI()
    {
        thisCanvas.alpha = 0f;
        thisCanvas.interactable = false;
        thisCanvas.blocksRaycasts = false;
    }
}
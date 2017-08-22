using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BPMain : MonoBehaviour {

    public static BPMain instance;

    public CanvasGroup canvasA; // Status
    public CanvasGroup canvasB; // Shop
    public CanvasGroup canvasC; // Next Chapter

    public bool npDead = false;
    public bool ksDead = false;
    public bool yrDead = false;
    public bool bhDead = false;

	// Use this for initialization
	void Start () {
        canvasA = this.GetComponentsInChildren<CanvasGroup>()[0];
        canvasB = this.GetComponentsInChildren<CanvasGroup>()[1];
        canvasC = this.GetComponentsInChildren<CanvasGroup>()[2];

        GameControl.instance.Load();
        EventManager.StartListening("ToggleStatus", ToggleStatus);
        EventManager.StartListening("ToggleNext", ToggleNext);
        EventManager.StartListening("ToggleShop", ToggleShop);
        IntHeroes();
        EventManager.TriggerEvent("IntStatus");
        BPShopInventory.instance.initialiseItems();
        BPShopInventory.instance.initialiseShop();
	}

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update () {

	}

    /**
    * EventManager: Toggles the status window
    * 
    * @author Wayne Neo
    * @version 1.0
    * @updated on 22/08/17
    */
    void ToggleStatus()
    {
        if (canvasA.alpha == 0)
        {
            canvasA.alpha = 1f;
            canvasA.interactable = true;
            canvasA.blocksRaycasts = true;
        }
        else
        {
            canvasA.alpha = 0f;
            canvasA.interactable = false;
            canvasA.blocksRaycasts = false;
        }

        canvasB.alpha = 0f;
        canvasB.interactable = false;
        canvasB.blocksRaycasts = false;
        canvasC.alpha = 0f;
        canvasC.interactable = false;
        canvasC.blocksRaycasts = false;

    }

    /**
    * EventManager: Toggles the shop window
    * 
    * @author Wayne Neo
    * @version 1.0
    * @updated on 22/08/17
    */
    void ToggleShop()
    {
        if (canvasB.alpha == 0)
        { 
            canvasB.alpha = 1f;
            canvasB.interactable = true;
            canvasB.blocksRaycasts = true;
        }
        else
        {
            canvasB.alpha = 0f;
            canvasB.interactable = false;
            canvasB.blocksRaycasts = false;
        }

        canvasA.alpha = 0f;
        canvasA.interactable = false;
        canvasA.blocksRaycasts = false;
        canvasC.alpha = 0f;
        canvasC.interactable = false;
        canvasC.blocksRaycasts = false;

    }

    /**
    * EventManager: Toggles the next chapter window
    * 
    * @author Wayne Neo
    * @version 1.0
    * @updated on 22/08/17
    */
    void ToggleNext()
    {
        if (canvasC.alpha == 0)
        {
            canvasC.alpha = 1f;
            canvasC.interactable = true;
            canvasC.blocksRaycasts = true;
        }
        else
        {
            canvasC.alpha = 0f;
            canvasC.interactable = false;
            canvasC.blocksRaycasts = false;
        }

        canvasB.alpha = 0f;
        canvasB.interactable = false;
        canvasB.blocksRaycasts = false;
        canvasA.alpha = 0f;
        canvasA.interactable = false;
        canvasA.blocksRaycasts = false;

    }


    /**
    * Proceed to the next chapter
    *
    * @author Wayne Neo
    * @version 1.0
    * @updated on 22/08/17
    */
    public void ToNextChapter()
    {
        if (GameControl.instance.chapterID == 2)
        {
            GameControl.instance.SaveWithoutGrid();
            SceneManager.LoadScene("Chapter2");
        }
    }


    /**
    * Initialises the boolean to check whether any main character died
    *
    * @author Wayne Neo
    * @version 1.0
    * @updated on 22/08/17
    */
    private void IntHeroes()
    {
		if (GameControl.instance.npIntData.Count != 0)
		{
			if (GameControl.instance.npIntData[3] <= 0)
			{
				npDead = true;
			}
		}
		if (GameControl.instance.yrIntData.Count != 0)
		{
			if (GameControl.instance.yrIntData[3] <= 0)
			{
				yrDead = true;
			}
		}
		if (GameControl.instance.ksIntData.Count != 0)
		{
			if (GameControl.instance.ksIntData[3] <= 0)
			{
				ksDead = true;
			}
		}
		if (GameControl.instance.bhIntData.Count != 0)
		{
			if (GameControl.instance.bhIntData[3] <= 0)
			{
				bhDead = true;
			}
		}
    }
}

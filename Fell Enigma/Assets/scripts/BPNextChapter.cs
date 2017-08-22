using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BPNextChapter : MonoBehaviour {

    public Text ultDes;
    public string ultDesString;
    public int unitID;
    private RawImage unitProfile;

	// Use this for initialization
	void Start () {
        unitProfile = this.GetComponent<RawImage>();
        IntNext();
	}
	
	// Update is called once per frame
	void Update () {
        // If window is open and mouse is near UI element
        if (BPMain.instance.canvasC.alpha == 1 && Mathf.Abs(Input.mousePosition.x - this.transform.position.x) <= 27 && Mathf.Abs(Input.mousePosition.y - this.transform.position.y) <= 33)
        {
            // Mimics OnMouseOver, description written in Inspector
            ultDes.text = ultDesString;
            // If mouse is clicked at that time
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                // Check whether dead.
                if (!BPMain.instance.npDead && unitID == 1)
                {
                    GameControl.instance.ultID = unitID;
                    BPMain.instance.ToNextChapter();
                }
                else if (!BPMain.instance.ksDead && unitID == 2)
                {
                    GameControl.instance.ultID = unitID;
                    BPMain.instance.ToNextChapter();
                }
                else if (!BPMain.instance.yrDead && unitID == 3)
                {
                    GameControl.instance.ultID = unitID;
                    BPMain.instance.ToNextChapter();
                }
                else if (!BPMain.instance.bhDead && unitID == 4)
                {
                    GameControl.instance.ultID = unitID;
                    BPMain.instance.ToNextChapter();
                }
            }
        }
	}


    /**
    * Initialises the profiles to match deaths for main characters
    *
    * @author Wayne Neo
    * @version 1.0
    * @updated on 22/08/17
    */
    private void IntNext()
    {
        if (BPMain.instance.npDead && unitID == 1)
        {
            unitProfile.color = new Color(0.28f, 0f, 0f, 1f);
        }
        else if (BPMain.instance.ksDead && unitID == 2)
        {
            unitProfile.color = new Color(0.28f, 0f, 0f, 1f);
        }
        else if (BPMain.instance.yrDead && unitID == 3)
        {
            unitProfile.color = new Color(0.28f, 0f, 0f, 1f);
        }
        else if (BPMain.instance.bhDead && unitID == 4)
        {
            unitProfile.color = new Color(0.28f, 0f, 0f, 1f);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatLog : MonoBehaviour{

    public static CombatLog instance;

    private Queue<string> Eventlog = new Queue<string>();
    private string fullLog = "";
    private string nowtoPrint;
    private Text textBar;

    private bool fullLogOut = false;

    private int counter = 0;
    private int maxLines = Screen.height/22; //Adjusted to fit screen size

    // Use this for initialization
    void Start () {
        textBar = GetComponent<Text>();
        Debug.Log(maxLines);
	}

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update () {
		
	}

   
    /**
     * Adds a string to the bar to be printed afterwards
     *
     * v1.1
     * Updated Full Log to save previous lines
     * 
     * @param eventString string to be added into the log
     * @author Wayne Neo
     * @version 1.1
     * @updated on 23/7/17
     */
    public void AddEvent(string eventString)
    {
        if (Eventlog.Count >= maxLines)
            Eventlog.Dequeue();

        if (counter < maxLines)
        {
            fullLog += eventString;
            fullLog += "\n";
            counter++;
        }
        else
        {
            string[] lines = fullLog.Split("\n"[0]);
            fullLog = "Prev Line: " + lines[lines.Length - 2];
            fullLog += "\n";
            counter = 1;
        }
        Eventlog.Enqueue(eventString);

    }

    /**
    * Prints all the strings stored
    *
    * 
    * @author Wayne Neo
    * @version 1.0
    * @updated on 14/7/17
    */
    public void PrintEvent()
    {
        if (Eventlog.Count != 0)
        StartCoroutine(ExecuteAfterTime(1f));
    }

    /**
    * Creates a delay between each print
    *
    * 
    * @author Wayne Neo
    * @version 1.0
    * @updated on 14/7/17
    */
    public IEnumerator ExecuteAfterTime(float time)
    {
        while (Eventlog.Count != 0)
        {
            yield return new WaitForSeconds(time);
            if (Eventlog.Count != 0)
            textBar.text = Eventlog.Dequeue();
        }
    }
    /**
    * If user clicks on the bar, a full log is produced
    *
    * 
    * @author Wayne Neo
    * @version 1.0
    * @updated on 14/7/17
    */
    public void ShowFullLog()
    {
        fullLogOut = true;
    }

    /**
   * If user clicks on the bar when log is shown, the full log is hidden
   *
   * 
   * @author Wayne Neo
   * @version 1.0
   * @updated on 14/7/17
   */
    public void HideFullLog()
    {
        fullLogOut = false;
    }

    private void OnGUI()
    {
        if (fullLogOut)
        GUI.Label(new Rect(0, Screen.height - (Screen.height / 1.1f), Screen.width, Screen.height / 1.35f), fullLog, GUI.skin.textArea);
    }
}

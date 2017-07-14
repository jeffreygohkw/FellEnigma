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

    public int maxLines = 100;

    // Use this for initialization
    void Start () {
        textBar = GetComponent<Text>();
	}

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update () {
		
	}

   

    public void AddEvent(string eventString)
    {
        if (Eventlog.Count >= maxLines)
            Eventlog.Dequeue();

        Eventlog.Enqueue(eventString);

        fullLog = "";
        foreach (string logEvent in Eventlog)
        {
            fullLog += logEvent;
            fullLog += "\n";
        }

    }

    public void PrintEvent()
    {
        if (Eventlog.Count != 0)
        StartCoroutine(ExecuteAfterTime(2f));
    }

    public IEnumerator ExecuteAfterTime(float time)
    {
        while (Eventlog.Count != 0)
        {
            yield return new WaitForSeconds(time);
            textBar.text = Eventlog.Dequeue();
        }
    }

}

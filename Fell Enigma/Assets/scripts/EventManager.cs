using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour {

    private Dictionary<string, UnityEvent> eventDictionary;

    private static EventManager eventManager;

    public static EventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;
            }
            if (!eventManager)
            {
                Debug.LogError("There needs to be one active eventmanager script on a gameobject");
            }
            else
            {
                eventManager.Start();
            }

            return eventManager;

        }
    }


    // Use this for initialization
    void Start () {

        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, UnityEvent>();
        }

	}
    

    // Update is called once per frame
    void Update () {
		
	}

    public static void StartListening(string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        // Search for event in dictionary
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            // thisEvent.AddListener(listener);
            Debug.LogError("This event " + eventName + " is already listening");
        }
        // If event does not exist, create event
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionary.Add(eventName, thisEvent);
            // Debug.Log("This event " + eventName + " started listening");
        }
    }

    public static void StopListening(string eventName, UnityAction listener)
    {
        // Failsafe, in case eventManager is removed
        if (eventManager == null) return;
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            // Debug.Log("This event " + eventName + " stopped listening");
            thisEvent.RemoveListener(listener);
            instance.eventDictionary.Remove(eventName);
        }
    }

    public static void TriggerEvent(string eventName)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            // Will call all listeners
            thisEvent.Invoke();
            if (!eventName.Equals("GetStats") && !eventName.Equals("RemoveStats"))
            // Debug.Log("This event " + eventName + " is triggered");
        }
        else
        {
            Debug.LogError("This event " + eventName + " doesn't exist!");
        }
    }
}


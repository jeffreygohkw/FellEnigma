using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTextAtLine : MonoBehaviour {

	public TextAsset theText;

	public int startLine;
	public int endLine;

	public TextBoxManager theTextBox;

	// Use this for initialization
	void Start ()
	{
		theTextBox = FindObjectOfType<TextBoxManager>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void onTriggerEnter2D(Collider2D other)
	{
		if (other.name == "Player")
		{
			theTextBox.ReloadScript(theText);
			theTextBox.currentLine = startLine;
			theTextBox.endAtLine = endLine;
			theTextBox.enableTextBox();
		}
	}
}

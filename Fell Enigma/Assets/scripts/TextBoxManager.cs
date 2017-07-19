using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxManager : MonoBehaviour {

	public GameObject textBox;

	public Text theText;

	public TextAsset textFile;
	public string[] textLines;

	public int currentLine;
	public int endAtLine;

	public bool isActive;

	private bool isTyping = false;
	private bool cancelTyping = false;

	public float typeSpeed = 0.03f;

	// Use this for initialization
	void Start ()
	{
		if (textFile != null)
		{
			textLines = (textFile.text.Split('\n'));
			if (endAtLine == 0)
			{
				endAtLine = textLines.Length - 1;
			}
		}


		if (isActive)
		{
			enableTextBox();
		}
		else
		{
			disableTextBox();
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (isActive)
		{
			//theText.text = textLines[currentLine];

			if (Input.GetMouseButtonDown(0))
			{
				if (!isTyping)
				{

					// Disable the text box when we're done
					if (currentLine > endAtLine)
					{
						disableTextBox();
					}
					else
					{
						StartCoroutine(textScroll(textLines[currentLine]));
					}
					currentLine++;
				}
				//If mouse click while it is still typing, display the whole line
				else if (isTyping && !cancelTyping)
				{
					cancelTyping = true;
				}
			}
		}
		else
		{
			return;
		}
	}


	private IEnumerator textScroll(string lineOfText)
	{
		int letter = 0;
		theText.text = "";
		isTyping = true;
		cancelTyping = false;
		while (isTyping && !cancelTyping && (letter < lineOfText.Length - 1))
		{
			theText.text += lineOfText[letter];
			letter++;
			//Wait for however long out text scrolls
			yield return new WaitForSeconds(typeSpeed);
		}
		theText.text = lineOfText;
		isTyping = false;
		cancelTyping = false;
	}

	public void enableTextBox()
	{
		textBox.SetActive(true);
		isActive = true;
	}

	public void disableTextBox()
	{
		textBox.SetActive(false);
		isActive = false;
	}

	public void ReloadScript(TextAsset theText)
	{
		if (theText != null)
		{
			textLines = new string[1];
			textLines = (theText.text.Split('\n'));
		}
	}
}

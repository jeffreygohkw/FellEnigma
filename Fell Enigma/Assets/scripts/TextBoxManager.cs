using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxManager : MonoBehaviour {

	public GameObject textBox;
	public RawImage displayProfile;

	public Text theText;

	public TextAsset textFile;
	public string[] textLines;
	public string[] split;

	public Texture[] profiles = new Texture[5];


	public int currentLine;
	public int endAtLine;

	public bool isActive;

	private bool isTyping = false;
	private bool cancelTyping = false;

	public float typeSpeed = 0.03f;

	public static TextBoxManager instance;

	public void Awake()
	{
		instance = this;
		
	}


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
					split = textLines[currentLine].Split(' ');
					if (split[0] == "Naive")
					{
						loadPortrait(1, 0);
					}
					else if (split[0] == "Kind")
					{
						loadPortrait(2, 0);
					}
					else if (split[0] == "Young")
					{
						if (Grid.instance.mapName == "tutorial")
						{
							if (currentLine == 3)
							{
								loadPortrait(3, 2);
							}
							else
							{
								loadPortrait(3, 0);
							}
						}
						else
						{
							loadPortrait(3, 0);
						}
					}
					else if (split[0] == "Black")
					{
						loadPortrait(4, 0);
					}
					else
					{
						loadPortrait(0, 0);
					}
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

	private void loadPortrait(int index, int expression)
	{
		if (index == 1)
		{
			if (expression == 0)
			{
				displayProfile.texture = profiles[0];
				displayProfile.uvRect = new Rect(0.68f, 0.38f, 0.3f, 0.55f);
			}
			else if (expression == 1)
			{
				//Calm down
				displayProfile.texture = profiles[0];
				displayProfile.uvRect = new Rect(0.34f, 0.38f, 0.3f, 0.55f);
			}
			else if (expression == 2)
			{
				//Facepalm
				displayProfile.texture = profiles[0];
				displayProfile.uvRect = new Rect(0f, 0.38f, 0.3f, 0.55f);
			}
		}
		else if (index == 2)
		{
			if (expression == 0)
			{
				displayProfile.texture = profiles[1];
				displayProfile.uvRect = new Rect(0.55f, 0.3f, 0.4f, 0.55f);
			}
			else
			{
				//Worry
				displayProfile.texture = profiles[1];
				displayProfile.uvRect = new Rect(0f, 0.3f, 0.4f, 0.55f);
			}
		}
		else if (index == 3)
		{
			if (expression == 0)
			{
				displayProfile.texture = profiles[2];
				displayProfile.uvRect = new Rect(0.67f, 0.38f, 0.28f, 0.55f);
			}
			else if (expression == 1)
			{
				//Smug
				displayProfile.texture = profiles[2];
				displayProfile.uvRect = new Rect(0f, 0.38f, 0.28f, 0.55f);
			}
			else if (expression == 2)
			{
				//Mouth close
				displayProfile.texture = profiles[2];
				displayProfile.uvRect = new Rect(0.34f, 0.38f, 0.28f, 0.55f);
			}
		}
		else if (index == 4)
		{
			if (expression == 0)
			{
				displayProfile.texture = profiles[3];
				displayProfile.uvRect = new Rect(0f, 0.38f, 0.3f, 0.55f);
			}
			else if (expression == 1)
			{
				//Hmph
				displayProfile.texture = profiles[3];
				displayProfile.uvRect = new Rect(0.34f, 0.38f, 0.3f, 0.55f);
			}
			else if (expression == 2)
			{
				//Angry
				displayProfile.texture = profiles[3];
				displayProfile.uvRect = new Rect(0.68f, 0.38f, 0.3f, 0.55f);
			}
		}
		else
		{
			displayProfile.texture = profiles[4];
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

	public void setCurrentLine(int line)
	{
		currentLine = line;
		StartCoroutine(textScroll(textLines[currentLine]));
		cancelTyping = true;
		
	}
}

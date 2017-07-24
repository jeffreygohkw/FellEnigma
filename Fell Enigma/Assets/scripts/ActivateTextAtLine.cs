using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTextAtLine : MonoBehaviour {

	public static ActivateTextAtLine instance;

	public TextAsset theText;

	public TextBoxManager theTextBox;

	public List<int> dialogue;

	public int recruiting = 0;

	public void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start ()
	{
		theTextBox = FindObjectOfType<TextBoxManager>();
		dialogue = new List<int>();
		dialogue.Add(-1);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void startScript(int startLine, int endLine)
	{
		//theTextBox.ReloadScript(theText);
		theTextBox.currentLine = startLine;
		theTextBox.endAtLine = endLine;
		theTextBox.enableTextBox();
	}

	public void clearDialogue()
	{
		dialogue.Clear();
	}

	public bool scriptUpdate()
	{
		if (Grid.instance.mapName == "tutorial")
		{
			if (!dialogue.Contains(0) && ((Grid.instance.units[0][0].gridPosition != Grid.instance.map[5][25].gridPosition && Grid.instance.units[0][0].doneMoving) || (Grid.instance.units[0][1].gridPosition != Grid.instance.map[5][26].gridPosition && Grid.instance.units[0][1].doneMoving)))
			{
				//When either NP or YR moves from their start tile
				startScript(12, 14);
				dialogue.Add(0);
				return true;
			}
			else if (!dialogue.Contains(1) && Grid.instance.units[1][1].currentHP == 0)
			{
				// When first Bandit dies
				startScript(16, 17);
				dialogue.Add(1);
				return true;
			}
			else if (!dialogue.Contains(2) && dialogue.Contains(0) && dialogue.Contains(1) && Grid.instance.units[0][0].currentHP == Grid.instance.units[0][0].maxHP && Grid.instance.units[0][1].currentHP == Grid.instance.units[0][1].maxHP)
			{
				//When healed
				startScript(19, 21);
				dialogue.Add(2);
				return true;
			}
			else if (!dialogue.Contains(3) && Grid.instance.units[1][2].currentHP == 0)
			{
				//Bandit 2 dies
				startScript(25, 30);
				dialogue.Add(2);
				dialogue.Add(3);
				return true;
			}
			else if (recruiting == 1)
			{
				startScript(33, 44);
				dialogue.Add(4);
				recruiting = 0;
				return true;
			}

			else if (!dialogue.Contains(5) && Grid.instance.units[1][3].currentHP == 0)
			{
				//Bandit 3 dies
				startScript(46, 58);
				dialogue.Add(5);
				return true;
			}
			else
			{
				return false;
			}
		}
		else
		{
			return false;
		}
	}
}

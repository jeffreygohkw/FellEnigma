using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour
{
	Animator anim;

	void Awake()
	{
		anim = GetComponent<Animator>();
	}

	void Update()
	{
		int status = WinCon.checkWinCon(Grid.instance.mapName);
		//Win conditions

		if (status == 1)
		{
			anim.SetTrigger("GameOver");
            EventManager.TriggerEvent("DeselectUnit");
		}
		else if (status == 2)
		{
			anim.SetTrigger("Victory");
            EventManager.TriggerEvent("DeselectUnit");
        }
	}
}
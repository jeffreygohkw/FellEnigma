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
		if (Grid.instance.units[2][0].currentHP <= 0)
		{
			anim.SetTrigger("GameOver");
		}
		else if (Grid.instance.units[1][0].currentHP <= 0)
		{
			anim.SetTrigger("Victory");
		}
	}
}
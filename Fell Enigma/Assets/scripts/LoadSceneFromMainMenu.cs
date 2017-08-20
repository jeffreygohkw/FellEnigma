using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneFromMainMenu : MonoBehaviour {

	public void LoadByName(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}

	public void Continue(string sceneName)
	{
		GameControl.instance.Load();
		if (GameControl.instance.nextScene != "")
		{
			sceneName = GameControl.instance.nextScene;
		}
		SceneManager.LoadScene(sceneName);
	}

	public void exitGame()
	{
		UnityEditor.EditorApplication.isPlaying = false;
		Application.Quit();
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public void LoadLevel(string name)
	{
		Debug.Log("Start the POOP !! LET THE TURDS FLY !!!" + name);
		SceneManager.LoadScene(name);
	}

	public void QuitRequest()
	{
		Debug.Log("Get me outta here");
	}

}

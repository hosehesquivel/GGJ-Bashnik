using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
	public GameObject Transition;
	
	
	public void PlayGame ()
	{
		SceneManager.LoadScene(1);
	}
 
	public void MainMenu ()
	{
		//Transition.LoadNextLevel();
		
		SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex - 1));
	}
 
    public void QuitGame ()
    {
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
	public Animator animator;
 
	public float transitionTime = 30f;
 
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // need to press play button.
		{
			LoadNextLevel();
		}
    }
	
	public void LoadNextLevel()
	{
		StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
	}
	
	IEnumerator LoadLevel(int levelIndex)
	{
		animator.SetTrigger("Start");
		
		yield return new WaitForSeconds(transitionTime);
		
		SceneManager.LoadScene(levelIndex);
	}
}

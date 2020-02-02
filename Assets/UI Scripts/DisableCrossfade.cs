using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableCrossfade : MonoBehaviour
{
	public GameObject BlackPanel;

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void OnFadeComplete()
	{
		BlackPanel.SetActive(false);
		
		//transform.parent.gameObject;
	}
	
	public void OnFadeBegin()
	{
		BlackPanel.SetActive(true);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    public GameObject bar;
    public GameObject bg;

    private string state = "repairable";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setState(string newState)
    {
        state = newState;

        if (state == "repairable")
        {
            bar.SetActive(false);
            bg.SetActive(false);
        } else
        {
            bar.SetActive(true);
            bg.SetActive(true);
        }
    }

    public void setScale(float scale)
    {
        bar.transform.localScale = new Vector3(1, 1, scale);
        Debug.Log("SCALE " + scale);
    }
}

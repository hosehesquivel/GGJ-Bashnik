using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int gold = 100;
    public static int startingGold = 100;

    public static GameObject UI;

    // Start is called before the first frame update
    static void Start()
    {
        UI = GameObject.FindGameObjectWithTag("GameplayUI");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void ChangeGold(int difference)
    {
        gold += difference;

        if (gold <= 0)
        {
            HandleGameOver();
        }
    }

    private static void HandleGameOver()
    {
    }
}

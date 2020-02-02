using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int gold = 500;
    public static int startingGold = 500;
    public static float timeToWin = 60; 

    public static double timer = 0;

    public static GameObject UI;

    // Start is called before the first frame update
    static void Start()
    {
        UI = GameObject.FindGameObjectWithTag("GameplayUI");
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        
    }

    public static void ChangeGold(int difference)
    {
        gold += difference;
    }

    private static void HandleGameOver()
    {
    }
}

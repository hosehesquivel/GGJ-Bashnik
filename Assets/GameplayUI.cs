using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    public GameObject mainUI;
    public GameObject pregameScsreen;
    public GameObject gameOverScreen;
    public GameObject winScreen;
    public GameObject menuScreen;

    private static GameObject activeScreen;

    private void Update()
    {
        if (GameManager.gold <= 0)
        {

            gameOverScreen.SetActive(true);
            Time.timeScale = 0;
        }

        if (GameManager.timer >= GameManager.timeToWin)
        {
            winScreen.SetActive(true);
            Time.timeScale = 0;
        }
    }
    public static void setScreen(string screen)
    {
     
    }
}

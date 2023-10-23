using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool paused = false;
    public bool gameOver = false;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return; 
        }
    }

    public void PauseGame()
    {
        paused = true;
        Time.timeScale = 0f;
        //this is where you'd put your pause menu 
    }
    public void ResumeGame()
    {
        paused = false; 
        Time.timeScale = 1f; 
        //this is where you'd turn off your pause menu
    }
    public void GameOver()
    {
        gameOver = true;
        //this is where you decide what happens at game over 
    }
}

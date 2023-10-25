using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;         // Set variables
    public bool paused = false;
    public bool gameOver = false;

    void Awake()                                // Before first frame
    {
        if(instance == null)                        // if there isn't an instance, create one in Don't destroy
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else                                        // Else destroy self and return
        {
            Destroy(gameObject);
            return; 
        }
    }

    public void PauseGame(GameObject panel)                     // Pause game
    {
        paused = true;                              // Game is paused
        Time.timeScale = 0f;                        // Time scale is zero
        panel.SetActive(true);                      // Show pause menu
        //this is where you'd put your pause menu 
    }
    public void ResumeGame(GameObject panel)                    // Resume game
    {
        paused = false;                             // Game is unpaused
        Time.timeScale = 1f;                        // Time scale is set to normal
        panel.SetActive(false);                     // Hide pause menu
        //this is where you'd turn off your pause menu
    }
    public void GameOver()                      // Game over
    {
        gameOver = true;                            // Set the game to over
        Time.timeScale = 0f;
        //this is where you decide what happens at game over 
    }
}

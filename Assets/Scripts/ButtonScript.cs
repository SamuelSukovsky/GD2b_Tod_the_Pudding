using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    [SerializeField] private String context;                // Context assigned
    
    void Awake()                                            // Before first frame
    {                                                           // Add button press trigger
        GetComponent<Button>().onClick.AddListener(ButtonPressed);
    }

    public void ButtonPressed()                             // On button pressed
    {
        if(context.Equals("Quit"))                              // If the context is Quit
        {
            Application.Quit();                                     // Quit application
        }
        else if (context.Equals("Resume"))                      // Else if the context is Resume 
        {                                                           // Unpause the game
            GameManager.instance.ResumeGame(transform.parent.gameObject);
        }
        else SceneManager.LoadScene(context);                   // Else load the scene with the name in context
    }
}

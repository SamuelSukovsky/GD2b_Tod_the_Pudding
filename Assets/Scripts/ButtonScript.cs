using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    public String sceneName;
    
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(ChangeScene);
    }

    public void ChangeScene()
    {
        if(sceneName.Equals("Quit"))
        {
            Application.Quit();
        }
        else if (sceneName.Equals("Resume"))
        {
            GameManager.instance.ResumeGame(transform.parent.gameObject);
        }
        else SceneManager.LoadScene(sceneName);
    }
}

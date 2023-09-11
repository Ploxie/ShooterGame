using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuPhilip : MonoBehaviour
{
    public GameObject pauseMenuUI;
    bool isPaused = false;
    private void Update()
    {
        //Debug.Log(SceneManager.GetActiveScene().name);
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "PlayAreaTestMenu")
        {
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //SceneManager.LoadScene("MainMenu");
                if (isPaused == false)
                {
                    pauseMenuUI.SetActive(true);
                    isPaused = true;
                }
                else if (isPaused == true) 
                {
                    pauseMenuUI.SetActive(false);
                    isPaused = false;
                }
                
            }
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("PlayAreaTestMenu");
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void QuitGame()
    {
        Debug.Log("Quit");
        //Application.Quit();
    }
}

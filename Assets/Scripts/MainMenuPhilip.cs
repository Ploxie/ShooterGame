using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuPhilip : MonoBehaviour
{
    // Only for play scene
    public GameObject pauseMenuUI; 
    //public GameObject deathMenuUI;
    public GameObject settingsMenuUI;
    public GameObject overallUI;

    bool isPaused = false; 
    bool canPause = true;

    private void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "LevelGenererationTestScene")
        {
            isPaused = false;
            pauseMenuUI.SetActive(false);
            //deathMenuUI.SetActive(false);
        }
    }
    private void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "LevelGenererationTestScene")
        {
            if (canPause == true) // So you can't pause while in death screen
                PauseGame();

            //Die();

            if (settingsMenuUI.activeSelf /*|| deathMenuUI.activeSelf*/) //So that you cant pause when in setting and while in death menu
                canPause = false; //Else it would look weird with 2 canvas active same time
            else 
                canPause = true;
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("LevelGenererationTestScene");
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu"); 
    }
    //private void Die()
    //{
    //    if (Input.GetKeyDown(KeyCode.T))
    //    {
    //        Debug.Log("you died");
    //        canPause = false;
    //        pauseMenuUI.SetActive(false); //If we have the feature where pause = game still continue
    //        overallUI.SetActive(false);
    //        isPaused = false;
    //        deathMenuUI.SetActive(true);
    //    }
    //}
    private void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused == false)
            {
                pauseMenuUI.SetActive(true);
                overallUI.SetActive(false);
                isPaused = true;
            }
            else if (isPaused == true)
            {
                pauseMenuUI.SetActive(false);
                overallUI.SetActive(true);
                isPaused = false;
            }
        }
    }
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}

using Assets.Scripts.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMenuScript : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;
    public GameObject overallUI;

    bool isPaused = false;
    bool canPause = true;
    

    // Start is called before the first frame update
    private void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "LevelGenererationTestScene" || currentScene.name == "GameplayLoop" || currentScene.name == "Tutorial")
        {
            isPaused = false;
            pauseMenuUI.SetActive(false);
            //deathMenuUI.SetActive(false);
        }
    }
    // Update is called once per frame
    private void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "LevelGenererationTestScene" || currentScene.name == "GameplayLoop" || currentScene.name == "Tutorial")
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
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}

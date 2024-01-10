using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuPhilip : MonoBehaviour
{
    public static bool isToggled;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenuNew")
        {
            isToggled = false;
            AudioFmodManager.instance.InitializeMusic(FmodEvents.instance.MainMenu);
        }
        if (SceneManager.GetActiveScene().name == "GameOver")
        {
            AudioFmodManager.instance.InitializeMusic(FmodEvents.instance.DeathMenu);
        }
        Debug.Log(isToggled);
    }

    public void PlayGame()
    {
        AudioFmodManager.instance.StopMusic();
        if (isToggled)
        {
            SceneManager.LoadScene("GameplayLoop"); // Change to Play scene
        }
        else
        {
            SceneManager.LoadScene("Tutorial"); // Change to tutorial scene
        }
        
        //AudioFmodManager.instance
    }
    public void BackToMenu()
    {
        AudioFmodManager.instance.StopMusic();
        SceneManager.LoadScene("MainMenuNew");
    }
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    public void SetToggle()
    {
        if (!isToggled)
        {
            isToggled = true;
        }
        else
        {
            isToggled = false;
        }
        Debug.Log(isToggled);
    }
}

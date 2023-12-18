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
        if (SceneManager.GetActiveScene().name == "MainMenu" || SceneManager.GetActiveScene().name == "Demo1")
        {
            isToggled = false;
        }
        Debug.Log(isToggled);
    }

    public void PlayGame()
    {
        if (isToggled)
        {
            SceneManager.LoadScene("GamePlayLoop"); // Change to Play scene
        }
        else
        {
            SceneManager.LoadScene("Tutorial"); // Change to tutorial scene
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

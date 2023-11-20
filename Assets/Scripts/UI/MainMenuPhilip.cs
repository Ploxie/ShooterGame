using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuPhilip : MonoBehaviour
{
    public Toggle m_toggle;

    public void PlayGame()
    {
        if (m_toggle.isOn)
        {
            SceneManager.LoadScene("LevelGenererationTestScene"); // Change to Play scene
        }
        else
        {
            SceneManager.LoadScene("GameOver"); // Change to tutorial scene
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

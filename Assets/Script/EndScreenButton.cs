using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenButton : MonoBehaviour
{
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("UI"); //Change to Main menu instead of UI 
    }
}

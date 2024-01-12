using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static int score = 0;
    public TMP_Text scoreText;

    private void Awake()
    {
        
    }


    void Start()
    {
        EventManager.GetInstance().AddListener<ScoreChangedEvent>(OnScoreChanged);
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "Tutorial" || currentScene.name == "GameplayLoop")
            score = 0;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.T)) // Remove this?
        {
            SceneManager.LoadScene("VictoryScreen");
        }
    }

    private void OnScoreChanged(ScoreChangedEvent e)
    {
        score += e.Score;
        scoreText.text = $"Score: {score}";
    }
}

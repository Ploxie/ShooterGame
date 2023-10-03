using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public static int score = 0;
    public TMP_Text scoreText;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "LevelGenererationTestScene")
            score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            UpdateText(10);
        }
        if (Input.GetKeyUp(KeyCode.T))
        {
            SceneManager.LoadScene("VictoryScreen");
        }
    }

    public void UpdateText(int value)
    {
        score += value;
        scoreText.text = $"Score: {score}";
    }
}

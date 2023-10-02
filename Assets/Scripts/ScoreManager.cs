using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static int score = 0;
    public TMP_Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
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

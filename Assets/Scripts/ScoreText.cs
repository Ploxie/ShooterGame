using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    public TMP_Text text;

    private void Start()
    {
        text.text = $"Score: {ScoreManager.score}";
    }

}

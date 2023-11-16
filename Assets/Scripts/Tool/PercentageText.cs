using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PercentageText : MonoBehaviour
{
    public Slider PercentageSlider;
    public TMP_Text PercentageLabel;

    void Update() => PercentageLabel.text = $"{Math.Round(PercentageSlider.value * 100)}%";

    private void OnDisable() => PlayerPrefs.SetFloat("MissPercentage", PercentageSlider.value);
}

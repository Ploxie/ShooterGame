using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class HealthBarManager : MonoBehaviour
{
    public Image healthBar;
    public TMP_Text HealthText;

    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = player.Health / player.MaxHealth;
        HealthText.text = $"{player.Health}/{player.MaxHealth}";
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Assets.Scripts.EventSystem;

public class HealthBarManager : MonoBehaviour
{
    public Image healthBar;
    public TMP_Text HealthText;

    private void Awake()
    {
        EventManager.PlayerHealthChanged += SetHealth;
    }

    private void SetHealth(float health)
    {
        if (EnemyManager.Instance.Player == null)
            return;

        var player = EnemyManager.Instance.Player;

        healthBar.fillAmount = player.Health / player.MaxHealth;
        HealthText.text = $"{player.Health}/{player.MaxHealth}";
    }
}

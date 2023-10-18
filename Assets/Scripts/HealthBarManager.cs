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
        movingHealthBar.fillAmount = movingHealth / player.MaxHealth;
        if(movingHealth > player.Health)
        {
            timer += Time.deltaTime;
            if(timer > 0.5f)
            {
                movingHealth -= 0.25f;
            }
            if(movingHealth <= player.Health)
            {
                timer = 0;
            }
        }
    }


}

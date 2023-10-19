using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Assets.Scripts.Entity;

public class HealthBarManager : MonoBehaviour
{
    public Image healthBar;
    public Image movingHealthBar;
    public TMP_Text HealthText;

    private float lastHealth;

    private void Awake()
    {
        EventManager.GetInstance().AddListener<PlayerHealthChangeEvent>(SetHealth);
    }

    private void Update()
    {
        movingHealthBar.fillAmount = Mathf.Lerp(lastHealth, healthBar.fillAmount, Time.deltaTime);
    }

    private void SetHealth(PlayerHealthChangeEvent e)
    {
        var health = e.Health;

        lastHealth = healthBar.fillAmount;
        healthBar.fillAmount = health.CurrentHealth / health.MaxHealth;
        HealthText.text = $"{health.CurrentHealth}/{health.MaxHealth}";        
    }


}

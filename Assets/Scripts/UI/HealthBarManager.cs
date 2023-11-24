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
    [SerializeField] private Image movingHealthBar;
    public TMP_Text HealthText;

    private void Start()
    {
        EventManager.GetInstance().AddListener<PlayerHealthChangeEvent>(SetHealth);
    }

    private void Update()
    {
        
        movingHealthBar.fillAmount = Mathf.Lerp(movingHealthBar.fillAmount, healthBar.fillAmount, Time.deltaTime * 5.0f);
    }

    private void SetHealth(PlayerHealthChangeEvent e)
    {
        var health = e.Health;

        var currentHealthPercentage = health.CurrentHealth / health.MaxHealth;

        healthBar.fillAmount = currentHealthPercentage;
        HealthText.text = $"{(int)Mathf.Ceil(health.CurrentHealth)}/{health.MaxHealth}";        
    }


}

using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Threading;

public class HealthBarManager : MonoBehaviour
{
    public Image healthBar;
    public TMP_Text HealthText;
    public Image movingHealthBar;
    private Player player;
    public float movingHealth = 100;
    public float timer = 0;


    // Update is called once per frame
    void Update()
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

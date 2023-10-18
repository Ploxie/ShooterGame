using Assets.Scripts.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    public Image enemyHealthBar;
    public Image enemyHealthBarBorder;
    public Image enemyMovingHealthBar;
    public float enemyMovingHealth;
    public Transform target;
    public float timer;

    private Health health;
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("MainCamera").transform;
        health = GetComponentInParent<Health>();
        health.OnDamageTaken += UpdateHealthBar;
        health.OnHealthGained += UpdateHealthBar;
    }

    // Update is called once per frame
    void Update()
    {
        enemyMovingHealthBar.fillAmount = enemyMovingHealth / living.MaxHealth;
        if (enemyMovingHealth > living.Health)
        {
            timer += Time.deltaTime;
            if (timer > 0.5f)
            {
                enemyMovingHealth -= 0.25f;
            }
            if (enemyMovingHealth <= living.Health)
            {
                timer = 0;
            }
        }
        enemyHealthBarBorder.transform.LookAt(target);
    }


    private void UpdateHealthBar(float _)
    {
        enemyHealthBar.fillAmount = health.CurrentHealth / health.MaxHealth;
    }

}

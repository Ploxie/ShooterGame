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
    private Transform target;

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

        enemyHealthBarBorder.transform.LookAt(target);
    }


    private void UpdateHealthBar(float _)
    {
        enemyHealthBar.fillAmount = health.CurrentHealth / health.MaxHealth;
    }

}

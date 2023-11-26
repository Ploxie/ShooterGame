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
        enemyMovingHealth = health.MaxHealth;
        health.OnDamageTaken += UpdateHealthBar;
        health.OnHealthGained += UpdateHealthBar;
    }

    // Update is called once per frame
    void Update()
    {

        enemyMovingHealthBar.fillAmount = Mathf.Lerp(enemyMovingHealthBar.fillAmount, enemyHealthBar.fillAmount, Time.deltaTime * 5.0f);

        //enemyHealthBarBorder.transform.LookAt(target);
        enemyHealthBarBorder.transform.rotation = target.rotation;
    }


    private void UpdateHealthBar(float _)
    {
        enemyHealthBar.fillAmount = health.CurrentHealth / health.MaxHealth;
    }
}

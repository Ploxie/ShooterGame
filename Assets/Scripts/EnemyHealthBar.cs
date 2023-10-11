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
    private float enemyHealthAmount;
    public Transform target;
    public float timer;

    Living living;
    public float enemyMovingHealth;
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("MainCamera").transform;
        living = GetComponentInParent<Living>();
        enemyHealthAmount = living.MaxHealth;
        enemyMovingHealth = living.MaxHealth;
        timer = 0;
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

    public void TakeDamage(float damage)
    {
        enemyHealthAmount -= damage;
        enemyHealthBar.fillAmount = enemyHealthAmount / living.MaxHealth;
        
    }

}

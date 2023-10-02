using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    public Image enemyHealthBar;
    public Image enemyHealthBarBorder;
    private float enemyHealthAmount;
    public Transform target;

    Living living;
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("MainCamera").transform;
        living = GetComponentInParent<Living>();
        enemyHealthAmount = living.MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        

        enemyHealthBarBorder.transform.LookAt(target);
    }

    public void TakeDamage(float damage)
    {
        enemyHealthAmount -= damage;
        enemyHealthBar.fillAmount = enemyHealthAmount / living.MaxHealth;
    }

}

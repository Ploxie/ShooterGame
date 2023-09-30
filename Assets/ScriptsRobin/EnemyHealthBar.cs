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
    public float enemyHealthAmount = 100;
    public Transform target;

    Living living;
    void Start()
    {
        living = GetComponentInParent<Living>();
        enemyHealthAmount = living.MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            TakeDamage(20);
        }

        enemyHealthBarBorder.transform.LookAt(target);
    }

    public void TakeDamage(float damage)
    {
        enemyHealthAmount -= damage;
        enemyHealthBar.fillAmount = enemyHealthAmount / 100f;
    }

}
